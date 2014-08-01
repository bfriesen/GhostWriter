using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace GhostWriter
{
    public partial class MainForm : Form
    {
        private static readonly string _appDataPath =
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "RandomSkunk",
                "GhostWriter",
                "appData.xml");

        private readonly GhostKeyboard _ghostKeyboard;
        private readonly System.Threading.Timer _monitorTimer;

        private string _currentDemoFileName;
        private Demo _demo;
        private int _currentIndex;
        private IntPtr _targetApplication;

        private TabPage _selectedTab;

        private bool _swapFlag;

        public MainForm()
        {
            InitializeComponent();

            _ghostKeyboard  = new GhostKeyboard(
                () => SetForegroundWindow(Handle),
                () => SetForegroundWindow(_targetApplication),
                index => tabControl.SelectedIndex = index,
                SwapForegroundWindows);

            foreach (var command in GhostKeyboard.Commands)
            {
                commandContextMenuStrip.Items.Add(command).Click += CommandMenuItemOnClick;
            }

            if (!Directory.Exists(Path.GetDirectoryName(_appDataPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_appDataPath));
            }

            _monitorTimer = new System.Threading.Timer(x => SetPictureBoxImage());
            _monitorTimer.Change(50, 50);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            LoadAppData();

            CreateNew();
        }

        private void NewToolStripMenuItemClick(object sender, EventArgs e)
        {
            CreateNew();
        }

        private void CreateNew()
        {
            _currentDemoFileName = null;
            Text = "New Demo - Ghost Writer";
            reloadDemoToolStripMenuItem.Visible = false;

            _demo = new Demo
            {
                Steps = new List<Step>
                {
                    new Step { Number = 1 }
                },
                InitialCode = ""
            };

            _currentIndex = 0;

            btnExecute.Enabled = btnFirst.Enabled = btnPrevious.Enabled = btnNext.Enabled = btnLast.Enabled = false;

            LoadCurrentStep();
        }

        private void OpenDemoToolStripMenuItemOnClick(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                AddExtension = true,
                CheckFileExists = true,
                DefaultExt = "demo",
                Filter = "Demo Files (*.demo)|*.demo",
                Multiselect = false,
                RestoreDirectory = true,
                SupportMultiDottedExtensions = true,
                Title = "Open Demo",
            };

            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                _currentDemoFileName = dialog.FileName;
                InitializeDemo();

                if (!openRecentToolStripMenuItem.DropDownItems.Cast<ToolStripItem>()
                    .Any(x => x.Text == _currentDemoFileName))
                {
                    var recentFileToolStripMenuItem = new ToolStripMenuItem(_currentDemoFileName);
                    recentFileToolStripMenuItem.Click += recentFileToolStripMenuItem_Click;
                    openRecentToolStripMenuItem.DropDownItems.Insert(0, recentFileToolStripMenuItem);
                    openRecentToolStripMenuItem.Visible = true;

                    SaveAppData();
                }
            }
        }

        private void InitializeDemo()
        {
            Text = Path.GetFileNameWithoutExtension(_currentDemoFileName) + " - Ghost Writer";
            reloadDemoToolStripMenuItem.Visible = true;

            LoadDemo(true);

            _currentIndex = 0;
            LoadCurrentStep();

            if (setInitialCodeOnLoadToolStripMenuItem.Checked)
            {
                if (_targetApplication != IntPtr.Zero)
                {
                    SetForegroundWindow(_targetApplication);
                    _ghostKeyboard.TypeRaw("^(a){DEL}" + GhostKeyboard.EscapeInput(_demo.InitialCode ?? ""));
                    SetForegroundWindow(Handle);
                }
            }
        }

        private void SaveDemoToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (_currentDemoFileName == null)
            {
                SaveAs();
            }
            else
            {
                Save(_currentDemoFileName);
            }
        }

        private void SaveAsToolStripMenuItemClick(object sender, EventArgs e)
        {
            SaveAs();
        }

        private void SaveAs()
        {
            var dialog = new SaveFileDialog
            {
                AddExtension = true,
                DefaultExt = "demo",
                Filter = "Demo Files (*.demo)|*.demo",
                OverwritePrompt = true,
                RestoreDirectory = true,
                SupportMultiDottedExtensions = true,
                Title = "Save Demo",
            };

            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                _currentDemoFileName = dialog.FileName;

                Save(_currentDemoFileName);
            }
        }

        private void Save(string fileName)
        {
            var serializer = new XmlSerializer(typeof(Demo));
            using (var writer = new StreamWriter(fileName))
            {
                serializer.Serialize(writer, _demo);
            }

            Text = Path.GetFileNameWithoutExtension(_currentDemoFileName) + " - Ghost Writer";
        }

        private void BeforeCurrentToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (_demo.Steps.Count > 0)
            {
                _demo.Steps.Insert(_currentIndex, new Step());
            }
            else
            {
                _demo.Steps.Add(new Step());
                _currentIndex = 0;
            }

            RenumberSteps();
            LoadCurrentStep();
        }

        private void AfterCurrentToolStripMenuItemClick(object sender, EventArgs e)
        {
            _demo.Steps.Add(new Step());
            _currentIndex++;

            RenumberSteps();
            LoadCurrentStep();
        }

        private void RemoveStepToolStripMenuItemClick(object sender, EventArgs e)
        {
            _demo.Steps.RemoveAt(_currentIndex);
            
            if (_currentIndex >= _demo.Steps.Count)
            {
                _currentIndex--;

                if (_demo.Steps.Count == 0)
                {
                    txtExpectedCode.Clear();
                    txtNotes.Clear();
                    txtGhostKeyboardData.Clear();
                }
                else
                {
                    RenumberSteps();
                    LoadCurrentStep();
                }
            }
            else
            {
                RenumberSteps();
                LoadCurrentStep();
            }
        }

        private void SetTargetApplicationToolStripMenuItemClick(object sender, EventArgs e)
        {
            var dialog = new SetTargetApplicationTitleDialog(_demo.TargetApplicationTitle);

            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                _demo.TargetApplicationTitle = dialog.TargetApplicationTitle;
                SetTargetApplication();
            }
        }

        private void SetTargetApplication()
        {
            var windows = GetTargetWindows();

            if (!windows.Any())
            {
                MessageBox.Show("No matching windows found.");
            }
            else if (windows.Count == 1)
            {
                _targetApplication = windows[0];
            }
            else
            {
                _targetApplication = IntPtr.Zero;

                MessageBox.Show(
                    "More than one window matches. Each one will be focused, then you will be asked if it is the desired window.");

                foreach (var window in windows)
                {
                    var sb = new StringBuilder();
                    var placement = new WindowPlacement();
                    GetWindowText(window, sb, sb.Capacity);
                    GetWindowPlacement(window, ref placement);
                    var state = placement.showCmd;
                    
                    if (state == ShowWindowCommands.ShowMinimized)
                    {
                        ShowWindow(window, ShowWindowCommands.ShowMaximized);
                    }

                    SetForegroundWindow(window);
                    if (MessageBox.Show("Is this the correct window?", "Select Target Application", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        _targetApplication = window;
                        break;
                    }

                    if (state == ShowWindowCommands.ShowMinimized)
                    {
                        ShowWindow(window, ShowWindowCommands.ShowMinimized);
                    }
                }

                if (_targetApplication == IntPtr.Zero)
                {
                    MessageBox.Show("No window was selected.");
                }
            }
        }

        private void RenumberSteps()
        {
            for (int i = 0; i < _demo.Steps.Count; i++)
            {
                _demo.Steps[i].Number = i + 1;
            }
        }

        private void TxtNotesTextChanged(object sender, EventArgs e)
        {
            _demo.Steps[_currentIndex].Notes = ((RichTextBox)sender).Rtf;
        }

        private void TxtExpectedCodeTextChanged(object sender, EventArgs e)
        {
            if (rbStartingCode.Checked)
            {
                if (_currentIndex == 0)
                {
                    _demo.InitialCode = txtExpectedCode.Text;
                }
                else
                {
                    _demo.Steps[_currentIndex - 1].FinishedCode = txtExpectedCode.Rtf;
                }
            }
            else
            {
                _demo.Steps[_currentIndex].FinishedCode = txtExpectedCode.Rtf;
            }
        }

        private void TxtGhostKeyboardDataTextChanged(object sender, EventArgs e)
        {
            _demo.Steps[_currentIndex].GhostKeyboardData = txtGhostKeyboardData.Text;
        }

        private void LoadDemo(bool setTargetApplication)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(Demo));
                using (var reader = new StreamReader(_currentDemoFileName))
                {
                    _demo = (Demo) serializer.Deserialize(reader);
                }

                if (setTargetApplication)
                {
                    SetTargetApplication();
                }

                _demo.InitialCode = System.Text.RegularExpressions.Regex.Replace(_demo.InitialCode, "(?!<\r)\n", "\r\n");

                btnExecute.Enabled = _demo.Steps.Count > 0 && _targetApplication != IntPtr.Zero;
                btnNext.Enabled = btnLast.Enabled = _demo.Steps.Count > 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open demo: " + ex.Message);
            }
        }

        private void ReloadDemoToolStripMenuItemClick(object sender, EventArgs e)
        {
            LoadDemo(false);
            LoadCurrentStep();
        }

        private void BtnExecuteClick(object sender, EventArgs e)
        {
            RECT rect;
            GetWindowRect(_targetApplication, out rect);

            Cursor.Position =
                new Point(
                    rect.Left + ((rect.Right - rect.Left) / 2),
                    rect.Top + ((rect.Bottom - rect.Top) / 2));

            SetForegroundWindow(_targetApplication);

            _ghostKeyboard.Type(_demo.Steps[_currentIndex].GhostKeyboardData);

            if (presentationModeToolStripMenuItem.Checked)
            {
                Cursor.Position =
                    new Point(Left + (Width / 2), Top + (Height / 2));

                SetForegroundWindow(Handle);
            }
        }

        private void SwapForegroundWindows()
        {
            SetForegroundWindow(_swapFlag ? Handle : _targetApplication);
            _swapFlag = !_swapFlag;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        private void BtnNextClick(object sender, EventArgs e)
        {
            _currentIndex++;
            LoadCurrentStep();
        }

        private void BtnPreviousClick(object sender, EventArgs e)
        {
            _currentIndex--;
            LoadCurrentStep();
        }

        private void BtnFirstClick(object sender, EventArgs e)
        {
            _currentIndex = 0;
            LoadCurrentStep();
        }

        private void BtnLastClick(object sender, EventArgs e)
        {
            _currentIndex = _demo.Steps.Count - 1;
            LoadCurrentStep();
        }

        private void NoSoundToolStripMenuItemCheckedChanged(object sender, EventArgs e)
        {
            Sound.Enabled = !noSoundToolStripMenuItem.Checked;
            SaveAppData();
        }

        private void NormalToolStripMenuItemCheckedChanged(object sender, EventArgs e)
        {
            if (normalToolStripMenuItem.Checked)
            {
                fastToolStripMenuItem.Checked = false;
                uncheckedToolStripMenuItem.Checked = false;

                _ghostKeyboard.DelayStrategy = DelayStrategy.Normal;
            }

            SaveAppData();
        }

        private void FastToolStripMenuItemCheckedChanged(object sender, EventArgs e)
        {
            if (fastToolStripMenuItem.Checked)
            {
                normalToolStripMenuItem.Checked = false;
                uncheckedToolStripMenuItem.Checked = false;

                _ghostKeyboard.DelayStrategy = DelayStrategy.Fast;
            }

            SaveAppData();
        }

        private void UncheckedToolStripMenuItemCheckedChanged(object sender, EventArgs e)
        {
            if (uncheckedToolStripMenuItem.Checked)
            {
                normalToolStripMenuItem.Checked = false;
                fastToolStripMenuItem.Checked = false;

                _ghostKeyboard.DelayStrategy = DelayStrategy.Unchecked;
            }

            SaveAppData();
        }

        private void HighlightToolStripMenuItemClick(object sender, EventArgs e)
        {
            var owner = highlightToolStripMenuItem.Owner as ContextMenuStrip;
            var textBox = owner.SourceControl as RichTextBox;
            textBox.SelectionBackColor = Color.Yellow;
        }

        private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            var textBox = (RichTextBox)sender;

            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.H)
            {
                textBox.SelectionBackColor = Color.Yellow;
            }
        }

        private void RemoveHighlightsToolStripMenuItemClick(object sender, EventArgs e)
        {
            var owner = highlightToolStripMenuItem.Owner as ContextMenuStrip;
            var textBox = owner.SourceControl as RichTextBox;
            textBox.SelectionBackColor = textBox.BackColor;

            if (ReferenceEquals(textBox, txtExpectedCode))
            {
                _demo.Steps[_currentIndex].FinishedCode = textBox.Rtf;
            }
            else if (ReferenceEquals(textBox, txtNotes))
            {
                _demo.Steps[_currentIndex].Notes = textBox.Rtf;
            }
        }

        private void CommandMenuItemOnClick(object sender, EventArgs eventArgs)
        {
            var command = ((ToolStripItem)sender).Text;

            var previous = Clipboard.GetText();
            Clipboard.SetText(command);
            SendKeys.SendWait("^(v)");
            Clipboard.SetDataObject(previous);
        }

        private void LoadCurrentStep()
        {
            LoadNotes();

            txtExpectedCode.TextChanged -= TxtExpectedCodeTextChanged;
            txtGhostKeyboardData.TextChanged -= TxtGhostKeyboardDataTextChanged;

            btnNext.Enabled = btnLast.Enabled = _currentIndex < _demo.Steps.Count - 1;
            btnPrevious.Enabled = btnFirst.Enabled = _currentIndex > 0;

            if (rbStartingCode.Checked)
            {
                if (_currentIndex == 0)
                {
                    txtExpectedCode.Text = _demo.InitialCode;
                }
                else
                {
                    txtExpectedCode.Rtf = _demo.Steps[_currentIndex - 1].FinishedCode;
                }
            }
            else
            {
                txtExpectedCode.Rtf = _demo.Steps[_currentIndex].FinishedCode;
            }

            txtGhostKeyboardData.Text = _demo.Steps[_currentIndex].GhostKeyboardData;
            lblStepNumberA.Text = "Step " + _demo.Steps[_currentIndex].Number;
            lblStepNumberB.Text = "Step " + _demo.Steps[_currentIndex].Number;

            txtExpectedCode.TextChanged += TxtExpectedCodeTextChanged;
            txtGhostKeyboardData.TextChanged += TxtGhostKeyboardDataTextChanged;
        }

        private void LoadNotes()
        {
            txtNotes.TextChanged -= TxtNotesTextChanged;
            txtNotes2.TextChanged -= TxtNotesTextChanged;

            txtNotes.Rtf = _demo.Steps[_currentIndex].Notes;
            txtNotes2.Rtf = _demo.Steps[_currentIndex].Notes;

            txtNotes.TextChanged += TxtNotesTextChanged;
            txtNotes2.TextChanged += TxtNotesTextChanged;
        }

        private IList<IntPtr> GetTargetWindows()
        {
            var windows = new List<IntPtr>();

            if (_demo.TargetApplicationTitle == null)
            {
                return windows;
            }

            var nChildHandle = GetWindow(GetDesktopWindow(), GwChild);

            while (nChildHandle != IntPtr.Zero)
            {
                // don't get this application...
                if (nChildHandle == Handle)
                {
                    nChildHandle = GetWindow(nChildHandle, GwHwndnext);
                }

                if (IsWindowVisible(nChildHandle))
                {
                    var sbTitle = new StringBuilder(1024);
                    GetWindowText(nChildHandle, sbTitle, sbTitle.Capacity);
                    String sWinTitle = sbTitle.ToString();
                    {
                        if (sWinTitle.ToLower().Contains(_demo.TargetApplicationTitle.ToLower()))
                        {
                            windows.Add(nChildHandle);
                        }
                    }
                }

                nChildHandle = GetWindow(nChildHandle, GwHwndnext);
            }

            return windows;
        }

        [DllImport("user32.dll", SetLastError = false)]
        static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetWindow(IntPtr hWnd, int uCmd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowPlacement(IntPtr hWnd, ref WindowPlacement lpwndpl);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ShowWindow(IntPtr hWnd, ShowWindowCommands nCmdShow);

        // ReSharper disable UnusedMember.Local
        // ReSharper disable UnassignedField.Local
        private struct WindowPlacement
        {
            public int length;
            public int flags;
            public ShowWindowCommands showCmd;
            public Point ptMinPosition;
            public Point ptMaxPosition;
            public Rectangle rcNormalPosition;
        }
        // ReSharper restore UnassignedField.Local
        // ReSharper restore UnusedMember.Local

        /// <summary>Enumeration of the different ways of showing a window using 
        /// ShowWindow</summary>
        private enum ShowWindowCommands : uint
        {
            ShowMinimized = 2,
            ShowMaximized = 3,
        }

        private const int GwHwndnext = 2;
        private const int GwChild = 5;

        private void rbCodeStartingOrFinished_CheckedChanged(object sender, EventArgs e)
        {
            if (rbStartingCode.Checked)
            {
                if (_currentIndex == 0)
                {
                    txtExpectedCode.Text = _demo.InitialCode;
                }
                else
                {
                    txtExpectedCode.Rtf = _demo.Steps[_currentIndex - 1].FinishedCode;
                }
            }
            else
            {
                txtExpectedCode.Rtf = _demo.Steps[_currentIndex].FinishedCode;
            }
        }

        private void TabControl_TabIndexChanged(object sender, EventArgs e)
        {
            LoadNotes();
        }

        private void BtnPushToApplication_Click(object sender, EventArgs e)
        {
            if (_targetApplication != IntPtr.Zero)
            {
                SetForegroundWindow(_targetApplication);
                Clipboard.SetText(txtExpectedCode.Text);
                _ghostKeyboard.TypeRaw("^(a){DEL}");
                Thread.Sleep(1000);
                _ghostKeyboard.TypeRaw("^(v)");
                SetForegroundWindow(Handle);
            }
        }

        public class AppData
        {
            public bool SetInitialCodeOnLoad { get; set; }
            public bool PresentationMode { get; set; }
            public bool MonitorApplication { get; set; }
            public bool NoSound { get; set; }
            public DelayStrategy DelayStrategy { get; set; }

            [XmlElement("RecentFile")]
            public List<string> RecentFiles { get; set; }
        }

        private void SaveAppData()
        {
            var appData = new AppData
            {
                RecentFiles = new List<string>()
            };
            
            appData.SetInitialCodeOnLoad = setInitialCodeOnLoadToolStripMenuItem.Checked;
            appData.PresentationMode = presentationModeToolStripMenuItem.Checked;
            appData.MonitorApplication = monitorApplicationToolStripMenuItem.Checked;
            appData.NoSound = noSoundToolStripMenuItem.Checked;

            if (normalToolStripMenuItem.Checked)
            {
                appData.DelayStrategy = DelayStrategy.Normal;
            }
            else if (fastToolStripMenuItem.Checked)
            {
                appData.DelayStrategy = DelayStrategy.Fast;
            }
            else
            {
                appData.DelayStrategy = DelayStrategy.Unchecked;
            }
            
            foreach (ToolStripItem item in openRecentToolStripMenuItem.DropDownItems)
            {
                appData.RecentFiles.Add(item.Text);
            }

            using (var writer = new StreamWriter(_appDataPath))
            {
                var serializer = new XmlSerializer(typeof(AppData));
                serializer.Serialize(writer, appData);
            }
        }

        private void LoadAppData()
        {
            AppData appData;

            if (!File.Exists(_appDataPath))
            {
                appData = new AppData
                {
                    SetInitialCodeOnLoad = true,
                    RecentFiles = new List<string>(),
                    PresentationMode = false
                };

                SaveAppData();
            }
            else
            {
                using (var reader = new StreamReader(_appDataPath))
                {
                    var serializer = new XmlSerializer(typeof(AppData));
                    appData = (AppData)serializer.Deserialize(reader);
                }
            }

            if (appData.RecentFiles.Count > 0)
            {
                foreach (ToolStripMenuItem recentFileToolStripMenuItem in openRecentToolStripMenuItem.DropDownItems)
                {
                    recentFileToolStripMenuItem.Click -= recentFileToolStripMenuItem_Click;
                }

                openRecentToolStripMenuItem.DropDownItems.Clear();

                foreach (var recentFile in appData.RecentFiles)
                {
                    var recentFileToolStripMenuItem = new ToolStripMenuItem(recentFile);
                    recentFileToolStripMenuItem.Click += recentFileToolStripMenuItem_Click;
                    openRecentToolStripMenuItem.DropDownItems.Add(recentFileToolStripMenuItem);
                }

                openRecentToolStripMenuItem.Visible = true;
            }
            else
            {
                openRecentToolStripMenuItem.Visible = false;
            }

            setInitialCodeOnLoadToolStripMenuItem.Checked = appData.SetInitialCodeOnLoad;
            presentationModeToolStripMenuItem.Checked = appData.PresentationMode;
            monitorApplicationToolStripMenuItem.Checked = appData.MonitorApplication;
            noSoundToolStripMenuItem.Checked = appData.NoSound;

            normalToolStripMenuItem.Checked = false;
            fastToolStripMenuItem.Checked = false;
            uncheckedToolStripMenuItem.Checked = false;

            switch (appData.DelayStrategy)
            {
                case DelayStrategy.Normal:
                    normalToolStripMenuItem.Checked = true;
                    break;
                case DelayStrategy.Fast:
                    fastToolStripMenuItem.Checked = true;
                    break;
                default:
                    uncheckedToolStripMenuItem.Checked = true;
                    break;
            }
        }

        private void recentFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _currentDemoFileName = ((ToolStripMenuItem)sender).Text;
            InitializeDemo();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.P:
                        tabControl.SelectedIndex = 0;
                        break;
                    case Keys.T:
                        tabControl.SelectedIndex = 1;
                        break;
                    case Keys.M:
                        tabControl.SelectedIndex = 2;
                        break;
                    default:
                        return;
                }

                e.Handled = true;
                e.SuppressKeyPress = true;
            }

            if (presentationModeToolStripMenuItem.Checked)
            {
                switch (e.KeyData)
                {
                    case Keys.Next:
                        if (btnNext.Enabled)
                        {
                            BtnNextClick(null, null);
                        }
                        break;
                    case Keys.PageUp:
                        if (btnPrevious.Enabled)
                        {
                            BtnPreviousClick(null, null);
                        }
                        break;
                    case Keys.F5:
                    case Keys.Escape:
                        if (btnExecute.Enabled)
                        {
                            BtnExecuteClick(null, null);
                        }
                        break;
                    //case Keys.OemPeriod:
                    //    break;
                }
            }
        }

        private void OptionToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            SaveAppData();
        }

        private volatile bool _running;

        private void SetPictureBoxImage()
        {
            if (monitorApplicationToolStripMenuItem.Checked
                && _selectedTab == tabAppMonitor)
            {
                if (_running)
                {
                    return;
                }

                _running = true;

                var currentImage = pictureBox.Image;

                Image nextImage = null;

                try
                {
                    nextImage = CaptureScreen.CaptureWithCursor(_targetApplication);
                }
                catch
                {
                }

                if (nextImage != null)
                {
                    pictureBox.Image = nextImage;
                }

                if (currentImage != null)
                {
                    try
                    {
                        currentImage.Dispose();
                    }
                    catch
                    {
                    }
                }

                _running = false;
            }
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedTab = tabControl.TabPages[tabControl.SelectedIndex];
        }
    }
}
