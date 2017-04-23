using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            using (FileStream fs = File.Open(@"C:\temp\0.txt", FileMode.Open))
            {
                new SongService.Lyrics().UploadSong(fs);
            }
        }
    }
}
