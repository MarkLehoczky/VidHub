namespace VidHub.Core.Models
{
    public enum HealthState
    {
        NOTCHECKED,
        INPROGRESS,
        HEALTHY,
        MINORCORRUPTION,
        SERIOUSCORRUPTION,
        CRITICALCORRUPTION,
        UNKNOWNERROR,
        FILENOTFOUND
    }


    public enum HealthType
    {
        NONE,
        EXISTENCECHECK,
        QUICKCHECK,
        FULLCHECK
    }


    public class DetailedHealth
    {
        public string Details { get; set; } = "Video has not been checked";
        public HealthState State { get; set; } = HealthState.NOTCHECKED;
        public HealthType Type { get; set; } = HealthType.NONE;


        public DetailedHealth() { }
        public DetailedHealth(HealthState state, string details)
        {
            State = state;
            Details = details;
        }


        public static implicit operator DetailedHealth(HealthState health)
        {
            return health switch
            {
                HealthState.HEALTHY => new DetailedHealth(HealthState.HEALTHY, "Video file appears to be in good condition"),
                HealthState.INPROGRESS => new DetailedHealth(HealthState.INPROGRESS, "Health check is in progress..."),
                HealthState.FILENOTFOUND => new DetailedHealth(HealthState.FILENOTFOUND, "Video file could not be found at the specified location"),
                HealthState.MINORCORRUPTION => new DetailedHealth(HealthState.MINORCORRUPTION, "Minor issue was found that are unlikely to affect playback or functionality"),
                HealthState.SERIOUSCORRUPTION => new DetailedHealth(HealthState.SERIOUSCORRUPTION, "Serious issue was found that may affect playback or functionality"),
                HealthState.CRITICALCORRUPTION => new DetailedHealth(HealthState.CRITICALCORRUPTION, "Critical issue was found that likely render the video unplayable"),
                HealthState.UNKNOWNERROR => new DetailedHealth(HealthState.UNKNOWNERROR, "Unexpected error occurred while checking the video health"),
                _ => new DetailedHealth(HealthState.NOTCHECKED, "Video has not been checked"),
            };
        }
    }
}
