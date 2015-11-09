using Newtonsoft.Json;
using System.Collections.ObjectModel;


namespace iFixit.Domain.Models.UI
{

    public class FlatCategory
    {


        public string Parent { get; set; }
        public string Name { get; set; }

    }

    public enum CategoryPage { Index, Intro, Root, Categories }

    public class Category : CustomGridBase
    {


         string _content;
        public string Content
        {
            get { return _content; }
            set
            {
                if (_content == value) return;
                _content = value;
                NotifyPropertyChanged();
            }
        }
        
         CategoryPage _type;
        public CategoryPage Type
        {
            get { return _type; }
            set
            {
                if (_type == value) return;
                _type = value;
                NotifyPropertyChanged();
            }
        }
        

         string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value) return;
                _name = value;
                NotifyPropertyChanged();
            }
        }


         int _childrens;
        public int Childrens
        {
            get { return _childrens; }
            set
            {
                if (_childrens == value) return;
                _childrens = value;
                NotifyPropertyChanged();
            }
        }


         string _uniqueId;
        public string UniqueId
        {
            get { return _uniqueId; }
            set
            {
                if (_uniqueId == value) return;
                _uniqueId = value;
                NotifyPropertyChanged();
            }
        }

         string _imageUrl;
        public string ImageUrl
        {
            get { return _imageUrl; }
            set
            {
                if (_imageUrl == value) return;
                _imageUrl = value;
                NotifyPropertyChanged();
            }
        }

         ObservableCollection<Category> _items = new ObservableCollection<Category>();
        public ObservableCollection<Category> Items
        {
            get { return _items; }
            set
            {
                if (_items == value) return;
                _items = value;
                NotifyPropertyChanged();
            }
        }



         int _order ;
        public int Order
        {
            get { return _order; }
            set
            {
                if (_order == value) return;
                _order = value;
                NotifyPropertyChanged();
            }
        }



         bool _isGroup ;
        public bool IsGroup
        {
            get { return _isGroup; }
            set
            {
                if (_isGroup == value) return;
                _isGroup = value;
                NotifyPropertyChanged();
            }
        }



        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
