using System.Collections.Generic;
using VidHub.Core.Streams;

namespace VidHub.WinUI.Converters
{
    public static class ConverterHelpers
    {
        public static string FindFramerate(this VideoStream stream)
        {
            SortedDictionary<string, int> framerates = new()
            {
                { "240", 240 },
                { "120", 120 },
                { "90", 90 },
                { "60", 60 },
                { "30", 30 },
                { "23", 23 },
                { "12", 12 },
                { "low", 1 }
            };
            foreach (var item in framerates)
            {
                if (stream.Framerate >= item.Value)
                {
                    return item.Key;
                }
            }
            return "n/a";
        }
        public static T? ConvertFramerate<T>(this VideoStream stream, Dictionary<string, T?> dictionary)
        {
            return dictionary.GetValueOrDefault(stream.FindFramerate(), default);
        }

        public static string FindResolution(this VideoStream stream)
        {
            SortedDictionary<string, int> framerates = new()
            {
                { "8K", 7680 * 4320 },
                { "4K", 3840 * 2160 },
                { "1440p", 2560 * 1440 },
                { "1080p", 1920 * 1080 },
                { "720p", 1280 * 720 },
                { "480p", 640 * 480 },
                { "low", 1 }
            };
            foreach (var item in framerates)
            {
                if (stream.Width * stream.Height >= item.Value)
                {
                    return item.Key;
                }
            }
            return "n/a";
        }
        public static T? ConvertResolution<T>(this VideoStream stream, Dictionary<string, T?> dictionary)
        {
            return dictionary.GetValueOrDefault(stream.FindResolution(), default);
        }
    }
}
