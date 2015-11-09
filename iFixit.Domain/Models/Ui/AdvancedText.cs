using System.Collections.ObjectModel;

namespace iFixit.Domain.Models.UI
{
    public class AdvancedText : ModelBase
    {

        private ObservableCollection<AdvancedTextItem> _content;
        public ObservableCollection<AdvancedTextItem> Content
        {
            get { return _content; }
            set
            {
                if (value == _content) return;
                _content = value;
                NotifyPropertyChanged();
            }
        }

        public AdvancedText()
        {
            Content = new ObservableCollection<AdvancedTextItem>();
        }

        public AdvancedText FromStringToAdvancedText(string input)
        {

            var result = new AdvancedText();



            return result;
        }


    }

    public enum TextItemType { Text = 0, Url = 1, View = 2 };

    public class AdvancedTextItem : ModelBase
    {

        private string _text;
        public string Text
        {
            get { return _text; }
            set
            {
                if (value == _text) return;
                _text = value;
                NotifyPropertyChanged();
            }
        }


        private TextItemType _type;
        public TextItemType Type
        {
            get { return _type; }
            set
            {
                if (value == _type) return;
                _type = value;
                NotifyPropertyChanged();
            }
        }


    }
}
