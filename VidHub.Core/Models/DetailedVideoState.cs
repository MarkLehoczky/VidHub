using System.Diagnostics;
using VidHub.Core.Enums;

namespace VidHub.Core.Models
{
    public class DetailedVideoState
    {
        public string Details { get; set; } = "Video has not been checked";
        public VideoHealth State { get; set; } = VideoHealth.NOTCHECKED;
        public VideoHealthCheckType Type { get; set; } = VideoHealthCheckType.NONE;


        public DetailedVideoState() { }
        public DetailedVideoState(VideoHealth state, string details)
        {
            State = state;
            Details = details;
        }

        public static implicit operator DetailedVideoState(VideoHealth health)
        {
            return health switch
            {
                VideoHealth.HEALTHY => new DetailedVideoState(VideoHealth.HEALTHY, "Video file appears to be in good condition"),
                VideoHealth.INPROGRESS => new DetailedVideoState(VideoHealth.INPROGRESS, "Health check is in progress..."),
                VideoHealth.FILENOTFOUND => new DetailedVideoState(VideoHealth.FILENOTFOUND, "Video file could not be found at the specified location"),
                VideoHealth.MINORCORRUPTION => new DetailedVideoState(VideoHealth.MINORCORRUPTION, "Minor issue was found that are unlikely to affect playback or functionality"),
                VideoHealth.SERIOUSCORRUPTION => new DetailedVideoState(VideoHealth.SERIOUSCORRUPTION, "Serious issue was found that may affect playback or functionality"),
                VideoHealth.CRITICALCORRUPTION => new DetailedVideoState(VideoHealth.CRITICALCORRUPTION, "Critical issue was found that likely render the video unplayable"),
                VideoHealth.UNKNOWNERROR => new DetailedVideoState(VideoHealth.UNKNOWNERROR, "Unexpected error occurred while checking the video health"),
                _ => new DetailedVideoState(VideoHealth.NOTCHECKED, "Video has not been checked"),
            };
        }
    }
}
