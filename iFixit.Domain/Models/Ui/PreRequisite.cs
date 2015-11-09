namespace iFixit.Domain.Models.UI
{
    public class PreRequesites : ModelBase
    {

         string _title;
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


         string _view;
        public string View
        {
            get { return _view; }
            set
            {
                if (value == _view) return;
                _view = value;
                NotifyPropertyChanged();
            }
        }



         int _guideId;
        public int GuideId
        {
            get { return _guideId; }
            set
            {
                if (value == _guideId) return;
                _guideId = value;
                NotifyPropertyChanged();
            }
        }



    }
}
