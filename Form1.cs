using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Cryptography.Pkcs;

namespace XYZDownloadManager
{
    public partial class Form1 : Form
    {
        private DownloadManager downloadManager = new DownloadManager();
        private ConcurrentDictionary<string, string> urls = new ConcurrentDictionary<string, string>();

        private string userPath;
        private string listPath;

        public Form1()
        {
            InitializeComponent();

            // TODO: Update progress function to remove file from list if download finished
            downloadManager.Progress += (url, received, total) =>
            {
                urls[url] = received + "/" + total;
            };

            userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            listPath = Path.Combine(userPath, "XYZDownloadManager.lst");

            try
            {
                using (StreamReader reader = new StreamReader(listPath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Skip blank lines
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            // Call AddURL method with the non-blank line
                            AddURL(line, false);
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                // list not created yet. all good
                Console.WriteLine($"File not found: {listPath}");
            }
            catch (Exception ex)
            {
                Error($"Unhandled error loading list: {ex.Message}");
            }
        }

        private void Error(string message)
        {
            ErrorDialog errorDialog = new ErrorDialog();
            errorDialog.SetMessage(message);
            errorDialog.ShowDialog();
        }

        private void AddURL(string url, bool save)
        {
            // TODO: Check before adding URL to make sure file does not already exist
            if (urls.ContainsKey(url))
            {
                Error("URL already in download list");
                return;
            }
            urls[url] = "0/0";
            downloadListView.Items.Add(url);
            downloadManager.AddURL(url);

            if(save)
            {
                try
                {
                    using (StreamWriter writer = File.AppendText(listPath))
                    {
                        writer.WriteLine(url);
                    }
                }
                catch (Exception ex)
                {
                    Error($"Error saving URL: {ex.Message}");
                }
            }
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

            progressText.Text = urls[downloadListView.Items[downloadListView.SelectedIndices[0]].Text];
            progressBar.Maximum = int.Parse(progressText.Text.Split("/")[1]);
            progressBar.Value = int.Parse(progressText.Text.Split("/")[0]);
        }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            downloadListView_SelectedIndexChanged(null, null);
        }

        private void deleteURLButton_Click(object sender, EventArgs e)
        {
            if (downloadListView.SelectedIndices.Count == 0) return;

            string url = downloadListView.Items[downloadListView.SelectedIndices[0]].Text;
            downloadManager.DeleteURL(url);
            urls.TryRemove(new KeyValuePair<string, string>(url, urls[url]));
            downloadListView.Items.RemoveAt(downloadListView.SelectedIndices[0]);
        }
    }
}
