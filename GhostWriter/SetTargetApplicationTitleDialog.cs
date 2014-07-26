using System;
using System.Windows.Forms;

namespace GhostWriter
{
    public partial class SetTargetApplicationTitleDialog : Form
    {
        public SetTargetApplicationTitleDialog(string currentTargetApplicationTitle)
        {
            InitializeComponent();

            txtTitle.Text = currentTargetApplicationTitle;
        }

        public string TargetApplicationTitle
        {
            get { return txtTitle.Text; }
        }

        private void BtnOkOnClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
