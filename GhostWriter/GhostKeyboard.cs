using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace GhostWriter
{
    public sealed class GhostKeyboard : IDisposable
    {
        private const string _escapeRegexPattern = @"[+\^%~(){}]|\[(?!`)|(?<!`)\]";
        private static readonly Regex _escapeRegex = new Regex(_escapeRegexPattern);
        private static readonly Regex _unescapeRegex = new Regex(@"\{(" + _escapeRegexPattern + @")\}");
        private static readonly Regex _unescapeNewlineRegex = new Regex(@"(?<!\{)~(?!\})");
        private static readonly Regex _pauseRegex = new Regex(@"\[(Pause ((?:\d+(?:\.\d+)?)|\.\d+))\]");

        private readonly Random _random = new Random();

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYDOWN = 0x0104;

        private readonly LowLevelKeyboardProc _proc;
        private readonly IntPtr _hookID = IntPtr.Zero;

        private readonly Action _setFocusOnMainForm;
        private readonly Action _setFocusOnTargetApplication;
        private readonly Action<int> _setActiveTabIndex;
        private readonly Action _swapForegroundWindows;

        private volatile bool _abort;
        private volatile bool _fast;
        private volatile bool _isTyping;

        private readonly object _breakDialogLocker = new object();

        private static readonly IEnumerable<KeyValuePair<string, MatchEvaluator>> _commands = new Dictionary<string, MatchEvaluator>
        {
            { @"GotoLine (\d+)", GotoLine },
            { @"End", End },
            { @"Backspace (\d+)", Backspace },
            { @"Tab (\d+)", Tab },
            { @"Up", Up },
            { @"Down", Down },
            { @"Left", Left },
            { @"Right", Right },
            { @"Up (\d+)", UpMultiple },
            { @"Down (\d+)", DownMultiple },
            { @"Left (\d+)", LeftMultiple },
            { @"Right (\d+)", RightMultiple },
            { @"SelectText (\d+),(\d+) (-?\d+),(-?\d+)", SelectText },
            { @"Delete (\d+)", Delete },
            { @"SelectTextFromHere (-?\d+),(-?\d+)", SelectTextFromHere },
            { @"DeleteLine", DeleteLine },
            { @"DeleteAll", DeleteAll },
            { @"Fast", Fast },
            { @"/Fast", Fast },
            { @"CommentLines", CommentLines },
            { @"UncommentLines", UncommentLines },
            { @"ToggleCollapse", ToggleCollapse },
        };

        public GhostKeyboard(
            Action setFocusOnMainForm,
            Action setFocusOnTargetApplication,
            Action<int> setActiveTabIndex,
            Action swapForegroundWindows)
        {
            _setFocusOnMainForm = setFocusOnMainForm;
            _setFocusOnTargetApplication = setFocusOnTargetApplication;
            _setActiveTabIndex = setActiveTabIndex;
            _swapForegroundWindows = swapForegroundWindows;

            _proc = HookCallback;
            _hookID = SetHook(_proc);
        }

        ~GhostKeyboard()
        {
            Dispose(false);
        }

        public static IEnumerable<string> Commands
        {
            get { return new[] { "[Pause #]", "[Wait]" }.Concat(_commands.Select(kvp => kvp.Key).Select(commandRegex => "[" + commandRegex.Replace("(", "").Replace(")", "").Replace(@"-?\d+", "#").Replace(@"\d+", "#") + "]")); }
        }

        public DelayStrategy DelayStrategy { get; set; }
        public bool IsMonitoring { get; set; }

        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                
            }

            UnhookWindowsHookEx(_hookID);
        }

        public void TypeRaw(string input)
        {
            SendKeys.SendWait(input);
        }

        public void Abort()
        {
            _abort = true;
        }

        public void Type(string rawInput)
        {
            _abort = false;
            _fast = false;
            _isTyping = true;

            var escapedInput = EscapeInput(rawInput ?? "");

            var sb = new StringBuilder();
            for (int i = 0; i < escapedInput.Length; i++)
            {
                if (_abort)
                {
                    _isTyping = false;
                    return;
                }

                var c = escapedInput[i];
                sb.Append(c);
                if (c == '{')
                {
                    AddEscapedCharacters(escapedInput, c, sb, ref i);
                }
                else if (c == '+' || c == '^' || c == '%')
                {
                    AddCombo(escapedInput, sb, ref i);
                }
                else if (c == '[')
                {
                    ProcessCommand(escapedInput, sb, ref i);
                }
                else
                {
                    ProcessKey(sb);
                }
            }

            _isTyping = false;
        }

        private void ProcessKey(StringBuilder sb)
        {
            var key = sb.ToString();

            if (_abort)
            {
                _isTyping = false;
                return;
            }

            SendKeys.SendWait(key);

            if (!_fast)
            {
                int milliseconds;

                if (DelayStrategy == DelayStrategy.Normal)
                {
                    milliseconds =
                        IsEnter(key)
                            ? _random.Next(200, 300)
                            : (_random.NextDouble() > 0.80 ? _random.Next(80, 160) : _random.Next(30, 90));
                }
                else if (DelayStrategy == DelayStrategy.Fast)
                {
                    milliseconds = IsEnter(key) ? 210 : 50;
                }
                else
                {
                    milliseconds = 0;
                }

                if (_abort)
                {
                    _isTyping = false;
                    return;
                }

                switch (key)
                {
                    case " ":
                        Sound.PlaySpace();
                        break;
                    case "~":
                        Sound.PlayCarriageReturn();
                        break;
                    default:
                        Sound.PlayKeystroke();
                        break;
                }

                Sleep(ReduceIfIsMonitoring(milliseconds));
            }

            sb.Clear();
        }

        private void ProcessCommand(string escapedInput, StringBuilder sb, ref int i)
        {
            if (escapedInput[i + 1] == ']')
            {
                // A command wrapped in [] does not have a delay...
                AddAndExecuteFastCommand(escapedInput, sb, ref i);
            }
            else if (escapedInput[i + 1] == '`')
            {
                // When wrapped in [`text to paste`] indicates a paste command
                ExecutePasteCommand(escapedInput, ref i);
            }
            else if (_pauseRegex.IsMatch(escapedInput, i))
            {
                ExecutePauseCommand(escapedInput, sb, ref i);
            }
            else if (escapedInput.Substring(i).StartsWith("[Wait]"))
            {
                ExecuteWaitCommand(sb, ref i);
            }
            else
            {
                Debug.Fail("Unknown key sequence starting with '['.");
            }
        }

        private void ExecuteWaitCommand(StringBuilder sb, ref int i)
        {
            _setFocusOnMainForm();

            var dialog = new WaitDialog();
            dialog.ShowDialog();

            i = i + "Wait".Length + 1;
            sb.Clear();
            _fast = false;

            _setFocusOnTargetApplication();
        }

        private void ExecutePauseCommand(string escapedInput, StringBuilder sb, ref int i)
        {
            var match = _pauseRegex.Match(escapedInput, i);
            var seconds = double.Parse(match.Groups[2].Value);

            if (DelayStrategy != DelayStrategy.Unchecked)
            {
                Sleep((int)(1000 * seconds));
            }

            i = match.Index + match.Length - 1;
            sb.Clear();
            _fast = false;
        }

        private static bool IsEnter(string s)
        {
            return s == "~";
        }

        private int ReduceIfIsMonitoring(int value)
        {
            return IsMonitoring ? (int)(value * (3.0 / 5.0)) : value;
        }

        private static void Sleep(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        }

        public static string EscapeInput(string rawInput)
        {
            var escapedInput = _escapeRegex.Replace(rawInput, "{$0}");
            escapedInput = escapedInput.Replace("\r\n", "~").Replace("\n", "~");
            escapedInput = InsertCommands(escapedInput);
            return escapedInput;
        }

        private static string InsertCommands(string input)
        {
            var output = _commands.Aggregate(input, (current, command) => Regex.Replace(current, @"{\[}" + command.Key + @"{\]}", command.Value));
            return Regex.Replace(output, @"{\[}(Pause (?:(?:\d+(?:\.\d+)?)|\.\d+)|Wait){\]}", match => "[" + match.Groups[1] + "]");
        }

        private static void AddEscapedCharacters(string escapedInput, char c, StringBuilder sb, ref int i)
        {
            if (escapedInput[i + 1] == '}')
            {
                sb.Append("}}");
                i += 2;
            }
            else
            {
                while (c != '}')
                {
                    i++;
                    c = escapedInput[i];
                    sb.Append(c);
                }
            }
        }

        private static void AddCombo(string escapedInput, StringBuilder sb, ref int i)
        {
            for (i = i + 1; i < escapedInput.Length; i++)
            {
                var c = escapedInput[i];
                sb.Append(c);
                if (c == '{')
                {
                    if (escapedInput[i + 1] == '}')
                    {
                        sb.Append("}}");
                        i += 2;
                    }
                    else
                    {
                        while (c != '}')
                        {
                            i++;
                            c = escapedInput[i];
                            sb.Append(c);
                        }
                    }
                }
                else if (c == ')')
                {
                    break;
                }
            }
        }

        private static void AddAndExecuteFastCommand(string escapedInput, StringBuilder sb, ref int i)
        {
            char c;
            i += 2;
            sb.Clear();

            do
            {
                c = escapedInput[i];
                sb.Append(c);
                i++;
            } while (c != '[' && escapedInput[i + 1] != ']');

            i += 1;

            var command = sb.ToString();
            SendKeys.SendWait(command);
            sb.Clear();
        }

        private static void ExecutePasteCommand(string escapedInput, ref int i)
        {
            var sb = new StringBuilder();

            char c;
            i += 2;

            do
            {
                c = escapedInput[i];
                sb.Append(c);
                i++;
            } while (c != '`' && escapedInput[i + 1] != ']');

            i += 1;

            var pasteContents =
                _unescapeRegex.Replace(
                    _unescapeNewlineRegex.Replace(sb.ToString(), "\r\n"),
                    "$1");

            Clipboard.SetText(pasteContents);
            Thread.Sleep(500);
            SendKeys.SendWait("^(v)");
        }

        private static string GotoLine(Match match)
        {
            var sb = new StringBuilder();
            var line = int.Parse(match.Groups[1].Value);

            GotoLine(sb, line);

            var command = sb.ToString();
            return "[]" + command + "[]";
        }

        private static void GotoLine(StringBuilder sb, int line)
        {
            sb.Append("^({END})^(g)").Append(line).Append("~");
        }

        private static string End(Match match)
        {
            return "[]{END}[]";
        }

        private static string Backspace(Match match)
        {
            var sb = new StringBuilder();
            var count = int.Parse(match.Groups[1].Value);

            for (int i = 0; i < count; i++)
            {
                sb.Append("{BS}");
            }

            var command = sb.ToString();
            return command;
        }

        private static string Tab(Match match)
        {
            var sb = new StringBuilder();
            var tabs = int.Parse(match.Groups[1].Value);

            for (int i = 0; i < tabs * 4; i++)
            {
                sb.Append(" ");
            }

            var command = sb.ToString();
            return "[]" + command + "[]";
        }

        private static string Up(Match match)
        {
            return "[]{Up}[]";
        }

        private static string SelectText(Match match)
        {
            var sb = new StringBuilder();
            var lineNumber = int.Parse(match.Groups[1].Value);
            var columnNumber = int.Parse(match.Groups[2].Value);
            var lineCount = int.Parse(match.Groups[3].Value);
            var columnCount = int.Parse(match.Groups[4].Value);

            GotoLine(sb, lineNumber);
            sb.Append("{HOME}");

            for (int i = 0; i < columnNumber - 1; i++)
            {
                sb.Append("{RIGHT}");
            }

            SelectTextFromHere(sb, lineCount, columnCount);

            var command = sb.ToString();
            return "[]" + command + "[]";
        }

        private static string SelectTextFromHere(Match match)
        {
            var sb = new StringBuilder();
            var lineCount = int.Parse(match.Groups[1].Value);
            var columnCount = int.Parse(match.Groups[2].Value);

            SelectTextFromHere(sb, lineCount, columnCount);

            var command = sb.ToString();
            return "[]" + command + "[]";
        }

        private static void SelectTextFromHere(StringBuilder sb, int lineCount, int columnCount)
        {
            sb.Append("+(");
            for (int i = 0; i < Math.Abs(lineCount); i++)
            {
                if (lineCount > 0)
                {
                    sb.Append("{DOWN}");
                }
                else
                {
                    sb.Append("{UP}");
                }
            }

            for (int i = 0; i < Math.Abs(columnCount); i++)
            {
                if (columnCount > 0)
                {
                    sb.Append("{RIGHT}");
                }
                else
                {
                    sb.Append("{LEFT}");
                }
            }
            sb.Append(")");
        }

        private static string Delete(Match match)
        {
            var sb = new StringBuilder();
            var count = int.Parse(match.Groups[1].Value);

            for (int i = 0; i < count; i++)
            {
                sb.Append("{DEL}");
            }

            var command = sb.ToString();
            return command;
        }

        private static string Left(Match match)
        {
            return "[]{Left}[]";
        }

        private static string Down(Match match)
        {
            return "[]{Down}[]";
        }

        private static string DeleteLine(Match match)
        {
            return "[]^+(l)[]";
        }

        private static string DeleteAll(Match match)
        {
            return "[]^(a){DEL}[]";
        }

        private static string Fast(Match match)
        {
            return "[]";
        }

        private static string CommentLines(Match match)
        {
            return "[]^(k)^(c)[]";
        }

        private static string UncommentLines(Match match)
        {
            return "[]^(k)^(u)[]";
        }

        private static string ToggleCollapse(Match match)
        {
            return "[]^(m)^(m)[]";
        }

        private static string UpMultiple(Match match)
        {
            var sb = new StringBuilder();
            var count = int.Parse(match.Groups[1].Value);

            for (int i = 0; i < count; i++)
            {
                sb.Append("{Up}");
            }

            var command = sb.ToString();
            return "[]" + command + "[]";
        }

        private static string DownMultiple(Match match)
        {
            var sb = new StringBuilder();
            var count = int.Parse(match.Groups[1].Value);

            for (int i = 0; i < count; i++)
            {
                sb.Append("{Down}");
            }

            var command = sb.ToString();
            return "[]" + command + "[]";
        }

        private static string LeftMultiple(Match match)
        {
            var sb = new StringBuilder();
            var count = int.Parse(match.Groups[1].Value);

            for (int i = 0; i < count; i++)
            {
                sb.Append("{Left}");
            }

            var command = sb.ToString();
            return "[]" + command + "[]";
        }

        private static string RightMultiple(Match match)
        {
            var sb = new StringBuilder();
            var count = int.Parse(match.Groups[1].Value);

            for (int i = 0; i < count; i++)
            {
                sb.Append("{Right}");
            }

            var command = sb.ToString();
            return "[]" + command + "[]";
        }

        private static string Right(Match match)
        {
            return "[]{Right}[]";
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            {
                using (ProcessModule curModule = curProcess.MainModule)
                {
                    return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
                }
            }
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            var vkCode = (Keys)Marshal.ReadInt32(lParam);

            if (nCode >= 0
                && (int)wParam == WM_SYSKEYDOWN
                && Control.ModifierKeys == Keys.Alt)
            {
                if (_isTyping)
                {
                    if (!BreakDialog.Instance.Visible)
                    {
                        lock (_breakDialogLocker)
                        {
                            if (!BreakDialog.Instance.Visible)
                            {
                                _setFocusOnMainForm();

                                BreakDialog.Instance.ShowDialog();

                                switch (BreakDialog.Instance.BreakDialogResult)
                                {
                                    case BreakDialogResult.GoFast:
                                        _fast = true;
                                        break;
                                    case BreakDialogResult.Abort:
                                        _abort = true;
                                        break;
                                    case BreakDialogResult.GoToPresentationTab:
                                        _setActiveTabIndex(0);
                                        break;
                                    case BreakDialogResult.GoToAutoTypingTab:
                                        _setActiveTabIndex(1);
                                        break;
                                    case BreakDialogResult.GoToAppMonitorTab:
                                        _setActiveTabIndex(2);
                                        break;
                                }

                                _setFocusOnTargetApplication();
                            }
                        }
                    }
                }
                else
                {
                    if (vkCode == (Keys)220)
                    {
                        _swapForegroundWindows();
                        return (IntPtr)1;
                    }
                }
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}