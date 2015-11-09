namespace iFixit.Domain.Models.UI
{

    public class Document : Tool
    {
         int  _documentId;
        public int DocumentId
        {
            get { return _documentId; }
            set
            {
                if (value == _documentId) return;
                _documentId = value;
                NotifyPropertyChanged();
            }
        }
    }

    public class Tool : ModelBase
    {

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                if (value == _title) return;
                _title = value;
                NotifyPropertyChanged();
            }
        }


         string _Url;
        public string Url
        {
            get { return _Url; }
            set
            {
                if (value == _Url) return;
                _Url = value;
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
}
