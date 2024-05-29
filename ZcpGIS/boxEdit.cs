using MyMapObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ZcpGIS
{
    public partial class boxEdit : Form
    {
        public boxEdit()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        internal void GetData(out string modified)
        {
            modified = textBox1.Text;
        }
    }
}
