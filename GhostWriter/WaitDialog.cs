using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GhostWriter
{
    public partial class WaitDialog : Form
    {
        public WaitDialog()
        {
            InitializeComponent();
        }

        private void WaitDialog_KeyDown(object sender, KeyEventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
