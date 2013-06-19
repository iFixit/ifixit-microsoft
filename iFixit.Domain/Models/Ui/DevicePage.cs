using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iFixit.Domain.Models.UI
{
    public interface IDevicePage
    {

    }

    public enum DevicePageType { Intro = 0, GuideListing = 1, DeviceListing = 2 };

    public class DevicePage : ModelBase, IDevicePage
    {

        private string _PageTitle;
        public string PageTitle
        {
            get { return _PageTitle; }
            set
            {
                if (value != _PageTitle)
                {
                    _PageTitle = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private DevicePageType _PageType;
        public DevicePageType PageType
        {
            get { return _PageType; }
            set
            {
                if (value != _PageType)
                {
                    _PageType = value;
                    NotifyPropertyChanged();
                }
            }
        }

    }

    public class DeviceIntroPage : DevicePage
    {
        private int _NrSolutions;
        public int NrSolutions
        {
            get { return _NrSolutions; }
            set
            {
                if (value != _NrSolutions)
                {
                    _NrSolutions = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string _SolutionLink;
        public string SolutionLink
        {
            get { return _SolutionLink; }
            set
            {
                if (value != _SolutionLink)
                {
                    _SolutionLink = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string _Categories;
        public string Categories
        {
            get { return _Categories; }
            set
            {
                if (value != _Categories)
                {
                    _Categories = value;
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

        private string _Image;
        public string Image
        {
            get { return _Image; }
            set
            {
                if (value != _Image)
                {
                    _Image = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }

    public class DeviceListingPage : DevicePage
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
    }
}
