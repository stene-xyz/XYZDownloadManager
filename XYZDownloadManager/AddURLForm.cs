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
    public partial class AddURLForm : Form
    {
        // TODO: this sucks. use events?
        public string url;
        public bool finished;

        public AddURLForm()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            url = "";
            finished = true;
        }

        private void addURL_Click(object sender, EventArgs e)
        {
            url = urlBox.Text;
            finished = true;
            Close();
        }
    }
}
