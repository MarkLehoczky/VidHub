using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using VidHub.Core;
using VidHub.Core.Helpers;
using VidHub.Services.Base.Interfaces;
using VidHub.Services.Logics.Interfaces;
using VidHub.Services.Settings.Interfaces;

namespace VidHub.Services.Logics
{
    public class VideoCustomizationService(IMainService service, ISettingsService settings) : IVideoCustomizationService
    {
        public class FormattedVideo(Video video) : ObservableObject
        {
            private string title = string.Empty; 

            public string FilePath { get; } = video.FilePath;
            public string Title
            {
                get => title;
                set => SetProperty(ref title, value);
            }
            public int ID { get; } = video.ID;
        }

        public ObservableCollection<FormattedVideo> Videos { get; } = [];
        public bool IsTemplateMode { get; set; }

        public bool IncludePath
        {
            get => settings.IncludePath;
            set 
            {
                settings.IncludePath = value;
                UpdateFormats();
            }
        }
        public bool IncludeDate
        {
            get => settings.IncludeDate;
            set
            {
                settings.IncludeDate = value;
                UpdateFormats();
            }
        }
        public bool IncludeFilename
        {
            get => settings.IncludeFilename;
            set
            {
                settings.IncludeFilename = value;
                UpdateFormats();
            }
        }
        public bool IncludeMetadata
        {
            get => settings.IncludeMetadata;
            set
            {
                settings.IncludeMetadata = value;
                UpdateFormats();
            }
        }
        public bool IncludeExtension
        {
            get => settings.IncludeExtension;
            set
            {
                settings.IncludeExtension = value;
                UpdateFormats();
            }
        }

        public string Pattern
        {
            get => settings.Pattern;
            set
            {
                settings.Pattern = value;
                UpdateFormats();
            }
        }
        public string Replacement
        {
            get => settings.Replacement;
            set
            {
                settings.Replacement = value;
                UpdateFormats();
            }
        }
        public bool InvalidRegex { get; set; } = false;

        public bool IsRegexEnabled
        {
            get => settings.IsRegexEnabled;
            set
            {
                settings.IsRegexEnabled = value;
                UpdateFormats();
            }
        }
        public bool DontShowAgain
        {
            get => settings.DontShowTitleCustomizationAgain;
            set => settings.DontShowTitleCustomizationAgain = value;
        }



        public void LoadFormats()
        {
            if (IsTemplateMode)
            {
                Videos.Clear();
                foreach (var video in service.GetAllVideos())
                {
                    Videos.Add(new FormattedVideo(video));
                }
            }
            else
            {
                Videos.Clear();
                foreach (var video in service.GetLastLoadedVideos())
                {
                    Videos.Add(new FormattedVideo(video));
                }
            }
            UpdateFormats();
        }

        public void UpdateFormats()
        {
            try
            {
                var regex = new Regex(Pattern);
                InvalidRegex = false;
            }
            catch
            {
                InvalidRegex = true;
            }

            foreach (var video in Videos)
            {
                video.Title = CustomizeTitle(video.FilePath, IsRegexEnabled && !InvalidRegex);
                if (!IsTemplateMode)
                {
                    service.GetVideo(video.ID).Title = video.Title;
                    service.Update(UpdateType.ResetVideoCollection);
                }
            }
        }

        public void CustomizeTitle(Video video)
        {
            video.Title = CustomizeTitle(video.FilePath, IsRegexEnabled && !InvalidRegex);
        }
        private string CustomizeTitle(string title, bool useRegex)
        {
            var newTitle = "";
            if (IncludePath)
            {
                newTitle += Path.GetFullPath(title)[..^Path.GetFileName(title).Length];
            }
            if (IncludeDate)
            {
                newTitle += File.GetCreationTime(title).ToString("yyyy-MM-dd");
            }
            if (IncludeFilename)
            {
                if (IncludeDate)
                    newTitle += "_";
                newTitle += Path.GetFileNameWithoutExtension(title);
            }
            if (IncludeMetadata)
            {
                newTitle += "[Metadata]";
            }
            if (IncludeExtension)
            {
                newTitle += Path.GetExtension(title);
            }
            if (useRegex)
            {
                var regex = new Regex(Pattern);
                newTitle = regex.Replace(newTitle, Replacement);
            }
            return newTitle;
        }
    }
}
