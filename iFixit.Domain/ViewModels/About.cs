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


        private Models.UI.RssItem _selectedItem;
        public Models.UI.RssItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem == value) return;
                _selectedItem = value;
                NotifyPropertyChanged();
            }
        }


        private ObservableCollection<Models.UI.RssItem> _news = new ObservableCollection<Models.UI.RssItem>();
        public ObservableCollection<Models.UI.RssItem> News
        {
            get { return _news; }
            set
            {
                if (_news == value) return;
                _news = value;
                NotifyPropertyChanged();
            }
        }


        private int _selectedIndex;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (_selectedIndex == value) return;
                _selectedIndex = value;
                NotifyPropertyChanged();
            }
        }


        private RelayCommand<string> _goToItem;
        public RelayCommand<string> GoToItem
        {
            get
            {
                return _goToItem ?? (_goToItem = new RelayCommand<string>(
                async item =>
                {

                    try
                    {

                        await _uxService.OpenBrowser(item);
                        SelectedItem = null;

                    }
                    catch (Exception )
                    {
                        LoadingCounter--;
                        throw ;
                    }

                }));
            }

        }

        private RelayCommand<Models.UI.RssItem> _goToNewsItem;
        public RelayCommand<Models.UI.RssItem> GoToNewsItem
        {
            get
            {
                return _goToNewsItem ?? (_goToNewsItem = new RelayCommand<Models.UI.RssItem>(
                async item =>
                {

                    try
                    {

                        await _uxService.OpenBrowser(item.Url);
                        SelectedItem = null;

                    }
                    catch (Exception)
                    {
                        LoadingCounter--;
                        throw;
                    }

                }));
            }

        }


        private RelayCommand _load;
        public RelayCommand Load
        {
            get
            {
                return _load ?? (_load = new RelayCommand(
                async () =>
                {

                    try
                    {
                        LoadingCounter++;
                        SelectedIndex = NavigationParameter<int>();
                        await LoadRss();
                        LoadingCounter--;
                    }
                    catch (Exception )
                    {
                        LoadingCounter--;
                        throw ;
                    }

                }));
            }

        }


        public async Task LoadRss()
        {

            if (_settingsService.IsConnectedToInternet())
            {
                var nBroker = new NewsBroker();

                var xml = await nBroker.GetNews();

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

        public About(INavigation<NavigationModes> navigationService, IStorage storageService, ISettings settingsService, IUxService uxService, IPeerConnector peerConnector)
            : base(navigationService, storageService, settingsService, uxService, peerConnector)
        {


            PageTitle = International.Translation.About;
        }
    }
}
