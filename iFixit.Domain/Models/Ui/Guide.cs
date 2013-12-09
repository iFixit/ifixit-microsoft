using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;

namespace iFixit.Domain.Models.UI
{
    public interface IGuidePagePage
    {

    }

    public class Flag : ModelBase
    {
        private string _Title;
        public string Title
        {
            get { return _Title; }
            set
            {
                if (value != _Title)
                {
                    _Title = value;
                    NotifyPropertyChanged();
                }
            }


        }
        private string _Text;
        public string Text
        {
            get { return _Text; }
            set
            {
                if (value != _Text)
                {
                    _Text = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _Type;
        public string Type
        {
            get { return this._Type; }
            set
            {
                if (_Type != value)
                {
                    _Type = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }

    public enum GuideStepLineIcon { Black = 1, Red = 2, Orange = 3, Yellow = 4, Green = 5, Blue = 6, Violet = 7, Icon_Note = 8, Icon_reminder = 9, Icon_Caution = 10, Teal = 11 };

    public enum GuidesPageTypes { Intro, Prerequisites, Step }

    public class GuideBasePage : ModelBase, IGuidePagePage
    {

        private string _PageTitle;
        public string PageTitle
        {
            get { return this._PageTitle; }
            set
            {
                if (_PageTitle != value)
                {
                    _PageTitle = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private GuidesPageTypes _Type;
        public GuidesPageTypes Type
        {
            get { return this._Type; }
            set
            {
                if (_Type != value)
                {
                    _Type = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string _PageType;
        public string PageType
        {
            get { return this._PageType; }
            set
            {
                if (_PageType != value)
                {
                    _PageType = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _StepTitle;
        public string StepTitle
        {
            get { return this._StepTitle; }
            set
            {
                if (_StepTitle != value)
                {
                    _StepTitle = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }

    public class GuidePrerequisites : GuideBasePage
    {
        private ObservableCollection<SearchResultItem> _Items = new ObservableCollection<SearchResultItem>();
        public ObservableCollection<SearchResultItem> Items
        {
            get { return _Items; }
            set
            {
                if (value != _Items)
                {
                    _Items = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public GuidePrerequisites()
        {
            this.PageType = "prerequisites";
            this.Type = GuidesPageTypes.Prerequisites;
        }
    }

    public class GuideIntro : GuideBasePage
    {


        private string _Subject;
        public string Subject
        {
            get { return _Subject; }
            set
            {
                if (value != _Subject)
                {
                    _Subject = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _Difficulty;
        public string Difficulty
        {
            get { return _Difficulty; }
            set
            {
                if (value != _Difficulty)
                {
                    _Difficulty = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _TimeRequired;
        public string TimeRequired
        {
            get { return _TimeRequired; }
            set
            {
                if (value != _TimeRequired)
                {
                    _TimeRequired = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string _Summary;
        public string Summary
        {
            get { return _Summary; }
            set
            {
                if (value != _Summary)
                {
                    _Summary = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _Introduction;
        public string Introduction
        {
            get { return _Introduction; }
            set
            {
                if (value != _Introduction)
                {
                    _Introduction = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string _TypeOfGuide;
        public string TypeOfGuide
        {
            get { return _TypeOfGuide; }
            set
            {
                if (value != _TypeOfGuide)
                {
                    _TypeOfGuide = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _Author;
        public string Author
        {
            get { return _Author; }
            set
            {
                if (value != _Author)
                {
                    _Author = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private bool _HasPartsAndTools = false;
        public bool HasPartsAndTools
        {
            get { return this._HasPartsAndTools; }
            set
            {
                if (_HasPartsAndTools != value)
                {
                    _HasPartsAndTools = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private bool _HasPreRequisites = false;
        public bool HasPreRequisites
        {
            get { return this._HasPreRequisites; }
            set
            {
                if (_HasPreRequisites != value)
                {
                    _HasPreRequisites = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private ObservableCollection<SearchResultItem> _PreRequisites = new ObservableCollection<SearchResultItem>();
        public ObservableCollection<SearchResultItem> PreRequisites
        {
            get { return _PreRequisites; }
            set
            {
                if (value != _PreRequisites)
                {
                    _PreRequisites = value;

                    NotifyPropertyChanged();
                }
            }
        }


        private bool _HasTools = false;
        public bool HasTools
        {
            get { return this._HasTools; }
            set
            {
                if (_HasTools != value)
                {
                    _HasTools = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private ObservableCollection<Tool> _Tools = new ObservableCollection<Tool>();
        public ObservableCollection<Tool> Tools
        {
            get { return _Tools; }
            set
            {
                if (value != _Tools)
                {
                    _Tools = value;

                    NotifyPropertyChanged();
                }
            }
        }

        private bool _HasParts = false;
        public bool HasParts
        {
            get { return this._HasParts; }
            set
            {
                if (_HasParts != value)
                {
                    _HasParts = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private ObservableCollection<Tool> _Parts = new ObservableCollection<Tool>();
        public ObservableCollection<Tool> Parts
        {
            get { return _Parts; }
            set
            {
                if (value != _Parts)
                {
                    _Tools = value;

                    NotifyPropertyChanged();
                }
            }
        }


        private ObservableCollection<Flag> _Flags = new ObservableCollection<Flag>();
        public ObservableCollection<Flag> Flags
        {
            get { return _Flags; }
            set
            {
                if (value != _Flags)
                {
                    _Flags = value;

                    NotifyPropertyChanged();
                }
            }
        }


        private bool _HasDocuments;
        public bool HasDocuments
        {
            get { return Documents.Count > 0; }
            set
            {
                if (_HasDocuments != value)
                {
                    _HasDocuments = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private ObservableCollection<Document> _Documents = new ObservableCollection<Document>();
        public ObservableCollection<Document> Documents
        {
            get { return this._Documents; }
            set
            {
                if (_Documents != value)
                {
                    _Documents = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged("HasDocuments");
                }
            }
        }


        public GuideIntro()
        {

            this.PageType = "intro";
            this.Type = GuidesPageTypes.Intro;
        }
    }

    //public class Guide : ModelBase
    //{


    //    private ObservableCollection<IGuidePagePage> _Pages = new ObservableCollection<IGuidePagePage>();
    //    public ObservableCollection<IGuidePagePage> Pages
    //    {
    //        get { return _Pages; }
    //        set
    //        {
    //            if (value != _Pages)
    //            {
    //                _Pages = value;
    //                NotifyPropertyChanged();
    //            }
    //        }
    //    }

    //}

    public class GuideStepVideo : ModelBase
    {

        private string _VideoUrl = string.Empty;
        public string VideoUrl
        {
            get { return this._VideoUrl; }
            set
            {
                if (_VideoUrl != value)
                {
                    _VideoUrl = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _ImageUrl = string.Empty;
        public string ImageUrl
        {
            get { return this._ImageUrl; }
            set
            {
                if (_ImageUrl != value)
                {
                    _ImageUrl = value;
                    NotifyPropertyChanged();
                }
            }
        }


    }

    public class GuideStepItem : GuideBasePage
    {

        private int _StepIndex;
        public int StepIndex
        {
            get { return _StepIndex; }
            set
            {
                if (value != _StepIndex)
                {
                    _StepIndex = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private int _index;
        public int index
        {
            get { return _index; }
            set
            {
                if (value != _index)
                {
                    _index = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private GuideStepVideo _Video = new GuideStepVideo();
        public GuideStepVideo Video
        {
            get { return this._Video; }
            set
            {
                if (_Video != value)
                {
                    _Video = value;
                    NotifyPropertyChanged();
                }
            }
        }



        private bool _PlayVideo = false;
        public bool PlayVideo
        {
            get { return _PlayVideo; }
            set
            {
                if (value != _PlayVideo)
                {
                    _PlayVideo = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private GuideStepImage _MainImage = new GuideStepImage();
        public GuideStepImage MainImage
        {
            get { return _MainImage; }
            set
            {
                if (value != _MainImage)
                {
                    _MainImage = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private ObservableCollection<GuideStepLine> _Lines = new ObservableCollection<GuideStepLine>();
        public ObservableCollection<GuideStepLine> Lines
        {
            get { return _Lines; }
            set
            {
                if (value != _Lines)
                {
                    _Lines = value;
                    NotifyPropertyChanged();
                }
            }
        }



        private ObservableCollection<GuideStepImage> _Images = new ObservableCollection<GuideStepImage>();
        public ObservableCollection<GuideStepImage> Images
        {
            get { return _Images; }
            set
            {
                if (value != _Images)
                {
                    _Images = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private bool _ShowListImages=true;
        public bool ShowListImages
        {
            get { return this._ShowListImages; }
            set
            {
                if (_ShowListImages != value)
                {
                    _ShowListImages = value;
                    NotifyPropertyChanged();
                }
            }
        }

        

        public GuideStepItem()
        {
            this.PageType = "step";
            this.Type = GuidesPageTypes.Step;
        }

    }

    public class GuideStepImage : ModelBase
    {

        private int _ImageIndex;
        public int ImageIndex
        {
            get { return _ImageIndex; }
            set
            {
                if (value != _ImageIndex)
                {
                    _ImageIndex = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string _ImageId;
        public string ImageId
        {
            get { return _ImageId; }
            set
            {
                if (value != _ImageId)
                {
                    _ImageId = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private int _StepIndex;
        public int StepIndex
        {
            get { return _StepIndex; }
            set
            {
                if (value != _StepIndex)
                {
                    _StepIndex = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string _MediumImageUrl;
        public string MediumImageUrl
        {
            get { return _MediumImageUrl; }
            set
            {
                if (value != _MediumImageUrl)
                {
                    _MediumImageUrl = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _LargeImageUrl;
        public string LargeImageUrl
        {
            get { return _LargeImageUrl; }
            set
            {
                if (value != _LargeImageUrl)
                {
                    _LargeImageUrl = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _SmallImageUrl;
        public string SmallImageUrl
        {
            get { return this._SmallImageUrl; }
            set
            {
                if (_SmallImageUrl != value)
                {
                    _SmallImageUrl = value;
                    NotifyPropertyChanged();
                }
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

        private GuideStepLineIcon _GuideIcon;
        public GuideStepLineIcon GuideIcon
        {
            get { return _GuideIcon; }
            set
            {
                if (value != _GuideIcon)
                {
                    _GuideIcon = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string _Icon;
        public string Icon
        {
            get { return _Icon; }
            set
            {
                if (value != _Icon)
                {
                    _Icon = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string _BulletColor;
        public string BulletColor
        {
            get { return _BulletColor; }
            set
            {
                if (value != _BulletColor)
                {
                    _BulletColor = value;
                    NotifyPropertyChanged();
                }
            }
        }



        private AdvancedText _Body = new AdvancedText();
        public AdvancedText Body
        {
            get { return _Body; }
            set
            {
                if (value != _Body)
                {
                    _Body = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string _Index;
        public string Index
        {
            get { return _Index; }
            set
            {
                if (value != _Index)
                {
                    _Index = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string _Title;
        public string Title
        {
            get { return _Title; }
            set
            {
                if (value != _Title)
                {
                    _Title = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string _VoiceText;
        public string VoiceText
        {
            get { return this._VoiceText; }
            set
            {
                if (_VoiceText != value)
                {
                    _VoiceText = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private bool _BeingRead = false;
        public bool BeingRead
        {
            get { return this._BeingRead; }
            set
            {
                if (_BeingRead != value)
                {
                    _BeingRead = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private int _Level;
        public int Level
        {
            get { return _Level; }
            set
            {
                if (value != _Level)
                {
                    _Level = value;
                    NotifyPropertyChanged();
                }
            }
        }


    }







}
