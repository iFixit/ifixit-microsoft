using GalaSoft.MvvmLight.Command;
using iFixit.Domain.Code;
using iFixit.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ServicesEngine = iFixit.Domain.Services.V2_0;
using RESTModels = iFixit.Domain.Models.REST.V2_0;
using System.Diagnostics;
using iFixit.Domain.Models.UI;

namespace iFixit.Domain.ViewModels
{
    public class Guide : BaseViewModel
    {

        #region "properties"

        private string currentGuideId = string.Empty;

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


        private string _fullImagePath;
        public string FullImagePath
        {
            get { return _fullImagePath; }
            set
            {
                if (_fullImagePath == value) return;
                _fullImagePath = value;
                NotifyPropertyChanged();
            }
        }


        private bool _showingFullImage = false;
        public bool ShowingFullImage
        {
            get { return _showingFullImage; }
            set
            {

                _showingFullImage = value;
                NotifyPropertyChanged();

            }
        }


        private int _selectedPageIndex = 0;
        public int SelectedPageIndex
        {
            get
            {

                return _selectedPageIndex;
            }
            set
            {
                if (_selectedPageIndex != value)
                {
                    _selectedPageIndex = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private GuideBasePage _selectedPage;
        public GuideBasePage SelectedPage
        {
            get { return _selectedPage; }
            set
            {
                if (_selectedPage != value)
                {
                    _selectedPage = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private int _selectedStepLine = 0;
        public int SelectedStepLine
        {
            get { return _selectedStepLine; }
            set
            {
                if (_selectedStepLine == value) return;
                _selectedStepLine = value;
                NotifyPropertyChanged();
            }
        }


        private string _guideUrl;
        public string GuideUrl
        {
            get { return _guideUrl; }
            set
            {
                if (value == _guideUrl) return;
                _guideUrl = value;
                NotifyPropertyChanged();
            }
        }


        private string _guideTitle;
        public string GuideTitle
        {
            get { return _guideTitle; }
            set
            {
                if (value == _guideTitle) return;
                _guideTitle = value;
                NotifyPropertyChanged();
            }
        }


        private string _dificulty;
        public string Dificulty
        {
            get { return _dificulty; }
            set
            {
                if (value == _dificulty) return;
                _dificulty = value;
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

        private string _guideMainImage;
        public string GuideMainImage
        {
            get { return _guideMainImage; }
            set
            {
                if (value == _guideMainImage) return;
                _guideMainImage = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<GuideBasePage> _items = new ObservableCollection<GuideBasePage>();
        public ObservableCollection<GuideBasePage> Items
        {
            get { return _items; }
            set
            {
                if (_items == value) return;
                _items = value;
                NotifyPropertyChanged();
            }
        }



        private bool _notFavorite = true;
        public bool NotFavorite
        {
            get { return _notFavorite; }
            set
            {
                if (_notFavorite == value) return;
                _notFavorite = value;
                NotifyPropertyChanged();
            }
        }


        #endregion

        #region Commands

        private RelayCommand<SearchResultItem> _goToGuide;
        public RelayCommand<SearchResultItem> GoToGuide
        {
            get
            {
                return _goToGuide ?? (_goToGuide = new RelayCommand<SearchResultItem>(
                 selectedGuide =>
                 {

                     try
                     {
                         LoadingCounter++;
                         AppBase.Current.GuideId = selectedGuide.UniqueId;
                         _uxService.CancelSpeech();
                         _navigationService.Navigate<Guide>(true, selectedGuide.UniqueId);
                         selectedGuide = null;
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

        private RelayCommand<string> _showVideo;
        public RelayCommand<string> ShowVideo
        {
            get
            {
                return _showVideo ?? (_showVideo = new RelayCommand<string>(
                    url =>
                    {
                        _uxService.CancelSpeech();
                        _uxService.ShowVideo(url);
                    }));
            }

        }


        private RelayCommand _getGuide;
        public RelayCommand GetGuide
        {
            get
            {
                return _getGuide ?? (_getGuide = new RelayCommand(
                 async () =>
                 {
                     LoadingCounter++;
                     AppBase.Current.GuideId = NavigationParameter<string>();


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
                                 _storageService.Save(fileName,  rp.SaveAsJson());
                                 Debug.WriteLine("Caching Guide :" + AppBase.Current.GuideId);

                                 await BindGuide(rp);

                                 LoadingCounter--;

                             }
                             catch (Exception)
                             {
                                 LoadingCounter--;
                                 throw;
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

        private async Task<string> ImagePathTranslated(string url)
        {
            var isFavorite = await _storageService.FolderExists(string.Format(Constants.GUIDE_CACHE_FOLDER, currentGuideId));
            //TODO: 
            var imagePath = url;
            NotFavorite = !isFavorite;

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

            GuideTitle = rp.title;
            GuideUrl = rp.url;

            if (!string.IsNullOrEmpty(rp.author.username))
                Author = string.Format(International.Translation.GuideBy, rp.author.username);

            if (rp.image != null)
                GuideMainImage = rp.image.large;

            var firstPage = new GuideIntro()
            {
                TypeOfGuide = rp.type
                ,
                Subject = rp.subject
                ,
                Summary = rp.summary
                ,
                PageTitle = International.Translation.Intro
                ,
                Type = GuidesPageTypes.Intro
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



            for (var i = 0; i < rp.parts.Count(); i++)
            {
                var item = rp.parts[i];
                firstPage.Parts.Add(new Tool
                {
                    Image = await ImagePathTranslated(item.thumbnail),
                    Title = GeneratePartName(item),
                    Url = item.url
                });
            }
            if (firstPage.Parts.Count > 0)
                firstPage.HasParts = true;


            for (var i = 0; i < rp.tools.Count(); i++)
            {
                var item = rp.tools[i];
                firstPage.Tools.Add(new Tool
                {
                    Image = await ImagePathTranslated(item.thumbnail),
                    Title = item.text,
                    Url = item.url
                });
            }





            for (var i = 0; i < rp.documents.Count(); i++)
            {
                var item = rp.documents[i];
                firstPage.Documents.Add(new Document
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







            var stepNum = 1;
            foreach (var item in rp.steps)
            {
                var newStep = new GuideStepItem
                {
                    index = item.orderby,
                    PageTitle = string.Format("{0} - {1}", International.Translation.Step, stepNum)
                    ,
                    Type = GuidesPageTypes.Step
                    ,
                    StepTitle = string.Format(International.Translation.StepNum, stepNum)
                    ,
                    StepIndex = stepNum
                };
                var m = 0;
                foreach (var lineitem in item.lines)
                {
                    newStep.Lines.Add(new GuideStepLine
                    {
                        Index = (m + 1).ToString(),
                        Title = lineitem.text_rendered,
                        VoiceText = Utils.StripHtml(lineitem.text_rendered),
                        BulletColor = GuideStepLine.SetIconColor(lineitem.bullet),
                        GuideIcon = GuideStepLine.SetIcon(lineitem.bullet),
                        Level = lineitem.level
                    });
                    m++;
                }



                // images
                if (item.media != null)
                {
                    if (item.media.videodata != null)
                    {
                        newStep.Video = new GuideStepVideo
                        {
                            VideoUrl = item.media.videodata.encodings.Where(o => o.mime == "video/mp4").First().url,
                            ImageUrl = item.media.videodata.image.image.medium
                        };
                        newStep.MainImage = new GuideStepImage
                        {
                            MediumImageUrl = item.media.videodata.image.image.medium
                        };
                    }
                    else
                    {

                        var n = 0;

                        if (item.media.type == "image")
                        {



                            foreach (var media in item.media.data)
                            {

                                if (n == 0)
                                    newStep.MainImage = new GuideStepImage
                                    {
                                        MediumImageUrl = media != null ? await ImagePathTranslated(media.medium) : ""
                                        ,
                                        SmallImageUrl = media != null ? await ImagePathTranslated(media.standard) : ""
                                        ,
                                        LargeImageUrl = media != null ? await ImagePathTranslated(media.original) : ""
                                    };



                                newStep.Images.Add(new GuideStepImage
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

                Items.Add(newStep);

                stepNum++;
            }
        }

        private static string GeneratePartName(RESTModels.Guide.Part item)
        {
            var title = item.text;
            // return string.Format("{0}, {1}, {2}", item.text, item.notes, item.type).TrimEnd(',');
            if (!string.IsNullOrEmpty(item.notes))
                title += ", " + item.notes;
            if (!string.IsNullOrEmpty(item.type))
                title += ", " + item.type;

            return title;
        }

        private RelayCommand _pauseTextSpeech;
        public RelayCommand PauseTextSpeech
        {
            get
            {
                return _pauseTextSpeech ?? (_pauseTextSpeech = new RelayCommand(
                  () =>
                  {

                      try
                      {

                          BeingRead = false;
                          _uxService.CancelSpeech();

                      }
                      catch (Exception)
                      {
                          LoadingCounter--;
                          throw ;
                      }

                  }));
            }

        }

        private RelayCommand _ffTextSpeech;
        public RelayCommand FFTextSpeech
        {
            get
            {
                return _ffTextSpeech ?? (_ffTextSpeech = new RelayCommand(
                  async () =>
                  {

                      try
                      {
                          BeingRead = false;
                          if (SelectedPageIndex >= Items.Count) return;
                          SelectedStepLine++;
                          SelectedPageIndex++;
                          _uxService.CancelSpeech();
                          await _uxService.OpenTextSpeechUI(this);
                      }
                      catch (Exception )
                      {

                          throw ;
                      }

                  }));
            }

        }

        private RelayCommand _rwTextSpeech;
        public RelayCommand RWTextSpeech
        {
            get
            {
                return _rwTextSpeech ?? (_rwTextSpeech = new RelayCommand(
                 async () =>
                 {

                     try
                     {
                         BeingRead = false;
                         if (SelectedPageIndex <= 0) return;
                         SelectedStepLine--;
                         SelectedPageIndex--;
                         _uxService.CancelSpeech();
                         await _uxService.OpenTextSpeechUI(this);
                     }
                     catch (Exception ex)
                     {

                         throw ex;
                     }

                 }));
            }

        }

        private RelayCommand _stopTextSpeech;
        public RelayCommand StopTextSpeech
        {
            get
            {
                return _stopTextSpeech ?? (_stopTextSpeech = new RelayCommand(
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

        private RelayCommand _share;
        public RelayCommand Share
        {
            get
            {
                return _share ?? (_share = new RelayCommand(
                  () =>
                  {

                      try
                      {

                          _uxService.CancelSpeech();
                          _uxService.Share(GuideUrl, GuideTitle);

                      }
                      catch (Exception )
                      {
                          LoadingCounter--;
                          throw ;
                      }

                  }));
            }

        }

        private RelayCommand _addFavorite;
        public RelayCommand AddFavorite
        {
            get
            {
                return _addFavorite ?? (_addFavorite = new RelayCommand(
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

                              var imagesToDownload = new List<string>();
                              foreach (var step in Items.Where(item => item.Type == GuidesPageTypes.Step).Cast<GuideStepItem>())
                              {
                                  imagesToDownload.Add(step.MainImage.MediumImageUrl);

                                  imagesToDownload.AddRange(from image in step.Images where image != null select image.MediumImageUrl);
                              }

                              await Broker.AddFavorites(AppBase.Current.User, currentGuideId);

                              var howMany = imagesToDownload.Count();
                              Debug.WriteLine("downloading: " + howMany);
                              var index = 1;
                              foreach (var item in imagesToDownload)
                              {
                                  Debug.WriteLine("downloading: " + index);
                                  var a = await Broker.GetImage(item);

                                  var imageName = item.Substring(item.LastIndexOf('/') + 1);
                                  await _storageService.WriteBinary(string.Format(Constants.GUIDE_CACHE_FOLDER, currentGuideId), imageName, a);
                                  Debug.WriteLine("downloaded: " + index);
                                  index++;
                              }


                              //TODO: save the images and the guide as a JSON
                              //then invoke the service
                              await _uxService.ShowToast(International.Translation.GuideSuccessfullySaved);
                              NotFavorite = false;
                              LoadingCounter--;

                          }



                      }
                      catch (Exception )
                      {
                          LoadingCounter--;
                          throw ;
                      }

                  }));
            }

        }


        private RelayCommand _deleteFavorite;
        public RelayCommand DeleteFavorite
        {
            get
            {
                return _deleteFavorite ?? (_deleteFavorite = new RelayCommand(
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

        private RelayCommand _openTextSpeech;
        public RelayCommand OpenTextSpeech
        {
            get
            {
                return _openTextSpeech ?? (_openTextSpeech = new RelayCommand(
                 async () =>
                 {

                     try
                     {
                         if (SelectedPageIndex == 0)
                             SelectedPage = Items[0];
                         await _uxService.OpenTextSpeechUI(this);

                     }
                     catch (Exception )
                     {

                         throw ;
                     }

                 }));
            }

        }

        private RelayCommand<string> _showFullImage;
        public RelayCommand<string> ShowFullImage
        {
            get
            {
                return _showFullImage ?? (_showFullImage = new RelayCommand<string>(
                 async image =>
                 {

                     try
                     {


                         FullImagePath = await ImagePathTranslated(image);

                     }
                     catch (Exception)
                     {
                         LoadingCounter--;
                         throw;
                     }

                 }));
            }

        }

        private RelayCommand<GuideStepImage> _swapImage;
        public RelayCommand<GuideStepImage> SwapImage
        {
            get
            {
                return _swapImage ?? (_swapImage = new RelayCommand<GuideStepImage>(
                  image =>
                  {

                      try
                      {
                          var filt = Items.Where(o => o.Type == GuidesPageTypes.Step).ToList();
                          foreach (GuideStepItem item in filt)
                          {
                              if (item.Images.Any(o => o == image))
                                  item.MainImage = image;

                          }


                      }
                      catch (Exception )
                      {
                          LoadingCounter--;
                          throw ;
                      }

                  }));
            }

        }


        private RelayCommand _shareNfc;
        public RelayCommand ShareNFC
        {
            get
            {
                return _shareNfc ?? (_shareNfc = new RelayCommand(
                  () =>
                  {

                      try
                      {

                          StartSharingSession();

                      }
                      catch (Exception )
                      {
                          LoadingCounter--;
                          throw ;
                      }

                  }));
            }

        }


        #endregion


        public void Reset()
        {
            Items.Clear();
            Items = new ObservableCollection<GuideBasePage>();
            SelectedPage = null;
            // = new ObservableCollection<Models.UI.GuideBasePage>();
            ShowingFullImage = false;
            FullImagePath = string.Empty;
            SelectedPageIndex = 0;
            SelectedStepLine = 0;
            GuideTitle = string.Empty;
            GuideUrl = string.Empty;
            Author = string.Empty;

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
            if (!IsConnected)
            {
                StartSharingSession();
            }
            else
            {
                _peerConnector?.SendGuideAsync(imageBytes);
            }
        }

        public Guide(INavigation<NavigationModes> navigationService, IStorage storageService, ISettings settingsService, IUxService uxService, IPeerConnector peerConnector)
            : base(navigationService, storageService, settingsService, uxService, peerConnector)
        {
            Reset();


        }


    }
}
