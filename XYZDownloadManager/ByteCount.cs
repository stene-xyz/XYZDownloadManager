using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYZDownloadManager
{
    public class ByteCount
    {
        public long received;
        public long total;

        public override string ToString()
        {
            // TODO: Add KiB, MiB, GiB, TiB
            return $"{received}/{total}";
        }

        public bool Complete()
        {
            if (received == 0) return false;
            if (received == total) return true;
            return false;
        }

        public int Percent()
        {
            if (total == 0) return 0;
            double percent = (double) received / total;
            return (int) (percent * 100);
        }
    }
}
