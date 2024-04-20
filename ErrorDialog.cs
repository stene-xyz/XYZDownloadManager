using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XYZDownloadManager
{
    public partial class ErrorDialog : Form
    {
        public ErrorDialog()
        {
            InitializeComponent();
        }

        public void SetMessage(string message)
        {
            errorText.Text = message;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
