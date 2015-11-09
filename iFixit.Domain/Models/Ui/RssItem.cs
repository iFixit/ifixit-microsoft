using System;

namespace iFixit.Domain.Models.UI
{
    public class RssItem:ModelBase
    {

         string _title;
        public string Title
        {
            get { return this._title; }
            set
            {
                if (_title == value) return;
                _title = value;
                NotifyPropertyChanged();
            }
        }


         string _imageUrl;
        public string ImageUrl
        {
            get { return this._imageUrl; }
            set
            {
                if (_imageUrl == value) return;
                _imageUrl = value;
                NotifyPropertyChanged();
            }
        }


         string _summary;
        public string Summary
        {
            get { return  this._summary; }
            set
            {
                if (_summary == value) return;
                _summary = value;
                NotifyPropertyChanged();
            }
        }


         string _url;
        public string Url
        {
            get { return this._url; }
            set
            {
                if (_url == value) return;
                _url = value;
                NotifyPropertyChanged();
            }
        }


         string _author;
        public string Author
        {
            get { return this._author; }
            set
            {
                if (_author == value) return;
                _author = value;
                NotifyPropertyChanged();
            }
        }
        

         DateTime _pubDate;
        public DateTime PubDate
        {
            get { return 
                this._pubDate.ToLocalTime(); }
            set
            {
                if (_pubDate == value) return;
                _pubDate = value;
                NotifyPropertyChanged();
            }
        }


         string _dateString;
        public string DateString
        {
            get { return this._dateString; }
            set
            {
                if (_dateString == value) return;
                _dateString = value;
                NotifyPropertyChanged();
            }
        }
        
        
    }
}
