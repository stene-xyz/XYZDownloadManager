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
        private ConcurrentDictionary<string, string> urls = new ConcurrentDictionary<string, string>();

        public DownloadListForm()
        {
            InitializeComponent();

            // This function is called whenever a download task reports a status update
            // This saves the current progress to the URL list
            downloadManager.Progress += (url, received, total) =>
            {
                urls[url] = $"{received}/{total}";
            };

            // Load URL list from disk
            string[] urlList = FileList.Load();
            foreach (string url in urlList)
            {
                AddURL(url, false);
            }

            // RefreshTimer is used to refresh the download progress and check for finished downloads
            refreshTimer.Enabled = true;
        }

        private void AddURL(string url, bool save)
        {
            // Check to make sure file not already saved on disk
            string userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string downloadsPath = Path.Combine(userPath, "Downloads");
            string filename = Path.Combine(downloadsPath, Path.GetFileName(url));

            if (Path.Exists(filename))
            {
                MessageBox.Error($"File already exists in {downloadsPath}");
                return;
            }

            // Make sure we're not currently downloading this URL
            if (urls.ContainsKey(url))
            {
                MessageBox.Error("URL already in download list");
                return;
            }

            // Add the URL to the download list
            urls[url] = "0/0";
            downloadListView.Items.Add(url);
            downloadManager.AddURL(url);

            // save will be false if we're loading from disk currently
            // (as we don't want to overwrite the URL list)
            if(save)
            {
                FileList.Save(urls);
            }
        }

        public void DeleteURL(string url)
        {
            // Pull the URL from both the download manager and the URL list
            downloadManager.DeleteURL(url);
            urls.TryRemove(new KeyValuePair<string, string>(url, urls[url]));

            // Without this check the program can crash or delete the wrong download from the list
            int urlIndex = downloadListView.Items.IndexOfKey(url);
            if(urlIndex != -1)
            {
                downloadListView.Items.RemoveAt(urlIndex);
            }

            // Save the file list
            FileList.Save(urls);
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
                AddURL(addURLForm.url, true);
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

            // This solves a crash when the selected item is the same as an item we've just deleted
            // (like when the currently selected download finishes)
            if (urls.ContainsKey(downloadListView.Items[downloadListView.SelectedIndices[0]].Text))
            {
                progressText.Text = urls[downloadListView.Items[downloadListView.SelectedIndices[0]].Text];
                progressBar.Maximum = int.Parse(progressText.Text.Split("/")[1]);
                progressBar.Value = int.Parse(progressText.Text.Split("/")[0]);
            }
        }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            // Refresh the progress bar
            // TODO: is this causing the "show progress on hover" bug (and should that be a feature?)
            downloadListView_SelectedIndexChanged(null, null);

            // Cleanup finished downloads
            foreach(string url in urls.Keys)
            {
                // TODO: probably shouldn't be parsing these longs so much
                // will fix when URL management moves to the DownloadManager class
                if (long.Parse(urls[url].Split("/")[0]) == 0) continue;

                if (long.Parse(urls[url].Split("/")[0]) == long.Parse(urls[url].Split("/")[1]))
                {
                    DeleteURL(url);
                }
            }

            // Cleanup orphaned list entries
            foreach(ListViewItem url in downloadListView.Items)
            {
                if(!urls.ContainsKey(url.Text))
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
