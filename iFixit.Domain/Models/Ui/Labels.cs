

namespace iFixit.Domain.Models.UI
{

    public class LabelsManager : ModelBase
    {
        private International.PublicInternational label;

        public International.PublicInternational Labels
        {

            get { return label; }
            set
            {
                if (value != label)
                {
                    label = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public LabelsManager()
        {
            Labels = new International.PublicInternational();
            
        }


    }
}
