using System.Collections.ObjectModel;

namespace iFixit.Domain.Models.UI
{
    public class AdvancedText : ModelBase
    {

        private ObservableCollection<AdvancedTextItem> _Content;
        public ObservableCollection<AdvancedTextItem> Content
        {
            get { return _Content; }
            set
            {
                if (value != _Content)
                {
                    _Content = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public AdvancedText()
        {
            Content = new ObservableCollection<AdvancedTextItem>();
        }

        public AdvancedText FromStringToAdvancedText(string input)
        {

            AdvancedText Result = new AdvancedText();



            return Result;
        }


    }

    public enum TextItemType { Text = 0, Url = 1, View = 2 };

    public class AdvancedTextItem : ModelBase
    {

        private string _Text;
        public string Text
        {
            get { return _Text; }
            set
            {
                if (value != _Text)
                {
                    _Text = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private TextItemType _Type;
        public TextItemType Type
        {
            get { return _Type; }
            set
            {
                if (value != _Type)
                {
                    _Type = value;
                    NotifyPropertyChanged();
                }
            }
        }


    }
}
