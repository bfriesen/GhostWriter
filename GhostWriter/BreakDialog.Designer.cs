namespace GhostWriter
{
    partial class BreakDialog
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
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnGoFast = new System.Windows.Forms.Button();
            this.btnAbort = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnGoToPresentationTab = new System.Windows.Forms.Button();
            this.btnGoToAutoTypingTab = new System.Windows.Forms.Button();
            this.btnGoToAppMonitorTab = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnGoFast
            // 
            this.btnGoFast.Location = new System.Drawing.Point(12, 12);
            this.btnGoFast.Name = "btnGoFast";
            this.btnGoFast.Size = new System.Drawing.Size(75, 23);
            this.btnGoFast.TabIndex = 0;
            this.btnGoFast.Text = "Go Fast!";
            this.btnGoFast.UseVisualStyleBackColor = true;
            this.btnGoFast.Click += new System.EventHandler(this.btnGoFast_Click);
            // 
            // btnAbort
            // 
            this.btnAbort.Location = new System.Drawing.Point(93, 12);
            this.btnAbort.Name = "btnAbort";
            this.btnAbort.Size = new System.Drawing.Size(75, 23);
            this.btnAbort.TabIndex = 1;
            this.btnAbort.Text = "Abort!";
            this.btnAbort.UseVisualStyleBackColor = true;
            this.btnAbort.Click += new System.EventHandler(this.btnAbort_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(174, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnGoToPresentationTab
            // 
            this.btnGoToPresentationTab.Location = new System.Drawing.Point(12, 41);
            this.btnGoToPresentationTab.Name = "btnGoToPresentationTab";
            this.btnGoToPresentationTab.Size = new System.Drawing.Size(75, 51);
            this.btnGoToPresentationTab.TabIndex = 3;
            this.btnGoToPresentationTab.Text = "Go to Presentation Tab";
            this.btnGoToPresentationTab.UseVisualStyleBackColor = true;
            this.btnGoToPresentationTab.Click += new System.EventHandler(this.btnGoToPresentationTab_Click);
            // 
            // btnGoToAutoTypingTab
            // 
            this.btnGoToAutoTypingTab.Location = new System.Drawing.Point(93, 41);
            this.btnGoToAutoTypingTab.Name = "btnGoToAutoTypingTab";
            this.btnGoToAutoTypingTab.Size = new System.Drawing.Size(75, 51);
            this.btnGoToAutoTypingTab.TabIndex = 4;
            this.btnGoToAutoTypingTab.Text = "Go to Auto-Typing Tab";
            this.btnGoToAutoTypingTab.UseVisualStyleBackColor = true;
            this.btnGoToAutoTypingTab.Click += new System.EventHandler(this.btnGoToAutoTypingTab_Click);
            // 
            // btnGoToAppMonitorTab
            // 
            this.btnGoToAppMonitorTab.Location = new System.Drawing.Point(174, 41);
            this.btnGoToAppMonitorTab.Name = "btnGoToAppMonitorTab";
            this.btnGoToAppMonitorTab.Size = new System.Drawing.Size(75, 51);
            this.btnGoToAppMonitorTab.TabIndex = 5;
            this.btnGoToAppMonitorTab.Text = "Go to App Monitor Tab";
            this.btnGoToAppMonitorTab.UseVisualStyleBackColor = true;
            this.btnGoToAppMonitorTab.Click += new System.EventHandler(this.btnGoToAppMonitorTab_Click);
            // 
            // BreakDialog
            // 
            this.AcceptButton = this.btnGoFast;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(261, 104);
            this.Controls.Add(this.btnGoToAppMonitorTab);
            this.Controls.Add(this.btnGoToAutoTypingTab);
            this.Controls.Add(this.btnGoToPresentationTab);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAbort);
            this.Controls.Add(this.btnGoFast);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "BreakDialog";
            this.Text = "Break!";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnGoFast;
        private System.Windows.Forms.Button btnAbort;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnGoToPresentationTab;
        private System.Windows.Forms.Button btnGoToAutoTypingTab;
        private System.Windows.Forms.Button btnGoToAppMonitorTab;
    }
}