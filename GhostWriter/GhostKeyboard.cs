using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace GhostWriter
{
    public static class GhostKeyboard
    {
        private static readonly Regex escapeRegex = new Regex(@"[+\^%~(){}[\]]");
        private static readonly Regex pauseRegex = new Regex(@"\[(Pause (\d+))\]");

        private static readonly Random random = new Random();

        private static readonly IEnumerable<KeyValuePair<string, MatchEvaluator>> commands = new Dictionary<string, MatchEvaluator>
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
            { @"SelectText (\d+),(\d+) (\d+),(\d+)", SelectText },
            { @"Delete (\d+)", Delete },
            { @"SelectTextFromHere (\d+),(\d+)", SelectTextFromHere },
            { @"DeleteLine", DeleteLine },
            { @"DeleteAll", DeleteAll },
            { @"Fast", Fast },
            { @"/Fast", Fast },
            { @"CommentLines", CommentLines },
            { @"UncommentLines", UncommentLines },
        };

        public static IEnumerable<string> Commands
        {
            get { return new[] { "[Pause #]" }.Concat(commands.Select(kvp => kvp.Key).Select(commandRegex => "[" + commandRegex.Replace("(", "").Replace(")", "").Replace(@"\d+", "#") + "]")); }
        }

        public static DelayStrategy DelayStrategy { get; set; }

        public static void TypeRaw(string input)
        {
            SendKeys.SendWait(input);
        }

        public static void Type(string rawInput)
        {
            var escapedInput = EscapeInput(rawInput);

            var sb = new StringBuilder();
            for (int i = 0; i < escapedInput.Length; i++)
            {
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
                    if (escapedInput[i + 1] == ']')
                    {
                        // A command wrapped in [] does not have a delay...
                        AddAndExecuteFastCommand(escapedInput, sb, ref i);
                        continue;
                    }

                    if (pauseRegex.IsMatch(escapedInput, i))
                    {
                        var match = pauseRegex.Match(escapedInput, i);
                        var seconds = int.Parse(match.Groups[2].Value);

                        if (DelayStrategy != DelayStrategy.Unchecked)
                        {
                            Sleep(1000 * seconds);
                        }

                        i = match.Index + match.Length - 1;
                        sb.Clear();
                        continue;
                    }
                }

                var key = sb.ToString();

                if (key == "~")
                {
                    Sound.PlayCarriageReturn();

                    if (DelayStrategy == DelayStrategy.Normal)
                    {
                        Sleep(random.Next(200, 300));
                    }
                    else if (DelayStrategy == DelayStrategy.Fast)
                    {
                        Sleep(210);
                    }

                    SendKeys.SendWait(key);
                }
                else
                {
                    if (key == " ")
                    {
                        Sound.PlaySpace();
                    }
                    else
                    {
                        Sound.PlayKeystroke();
                    }

                    SendKeys.SendWait(key);

                    if (DelayStrategy == DelayStrategy.Normal)
                    {
                        Sleep(random.NextDouble() > 0.80 ? random.Next(80, 160) : random.Next(30, 90));
                    }
                    else if (DelayStrategy == DelayStrategy.Fast)
                    {
                        Sleep(50);
                    }
                }

                sb.Clear();
            }
        }

        private static void Sleep(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        }

        public static string EscapeInput(string rawInput)
        {
            var escapedInput = escapeRegex.Replace(rawInput, "{$0}");
            escapedInput = escapedInput.Replace("\r\n", "~").Replace("\n", "~");
            escapedInput = InsertCommands(escapedInput);
            return escapedInput;
        }

        private static string InsertCommands(string input)
        {
            var output = commands.Aggregate(input, (current, command) => Regex.Replace(current, @"{\[}" + command.Key + @"{\]}", command.Value));
            return Regex.Replace(output, @"{\[}(Pause (\d+)){\]}", match => "[" + match.Groups[1] + "]");
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
            for (int i = 0; i < lineCount; i++)
            {
                sb.Append("{DOWN}");
            }

            for (int i = 0; i < columnCount; i++)
            {
                sb.Append("{RIGHT}");
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
    }
}