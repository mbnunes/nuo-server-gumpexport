using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace NUOServerExport
{
    public partial class NUOServerExportForm : Form
    {
        private NUOServerExport uphpExport;

        public NUOServerExportForm(NUOServerExport NUOServerExport)
        {
            this.InitializeComponent();
            this.uphpExport = NUOServerExport;
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btn_Export_Click(object sender, EventArgs e)
        {
            this.uphpExport.OnExportNUOServer(sender, e, (int)numX.Value, (int)numY.Value, chkNoDispose.Checked, chkNoClose.Checked, chkNoMove.Checked, txt_GumpName.Text);
        }
    }
}
