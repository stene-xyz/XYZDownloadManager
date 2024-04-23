using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYZDownloadManager
{
    public class FileSize
    {
        public static string Scale(long value)
        {
            string[] sizeSuffixes = { "B", "KiB", "MiB", "GiB", "TiB" };

            // Determine the appropriate size suffix
            int suffixIndex = 0;
            long size = value;

            while (size >= 1024 && suffixIndex < sizeSuffixes.Length - 1)
            {
                size /= 1024;
                suffixIndex++;
            }

            return $"{size} {sizeSuffixes[suffixIndex]}";
        }
    }
}
