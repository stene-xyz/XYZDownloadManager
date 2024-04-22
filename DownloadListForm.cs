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

            downloadManager.Progress += (url, received, total) =>
            {
                urls[url] = received + "/" + total;
            };

            // Load URL list
            string[] urlList = FileList.Load();
            foreach (string url in urlList)
            {
                AddURL(url, false);
            }

            refreshTimer.Enabled = true;
        }

        private void AddURL(string url, bool save)
        {
            string userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string downloadsPath = Path.Combine(userPath, "Downloads");
            string filename = Path.Combine(downloadsPath, Path.GetFileName(url));

            if (Path.Exists(filename))
            {
                MessageBox.Error($"File already exists in {downloadsPath}");
                return;
            }

            if (urls.ContainsKey(url))
            {
                MessageBox.Error("URL already in download list");
                return;
            }

            urls[url] = "0/0";
            downloadListView.Items.Add(url);
            downloadManager.AddURL(url);

            if(save)
            {
                FileList.Save(urls);
            }
        }

        public void DeleteURL(string url)
        {
            downloadManager.DeleteURL(url);
            urls.TryRemove(new KeyValuePair<string, string>(url, urls[url]));

            int urlIndex = downloadListView.Items.IndexOfKey(url);
            if(urlIndex != -1)
            {
                downloadListView.Items.RemoveAt(urlIndex);
            }

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

            while (!addURLForm.finished) { }
            if (!addURLForm.url.Equals(""))
            {
                AddURL(addURLForm.url, true);
            }
        }

        private void downloadListView_SelectedIndexChanged(object sender, EventArgs e)
        {
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
            downloadListView_SelectedIndexChanged(null, null);

            // Cleanup finished downloads
            foreach(string url in urls.Keys)
            {
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
            if (downloadListView.SelectedIndices.Count == 0) return;

            string url = downloadListView.Items[downloadListView.SelectedIndices[0]].Text;
            DeleteURL(url);
        }
    }
}
