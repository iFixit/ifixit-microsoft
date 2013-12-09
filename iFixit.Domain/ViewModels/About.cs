using GalaSoft.MvvmLight.Command;
using iFixit.Domain.Interfaces;
using iFixit.Domain.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace iFixit.Domain.ViewModels
{
    public class About : BaseViewModel
    {


        private Models.UI.RssItem _SelectedItem;
        public Models.UI.RssItem SelectedItem
        {
            get { return this._SelectedItem; }
            set
            {
                if (_SelectedItem != value)
                {
                    _SelectedItem = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private ObservableCollection<Models.UI.RssItem> _News = new ObservableCollection<Models.UI.RssItem>();
        public ObservableCollection<Models.UI.RssItem> News
        {
            get { return this._News; }
            set
            {
                if (_News != value)
                {
                    _News = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private int _SelectedIndex = 0;
        public int SelectedIndex
        {
            get { return this._SelectedIndex; }
            set
            {
                if (_SelectedIndex != value)
                {
                    _SelectedIndex = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private RelayCommand<string> _GoToItem;
        public RelayCommand<string> GoToItem
        {
            get
            {
                return _GoToItem ?? (_GoToItem = new RelayCommand<string>(
                async (item) =>
                {

                    try
                    {

                        await _uxService.OpenBrowser(item);
                        SelectedItem = null;

                    }
                    catch (Exception ex)
                    {
                        LoadingCounter--;
                        throw ex;
                    }

                }));
            }

        }

        private RelayCommand<Models.UI.RssItem> _GoToNewsItem;
        public RelayCommand<Models.UI.RssItem> GoToNewsItem
        {
            get
            {
                return _GoToNewsItem ?? (_GoToNewsItem = new RelayCommand<Models.UI.RssItem>(
                async (item) =>
                {

                    try
                    {

                        await _uxService.OpenBrowser(item.Url);
                        SelectedItem = null;

                    }
                    catch (Exception ex)
                    {
                        LoadingCounter--;
                        throw ex;
                    }

                }));
            }

        }


        private RelayCommand _Load;
        public RelayCommand Load
        {
            get
            {
                return _Load ?? (_Load = new RelayCommand(
                async () =>
                {

                    try
                    {
                        LoadingCounter++;
                        SelectedIndex = this.NavigationParameter<int>();
                        await LoadRss();
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


        public async Task LoadRss()
        {

            if (_settingsService.IsConnectedToInternet())
            {
                NewsBroker nBroker = new NewsBroker();

                var xml = await nBroker.GetNews();

                var itemNodes = xml.Nodes();

                var morenodes = (from n in xml.Descendants("rss") select n).Descendants("item").Select(x => new
                Models.UI.RssItem
                {
                    Title = (string)x.Element("title")
                    ,
                    Summary = (string)x.Element("description")

                    ,
                    ImageUrl = Regex.Match(((string)(x.Element("htmlcontent"))), "<img.+?src=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase).Groups[1].Value

                    ,
                    Url = (string)x.Element("guid")
                    ,
                    PubDate = (DateTime)x.Element("pubDate")
                    ,
                    Author = string.Format(International.Translation.RssDateAndAuthor, (string)x.Element("creator"))
                }).ToList();


                News.Clear();
                foreach (var item in morenodes)
                {
                    News.Add(item);
                }
            }
            else
            {
                await _uxService.ShowAlert(International.Translation.NoConnection);

            }

        }

        public About(INavigation<Domain.Interfaces.NavigationModes> navigationService, IStorage storageService, ISettings settingsService, IUxService uxService, IPeerConnector peerConnector)
            : base(navigationService, storageService, settingsService, uxService, peerConnector)
        {


            this.PageTitle = International.Translation.About;
        }
    }
}
