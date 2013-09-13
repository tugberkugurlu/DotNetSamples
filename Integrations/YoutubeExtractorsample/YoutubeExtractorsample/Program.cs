using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YoutubeExtractor;

namespace YoutubeExtractorsample {

    class Program {

        static void Main(string[] args) {

            // Our test youtube link
            string link = "http://www.youtube.com/watch?v=5y_KJAg8bHI";

            IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(link);
            VideoInfo video = videoInfos
                .Where(info => info.CanExtractAudio)
                .OrderByDescending(info => info.AudioBitrate)
                .First();

            AudioDownloader audioDownloader = new AudioDownloader(video, Path.Combine(@"C:\Users\Tugberk\Downloads", video.Title + video.AudioExtension));

            audioDownloader.DownloadProgressChanged += (sender, _args) => Console.WriteLine(_args.ProgressPercentage * 0.85);
            audioDownloader.AudioExtractionProgressChanged += (sender, _args) => Console.WriteLine(85 + _args.ProgressPercentage * 0.15);
            audioDownloader.Execute();
            Console.ReadLine();
        }
    }
}
