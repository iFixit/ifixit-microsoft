namespace iFixit.Domain.Models.UI
{
    public class User : ModelBase
    {

         string _email;
        public string Email
        {
            get { return this._email; }
            set
            {
                if (_email == value) return;
                _email = value;
                NotifyPropertyChanged();
            }
        }
        

         string _token;
        public string Token
        {
            get { return this._token; }
            set
            {
                if (_token == value) return;
                _token = value;
                NotifyPropertyChanged();
            }
        }


         string _userName;
        public string UserName
        {
            get { return this._userName; }
            set
            {
                if (_userName == value) return;
                _userName = value;
                NotifyPropertyChanged();
            }
        }


         int _userId;
        public int UserId
        {
            get { return this._userId; }
            set
            {
                if (_userId == value) return;
                _userId = value;
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


    }
}
