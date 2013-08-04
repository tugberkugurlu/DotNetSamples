using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YoutubeExtractor;

namespace YoutubeExtractorsample {

    class Program {

        static void Main(string[] args) {

            // Our test youtube link
            string link = "http://www.youtube.com/watch?v=3O3-yDmhEcU";
            IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(link);

            Console.ReadLine();
        }
    }
}
