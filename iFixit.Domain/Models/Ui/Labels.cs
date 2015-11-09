

namespace iFixit.Domain.Models.UI
{

    public class LabelsManager : ModelBase
    {
         International.PublicInternational _label;

        public International.PublicInternational Labels
        {

            get { return _label; }
            set
            {
                if (value == _label) return;
                _label = value;
                NotifyPropertyChanged();
            }
        }
        public LabelsManager()
        {
            Labels = new International.PublicInternational();
            
        }


    }
}
