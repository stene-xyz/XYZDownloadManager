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
        private readonly Dictionary<string, CancellationTokenSource> _downloadTasks;

        public DownloadManager()
        {
            _downloadTasks = new Dictionary<string, CancellationTokenSource>();
        }

        public void AddURL(string url)
        {
            if (!_downloadTasks.ContainsKey(url))
            {
                var cancellationTokenSource = new CancellationTokenSource();
                _downloadTasks.Add(url, cancellationTokenSource);
                Task.Run(() => DownloadFileAsync(url, cancellationTokenSource.Token));
            }
        }

        public void DeleteURL(string url)
        {
            if (_downloadTasks.ContainsKey(url))
            {
                _downloadTasks[url].Cancel();
                _downloadTasks.Remove(url);
            }
        }

        public event Action<string, long, long> Progress;

        public async Task DownloadFileAsync(string url, CancellationToken cancellationToken)
        {
            // Extract the filename from the URL
            string userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string downloadsPath = Path.Combine(userPath, "Downloads");
            string filename = Path.Combine(downloadsPath, Path.GetFileName(url));
            string tempFilePath = $"{filename}.part";

            while (true)
            {
                try
                {
                    // Create a new HttpClient for this download
                    using var httpClient = new HttpClient();

                    // Check if the file already exists
                    if (File.Exists(tempFilePath))
                    {
                        // Get the existing file's length
                        long existingLength = new FileInfo(tempFilePath).Length;

                        // Set the Range header to resume from the correct point
                        httpClient.DefaultRequestHeaders.Range = new System.Net.Http.Headers.RangeHeaderValue(existingLength, null);

                        // Resume download by appending to existing file
                        using var existingFileStream = new FileStream(tempFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                        await DownloadWithProgressAsync(httpClient, url, existingFileStream, cancellationToken);
                        cancellationToken.ThrowIfCancellationRequested();
                    }
                    else
                    {
                        // Start a fresh download
                        using var newFileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                        await DownloadWithProgressAsync(httpClient, url, newFileStream, cancellationToken);
                        cancellationToken.ThrowIfCancellationRequested();
                    }

                    // Rename the temporary file to the final filename
                    File.Move(tempFilePath, filename);

                    Console.WriteLine($"Download completed: {filename}");
                    return;
                }
                catch(OperationCanceledException ex)
                {
                    Console.WriteLine("User cancelled download");
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error downloading file: {ex.Message}");
                }
            }
            
        }

        private async Task DownloadWithProgressAsync(HttpClient httpClient, string url, Stream outputStream, CancellationToken cancellationToken)
        {
            try
            {
                using var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                long startingAt = 0;
                foreach (RangeItemHeaderValue rangeItem in httpClient.DefaultRequestHeaders.Range.Ranges)
                {
                    if (rangeItem.From.HasValue)
                    {
                        startingAt = rangeItem.From.Value;
                    }
                }

                var totalBytes = response.Content.Headers.ContentLength ?? -1L;
                var bytesRead = 0L;

                using var contentStream = await response.Content.ReadAsStreamAsync();
                var buffer = new byte[8192];
                int bytesReadThisChunk;

                while ((bytesReadThisChunk = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await outputStream.WriteAsync(buffer, 0, bytesReadThisChunk);
                    bytesRead += bytesReadThisChunk;

                    // Report progress
                    Progress?.Invoke(url, startingAt + bytesRead, startingAt + totalBytes);
                }
            }
            catch(OperationCanceledException ex)
            {
                return;
            }
        }
    }

}
