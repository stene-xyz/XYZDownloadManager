using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Cryptography.Pkcs;
using System.Security.Policy;

namespace XYZDownloadManager
{
    public partial class DownloadListForm : Form
    {
        private DownloadManager downloadManager = new DownloadManager();

        public DownloadListForm()
        {
            InitializeComponent();

            // Load URL list from disk
            string[] urlList = FileList.Load();
            foreach (string url in urlList)
            {
                downloadListView.Items.Add(url);
                downloadManager.AddURL(url);
            }

            // RefreshTimer is used to refresh the download progress and check for finished downloads
            refreshTimer.Enabled = true;
        }

        public void DeleteURL(string url)
        {
            // Pull the URL from both the download manager and the URL list
            downloadManager.CancelURL(url);

            // Without this check the program can crash or delete the wrong download from the list
            int urlIndex = downloadListView.Items.IndexOfKey(url);
            if(urlIndex != -1)
            {
                downloadListView.Items.RemoveAt(urlIndex);
            }

            // Save the file list
            FileList.Save(downloadManager.GetUrls());
        }

        private void aboutButton_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.Show();
        }

        private void addURLButton_Click(object sender, EventArgs e)
        {
            AddURLForm addURLForm = new AddURLForm();
            addURLForm.ShowDialog();

            // Wait for URL to be chosen
            // TODO: this sucks
            while (!addURLForm.finished) { }
            if (!addURLForm.url.Equals(""))
            {
                downloadListView.Items.Add(addURLForm.url);
                downloadManager.AddURL(addURLForm.url);
                FileList.Save(downloadManager.GetUrls());
            }
        }

        private void downloadListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Fix a crash when trying to get download progress for blank entry
            // (and clean up interface)
            if (downloadListView.SelectedIndices.Count == 0)
            {
                progressText.Text = "No File Selected.";
                progressBar.Value = 0;
                return;
            }

            string url = downloadListView.Items[downloadListView.SelectedIndices[0]].Text;
            ByteCount count = downloadManager.GetProgress(url);

            progressText.Text = count.ToString();
            progressBar.Value = count.Percent();
        }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            // Refresh the progress bar
            // TODO: is this causing the "show progress on hover" bug (and should that be a feature?)
            downloadListView_SelectedIndexChanged(null, null);

            // Cleanup finished downloads
            foreach(string url in downloadManager.GetUrls())
            {

                if (downloadManager.GetProgress(url).Complete())
                {
                    DeleteURL(url);
                }
            }

            // Cleanup orphaned list entries
            foreach(ListViewItem url in downloadListView.Items)
            {
                if(!downloadManager.Exists(url.Text))
                {
                    downloadListView.Items.Remove(url);
                }
            }
        }

        private void deleteURLButton_Click(object sender, EventArgs e)
        {
            // Don't delete a blank entry (bad things happen!)
            if (downloadListView.SelectedIndices.Count == 0) return;

            string url = downloadListView.Items[downloadListView.SelectedIndices[0]].Text;
            DeleteURL(url);
        }
    }
}
