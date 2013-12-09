using GalaSoft.MvvmLight.Command;
using iFixit.Domain.Code;
using iFixit.Domain.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicesEngine = iFixit.Domain.Services.V2_0;
using RESTModels = iFixit.Domain.Models.REST.V2_0;
using System.Diagnostics;

namespace iFixit.Domain.ViewModels
{
    public class Guide : BaseViewModel
    {

        #region "properties"

        private string currentGuideId = string.Empty;

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


        private string _FullImagePath;
        public string FullImagePath
        {
            get { return this._FullImagePath; }
            set
            {
                if (_FullImagePath != value)
                {
                    _FullImagePath = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private bool _ShowingFullImage = false;
        public bool ShowingFullImage
        {
            get { return this._ShowingFullImage; }
            set
            {

                _ShowingFullImage = value;
                NotifyPropertyChanged();

            }
        }


        private int _SelectedPageIndex = 0;
        public int SelectedPageIndex
        {
            get
            {

                return _SelectedPageIndex;
            }
            set
            {
                if (_SelectedPageIndex != value)
                {
                    _SelectedPageIndex = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private Models.UI.GuideBasePage _SelectedPage;
        public Models.UI.GuideBasePage SelectedPage
        {
            get { return this._SelectedPage; }
            set
            {
                if (_SelectedPage != value)
                {
                    _SelectedPage = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private int _SelectedStepLine = 0;
        public int SelectedStepLine
        {
            get { return this._SelectedStepLine; }
            set
            {
                if (_SelectedStepLine != value)
                {
                    _SelectedStepLine = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string _GuideUrl;
        public string GuideUrl
        {
            get { return _GuideUrl; }
            set
            {
                if (value != _GuideUrl)
                {
                    _GuideUrl = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string _GuideTitle;
        public string GuideTitle
        {
            get { return _GuideTitle; }
            set
            {
                if (value != _GuideTitle)
                {
                    _GuideTitle = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string _Dificulty;
        public string Dificulty
        {
            get { return _Dificulty; }
            set
            {
                if (value != _Dificulty)
                {
                    _Dificulty = value;
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

        private string _GuideMainImage;
        public string GuideMainImage
        {
            get { return _GuideMainImage; }
            set
            {
                if (value != _GuideMainImage)
                {
                    _GuideMainImage = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private ObservableCollection<Models.UI.GuideBasePage> _Items = new ObservableCollection<Models.UI.GuideBasePage>();
        public ObservableCollection<Models.UI.GuideBasePage> Items
        {
            get { return this._Items; }
            set
            {
                if (_Items != value)
                {
                    _Items = value;
                    NotifyPropertyChanged();
                }
            }
        }



        private bool _NotFavorite = true;
        public bool NotFavorite
        {
            get { return this._NotFavorite; }
            set
            {
                if (_NotFavorite != value)
                {
                    _NotFavorite = value;
                    NotifyPropertyChanged();
                }
            }
        }


        #endregion

        #region Commands

        private RelayCommand<Models.UI.SearchResultItem> _GoToGuide;
        public RelayCommand<Models.UI.SearchResultItem> GoToGuide
        {
            get
            {
                return _GoToGuide ?? (_GoToGuide = new RelayCommand<Models.UI.SearchResultItem>(
                 (SelectedGuide) =>
                 {

                     try
                     {
                         LoadingCounter++;
                         AppBase.Current.GuideId = SelectedGuide.UniqueId;
                         _uxService.CancelSpeech();
                         _navigationService.Navigate<Guide>(true, SelectedGuide.UniqueId);
                         SelectedGuide = null;
                         LoadingCounter--;
                     }
                     catch (Exception ex)
                     {
                         LoadingCounter--;
                         throw ex;
                     }

                 }));
            }
        }

        private RelayCommand<string> _ShowVideo;
        public RelayCommand<string> ShowVideo
        {
            get
            {
                return _ShowVideo ?? (_ShowVideo = new RelayCommand<string>(
                    (url) =>
                    {
                        _uxService.CancelSpeech();
                        _uxService.ShowVideo(url);
                    }));
            }

        }


        private RelayCommand _GetGuide;
        public RelayCommand GetGuide
        {
            get
            {
                return _GetGuide ?? (_GetGuide = new RelayCommand(
                 async () =>
                 {
                     LoadingCounter++;
                     AppBase.Current.GuideId = this.NavigationParameter<string>();


                     //TODO: Check if Guide is in Favorites List no need to Download
                     var fileName = string.Format(Constants.GUIDE, AppBase.Current.GuideId);
                     var isCached = await _storageService.Exists(fileName, new TimeSpan(5, 0, 0, 0));
                     RESTModels.Guide.RootObject rp = null;
                     if (isCached)
                     {
                         Debug.WriteLine("Cached Guide :" + AppBase.Current.GuideId);
                         var jobj = await _storageService.ReadData(fileName);
                         rp = jobj.LoadFromJson<RESTModels.Guide.RootObject>();
                         await BindGuide(rp);
                     }
                     else

                         if (_settingsService.IsConnectedToInternet())
                         {

                             try
                             {

                                 Reset();
                                 LoadingCounter++;
                                 Debug.WriteLine("Going for Guide :" + AppBase.Current.GuideId);
                                 rp = await Broker.GetGuide(AppBase.Current.GuideId);
                                 _storageService.Save(fileName, await rp.SaveAsJson());
                                 Debug.WriteLine("Caching Guide :" + AppBase.Current.GuideId);

                                 await BindGuide(rp);

                                 LoadingCounter--;

                             }
                             catch (Exception ex)
                             {
                                 LoadingCounter--;
                                 throw ex;
                             }

                         }



                         else
                         {
                             await _uxService.ShowAlert(International.Translation.NoConnection);

                         }


                     LoadingCounter--;





                 }));
            }
        }

        private async Task<string> ImagePathTranslated(string Url)
        {
            var isFavorite = await _storageService.FolderExists(string.Format(Constants.GUIDE_CACHE_FOLDER, this.currentGuideId));
            //TODO: 
            string imagePath = Url;
            this.NotFavorite = !isFavorite;

            //if (this.IsOffline || isFavorite)
            //{
            //    string imageName = Url.Substring(Url.LastIndexOf('/') + 1);
            //    if (await _storageService.Exists(imageName, string.Format(Constants.GUIDE_CACHE_FOLDER, this.currentGuideId)))
            //    {
            //        imagePath = _storageService.BasePath() + string.Format(Constants.GUIDE_CACHE_FOLDER, this.currentGuideId) + "/" + imageName;
            //    }
            //}

            return imagePath;
        }

        private async Task BindGuide(RESTModels.Guide.RootObject rp)
        {
            //TODO: TESTE
            Items.Clear();
            // DANGEROUS
            currentGuideId = AppBase.Current.GuideId;

            this.GuideTitle = rp.title;
            this.GuideUrl = rp.url;

            if (!string.IsNullOrEmpty(rp.author.username))
                this.Author = string.Format(International.Translation.GuideBy, rp.author.username);

            if (rp.image != null)
                this.GuideMainImage = rp.image.large;

            Models.UI.GuideIntro firstPage = new Models.UI.GuideIntro()
            {
                TypeOfGuide = rp.type
                ,
                Subject = rp.subject
                ,
                Summary = rp.summary
                ,
                PageTitle = International.Translation.Intro
                ,
                Type = Models.UI.GuidesPageTypes.Intro
                ,
                StepTitle = International.Translation.Intro
                ,
                Difficulty = rp.difficulty
                ,
                TimeRequired = rp.time_required
                ,
                Author = rp.author.username
                ,
                Introduction = rp.introduction_rendered

            };

            //if (rp.flags != null)
            //{
            //    foreach (var item in rp.flags)
            //    {
            //        firstPage.Flags.Add(new Models.UI.Flag
            //        {
            //             Text = item.text, Title = item.title, Type = item.flagid
            //        });
            //    }
            //}


            //for (int i = 0; i < rp.prerequisites.Count(); i++)
            //{
            //    var item = rp.prerequisites[i];
            //    firstPage.PreRequisites.Add(new Models.UI.SearchResultItem
            //    {
            //        UniqueId = item.guideid.ToString(),
            //        Name = string.Format("{0}) {1}", i + 1, item.title),
            //        ImageUrl = item.image != null ? await ImagePathTranslated(item.image.medium) : "",
            //        Summary = item.subject
            //    });
            //}
            if (firstPage.PreRequisites.Count > 0)
                firstPage.HasPreRequisites = true;



            for (int i = 0; i < rp.parts.Count(); i++)
            {
                var item = rp.parts[i];
                firstPage.Parts.Add(new Models.UI.Tool
                {
                    Image = await ImagePathTranslated(item.thumbnail),
                    Title = GeneratePartName(item),
                    Url = item.url
                });
            }
            if (firstPage.Parts.Count > 0)
                firstPage.HasParts = true;


            for (int i = 0; i < rp.tools.Count(); i++)
            {
                var item = rp.tools[i];
                firstPage.Tools.Add(new Models.UI.Tool
                {
                    Image = await ImagePathTranslated(item.thumbnail),
                    Title = item.text,
                    Url = item.url
                });
            }





            for (int i = 0; i < rp.documents.Count(); i++)
            {
                var item = rp.documents[i];
                firstPage.Documents.Add(new Models.UI.Document
                {
                    DocumentId = item.documentid

                    ,
                    Title = item.text
                    ,
                    Url = item.url
                });
            }

            if (firstPage.Tools.Count > 0)
                firstPage.HasTools = true;


            if (firstPage.HasTools && firstPage.HasParts)
                firstPage.HasPartsAndTools = true;
            else
                firstPage.HasPartsAndTools = false;
            Items.Add(firstPage);







            int stepNum = 1;
            foreach (var item in rp.steps)
            {
                Models.UI.GuideStepItem newStep = new Models.UI.GuideStepItem
                {
                    index = item.orderby,
                    PageTitle = string.Format("{0} - {1}", International.Translation.Step, stepNum)
                    ,
                    Type = Models.UI.GuidesPageTypes.Step
                    ,
                    StepTitle = string.Format(International.Translation.StepNum, stepNum)
                    ,
                    StepIndex = stepNum
                };
                int m = 0;
                foreach (var lineitem in item.lines)
                {
                    newStep.Lines.Add(new Models.UI.GuideStepLine
                    {
                        Index = (m + 1).ToString(),
                        Title = lineitem.text_rendered,
                        VoiceText = Utils.StripHTML(lineitem.text_rendered),
                        BulletColor = Models.UI.GuideStepLine.SetIconColor(lineitem.bullet),
                        GuideIcon = Models.UI.GuideStepLine.SetIcon(lineitem.bullet),
                        Level = lineitem.level
                    });
                    m++;
                }



                // images
                if (item.media != null)
                {
                    if (item.media.videodata != null)
                    {
                        newStep.Video = new Models.UI.GuideStepVideo
                        {
                            VideoUrl = item.media.videodata.encodings.Where(o => o.mime == "video/mp4").First().url,
                            ImageUrl = item.media.videodata.image.image.medium
                        };
                        newStep.MainImage = new Models.UI.GuideStepImage
                        {
                            MediumImageUrl = item.media.videodata.image.image.medium
                        };
                    }
                    else
                    {

                        int n = 0;

                        if (item.media.type == "image")
                        {



                            foreach (var media in item.media.data)
                            {

                                if (n == 0)
                                    newStep.MainImage = new Models.UI.GuideStepImage
                                    {
                                        MediumImageUrl = media != null ? await ImagePathTranslated(media.medium) : ""
                                        ,
                                        SmallImageUrl = media != null ? await ImagePathTranslated(media.standard) : ""
                                        ,
                                        LargeImageUrl = media != null ? await ImagePathTranslated(media.original) : ""
                                    };



                                newStep.Images.Add(new Models.UI.GuideStepImage
                                {
                                    StepIndex = stepNum,
                                    ImageIndex = m,
                                    ImageId = media.id.ToString(),
                                    MediumImageUrl = media != null ? await ImagePathTranslated(media.medium) : ""
                                    ,
                                    SmallImageUrl = media != null ? await ImagePathTranslated(media.standard) : ""
                                    ,
                                    LargeImageUrl = media != null ? await ImagePathTranslated(media.original) : ""
                                });

                                n++;
                            }



                        }
                    }




                }

                if (AppBase.Current.ExtendeInfoApp)
                {
                    newStep.ShowListImages = newStep.Images.Count > 1;
                }

                this.Items.Add(newStep);

                stepNum++;
            }
        }

        private static string GeneratePartName(RESTModels.Guide.Part item)
        {
            string title = item.text;
            // return string.Format("{0}, {1}, {2}", item.text, item.notes, item.type).TrimEnd(',');
            if (!string.IsNullOrEmpty(item.notes))
                title += ", " + item.notes;
            if (!string.IsNullOrEmpty(item.type))
                title += ", " + item.type;

            return title;
        }

        private RelayCommand _PauseTextSpeech;
        public RelayCommand PauseTextSpeech
        {
            get
            {
                return _PauseTextSpeech ?? (_PauseTextSpeech = new RelayCommand(
                  () =>
                  {

                      try
                      {

                          BeingRead = false;
                          _uxService.CancelSpeech();

                      }
                      catch (Exception ex)
                      {
                          LoadingCounter--;
                          throw ex;
                      }

                  }));
            }

        }

        private RelayCommand _FFTextSpeech;
        public RelayCommand FFTextSpeech
        {
            get
            {
                return _FFTextSpeech ?? (_FFTextSpeech = new RelayCommand(
                  async () =>
                  {

                      try
                      {
                          BeingRead = false;
                          if (SelectedPageIndex < Items.Count)
                          {
                              SelectedStepLine++;
                              SelectedPageIndex++;
                              _uxService.CancelSpeech();
                              await _uxService.OpenTextSpeechUI(this);
                          }


                      }
                      catch (Exception ex)
                      {

                          throw ex;
                      }

                  }));
            }

        }

        private RelayCommand _RWTextSpeech;
        public RelayCommand RWTextSpeech
        {
            get
            {
                return _RWTextSpeech ?? (_RWTextSpeech = new RelayCommand(
                 async () =>
                 {

                     try
                     {
                         BeingRead = false;
                         if (SelectedPageIndex > 0)
                         {
                             SelectedStepLine--;
                             SelectedPageIndex--;
                             _uxService.CancelSpeech();
                             await _uxService.OpenTextSpeechUI(this);
                         }


                     }
                     catch (Exception ex)
                     {

                         throw ex;
                     }

                 }));
            }

        }

        private RelayCommand _StopTextSpeech;
        public RelayCommand StopTextSpeech
        {
            get
            {
                return _StopTextSpeech ?? (_StopTextSpeech = new RelayCommand(
                  () =>
                  {

                      try
                      {

                          BeingRead = false;
                          _uxService.CancelSpeech();
                          SelectedPageIndex = 0;

                      }
                      catch (Exception ex)
                      {

                          throw ex;
                      }

                  }));
            }

        }

        private RelayCommand _Share;
        public RelayCommand Share
        {
            get
            {
                return _Share ?? (_Share = new RelayCommand(
                  () =>
                  {

                      try
                      {

                          _uxService.CancelSpeech();
                          _uxService.Share(this.GuideUrl, this.GuideTitle);

                      }
                      catch (Exception ex)
                      {
                          LoadingCounter--;
                          throw ex;
                      }

                  }));
            }

        }

        private RelayCommand _AddFavorite;
        public RelayCommand AddFavorite
        {
            get
            {
                return _AddFavorite ?? (_AddFavorite = new RelayCommand(
                  async () =>
                  {

                      try
                      {
                          _uxService.CancelSpeech();
                          if (AppBase.Current.User == null)
                          {
                              //_navigationService.Navigate<Login>(false);

                              _uxService.GoToLogin();

                          }
                          else
                          {
                              LoadingCounter++;

                              List<string> ImagesToDownload = new List<string>();
                              foreach (var item in Items)
                              {
                                  if (item.Type == Models.UI.GuidesPageTypes.Step)
                                  {
                                      var step = (Models.UI.GuideStepItem)item;

                                      ImagesToDownload.Add(step.MainImage.MediumImageUrl);

                                      foreach (var image in step.Images)
                                      {
                                          if (image != null)
                                          {
                                              ImagesToDownload.Add(image.MediumImageUrl);

                                          }

                                      }
                                  }
                              }

                              await Broker.AddFavorites(AppBase.Current.User, this.currentGuideId);

                              int HowMany = ImagesToDownload.Count();
                              Debug.WriteLine("downloading: " + HowMany.ToString());
                              int Index = 1;
                              foreach (var item in ImagesToDownload)
                              {
                                  Debug.WriteLine("downloading: " + Index.ToString());
                                  var a = await Broker.GetImage(item);

                                  string imageName = item.Substring(item.LastIndexOf('/') + 1);
                                  await _storageService.WriteBinary(string.Format(Constants.GUIDE_CACHE_FOLDER, this.currentGuideId), imageName, a);
                                  Debug.WriteLine("downloaded: " + Index.ToString());
                                  Index++;
                              }


                              //TODO: save the images and the guide as a JSON
                              //then invoke the service
                              await _uxService.ShowToast(International.Translation.GuideSuccessfullySaved);
                              NotFavorite = false;
                              LoadingCounter--;

                          }



                      }
                      catch (Exception ex)
                      {
                          LoadingCounter--;
                          throw ex;
                      }

                  }));
            }

        }


        private RelayCommand _DeleteFavorite;
        public RelayCommand DeleteFavorite
        {
            get
            {
                return _DeleteFavorite ?? (_DeleteFavorite = new RelayCommand(
                 async () =>
                 {
                     LoadingCounter++;

                     await Broker.RemoveFavorites(AppBase.Current.User, currentGuideId);
                     await _storageService.RemoveFolder(string.Format(Constants.GUIDE_CACHE_FOLDER, currentGuideId));
                     await _uxService.ShowToast(International.Translation.GuideRemovedFavorites);
                     NotFavorite = true;
                     LoadingCounter--;
                 }));
            }
        }

        private RelayCommand _OpenTextSpeech;
        public RelayCommand OpenTextSpeech
        {
            get
            {
                return _OpenTextSpeech ?? (_OpenTextSpeech = new RelayCommand(
                 async () =>
                 {

                     try
                     {
                         if (SelectedPageIndex == 0)
                             SelectedPage = Items[0];
                         await _uxService.OpenTextSpeechUI(this);

                     }
                     catch (Exception ex)
                     {

                         throw ex;
                     }

                 }));
            }

        }

        private RelayCommand<string> _ShowFullImage;
        public RelayCommand<string> ShowFullImage
        {
            get
            {
                return _ShowFullImage ?? (_ShowFullImage = new RelayCommand<string>(
                 async (image) =>
                 {

                     try
                     {


                         FullImagePath = await ImagePathTranslated(image);

                     }
                     catch (Exception ex)
                     {
                         LoadingCounter--;
                         throw ex;
                     }

                 }));
            }

        }

        private RelayCommand<Models.UI.GuideStepImage> _SwapImage;
        public RelayCommand<Models.UI.GuideStepImage> SwapImage
        {
            get
            {
                return _SwapImage ?? (_SwapImage = new RelayCommand<Models.UI.GuideStepImage>(
                  (image) =>
                  {

                      try
                      {
                          var filt = this.Items.Where(o => o.Type == Models.UI.GuidesPageTypes.Step).ToList();
                          foreach (Models.UI.GuideStepItem item in filt)
                          {
                              if (item.Images.Any(o => o == image))
                                  item.MainImage = image;

                          }


                      }
                      catch (Exception ex)
                      {
                          LoadingCounter--;
                          throw ex;
                      }

                  }));
            }

        }


        private RelayCommand _ShareNFC;
        public RelayCommand ShareNFC
        {
            get
            {
                return _ShareNFC ?? (_ShareNFC = new RelayCommand(
                  () =>
                  {

                      try
                      {

                          StartSharingSession();

                      }
                      catch (Exception ex)
                      {
                          LoadingCounter--;
                          throw ex;
                      }

                  }));
            }

        }


        #endregion


        public void Reset()
        {
            Items.Clear();
            Items = new ObservableCollection<Models.UI.GuideBasePage>();
            this.SelectedPage = null;
            // = new ObservableCollection<Models.UI.GuideBasePage>();
            this.ShowingFullImage = false;
            this.FullImagePath = string.Empty;
            this.SelectedPageIndex = 0;
            this.SelectedStepLine = 0;
            this.GuideTitle = string.Empty;
            this.GuideUrl = string.Empty;
            this.Author = string.Empty;

        }



        public void StartSharingSession()
        {
            if (!IsConnected)
            {

                _peerConnector.StartConnect();
            }
        }

        public void SendPictureToPeer(byte[] imageBytes)
        {
            if (!this.IsConnected)
            {
                StartSharingSession();
            }
            else
            {
                if (_peerConnector != null)
                {
                    _peerConnector.SendGuideAsync(imageBytes);
                }
            }
        }

        public Guide(INavigation<Domain.Interfaces.NavigationModes> navigationService, IStorage storageService, ISettings settingsService, IUxService uxService, IPeerConnector peerConnector)
            : base(navigationService, storageService, settingsService, uxService, peerConnector)
        {
            Reset();


        }


    }
}
