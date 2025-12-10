namespace VidHub.Core.Utilities.Helper
{
    public enum VideoHealth
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
}
