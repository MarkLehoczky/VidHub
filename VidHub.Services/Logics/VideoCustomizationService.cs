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
        public bool IsTemplateMode { get; set; } = true;

        private bool includePath = false;
        private bool includeDate = false;
        private bool includeFilename = true;
        private bool includeMetadata = false;
        private bool includeExtension = false;

        string pattern = "";
        string replacement = "";

        bool isRegexEnabled = false;


        public bool IncludePath
        {
            get => includePath;
            set
            {
                includePath = value;
                UpdateFormats();
            }
        }
        public bool IncludeDate
        {
            get => includeDate;
            set
            {
                includeDate = value;
                UpdateFormats();
            }
        }
        public bool IncludeFilename
        {
            get => includeFilename;
            set
            {
                includeFilename = value;
                UpdateFormats();
            }
        }
        public bool IncludeMetadata
        {
            get => includeMetadata;
            set
            {
                includeMetadata = value;
                UpdateFormats();
            }
        }
        public bool IncludeExtension
        {
            get => includeExtension;
            set
            {
                includeExtension = value;
                UpdateFormats();
            }
        }

        public string Pattern
        {
            get => pattern;
            set
            {
                pattern = value;
                UpdateFormats();
            }
        }
        public string Replacement
        {
            get => replacement;
            set
            {
                replacement = value;
                UpdateFormats();
            }
        }
        public bool InvalidRegex { get; set; } = false;

        public bool IsRegexEnabled
        {
            get => isRegexEnabled;
            set
            {
                isRegexEnabled = value;
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
                var regex = new Regex(pattern);
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
                if (includeDate)
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
                var regex = new Regex(pattern);
                regex.Replace("", replacement);
                newTitle = regex.Replace(newTitle, replacement);
            }
            return newTitle;
        }
    }
}
