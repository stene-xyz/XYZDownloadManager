using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XYZDownloadManager
{
    public class DownloadManager
    {
        private Dictionary<string, DownloadTask> tasks = new Dictionary<string, DownloadTask>();
        string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

        public void AddURL(string url)
        {
            string filename = Path.Combine(downloadsPath, Path.GetFileName(url));

            if (Path.Exists(filename))
            {
                MessageBox.Error($"File already exists in {downloadsPath}");
                return;
            }

            if (tasks.ContainsKey(url))
            {
                MessageBox.Error("URL already exists!");
                return;
            }

            // Create new DownloadTask
            DownloadTask task = new DownloadTask(url, downloadsPath);
            tasks.Add(url, task);
            Task.Run(() => task.StartDownload());
        }

        public void CancelURL(string url) 
        {
            if (tasks.ContainsKey(url))
            {
                tasks[url].Cancel();
                tasks.Remove(url);
            }
        }

        public string[] GetUrls()
        {
            return tasks.Keys.ToArray();
        }

        public ByteCount GetProgress(string url)
        {
            if (!tasks.ContainsKey(url)) return null;
            ByteCount count = new ByteCount();
            count.received = tasks[url].ReceivedCount();
            count.total = tasks[url].TotalCount();
            return count;
        }

        public bool Exists(string url)
        {
            if (tasks.ContainsKey(url)) return true;
            return false;
        }
    }

}
