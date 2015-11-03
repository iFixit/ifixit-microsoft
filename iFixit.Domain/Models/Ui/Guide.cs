using System.Collections.ObjectModel;

namespace iFixit.Domain.Models.UI
{
    public interface IGuidePagePage
    {

    }

    public class Flag : ModelBase
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                if (value == _title) return;
                _title = value;
                NotifyPropertyChanged();
            }


        }
        private string _text;
        public string Text
        {
            get { return _text; }
            set
            {
                if (value == _text) return;
                _text = value;
                NotifyPropertyChanged();
            }
        }

        private string _type;
        public string Type
        {
            get { return _type; }
            set
            {
                if (_type == value) return;
                _type = value;
                NotifyPropertyChanged();
            }
        }
    }

    public enum GuideStepLineIcon { Black = 1, Red = 2, Orange = 3, Yellow = 4, Green = 5, Blue = 6, Violet = 7, Icon_Note = 8, Icon_reminder = 9, Icon_Caution = 10, Teal = 11 };

    public enum GuidesPageTypes { Intro, Prerequisites, Step }

    public class GuideBasePage : ModelBase, IGuidePagePage
    {

        private string _pageTitle;
        public string PageTitle
        {
            get { return _pageTitle; }
            set
            {
                if (_pageTitle == value) return;
                _pageTitle = value;
                NotifyPropertyChanged();
            }
        }


        private GuidesPageTypes _type;
        public GuidesPageTypes Type
        {
            get { return _type; }
            set
            {
                if (_type == value) return;
                _type = value;
                NotifyPropertyChanged();
            }
        }


        private string _pageType;
        public string PageType
        {
            get { return _pageType; }
            set
            {
                if (_pageType == value) return;
                _pageType = value;
                NotifyPropertyChanged();
            }
        }

        private string _stepTitle;
        public string StepTitle
        {
            get { return _stepTitle; }
            set
            {
                if (_stepTitle == value) return;
                _stepTitle = value;
                NotifyPropertyChanged();
            }
        }
    }

    public class GuidePrerequisites : GuideBasePage
    {
        private ObservableCollection<SearchResultItem> _items = new ObservableCollection<SearchResultItem>();
        public ObservableCollection<SearchResultItem> Items
        {
            get { return _items; }
            set
            {
                if (value == _items) return;
                _items = value;
                NotifyPropertyChanged();
            }
        }

        public GuidePrerequisites()
        {
            PageType = "prerequisites";
            Type = GuidesPageTypes.Prerequisites;
        }
    }

    public class GuideIntro : GuideBasePage
    {


        private string _subject;
        public string Subject
        {
            get { return _subject; }
            set
            {
                if (value == _subject) return;
                _subject = value;
                NotifyPropertyChanged();
            }
        }

        private string _difficulty;
        public string Difficulty
        {
            get { return _difficulty; }
            set
            {
                if (value == _difficulty) return;
                _difficulty = value;
                NotifyPropertyChanged();
            }
        }

        private string _timeRequired;
        public string TimeRequired
        {
            get { return _timeRequired; }
            set
            {
                if (value == _timeRequired) return;
                _timeRequired = value;
                NotifyPropertyChanged();
            }
        }


        private string _summary;
        public string Summary
        {
            get { return _summary; }
            set
            {
                if (value == _summary) return;
                _summary = value;
                NotifyPropertyChanged();
            }
        }

        private string _introduction;
        public string Introduction
        {
            get { return _introduction; }
            set
            {
                if (value == _introduction) return;
                _introduction = value;
                NotifyPropertyChanged();
            }
        }


        private string _typeOfGuide;
        public string TypeOfGuide
        {
            get { return _typeOfGuide; }
            set
            {
                if (value == _typeOfGuide) return;
                _typeOfGuide = value;
                NotifyPropertyChanged();
            }
        }

        private string _author;
        public string Author
        {
            get { return _author; }
            set
            {
                if (value == _author) return;
                _author = value;
                NotifyPropertyChanged();
            }
        }

        private bool _hasPartsAndTools = false;
        public bool HasPartsAndTools
        {
            get { return _hasPartsAndTools; }
            set
            {
                if (_hasPartsAndTools == value) return;
                _hasPartsAndTools = value;
                NotifyPropertyChanged();
            }
        }

        private bool _hasPreRequisites = false;
        public bool HasPreRequisites
        {
            get { return _hasPreRequisites; }
            set
            {
                if (_hasPreRequisites == value) return;
                _hasPreRequisites = value;
                NotifyPropertyChanged();
            }
        }


        private ObservableCollection<SearchResultItem> _preRequisites = new ObservableCollection<SearchResultItem>();
        public ObservableCollection<SearchResultItem> PreRequisites
        {
            get { return _preRequisites; }
            set
            {
                if (value == _preRequisites) return;
                _preRequisites = value;

                NotifyPropertyChanged();
            }
        }


        private bool _hasTools = false;
        public bool HasTools
        {
            get { return _hasTools; }
            set
            {
                if (_hasTools == value) return;
                _hasTools = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<Tool> _tools = new ObservableCollection<Tool>();
        public ObservableCollection<Tool> Tools
        {
            get { return _tools; }
            set
            {
                if (value == _tools) return;
                _tools = value;

                NotifyPropertyChanged();
            }
        }

        private bool _hasParts = false;
        public bool HasParts
        {
            get { return _hasParts; }
            set
            {
                if (_hasParts == value) return;
                _hasParts = value;
                NotifyPropertyChanged();
            }
        }
        private ObservableCollection<Tool> _parts = new ObservableCollection<Tool>();
        public ObservableCollection<Tool> Parts
        {
            get { return _parts; }
            set
            {
                if (value == _parts) return;
                _tools = value;

                NotifyPropertyChanged();
            }
        }


        private ObservableCollection<Flag> _flags = new ObservableCollection<Flag>();
        public ObservableCollection<Flag> Flags
        {
            get { return _flags; }
            set
            {
                if (value == _flags) return;
                _flags = value;

                NotifyPropertyChanged();
            }
        }


        private bool _hasDocuments;
        public bool HasDocuments
        {
            get { return Documents.Count > 0; }
            set
            {
                if (_hasDocuments == value) return;
                _hasDocuments = value;
                NotifyPropertyChanged();
            }
        }


        private ObservableCollection<Document> _documents = new ObservableCollection<Document>();
        public ObservableCollection<Document> Documents
        {
            get { return _documents; }
            set
            {
                if (_documents == value) return;
                _documents = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("HasDocuments");
            }
        }


        public GuideIntro()
        {

            PageType = "intro";
            Type = GuidesPageTypes.Intro;
        }
    }



    public class GuideStepVideo : ModelBase
    {

        private string _videoUrl = string.Empty;
        public string VideoUrl
        {
            get { return _videoUrl; }
            set
            {
                if (_videoUrl == value) return;
                _videoUrl = value;
                NotifyPropertyChanged();
            }
        }

        private string _imageUrl = string.Empty;
        public string ImageUrl
        {
            get { return _imageUrl; }
            set
            {
                if (_imageUrl == value) return;
                _imageUrl = value;
                NotifyPropertyChanged();
            }
        }


    }

    public class GuideStepItem : GuideBasePage
    {

        private int _stepIndex;
        public int StepIndex
        {
            get { return _stepIndex; }
            set
            {
                if (value == _stepIndex) return;
                _stepIndex = value;
                NotifyPropertyChanged();
            }
        }

        private int _index;
        public int index
        {
            get { return _index; }
            set
            {
                if (value == _index) return;
                _index = value;
                NotifyPropertyChanged();
            }
        }


        private GuideStepVideo _video = new GuideStepVideo();
        public GuideStepVideo Video
        {
            get { return _video; }
            set
            {
                if (_video == value) return;
                _video = value;
                NotifyPropertyChanged();
            }
        }



        private bool _playVideo = false;
        public bool PlayVideo
        {
            get { return _playVideo; }
            set
            {
                if (value == _playVideo) return;
                _playVideo = value;
                NotifyPropertyChanged();
            }
        }

        private GuideStepImage _mainImage = new GuideStepImage();
        public GuideStepImage MainImage
        {
            get { return _mainImage; }
            set
            {
                if (value == _mainImage) return;
                _mainImage = value;
                NotifyPropertyChanged();
            }
        }


        private ObservableCollection<GuideStepLine> _lines = new ObservableCollection<GuideStepLine>();
        public ObservableCollection<GuideStepLine> Lines
        {
            get { return _lines; }
            set
            {
                if (value == _lines) return;
                _lines = value;
                NotifyPropertyChanged();
            }
        }



        private ObservableCollection<GuideStepImage> _images = new ObservableCollection<GuideStepImage>();
        public ObservableCollection<GuideStepImage> Images
        {
            get { return _images; }
            set
            {
                if (value == _images) return;
                _images = value;
                NotifyPropertyChanged();
            }
        }


        private bool _showListImages=true;
        public bool ShowListImages
        {
            get { return _showListImages; }
            set
            {
                if (_showListImages == value) return;
                _showListImages = value;
                NotifyPropertyChanged();
            }
        }

        

        public GuideStepItem()
        {
            PageType = "step";
            Type = GuidesPageTypes.Step;
        }

    }

    public class GuideStepImage : ModelBase
    {

        private int _imageIndex;
        public int ImageIndex
        {
            get { return _imageIndex; }
            set
            {
                if (value == _imageIndex) return;
                _imageIndex = value;
                NotifyPropertyChanged();
            }
        }


        private string _imageId;
        public string ImageId
        {
            get { return _imageId; }
            set
            {
                if (value == _imageId) return;
                _imageId = value;
                NotifyPropertyChanged();
            }
        }


        private int _stepIndex;
        public int StepIndex
        {
            get { return _stepIndex; }
            set
            {
                if (value == _stepIndex) return;
                _stepIndex = value;
                NotifyPropertyChanged();
            }
        }


        private string _mediumImageUrl;
        public string MediumImageUrl
        {
            get { return _mediumImageUrl; }
            set
            {
                if (value == _mediumImageUrl) return;
                _mediumImageUrl = value;
                NotifyPropertyChanged();
            }
        }

        private string _largeImageUrl;
        public string LargeImageUrl
        {
            get { return _largeImageUrl; }
            set
            {
                if (value == _largeImageUrl) return;
                _largeImageUrl = value;
                NotifyPropertyChanged();
            }
        }

        private string _smallImageUrl;
        public string SmallImageUrl
        {
            get { return _smallImageUrl; }
            set
            {
                if (_smallImageUrl == value) return;
                _smallImageUrl = value;
                NotifyPropertyChanged();
            }
        }


    }

    public class GuideStepLine : ModelBase
    {

        public static GuideStepLineIcon SetIcon(string val)
        {

            switch (val)
            {
                case "black":
                    return GuideStepLineIcon.Black;
                case "red":
                    return GuideStepLineIcon.Red;
                case "orange":
                    return GuideStepLineIcon.Orange;
                case "yellow":
                    return GuideStepLineIcon.Yellow;
                case "green":
                    return GuideStepLineIcon.Green;
                case "violet":
                    return GuideStepLineIcon.Violet;
                case "blue":
                    return GuideStepLineIcon.Blue;
                case "teal":
                    return GuideStepLineIcon.Teal;
                case "icon_note":
                    return GuideStepLineIcon.Icon_Note;
                case "icon_reminder":
                    return GuideStepLineIcon.Icon_reminder;
                case "icon_caution":
                    return GuideStepLineIcon.Icon_Caution;
                default:
                    return GuideStepLineIcon.Black;
            }
        }

        public static string SetIconColor(string val)
        {
            //TODO : real colors
            switch (val)
            {

                case "black":
                    return "#000000";
                case "red":
                    return "#c1280b";
                case "orange":
                    return "#ff9024";
                case "yellow":
                    return "#f3e00e";
                case "green":
                    return "#16dc81";
                case "violet":
                    return "#dc54b7";
                case "blue":
                    return "#2343e8";
                case "teal":
                    return "#1bb1e9";

                case "icon_reminder":
                case "icon_caution":
                case "icon_note":
                default:
                    return "#3361AD";
            }
        }

        private GuideStepLineIcon _guideIcon;
        public GuideStepLineIcon GuideIcon
        {
            get { return _guideIcon; }
            set
            {
                if (value == _guideIcon) return;
                _guideIcon = value;
                NotifyPropertyChanged();
            }
        }


        private string _icon;
        public string Icon
        {
            get { return _icon; }
            set
            {
                if (value == _icon) return;
                _icon = value;
                NotifyPropertyChanged();
            }
        }


        private string _bulletColor;
        public string BulletColor
        {
            get { return _bulletColor; }
            set
            {
                if (value == _bulletColor) return;
                _bulletColor = value;
                NotifyPropertyChanged();
            }
        }



        private AdvancedText _body = new AdvancedText();
        public AdvancedText Body
        {
            get { return _body; }
            set
            {
                if (value == _body) return;
                _body = value;
                NotifyPropertyChanged();
            }
        }


        private string _index;
        public string Index
        {
            get { return _index; }
            set
            {
                if (value == _index) return;
                _index = value;
                NotifyPropertyChanged();
            }
        }


        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                if (value == _title) return;
                _title = value;
                NotifyPropertyChanged();
            }
        }


        private string _voiceText;
        public string VoiceText
        {
            get { return _voiceText; }
            set
            {
                if (_voiceText == value) return;
                _voiceText = value;
                NotifyPropertyChanged();
            }
        }


        private bool _beingRead = false;
        public bool BeingRead
        {
            get { return _beingRead; }
            set
            {
                if (_beingRead == value) return;
                _beingRead = value;
                NotifyPropertyChanged();
            }
        }


        private int _level;
        public int Level
        {
            get { return _level; }
            set
            {
                if (value == _level) return;
                _level = value;
                NotifyPropertyChanged();
            }
        }


    }







}
