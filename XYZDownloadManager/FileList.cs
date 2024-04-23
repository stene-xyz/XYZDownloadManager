using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

// Should be relatively self-explanatory
// Will report back when I find a bug after dropping the project for 6 months

namespace XYZDownloadManager
{
    internal class FileList
    {
        public static string GetPath()
        {
            string userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            return Path.Combine(userPath, "XYZDownloadManager.lst");
        }

        public static void Save(string[] urls)
        {
            string listPath = GetPath();

            // Retry saving 3 times
            for(int i = 0; i < 3; i++)
            {
                try
                {
                    using (StreamWriter writer = File.CreateText(listPath))
                    {
                        foreach (string url in urls)
                        {
                            writer.WriteLine(url);
                        }
                    }
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            MessageBox.Error($"Failed to save URL list (failed after 3 retries)");
        }

        public static string[] Load() 
        {
            string listPath = GetPath();
            List<string> list = new List<string>();

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
                            list.Add(line);
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            { 
                Console.WriteLine($"File not found: {listPath}");
            }
            catch (Exception ex)
            {
                MessageBox.Error($"Unhandled error loading list: {ex.Message}");
            }

            return list.ToArray();
        }
    }
}
