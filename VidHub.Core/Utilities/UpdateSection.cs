namespace VidHub.Core.Utilities
{
    public enum UpdateSection
    {
        TITLEBAR,
        FILTERPANEL,
        LOADPANEL,
        VIDEOCOLLECTION,
        NOTIFICATIONS,
        DISPLAYFORMATMODAL,
        PREVIEWIMAGEFORMATMODAL,
        TITLEFORMATMODAL
    }


    public static class UpdateSections
    {
        public static IEnumerable<UpdateSection> ALL =>
        [
            UpdateSection.TITLEBAR,
            UpdateSection.FILTERPANEL,
            UpdateSection.LOADPANEL,
            UpdateSection.VIDEOCOLLECTION,
            UpdateSection.NOTIFICATIONS,
            UpdateSection.DISPLAYFORMATMODAL,
            UpdateSection.PREVIEWIMAGEFORMATMODAL,
            UpdateSection.TITLEFORMATMODAL
        ];
        public static IEnumerable<UpdateSection> SIDEPANEL =>
        [
            UpdateSection.FILTERPANEL,
            UpdateSection.LOADPANEL
        ];
        public static IEnumerable<UpdateSection> MODALS =>
        [
            UpdateSection.DISPLAYFORMATMODAL,
            UpdateSection.PREVIEWIMAGEFORMATMODAL,
            UpdateSection.TITLEFORMATMODAL
        ];
    }
}
