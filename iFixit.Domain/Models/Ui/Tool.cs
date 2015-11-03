namespace iFixit.Domain.Models.UI
{

    public class Document : Tool
    {
        private int  _DocumentId;
        public int DocumentId
        {
            get { return _DocumentId; }
            set
            {
                if (value != _DocumentId)
                {
                    _DocumentId = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }

    public class Tool : ModelBase
    {

        private string _Title;
        public string Title
        {
            get { return _Title; }
            set
            {
                if (value != _Title)
                {
                    _Title = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string _Url;
        public string Url
        {
            get { return _Url; }
            set
            {
                if (value != _Url)
                {
                    _Url = value;
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
}
