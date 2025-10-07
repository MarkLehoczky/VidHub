using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace VidHub.ViewModels.Modals
{
    public partial class TitleCustomizationViewModel : ObservableRecipient
    {
        public class TitleFormatter(string filePath) : ObservableObject
        {
            public string FilePath { get; set; } = filePath;
            public string SimpleTitle { get; set; } = Path.GetFileNameWithoutExtension(filePath);
            public string Title { get; set; } = Path.GetFileNameWithoutExtension(filePath);

            public void ChangeTitle(string newTitle)
            {
                Title = newTitle;
                OnPropertyChanged(nameof(Title));
            }
        }

        private bool includePath = false;
        private bool includeDate = false;
        private bool includeFilename = true;
        private bool includeMetadata = false;
        private bool includeExtension = false;

        string pattern = "";
        string replacement = "";
        bool invalidPattern = false;
        bool invalidReplacement = false;

        bool isRegexEnabled = false;
        bool dontShowAgain = false;


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
        public bool InvalidRegex => InvalidRegexText != "";
        public string InvalidRegexText =>
            invalidPattern && invalidReplacement
                ? "Invalid regex pattern and replacement"
                : invalidPattern
                    ? "Invalid regex pattern"
                    : invalidReplacement
                        ? "Invalid regex replacement"
                        : "";

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
            get => dontShowAgain;
            set
            {
                dontShowAgain = value;
            }
        }


        private void UpdateFormats()
        {
            foreach (var item in TitleCollection)
            {
                var newTitle = "";
                if (IncludePath)
                {
                    newTitle += Path.GetFullPath(item.FilePath)[..^Path.GetFileName(item.FilePath).Length];
                }
                if (IncludeDate)
                {
                    newTitle += File.GetCreationTime(item.FilePath).ToString("yyyy-MM-dd");
                }
                if (IncludeFilename)
                {
                    if (includeDate)
                        newTitle += "_";
                    newTitle += Path.GetFileNameWithoutExtension(item.FilePath);
                }
                if (IncludeMetadata)
                {
                    newTitle += "[Metadata]";
                }
                if (IncludeExtension)
                {
                    newTitle += Path.GetExtension(item.FilePath);
                }
                item.SimpleTitle = newTitle;
                item.ChangeTitle(newTitle);
            }

            if (IsRegexEnabled)
            {
                try
                {
                    var regex = new Regex(pattern);
                    invalidPattern = false;

                    try
                    {
                        regex.Replace("", replacement);
                        invalidReplacement = false;

                        foreach (var item in TitleCollection)
                        {
                            item.ChangeTitle(regex.Replace(item.SimpleTitle, replacement));
                        }
                    }
                    catch (ArgumentException)
                    {
                        invalidReplacement = true;
                    }
                }
                catch
                {
                    invalidPattern = true;

                    try
                    {
                        Regex.Replace("", "", replacement);
                        invalidReplacement = false;
                    }
                    catch (ArgumentException)
                    {
                        invalidReplacement = true;
                    }
                }

                OnPropertyChanged(nameof(InvalidRegex));
                OnPropertyChanged(nameof(InvalidRegexText));
            }
        }


        public ObservableCollection<TitleFormatter> TitleCollection { get; set; } =
        [
            new(@"D:\Youtube Channels\Jaiden\Videos\[ANIMATIC] JaidenAnimations the Anime.mkv"),
            new(@"D:\Youtube Channels\Jaiden\Videos\Among Us but I killed the other Imposter....mkv"),
            new(@"D:\Youtube Channels\Jaiden\Videos\Among Us but I try to get voted out.mkv"),
            new(@"D:\Youtube Channels\Jaiden\Videos\Defusing bombs with teamwork ：).mkv"),
            new(@"D:\Youtube Channels\Jaiden\Videos\How I won a Mario Party Tournament....mkv"),
            new(@"D:\Youtube Channels\Jaiden\Videos\I'M IN A POKEMON TOURNAMENT.mkv"),
            new(@"D:\Youtube Channels\Jaiden\Videos\I'm not built for horror games... at all..mkv"),
            new(@"D:\Youtube Channels\Jaiden\Videos\Jacob and Jaiden attempt their SoulLink again.mkv"),
            new(@"D:\Youtube Channels\Jaiden\Videos\Jaiden and Jacob having a good time playing Mario ：).mkv"),
            new(@"D:\Youtube Channels\Jaiden\Videos\Jaiden attempted a Mario Party Tournament.mkv"),
            new(@"D:\Youtube Channels\Jaiden\Videos\Jaiden Attempts Competitive Pokemon....mkv"),
            new(@"D:\Youtube Channels\Jaiden\Videos\Jaiden Experiences Digi-Torture.mkv"),
            new(@"D:\Youtube Channels\Jaiden\Videos\Jaiden Experiences Mario DDR.mkv"),
            new(@"D:\Youtube Channels\Jaiden\Videos\Jaiden FINALLY Tries Stardew Valley.mkv"),
            new(@"D:\Youtube Channels\Jaiden\Videos\Jaiden is STRUGGLING in Lethal Company.mkv"),
            new(@"D:\Youtube Channels\Jaiden\Videos\Jaiden isn't that great at Terraria after all..mkv"),
            new(@"D:\Youtube Channels\Jaiden\Videos\Jaiden learns about Minecraft BounceSMP.mkv"),
            new(@"D:\Youtube Channels\Jaiden\Videos\Jaiden Plays Omori for the First Time.mkv"),
            new(@"D:\Youtube Channels\Jaiden\Videos\Jaiden plays Pokemon Violet for the first time.mkv"),
            new(@"D:\Youtube Channels\Jaiden\Videos\Jaiden talks about her VTuber Model.mkv"),
            new(@"D:\Youtube Channels\Jaiden\Videos\Jaiden won $0.mkv"),
            new(@"D:\Youtube Channels\Jaiden\Videos\Jaiden's ＂Normal＂ Pokemon Gameshow.mkv"),
            new(@"D:\Youtube Channels\Jaiden\Videos\Jaiden's Complete Pokemon Scartlet Violet Tier List.mkv"),
            new(@"D:\Youtube Channels\Jaiden\Videos\Jaiden's First Attempt at Lethal Company.mkv"),
            new(@"D:\Youtube Channels\Jaiden\Videos\Jaiden's Full Rhythm Heaven Speedrun.mkv"),
            new(@"D:\Youtube Channels\Jaiden\Videos\Mario but it's a Battle Royale.mkv"),
            new(@"D:\Youtube Channels\Jaiden\Videos\Pokémon but they're all CANDY.mkv"),
            new(@"D:\Youtube Channels\Jaiden\Videos\Rhythm Heaven but I Speedrun it.mkv"),
            new(@"D:\Youtube Channels\Jaiden\Videos\The STRANGEST SpongeBob Game You've Never Played.mkv"),
            new(@"D:\Youtube Channels\Jaiden\Videos\Tommy tried to teach Jaiden Minecraft.mkv"),
            new(@"D:\Youtube Channels\Jaiden\Videos\Too Many Types almost broke me.mkv"),
            new(@"D:\Youtube Channels\Jaiden\Videos\What REALLY happened in our SoulLink.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\A fan stopped me in public\A fan stopped me in public.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Could you find Ari？？\Could you find Ari？？.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\I accidentally made history\I accidentally made history.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\I almost ruined this…\I almost ruined this….mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\I tried to win money on a gameshow\I tried to win money on a gameshow.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\I won an “Interesting” award…\I won an “Interesting” award….mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Jaiden gives Schlatt a gift#\Jaiden gives Schlatt a gift..mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Jaiden got scammed#\Jaiden got scammed..mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Jaiden singing the FNAF song\Jaiden singing the FNAF song.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\My neighbor was spying on me#\My neighbor was spying on me..mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Quackity is corrupt with power\Quackity is corrupt with power.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： 10 Years of Jaiden (shot 1)\Storyboard vs Animation： 10 Years of Jaiden (shot 1).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： 10 Years of Jaiden (shot 2)\Storyboard vs Animation： 10 Years of Jaiden (shot 2).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： 10 Years of Jaiden (shot 10)\Storyboard vs Animation： 10 Years of Jaiden (shot 10).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： Hatsune Miku (shot 4)\Storyboard vs Animation： Hatsune Miku (shot 4).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： Hatsune Miku (shot 10)\Storyboard vs Animation： Hatsune Miku (shot 10).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： Hatsune Miku (shot 12)\Storyboard vs Animation： Hatsune Miku (shot 12).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： Hatsune Miku (shot 16)\Storyboard vs Animation： Hatsune Miku (shot 16).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： Hatsune Miku (shot 23)\Storyboard vs Animation： Hatsune Miku (shot 23).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： I got a cat. (shot 15)\Storyboard vs Animation： I got a cat. (shot 15).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： I got a cat. (shot 17)\Storyboard vs Animation： I got a cat. (shot 17).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： I got a cat. (shot 20)\Storyboard vs Animation： I got a cat. (shot 20).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： I have ADHD (shot 1)\Storyboard vs Animation： I have ADHD (shot 1).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： I have ADHD (shot 4)\Storyboard vs Animation： I have ADHD (shot 4).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： I have ADHD (shot 9)\Storyboard vs Animation： I have ADHD (shot 9).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： I have ADHD (shot 16)\Storyboard vs Animation： I have ADHD (shot 16).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： I have ADHD (shot 27)\Storyboard vs Animation： I have ADHD (shot 27).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： I was almost in Squid Game. (shot 1)\Storyboard vs Animation： I was almost in Squid Game. (shot 1).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： I was almost in Squid Game. (shot 4)\Storyboard vs Animation： I was almost in Squid Game. (shot 4).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： I was almost in Squid Game. (shot 9)\Storyboard vs Animation： I was almost in Squid Game. (shot 9).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： I was almost in Squid Game. (shot 11)\Storyboard vs Animation： I was almost in Squid Game. (shot 11).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： I was almost in Squid Game. (shot 12)\Storyboard vs Animation： I was almost in Squid Game. (shot 12).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： I was almost in Squid Game. (shot 14)\Storyboard vs Animation： I was almost in Squid Game. (shot 14).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： I was almost in Squid Game. (shot 15)\Storyboard vs Animation： I was almost in Squid Game. (shot 15).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： I was almost in Squid Game. (shot 18)\Storyboard vs Animation： I was almost in Squid Game. (shot 18).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： I was almost in Squid Game. (shot 19)\Storyboard vs Animation： I was almost in Squid Game. (shot 19).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： I was almost in Squid Game. (shot 21)\Storyboard vs Animation： I was almost in Squid Game. (shot 21).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： I was almost in Squid Game. (shot 22)\Storyboard vs Animation： I was almost in Squid Game. (shot 22).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： I was almost in Squid Game. (shot 23)\Storyboard vs Animation： I was almost in Squid Game. (shot 23).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： I was almost in Squid Game. (shot 24)\Storyboard vs Animation： I was almost in Squid Game. (shot 24).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： I was almost in Squid Game. (shot 25)\Storyboard vs Animation： I was almost in Squid Game. (shot 25).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： I went to Japan (shot 3)\Storyboard vs Animation： I went to Japan (shot 3).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： I went to Japan (shot 12)\Storyboard vs Animation： I went to Japan (shot 12).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： I went to Japan (shot 13)\Storyboard vs Animation： I went to Japan (shot 13).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： I went to Japan (shot 36)\Storyboard vs Animation： I went to Japan (shot 36).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： Mr. Beast Cube (shot 5)\Storyboard vs Animation： Mr. Beast Cube (shot 5).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： Mr. Beast Cube (shot 15)\Storyboard vs Animation： Mr. Beast Cube (shot 15).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： Mr. Beast Cube (shot 23)\Storyboard vs Animation： Mr. Beast Cube (shot 23).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： Mr. Beast Cube (shot 24)\Storyboard vs Animation： Mr. Beast Cube (shot 24).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： My Birds Laid Eggs (shot 2)\Storyboard vs Animation： My Birds Laid Eggs (shot 2).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： My Birds Laid Eggs (shot 3)\Storyboard vs Animation： My Birds Laid Eggs (shot 3).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： My Birds Laid Eggs (shot 8)\Storyboard vs Animation： My Birds Laid Eggs (shot 8).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： My Birds Laid Eggs (shot 9)\Storyboard vs Animation： My Birds Laid Eggs (shot 9).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： My Birds Laid Eggs (shot 10)\Storyboard vs Animation： My Birds Laid Eggs (shot 10).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： My Chiikawa Obsession (shot 2)\Storyboard vs Animation： My Chiikawa Obsession (shot 2).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： My Chiikawa Obsession (shot 9)\Storyboard vs Animation： My Chiikawa Obsession (shot 9).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： My Chiikawa Obsession (shot 11)\Storyboard vs Animation： My Chiikawa Obsession (shot 11).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： My Chiikawa Obsession (shot 14)\Storyboard vs Animation： My Chiikawa Obsession (shot 14).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： My Chiikawa Obsession (shot 18)\Storyboard vs Animation： My Chiikawa Obsession (shot 18).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： My Chiikawa Obsession (shot 22)\Storyboard vs Animation： My Chiikawa Obsession (shot 22).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： My Chiikawa Obsession (shot 23)\Storyboard vs Animation： My Chiikawa Obsession (shot 23).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： My Funniest Fan Interactions (shot 1)\Storyboard vs Animation： My Funniest Fan Interactions (shot 1).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： Pokemon Too Many Types (shot 20)\Storyboard vs Animation： Pokemon Too Many Types (shot 20).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： Pokemon Too Many Types (shot 25)\Storyboard vs Animation： Pokemon Too Many Types (shot 25).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： Pokemon Too Many Types (shot 26)\Storyboard vs Animation： Pokemon Too Many Types (shot 26).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： Pokemon Too Many Types (shot 31)\Storyboard vs Animation： Pokemon Too Many Types (shot 31).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： Pokemon Too Many Types (shot 33)\Storyboard vs Animation： Pokemon Too Many Types (shot 33).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： The Pokemon Cafe! (shot 1)\Storyboard vs Animation： The Pokemon Cafe! (shot 1).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： The Pokemon Cafe! (shot 6)\Storyboard vs Animation： The Pokemon Cafe! (shot 6).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： The Pokemon Cafe! (shot 8)\Storyboard vs Animation： The Pokemon Cafe! (shot 8).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： The Pokemon Cafe! (shot 12)\Storyboard vs Animation： The Pokemon Cafe! (shot 12).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： The Pokemon Cafe! (shot 13)\Storyboard vs Animation： The Pokemon Cafe! (shot 13).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\Storyboard vs Animation： The Pokemon Cafe! (shot 14)\Storyboard vs Animation： The Pokemon Cafe! (shot 14).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Shorts\The coolest thing we’ve made yet\The coolest thing we’ve made yet.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\1,000,000 Subscriber thing (holy mother)\1,000,000 Subscriber thing (holy mother).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\7 Day Vegan Challenge, baby (solves all yo' problems) ｜ Nominated by theodd1sout\7 Day Vegan Challenge, baby (solves all yo' problems) ｜ Nominated by theodd1sout.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\An Uncomfortable Trip to the UK\An Uncomfortable Trip to the UK.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Animal Crossing used to be so much darker..#\Animal Crossing used to be so much darker....mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Animated Pokemon Shorts (ORAS Special)\Animated Pokemon Shorts (ORAS Special).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Anxiety is the Greatest! (jk it can go jump off a microwave)\Anxiety is the Greatest! (jk it can go jump off a microwave).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Ari's Birthday Party & New Friend\Ari's Birthday Party & New Friend.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Ari's Birthday! (again)\Ari's Birthday! (again).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Ari's First Christmas feat. Weeaboo Misaki (read description)\Ari's First Christmas feat. Weeaboo Misaki (read description).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Being Not Straight\Being Not Straight.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Being the Best⧸Worst Ever\Being the Best⧸Worst Ever.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Can You ACTUALLY Win Money on Gameshows？\Can You ACTUALLY Win Money on Gameshows？.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Combining Pokemon w⧸ theodd1sout\Combining Pokemon w⧸ theodd1sout.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Crazy Substitute Teachers\Crazy Substitute Teachers.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Cringing at My Old Drawings (100k Milestone)\Cringing at My Old Drawings (100k Milestone).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Dating things I shouldn’t\Dating things I shouldn’t.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Dough D-D-Dear\Dough D-D-Dear.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Drawing Alola Pokemon w⧸ theodd1sout\Drawing Alola Pokemon w⧸ theodd1sout.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Drawing Characters from Memory w⧸ theodd1sout & SomethingElseYT\Drawing Characters from Memory w⧸ theodd1sout & SomethingElseYT.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Drawing our Childhood Drawings w⧸ theodd1sout\Drawing our Childhood Drawings w⧸ theodd1sout.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Drawing YuGiOh Monsters We don't know w⧸ Drawfee\Drawing YuGiOh Monsters We don't know w⧸ Drawfee.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Exploiting Everything w⧸ DrawWithJazza\Exploiting Everything w⧸ DrawWithJazza.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Flirting & My Stories\Flirting & My Stories.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\FNAF 4： Dan & Phil Animated\FNAF 4： Dan & Phil Animated.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Frozen Yogurt Freak Out\Frozen Yogurt Freak Out.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Happy Birthday Ari!\Happy Birthday Ari!.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Hide and Pee\Hide and Pee.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\How hard could BLINDFOLDED Mario be？\How hard could BLINDFOLDED Mario be？.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\How to be Stupid\How to be Stupid.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\How to Make Life More Interesting\How to Make Life More Interesting.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\I Attempted a Pokemon Platinum Nuzlocke\I Attempted a Pokemon Platinum Nuzlocke.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\I Attempted a Speedrun (and got a world record)\I Attempted a Speedrun (and got a world record).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\I Attempted a Two Player Nuzlocke\I Attempted a Two Player Nuzlocke.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\I Attempted my First Pokemon Nuzlocke\I Attempted my First Pokemon Nuzlocke.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\I caved and tried Genshin Impact..#\I caved and tried Genshin Impact....mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\I Don't Like the Dentist\I Don't Like the Dentist.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\I found out I have ADHD#\I found out I have ADHD..mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\I got a cat#\I got a cat..mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\I Hate High Heels\I Hate High Heels.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\I Hate Reading\I Hate Reading.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\I Let Psychics Read my Future\I Let Psychics Read my Future.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\I played Pokemon, but with 50+ New Types\I played Pokemon, but with 50+ New Types.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\I tried to go to Canada but got stuck in Minneapolis\I tried to go to Canada but got stuck in Minneapolis.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\I was almost in Squid Game#\I was almost in Squid Game..mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\I went to the Pokemon Cafe!\I went to the Pokemon Cafe!.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\I Won Mr. Beast's $1,000,000 Youtuber Challenge\I Won Mr. Beast's $1,000,000 Youtuber Challenge.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\If you don't like reading, I've got the book for you\If you don't like reading, I've got the book for you.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\I'm too Awkward for My Own Good\I'm too Awkward for My Own Good.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\I'm Totally a Skating Pro\I'm Totally a Skating Pro.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Injuries & Being Sick\Injuries & Being Sick.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\JaidenAnimations Intro!\JaidenAnimations Intro!.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\JaidenAnimations the Anime\JaidenAnimations the Anime.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Living with Ari\Living with Ari.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Locked out of my House\Locked out of my House.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\My Biggest Hyperfixation Yet#\My Biggest Hyperfixation Yet..mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\My Birds Laid Eggs..#\My Birds Laid Eggs....mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\My Childhood Obsession with Animals\My Childhood Obsession with Animals.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\My Childhood Stories\My Childhood Stories.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\My Dog Stories\My Dog Stories.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\My Embarrassing Old Plays w⧸ theodd1sout\My Embarrassing Old Plays w⧸ theodd1sout.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\My Experience Living in Los Angeles\My Experience Living in Los Angeles.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\My Experience with Sports\My Experience with Sports.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\My First Time Playing DUNGEONS & DRAGONS\My First Time Playing DUNGEONS & DRAGONS.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\My Funniest Fan Interactions\My Funniest Fan Interactions.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\My Hero's Journey ｜ JaidenAnimations\My Hero's Journey ｜ JaidenAnimations.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\My Horrible Nightmare Group Project\My Horrible Nightmare Group Project.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\My Insane Experience with Misaki⧸Samurai Buyer (read description)\My Insane Experience with Misaki⧸Samurai Buyer (read description).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\My Instrument Experiences\My Instrument Experiences.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\My Obsession with Hatsune Miku\My Obsession with Hatsune Miku.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\My Opinion on Halloween\My Opinion on Halloween.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\My Opinion on Traveling\My Opinion on Traveling.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\My Random Thoughts\My Random Thoughts.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\My School Stories\My School Stories.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\My Time at ＂Camp Operetta＂\My Time at ＂Camp Operetta＂.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\New Member of the Family!\New Member of the Family!.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\ooooooo surprises!! (feat. ari)\ooooooo surprises!! (feat. ari).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Our tour went wrong in all the best ways\Our tour went wrong in all the best ways.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Parent Stories\Parent Stories.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Pokémon But Ugly\Pokémon But Ugly.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Pokemon Fan plays Digimon and hated it\Pokemon Fan plays Digimon and hated it.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Pokemon Ruby⧸Sapphire Medley (Piano)\Pokemon Ruby⧸Sapphire Medley (Piano).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Pokemon Sent Me ALL Their Plushies\Pokemon Sent Me ALL Their Plushies.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Pokemon sent me to Japan!\Pokemon sent me to Japan!.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Q&A #1： Why do I Animate for iHasCupquake？\Q&A #1： Why do I Animate for iHasCupquake？.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Q&A #2： What animal am I？\Q&A #2： What animal am I？.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Q&A #3： What are my Inspirations？\Q&A #3： What are my Inspirations？.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Random Thoughts (Part 2 Edition)\Random Thoughts (Part 2 Edition).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Ruining Everything w⧸ DrawWithJazza\Ruining Everything w⧸ DrawWithJazza.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Sneaky Advertisements\Sneaky Advertisements.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\So it's been 10 years huh..#\So it's been 10 years huh....mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Speedrunning a rhythm game is hard\Speedrunning a rhythm game is hard.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Strange Video Games I Played as a Kid\Strange Video Games I Played as a Kid.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Stupid Lies I Believed for Way Too Long\Stupid Lies I Believed for Way Too Long.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\The best pokemon game you never played\The best pokemon game you never played.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\The Closest Feeling to Death that isn't Death\The Closest Feeling to Death that isn't Death.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\The College Struggle\The College Struggle.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\The Darkest Pokemon game you've never played\The Darkest Pokemon game you've never played.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\The Funny Stories Tag (w⧸ TonyvToons)\The Funny Stories Tag (w⧸ TonyvToons).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\The game Nintendo wants you to forget\The game Nintendo wants you to forget.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\The Hardest Mario Game Ever\The Hardest Mario Game Ever.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\The History of my Hair\The History of my Hair.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\The Most Underrated Game Ever\The Most Underrated Game Ever.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\The Weirdest Pet Games\The Weirdest Pet Games.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\The worst thing that's ever happened to me\The worst thing that's ever happened to me.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\theodd1sout and I Complain About Arizona\theodd1sout and I Complain About Arizona.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Thesaurus.com is Kinda Dumb\Thesaurus.com is Kinda Dumb.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\They put me in a video game..#\They put me in a video game....mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Things about Relationships I wish someone told me about\Things about Relationships I wish someone told me about.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Things I Do that Adults Probably Don't Do (Jaiden Edition)\Things I Do that Adults Probably Don't Do (Jaiden Edition).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Things I Feel Guilty About\Things I Feel Guilty About.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Things that Freak Me Out\Things that Freak Me Out.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Things that Freak Me Out (part 2)\Things that Freak Me Out (part 2).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Things that Happened While I Grew up\Things that Happened While I Grew up.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\To the Moon (Piano)\To the Moon (Piano).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Trying to Get Into Fitness & Health\Trying to Get Into Fitness & Health.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Vidcon fun times wow so fun excitement 10⧸10 would vidcon again\Vidcon fun times wow so fun excitement 10⧸10 would vidcon again.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Watching my childhood videos w⧸ theodd1sout\Watching my childhood videos w⧸ theodd1sout.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Welcome to JaidenAnimations! (the better intro)\Welcome to JaidenAnimations! (the better intro).mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\What it's Been Like to be on YouTube\What it's Been Like to be on YouTube.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\What my trip to Japan was like\What my trip to Japan was like.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Why I Love⧸Hate Reality TV\Why I Love⧸Hate Reality TV.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\Winter & My Traumatic Skiing Trip\Winter & My Traumatic Skiing Trip.mkv"),
            new(@"D:\Youtube Channels\JaidenAnimations\Videos\you guys BEGGED for this\you guys BEGGED for this.mkv"),
        ];
    }
}
