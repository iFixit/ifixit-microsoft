using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iFixit.Domain.Models.UI
{
    public class ProductItemList : ModelBase
    {
        private string _UniqueId;
        public string UniqueId
        {
            get { return this._UniqueId; }
            set
            {
                if (_UniqueId != value)
                {
                    _UniqueId = value;
                    NotifyPropertyChanged();
                }
            }
        }



        private string _Title;
        public string Title
        {
            get { return this._Title; }
            set
            {
                if (_Title != value)
                {
                    _Title = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _Summary;
        public string Summary
        {
            get { return this._Summary; }
            set
            {
                if (_Summary != value)
                {
                    _Summary = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private double _Price;
        public double Price
        {
            get { return this._Price; }
            set
            {
                if (_Price != value)
                {
                    _Price = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string _ImageUrl;
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

    public class SearchResultItem : CustomGridBase
    {

        private string _UniqueId;
        public string UniqueId
        {
            get { return this._UniqueId; }
            set
            {
                if (_UniqueId != value)
                {
                    _UniqueId = value;
                    NotifyPropertyChanged();
                }
            }
        }



        private string _Name;
        public string Name
        {
            get { return this._Name; }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string _Summary;
        public string Summary
        {
            get { return this._Summary; }
            set
            {
                if (_Summary != value)
                {
                    _Summary = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string _ImageUrl;
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

        private string _BigImageUrl;
        public string BigImageUrl
        {
            get { return this._BigImageUrl; }
            set
            {
                if (_BigImageUrl != value)
                {
                    _BigImageUrl = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _Author;
        public string Author
        {
            get { return this._Author; }
            set
            {
                if (_Author != value)
                {
                    _Author = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string _GuideType;
        public string GuideType
        {
            get { return this._GuideType; }
            set
            {
                if (_GuideType != value)
                {
                    _GuideType = value;
                    NotifyPropertyChanged();
                }
            }
        }


    }
}
