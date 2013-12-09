using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iFixit.Domain.Models.UI
{
    public class RssItem:ModelBase
    {

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


        private string _Summary;
        public string Summary
        {
            get { return  this._Summary; }
            set
            {
                if (_Summary != value)
                {
                    _Summary = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string _Url;
        public string Url
        {
            get { return this._Url; }
            set
            {
                if (_Url != value)
                {
                    _Url = value;
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
        

        private DateTime _PubDate;
        public DateTime PubDate
        {
            get { return 
                this._PubDate.ToLocalTime(); }
            set
            {
                if (_PubDate != value)
                {
                    _PubDate = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string _DateString;
        public string DateString
        {
            get { return this._DateString; }
            set
            {
                if (_DateString != value)
                {
                    _DateString = value;
                    NotifyPropertyChanged();
                }
            }
        }
        
        
    }
}
