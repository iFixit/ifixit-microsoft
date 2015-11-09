using System.Collections.ObjectModel;

namespace iFixit.Domain.Models.UI
{
    public interface IDevicePage
    {

    }

    public enum DevicePageType { Intro = 0, GuideListing = 1, DeviceListing = 2 };

    public class DevicePage : ModelBase, IDevicePage
    {

         string _pageTitle;
        public string PageTitle
        {
            get { return _pageTitle; }
            set
            {
                if (value == _pageTitle) return;
                _pageTitle = value;
                NotifyPropertyChanged();
            }
        }


         DevicePageType _pageType;
        public DevicePageType PageType
        {
            get { return _pageType; }
            set
            {
                if (value == _pageType) return;
                _pageType = value;
                NotifyPropertyChanged();
            }
        }

    }

    public class DeviceIntroPage : DevicePage
    {
         int _nrSolutions;
        public int NrSolutions
        {
            get { return _nrSolutions; }
            set
            {
                if (value == _nrSolutions) return;
                _nrSolutions = value;
                NotifyPropertyChanged();
            }
        }


         string _displayTitle;
        public string DisplayTitle
        {
            get { return _displayTitle; }
            set
            {
                if (_displayTitle == value) return;
                _displayTitle = value;
                NotifyPropertyChanged();
            }
        }
        

         string _solutionLink;
        public string SolutionLink
        {
            get { return _solutionLink; }
            set
            {
                if (value == _solutionLink) return;
                _solutionLink = value;
                NotifyPropertyChanged();
            }
        }


         string _categories;
        public string Categories
        {
            get { return _categories; }
            set
            {
                if (value == _categories) return;
                _categories = value;
                NotifyPropertyChanged();
            }
        }


         string _summary;
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

         string _image;
        public string Image
        {
            get { return _image; }
            set
            {
                if (value == _image) return;
                _image = value;
                NotifyPropertyChanged();
            }
        }
    }

    public class DeviceListingPage : DevicePage
    {

         string _hasItems;
        public string HasItems
        {
            get { return _hasItems; }
            set
            {
                if (_hasItems == value) return;
                _hasItems = value;
                NotifyPropertyChanged();
            }
        }


        private ObservableCollection<SearchResultItem> _items = new ObservableCollection<SearchResultItem>();
        public ObservableCollection<SearchResultItem> Items
        {
            get { return _items; }
            set
            {
                if (value == _items) return;
                _items = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("HasItems");
            }
        }
    }
}
