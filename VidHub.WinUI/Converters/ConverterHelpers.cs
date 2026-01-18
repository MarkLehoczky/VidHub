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
                { "24", 24 },
                { "12", 12 },
                { "low", 1 }
            };
            foreach (var item in framerates)
            {
                if (stream.Framerate.Value >= item.Value)
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
            return stream.Resolution.Definition;
        }
        public static T? ConvertResolution<T>(this VideoStream stream, Dictionary<string, T?> dictionary)
        {
            return dictionary.GetValueOrDefault(stream.FindResolution(), default);
        }
    }
}
