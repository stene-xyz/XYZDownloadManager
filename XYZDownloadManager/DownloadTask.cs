using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Diagnostics;

namespace XYZDownloadManager
{
    internal class DownloadTask
    {
        private long receivedBytes;
        private long totalBytes;
        private string downloadLocation;
        private string downloadUrl;
        private string filename;
        private CancellationTokenSource cancellationTokenSource;
        private CancellationToken cancellationToken;
        private static Mutex mut = new Mutex();

        public event Action<string, long, long> Progress;

        public DownloadTask(string url, string downloadLocation) 
        { 
            receivedBytes = 0;
            totalBytes = 0;
            this.downloadLocation = downloadLocation;
            downloadUrl = url;
            filename = Path.Combine(this.downloadLocation, Path.GetFileName(downloadUrl));

            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;
        }

        public async Task StartDownload()
        {
            string tempFilePath = $"{filename}.part";

            // This infinite While loop makes sure we keep retrying if errors happen.
            while(true)
            {
                try
                {
                    // Prepare for download
                    HttpClient client = new HttpClient();
                    FileStream fileStream;
                    long startingAt = 0;

                    if (File.Exists(tempFilePath))
                    {
                        // Setup download range
                        startingAt = new FileInfo(tempFilePath).Length;
                        client.DefaultRequestHeaders.Range = new RangeHeaderValue(startingAt, null);

                        // Resume download by appending to existing file
                        fileStream = new FileStream(tempFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                    }
                    else
                    {
                        // Setup download range
                        client.DefaultRequestHeaders.Range = new RangeHeaderValue(0, null);

                        // Start a fresh download
                        fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                    }

                    // TODO: Handle 400/500 responses
                    HttpResponseMessage response = await client.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead);
                    response.EnsureSuccessStatusCode();

                    totalBytes = startingAt + response.Content.Headers.ContentLength ?? -1L;
                    receivedBytes = startingAt;

                    // Actual download logic
                    Stream contentStream = await response.Content.ReadAsStreamAsync();
                    byte[] buffer = new byte[8192];
                    int bytesReadThisChunk = 0;

                    while((bytesReadThisChunk = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        await fileStream.WriteAsync(buffer, 0, bytesReadThisChunk);

                        mut.WaitOne();
                        receivedBytes += bytesReadThisChunk;
                        mut.ReleaseMutex();

                        // Report progress
                        Progress?.Invoke(downloadUrl, receivedBytes, totalBytes);
                    }

                    fileStream.Close();

                    // Rename temporary filename to final filename
                    File.Move(tempFilePath, filename);
                    
                    Console.WriteLine($"Download completed: {filename}");
                    return;
                } 
                catch(OperationCanceledException ex)
                {
                    Console.WriteLine($"Operation cancelled: {ex.Message}");
                    return;
                } 
                catch(IOException ex)
                {
                    Console.WriteLine($"IOException: {ex.Message}");
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Download error: {ex.Message}", ex);
                }
            }
        }

        public long ReceivedCount()
        {
            mut.WaitOne();
            long bytes = receivedBytes;
            mut.ReleaseMutex();
            return bytes;
        }
        public long TotalCount()
        {
            mut.WaitOne();
            long bytes = totalBytes;
            mut.ReleaseMutex();
            return bytes;
        }
        public string GetFilename()
        {
            return filename;
        }
        public string GetURL()
        {
            return downloadUrl;
        }
        public void Cancel()
        {
            cancellationTokenSource.Cancel();
        }
    }
}
