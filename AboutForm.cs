using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XYZDownloadManager
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void sourceCodeButton_Click(object sender, EventArgs e)
        {
            var startInfo = new ProcessStartInfo("https://github.com/stene-xyz/XYZDownloadManager");
            startInfo.UseShellExecute = true;

            Process.Start(startInfo);
        }
    }
}
