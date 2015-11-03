namespace iFixit.Domain.Models.UI
{
    public class PreRequesites : ModelBase
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


        private string _View;
        public string View
        {
            get { return _View; }
            set
            {
                if (value != _View)
                {
                    _View = value;
                    NotifyPropertyChanged();
                }
            }
        }



        private int _GuideId;
        public int GuideId
        {
            get { return _GuideId; }
            set
            {
                if (value != _GuideId)
                {
                    _GuideId = value;
                    NotifyPropertyChanged();
                }
            }
        }



    }
}
