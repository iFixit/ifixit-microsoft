namespace iFixit.Domain.Models.UI
{
    public class CustomGridBase : ModelBase
    {
         int _indexOf = 0;
        public int IndexOf
        {
            get { return (this._indexOf == 0) ? 2 : 1; }
            set
            {

                _indexOf = value;
                NotifyPropertyChanged();

            }
        }
    }

    public class HomeItem : CustomGridBase
    {

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

        private string _bigImageUrl;
        public string BigImageUrl
        {
            get { return this._bigImageUrl; }
            set
            {
                if (_bigImageUrl == value) return;
                _bigImageUrl = value;
                NotifyPropertyChanged();
            }
        }

        private string _name;
        public string Name
        {
            get { return this._name; }
            set
            {
                if (_name == value) return;
                _name = value;
                NotifyPropertyChanged();
            }
        }

        private string _uniqueId;
        public string UniqueId
        {
            get { return this._uniqueId; }
            set
            {
                if (_uniqueId == value) return;
                _uniqueId = value;
                NotifyPropertyChanged();
            }
        }


      

    }
}
