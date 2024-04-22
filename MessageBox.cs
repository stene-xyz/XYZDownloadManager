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
    public partial class MessageBox : Form
    {
        public MessageBox()
        {
            InitializeComponent();
        }

        public static void Error(string message)
        {
            MessageBox errorDialog = new MessageBox();
            errorDialog.SetTitle("An error has occurred");
            errorDialog.SetMessage(message);
            errorDialog.ShowDialog();
        }

        public static void Info(string message)
        {
            MessageBox errorDialog = new MessageBox();
            errorDialog.SetTitle("Information");
            errorDialog.SetMessage(message);
            errorDialog.ShowDialog();
        }

        public static void Warn(string message)
        {
            MessageBox errorDialog = new MessageBox();
            errorDialog.SetTitle("Warning");
            errorDialog.SetMessage(message);
            errorDialog.ShowDialog();
        }

        public void SetTitle(string title)
        {
            Text = title;
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
