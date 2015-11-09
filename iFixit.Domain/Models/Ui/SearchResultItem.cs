namespace iFixit.Domain.Models.UI
{
    public class ProductItemList : ModelBase
    {
        private string _uniqueId;
        public string UniqueId
        {
            get { return _uniqueId; }
            set
            {
                if (_uniqueId == value) return;
                _uniqueId = value;
                NotifyPropertyChanged();
            }
        }



        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title == value) return;
                _title = value;
                NotifyPropertyChanged();
            }
        }

        private string _summary;
        public string Summary
        {
            get { return _summary; }
            set
            {
                if (_summary == value) return;
                _summary = value;
                NotifyPropertyChanged();
            }
        }

        double _price;
        public double Price
        {
            get { return _price; }
            set
            {
                if (_price == value) return;
                _price = value;
                NotifyPropertyChanged();
            }
        }


         string _imageUrl;
        public string ImageUrl
        {
            get { return _imageUrl; }
            set
            {
                if (_imageUrl == value) return;
                _imageUrl = value;
                NotifyPropertyChanged();
            }
        }

    }

    public class SearchResultItem : CustomGridBase
    {

         string _uniqueId;
        public string UniqueId
        {
            get { return _uniqueId; }
            set
            {
                if (_uniqueId == value) return;
                _uniqueId = value;
                NotifyPropertyChanged();
            }
        }



         string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value) return;
                _name = value;
                NotifyPropertyChanged();
            }
        }


         string _summary;
        public string Summary
        {
            get { return _summary; }
            set
            {
                if (_summary == value) return;
                _summary = value;
                NotifyPropertyChanged();
            }
        }


        private string _imageUrl;
        public string ImageUrl
        {
            get { return _imageUrl; }
            set
            {
                if (_imageUrl == value) return;
                _imageUrl = value;
                NotifyPropertyChanged();
            }
        }

         string _bigImageUrl;
        public string BigImageUrl
        {
            get { return _bigImageUrl; }
            set
            {
                if (_bigImageUrl == value) return;
                _bigImageUrl = value;
                NotifyPropertyChanged();
            }
        }

         string _author;
        public string Author
        {
            get { return _author; }
            set
            {
                if (_author == value) return;
                _author = value;
                NotifyPropertyChanged();
            }
        }


         string _guideType;
        public string GuideType
        {
            get { return _guideType; }
            set
            {
                if (_guideType == value) return;
                _guideType = value;
                NotifyPropertyChanged();
            }
        }


    }
}
