using System;
using System.Windows.Forms;

namespace GhostWriter
{
    public partial class BreakDialog : Form
    {
        public BreakDialog()
        {
            InitializeComponent();
        }

        public BreakDialogResult BreakDialogResult { get; private set; }

        private void btnGoFast_Click(object sender, EventArgs e)
        {
            BreakDialogResult = BreakDialogResult.GoFast;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnAbort_Click(object sender, EventArgs e)
        {
            BreakDialogResult = BreakDialogResult.Abort;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            BreakDialogResult = BreakDialogResult.Cancel;
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnGoToPresentationTab_Click(object sender, EventArgs e)
        {
            BreakDialogResult = BreakDialogResult.GoToPresentationTab;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnGoToAutoTypingTab_Click(object sender, EventArgs e)
        {
            BreakDialogResult = BreakDialogResult.GoToAutoTypingTab;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnGoToAppMonitorTab_Click(object sender, EventArgs e)
        {
            BreakDialogResult = BreakDialogResult.GoToAppMonitorTab;
            DialogResult = DialogResult.OK;
            Close();
        }
    }

    public enum BreakDialogResult
    {
        GoFast,
        Cancel,
        Abort,
        GoToPresentationTab,
        GoToAutoTypingTab,
        GoToAppMonitorTab,
    }
}
