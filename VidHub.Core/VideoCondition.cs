namespace VidHub.Core
{
    public class VideoCondition
    {
        public enum State
        {
            NOTCHECKED,
            INPROGRESS,
            HEALTHY,
            CORRUPTED,
            UNKNOWNERROR,
            FILENOTFOUND
        }


        public State VideoState { get; set; } = State.NOTCHECKED;
        public string Description { get; set; } = "Condition not checked yet...";
    }
}
