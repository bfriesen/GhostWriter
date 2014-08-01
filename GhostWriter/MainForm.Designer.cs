using System.Threading;

namespace GhostWriter
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);

            _ghostKeyboard.Dispose();
            _monitorTimer.Change(Timeout.Infinite, Timeout.Infinite);
            var waitHandle = new AutoResetEvent(false);
            _monitorTimer.Dispose(waitHandle);
            waitHandle.WaitOne();
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.MenuStrip menuStrip;
            System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem openDemoToolStripMenuItem;
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openRecentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadDemoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveDemoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addStepToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.beforeCurrentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.afterCurrentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeStepToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setTargetApplicationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noSoundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.typingSpeedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.normalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fastToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uncheckedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setInitialCodeOnLoadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.presentationModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.monitorApplicationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnExecute = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.highlightingContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.highlightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeHighlightsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnLast = new System.Windows.Forms.Button();
            this.btnFirst = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPresentation = new System.Windows.Forms.TabPage();
            this.lblStepNumberA = new System.Windows.Forms.Label();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.lblNotes = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.RichTextBox();
            this.btnPushToApplication = new System.Windows.Forms.Button();
            this.rbFinishedCode = new System.Windows.Forms.RadioButton();
            this.rbStartingCode = new System.Windows.Forms.RadioButton();
            this.txtExpectedCode = new System.Windows.Forms.RichTextBox();
            this.tabAutoTyping = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtNotes2 = new System.Windows.Forms.RichTextBox();
            this.lblNotes2 = new System.Windows.Forms.Label();
            this.lblKeyboardData = new System.Windows.Forms.Label();
            this.txtGhostKeyboardData = new System.Windows.Forms.RichTextBox();
            this.commandContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.lblStepNumberB = new System.Windows.Forms.Label();
            this.tabAppMonitor = new System.Windows.Forms.TabPage();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            menuStrip = new System.Windows.Forms.MenuStrip();
            fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            openDemoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            menuStrip.SuspendLayout();
            this.highlightingContextMenuStrip.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPresentation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.tabAutoTyping.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabAppMonitor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.optionsToolStripMenuItem});
            menuStrip.Location = new System.Drawing.Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new System.Drawing.Size(900, 24);
            menuStrip.TabIndex = 0;
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            openDemoToolStripMenuItem,
            this.openRecentToolStripMenuItem,
            this.reloadDemoToolStripMenuItem,
            this.saveDemoToolStripMenuItem,
            this.saveAsToolStripMenuItem});
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.NewToolStripMenuItemClick);
            // 
            // openDemoToolStripMenuItem
            // 
            openDemoToolStripMenuItem.Name = "openDemoToolStripMenuItem";
            openDemoToolStripMenuItem.ShortcutKeyDisplayString = "";
            openDemoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            openDemoToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            openDemoToolStripMenuItem.Text = "Open";
            openDemoToolStripMenuItem.Click += new System.EventHandler(this.OpenDemoToolStripMenuItemOnClick);
            // 
            // openRecentToolStripMenuItem
            // 
            this.openRecentToolStripMenuItem.Name = "openRecentToolStripMenuItem";
            this.openRecentToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.openRecentToolStripMenuItem.Text = "Open Recent";
            // 
            // reloadDemoToolStripMenuItem
            // 
            this.reloadDemoToolStripMenuItem.Name = "reloadDemoToolStripMenuItem";
            this.reloadDemoToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.reloadDemoToolStripMenuItem.Text = "Reload";
            this.reloadDemoToolStripMenuItem.Click += new System.EventHandler(this.ReloadDemoToolStripMenuItemClick);
            // 
            // saveDemoToolStripMenuItem
            // 
            this.saveDemoToolStripMenuItem.Name = "saveDemoToolStripMenuItem";
            this.saveDemoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveDemoToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.saveDemoToolStripMenuItem.Text = "Save";
            this.saveDemoToolStripMenuItem.Click += new System.EventHandler(this.SaveDemoToolStripMenuItemClick);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.SaveAsToolStripMenuItemClick);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addStepToolStripMenuItem,
            this.removeStepToolStripMenuItem,
            this.setTargetApplicationToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // addStepToolStripMenuItem
            // 
            this.addStepToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.beforeCurrentToolStripMenuItem,
            this.afterCurrentToolStripMenuItem});
            this.addStepToolStripMenuItem.Name = "addStepToolStripMenuItem";
            this.addStepToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.addStepToolStripMenuItem.Text = "Add Step";
            // 
            // beforeCurrentToolStripMenuItem
            // 
            this.beforeCurrentToolStripMenuItem.Name = "beforeCurrentToolStripMenuItem";
            this.beforeCurrentToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.beforeCurrentToolStripMenuItem.Text = "Before current";
            this.beforeCurrentToolStripMenuItem.Click += new System.EventHandler(this.BeforeCurrentToolStripMenuItemClick);
            // 
            // afterCurrentToolStripMenuItem
            // 
            this.afterCurrentToolStripMenuItem.Name = "afterCurrentToolStripMenuItem";
            this.afterCurrentToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.afterCurrentToolStripMenuItem.Text = "After current";
            this.afterCurrentToolStripMenuItem.Click += new System.EventHandler(this.AfterCurrentToolStripMenuItemClick);
            // 
            // removeStepToolStripMenuItem
            // 
            this.removeStepToolStripMenuItem.Name = "removeStepToolStripMenuItem";
            this.removeStepToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.removeStepToolStripMenuItem.Text = "Remove Step";
            this.removeStepToolStripMenuItem.Click += new System.EventHandler(this.RemoveStepToolStripMenuItemClick);
            // 
            // setTargetApplicationToolStripMenuItem
            // 
            this.setTargetApplicationToolStripMenuItem.Name = "setTargetApplicationToolStripMenuItem";
            this.setTargetApplicationToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.setTargetApplicationToolStripMenuItem.Text = "Set target application";
            this.setTargetApplicationToolStripMenuItem.Click += new System.EventHandler(this.SetTargetApplicationToolStripMenuItemClick);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.noSoundToolStripMenuItem,
            this.typingSpeedToolStripMenuItem,
            this.setInitialCodeOnLoadToolStripMenuItem,
            this.presentationModeToolStripMenuItem,
            this.monitorApplicationToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // noSoundToolStripMenuItem
            // 
            this.noSoundToolStripMenuItem.CheckOnClick = true;
            this.noSoundToolStripMenuItem.Name = "noSoundToolStripMenuItem";
            this.noSoundToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.noSoundToolStripMenuItem.Text = "No Sound";
            this.noSoundToolStripMenuItem.CheckedChanged += new System.EventHandler(this.NoSoundToolStripMenuItemCheckedChanged);
            // 
            // typingSpeedToolStripMenuItem
            // 
            this.typingSpeedToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.normalToolStripMenuItem,
            this.fastToolStripMenuItem,
            this.uncheckedToolStripMenuItem});
            this.typingSpeedToolStripMenuItem.Name = "typingSpeedToolStripMenuItem";
            this.typingSpeedToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.typingSpeedToolStripMenuItem.Text = "Typing Speed";
            // 
            // normalToolStripMenuItem
            // 
            this.normalToolStripMenuItem.Checked = true;
            this.normalToolStripMenuItem.CheckOnClick = true;
            this.normalToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.normalToolStripMenuItem.Name = "normalToolStripMenuItem";
            this.normalToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.normalToolStripMenuItem.Text = "Normal";
            this.normalToolStripMenuItem.CheckedChanged += new System.EventHandler(this.NormalToolStripMenuItemCheckedChanged);
            // 
            // fastToolStripMenuItem
            // 
            this.fastToolStripMenuItem.CheckOnClick = true;
            this.fastToolStripMenuItem.Name = "fastToolStripMenuItem";
            this.fastToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.fastToolStripMenuItem.Text = "Fast";
            this.fastToolStripMenuItem.CheckedChanged += new System.EventHandler(this.FastToolStripMenuItemCheckedChanged);
            // 
            // uncheckedToolStripMenuItem
            // 
            this.uncheckedToolStripMenuItem.CheckOnClick = true;
            this.uncheckedToolStripMenuItem.Name = "uncheckedToolStripMenuItem";
            this.uncheckedToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.uncheckedToolStripMenuItem.Text = "Unchecked";
            this.uncheckedToolStripMenuItem.CheckedChanged += new System.EventHandler(this.UncheckedToolStripMenuItemCheckedChanged);
            // 
            // setInitialCodeOnLoadToolStripMenuItem
            // 
            this.setInitialCodeOnLoadToolStripMenuItem.Checked = true;
            this.setInitialCodeOnLoadToolStripMenuItem.CheckOnClick = true;
            this.setInitialCodeOnLoadToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.setInitialCodeOnLoadToolStripMenuItem.Name = "setInitialCodeOnLoadToolStripMenuItem";
            this.setInitialCodeOnLoadToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.setInitialCodeOnLoadToolStripMenuItem.Text = "Set initial code on Load";
            this.setInitialCodeOnLoadToolStripMenuItem.CheckedChanged += new System.EventHandler(this.OptionToolStripMenuItem_CheckedChanged);
            // 
            // presentationModeToolStripMenuItem
            // 
            this.presentationModeToolStripMenuItem.CheckOnClick = true;
            this.presentationModeToolStripMenuItem.Name = "presentationModeToolStripMenuItem";
            this.presentationModeToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.presentationModeToolStripMenuItem.Text = "Presentation Mode";
            this.presentationModeToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.OptionToolStripMenuItem_CheckedChanged);
            // 
            // monitorApplicationToolStripMenuItem
            // 
            this.monitorApplicationToolStripMenuItem.CheckOnClick = true;
            this.monitorApplicationToolStripMenuItem.Name = "monitorApplicationToolStripMenuItem";
            this.monitorApplicationToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.monitorApplicationToolStripMenuItem.Text = "Monitor Application";
            // 
            // btnExecute
            // 
            this.btnExecute.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnExecute.Enabled = false;
            this.btnExecute.Location = new System.Drawing.Point(413, 585);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(75, 23);
            this.btnExecute.TabIndex = 2;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.BtnExecuteClick);
            // 
            // btnNext
            // 
            this.btnNext.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnNext.Enabled = false;
            this.btnNext.Location = new System.Drawing.Point(494, 585);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 3;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.BtnNextClick);
            // 
            // btnPrevious
            // 
            this.btnPrevious.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnPrevious.Enabled = false;
            this.btnPrevious.Location = new System.Drawing.Point(332, 585);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(75, 23);
            this.btnPrevious.TabIndex = 1;
            this.btnPrevious.Text = "Previous";
            this.btnPrevious.UseVisualStyleBackColor = true;
            this.btnPrevious.Click += new System.EventHandler(this.BtnPreviousClick);
            // 
            // highlightingContextMenuStrip
            // 
            this.highlightingContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.highlightToolStripMenuItem,
            this.removeHighlightsToolStripMenuItem});
            this.highlightingContextMenuStrip.Name = "contextMenuStrip";
            this.highlightingContextMenuStrip.Size = new System.Drawing.Size(176, 48);
            // 
            // highlightToolStripMenuItem
            // 
            this.highlightToolStripMenuItem.Name = "highlightToolStripMenuItem";
            this.highlightToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.highlightToolStripMenuItem.Text = "Highlight";
            this.highlightToolStripMenuItem.Click += new System.EventHandler(this.HighlightToolStripMenuItemClick);
            // 
            // removeHighlightsToolStripMenuItem
            // 
            this.removeHighlightsToolStripMenuItem.Name = "removeHighlightsToolStripMenuItem";
            this.removeHighlightsToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.removeHighlightsToolStripMenuItem.Text = "Remove Highlights";
            this.removeHighlightsToolStripMenuItem.Click += new System.EventHandler(this.RemoveHighlightsToolStripMenuItemClick);
            // 
            // btnLast
            // 
            this.btnLast.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnLast.Enabled = false;
            this.btnLast.Location = new System.Drawing.Point(575, 585);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(75, 23);
            this.btnLast.TabIndex = 5;
            this.btnLast.Text = "Last";
            this.btnLast.UseVisualStyleBackColor = true;
            this.btnLast.Click += new System.EventHandler(this.BtnLastClick);
            // 
            // btnFirst
            // 
            this.btnFirst.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnFirst.Enabled = false;
            this.btnFirst.Location = new System.Drawing.Point(251, 585);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(75, 23);
            this.btnFirst.TabIndex = 6;
            this.btnFirst.Text = "First";
            this.btnFirst.UseVisualStyleBackColor = true;
            this.btnFirst.Click += new System.EventHandler(this.BtnFirstClick);
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabPresentation);
            this.tabControl.Controls.Add(this.tabAutoTyping);
            this.tabControl.Controls.Add(this.tabAppMonitor);
            this.tabControl.Location = new System.Drawing.Point(0, 27);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(900, 552);
            this.tabControl.TabIndex = 7;
            this.toolTip.SetToolTip(this.tabControl, "Presentation Notes");
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            this.tabControl.TabIndexChanged += new System.EventHandler(this.TabControl_TabIndexChanged);
            // 
            // tabPresentation
            // 
            this.tabPresentation.Controls.Add(this.lblStepNumberA);
            this.tabPresentation.Controls.Add(this.splitContainer);
            this.tabPresentation.Location = new System.Drawing.Point(4, 22);
            this.tabPresentation.Name = "tabPresentation";
            this.tabPresentation.Padding = new System.Windows.Forms.Padding(3);
            this.tabPresentation.Size = new System.Drawing.Size(892, 526);
            this.tabPresentation.TabIndex = 0;
            this.tabPresentation.Text = "Presentation";
            this.tabPresentation.UseVisualStyleBackColor = true;
            // 
            // lblStepNumberA
            // 
            this.lblStepNumberA.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStepNumberA.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStepNumberA.Location = new System.Drawing.Point(3, 3);
            this.lblStepNumberA.Name = "lblStepNumberA";
            this.lblStepNumberA.Size = new System.Drawing.Size(886, 20);
            this.lblStepNumberA.TabIndex = 1;
            // 
            // splitContainer
            // 
            this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer.Location = new System.Drawing.Point(-4, 26);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.lblNotes);
            this.splitContainer.Panel1.Controls.Add(this.txtNotes);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.btnPushToApplication);
            this.splitContainer.Panel2.Controls.Add(this.rbFinishedCode);
            this.splitContainer.Panel2.Controls.Add(this.rbStartingCode);
            this.splitContainer.Panel2.Controls.Add(this.txtExpectedCode);
            this.splitContainer.Size = new System.Drawing.Size(900, 504);
            this.splitContainer.SplitterDistance = 252;
            this.splitContainer.TabIndex = 5;
            // 
            // lblNotes
            // 
            this.lblNotes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNotes.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNotes.Location = new System.Drawing.Point(10, 3);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(889, 20);
            this.lblNotes.TabIndex = 6;
            this.lblNotes.Text = "Notes";
            // 
            // txtNotes
            // 
            this.txtNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNotes.ContextMenuStrip = this.highlightingContextMenuStrip;
            this.txtNotes.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNotes.Location = new System.Drawing.Point(5, 26);
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(891, 224);
            this.txtNotes.TabIndex = 0;
            this.txtNotes.Text = "";
            this.toolTip.SetToolTip(this.txtNotes, "Presentation Notes");
            this.txtNotes.TextChanged += new System.EventHandler(this.TxtNotesTextChanged);
            this.txtNotes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnTextBoxKeyDown);
            // 
            // btnPushToApplication
            // 
            this.btnPushToApplication.Location = new System.Drawing.Point(262, 0);
            this.btnPushToApplication.Name = "btnPushToApplication";
            this.btnPushToApplication.Size = new System.Drawing.Size(106, 23);
            this.btnPushToApplication.TabIndex = 8;
            this.btnPushToApplication.Text = "Push to Application";
            this.btnPushToApplication.UseVisualStyleBackColor = true;
            this.btnPushToApplication.Click += new System.EventHandler(this.BtnPushToApplication_Click);
            // 
            // rbFinishedCode
            // 
            this.rbFinishedCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbFinishedCode.Location = new System.Drawing.Point(135, 0);
            this.rbFinishedCode.Name = "rbFinishedCode";
            this.rbFinishedCode.Size = new System.Drawing.Size(121, 23);
            this.rbFinishedCode.TabIndex = 2;
            this.rbFinishedCode.Text = "Finished Code";
            this.rbFinishedCode.UseVisualStyleBackColor = true;
            // 
            // rbStartingCode
            // 
            this.rbStartingCode.Checked = true;
            this.rbStartingCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbStartingCode.Location = new System.Drawing.Point(13, 0);
            this.rbStartingCode.Name = "rbStartingCode";
            this.rbStartingCode.Size = new System.Drawing.Size(116, 23);
            this.rbStartingCode.TabIndex = 1;
            this.rbStartingCode.TabStop = true;
            this.rbStartingCode.Text = "Starting Code";
            this.rbStartingCode.UseVisualStyleBackColor = true;
            this.rbStartingCode.CheckedChanged += new System.EventHandler(this.rbCodeStartingOrFinished_CheckedChanged);
            // 
            // txtExpectedCode
            // 
            this.txtExpectedCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtExpectedCode.ContextMenuStrip = this.highlightingContextMenuStrip;
            this.txtExpectedCode.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtExpectedCode.Location = new System.Drawing.Point(5, 29);
            this.txtExpectedCode.Name = "txtExpectedCode";
            this.txtExpectedCode.Size = new System.Drawing.Size(891, 215);
            this.txtExpectedCode.TabIndex = 0;
            this.txtExpectedCode.Text = "";
            this.toolTip.SetToolTip(this.txtExpectedCode, "Code after this step has executed");
            this.txtExpectedCode.TextChanged += new System.EventHandler(this.TxtExpectedCodeTextChanged);
            this.txtExpectedCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnTextBoxKeyDown);
            // 
            // tabAutoTyping
            // 
            this.tabAutoTyping.Controls.Add(this.splitContainer1);
            this.tabAutoTyping.Controls.Add(this.lblStepNumberB);
            this.tabAutoTyping.Location = new System.Drawing.Point(4, 22);
            this.tabAutoTyping.Name = "tabAutoTyping";
            this.tabAutoTyping.Padding = new System.Windows.Forms.Padding(3);
            this.tabAutoTyping.Size = new System.Drawing.Size(892, 526);
            this.tabAutoTyping.TabIndex = 1;
            this.tabAutoTyping.Text = "Auto-Typing";
            this.tabAutoTyping.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(-4, 26);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtNotes2);
            this.splitContainer1.Panel1.Controls.Add(this.lblNotes2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lblKeyboardData);
            this.splitContainer1.Panel2.Controls.Add(this.txtGhostKeyboardData);
            this.splitContainer1.Size = new System.Drawing.Size(900, 504);
            this.splitContainer1.SplitterDistance = 252;
            this.splitContainer1.TabIndex = 6;
            // 
            // txtNotes2
            // 
            this.txtNotes2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNotes2.ContextMenuStrip = this.highlightingContextMenuStrip;
            this.txtNotes2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNotes2.Location = new System.Drawing.Point(5, 26);
            this.txtNotes2.Name = "txtNotes2";
            this.txtNotes2.Size = new System.Drawing.Size(891, 224);
            this.txtNotes2.TabIndex = 8;
            this.txtNotes2.Text = "";
            this.toolTip.SetToolTip(this.txtNotes2, "Presentation Notes");
            this.txtNotes2.TextChanged += new System.EventHandler(this.TxtNotesTextChanged);
            // 
            // lblNotes2
            // 
            this.lblNotes2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNotes2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNotes2.Location = new System.Drawing.Point(10, 3);
            this.lblNotes2.Name = "lblNotes2";
            this.lblNotes2.Size = new System.Drawing.Size(887, 20);
            this.lblNotes2.TabIndex = 7;
            this.lblNotes2.Text = "Notes";
            // 
            // lblKeyboardData
            // 
            this.lblKeyboardData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblKeyboardData.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKeyboardData.Location = new System.Drawing.Point(13, 3);
            this.lblKeyboardData.Name = "lblKeyboardData";
            this.lblKeyboardData.Size = new System.Drawing.Size(880, 20);
            this.lblKeyboardData.TabIndex = 8;
            this.lblKeyboardData.Text = "Keyboard Data";
            // 
            // txtGhostKeyboardData
            // 
            this.txtGhostKeyboardData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGhostKeyboardData.ContextMenuStrip = this.commandContextMenuStrip;
            this.txtGhostKeyboardData.Font = new System.Drawing.Font("Consolas", 12F);
            this.txtGhostKeyboardData.Location = new System.Drawing.Point(5, 29);
            this.txtGhostKeyboardData.Name = "txtGhostKeyboardData";
            this.txtGhostKeyboardData.Size = new System.Drawing.Size(891, 215);
            this.txtGhostKeyboardData.TabIndex = 2;
            this.txtGhostKeyboardData.Text = "";
            this.toolTip.SetToolTip(this.txtGhostKeyboardData, "Ghost Keyboard Data");
            this.txtGhostKeyboardData.TextChanged += new System.EventHandler(this.TxtGhostKeyboardDataTextChanged);
            // 
            // commandContextMenuStrip
            // 
            this.commandContextMenuStrip.Name = "commandContextMenuStrip";
            this.commandContextMenuStrip.Size = new System.Drawing.Size(61, 4);
            // 
            // lblStepNumberB
            // 
            this.lblStepNumberB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStepNumberB.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStepNumberB.Location = new System.Drawing.Point(3, 3);
            this.lblStepNumberB.Name = "lblStepNumberB";
            this.lblStepNumberB.Size = new System.Drawing.Size(886, 20);
            this.lblStepNumberB.TabIndex = 2;
            // 
            // tabAppMonitor
            // 
            this.tabAppMonitor.Controls.Add(this.pictureBox);
            this.tabAppMonitor.Location = new System.Drawing.Point(4, 22);
            this.tabAppMonitor.Name = "tabAppMonitor";
            this.tabAppMonitor.Padding = new System.Windows.Forms.Padding(3);
            this.tabAppMonitor.Size = new System.Drawing.Size(892, 526);
            this.tabAppMonitor.TabIndex = 2;
            this.tabAppMonitor.Text = "App Monitor";
            this.tabAppMonitor.UseVisualStyleBackColor = true;
            // 
            // pictureBox
            // 
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(3, 3);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(886, 520);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 620);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.btnFirst);
            this.Controls.Add(this.btnLast);
            this.Controls.Add(this.btnPrevious);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnExecute);
            this.Controls.Add(menuStrip);
            this.KeyPreview = true;
            this.MainMenuStrip = menuStrip;
            this.Name = "MainForm";
            this.Text = "Ghost Writer";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            this.highlightingContextMenuStrip.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPresentation.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.tabAutoTyping.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabAppMonitor.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.ToolStripMenuItem reloadDemoToolStripMenuItem;
        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.Button btnFirst;
        private System.Windows.Forms.ContextMenuStrip highlightingContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem highlightToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeHighlightsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveDemoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addStepToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem beforeCurrentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem afterCurrentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeStepToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPresentation;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Label lblStepNumberA;
        private System.Windows.Forms.RichTextBox txtNotes;
        private System.Windows.Forms.RichTextBox txtExpectedCode;
        private System.Windows.Forms.TabPage tabAutoTyping;
        private System.Windows.Forms.Label lblStepNumberB;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ContextMenuStrip commandContextMenuStrip;
        private System.Windows.Forms.RichTextBox txtGhostKeyboardData;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem noSoundToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem typingSpeedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem normalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fastToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uncheckedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setInitialCodeOnLoadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setTargetApplicationToolStripMenuItem;
        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.RadioButton rbFinishedCode;
        private System.Windows.Forms.RadioButton rbStartingCode;
        private System.Windows.Forms.Label lblNotes2;
        private System.Windows.Forms.RichTextBox txtNotes2;
        private System.Windows.Forms.Label lblKeyboardData;
        private System.Windows.Forms.Button btnPushToApplication;
        private System.Windows.Forms.ToolStripMenuItem openRecentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem presentationModeToolStripMenuItem;
        private System.Windows.Forms.TabPage tabAppMonitor;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.ToolStripMenuItem monitorApplicationToolStripMenuItem;

    }
}

