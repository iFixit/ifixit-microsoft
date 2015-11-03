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


        private string _Content;
        public string Content
        {
            get { return this._Content; }
            set
            {
                if (_Content != value)
                {
                    _Content = value;
                    NotifyPropertyChanged();
                }
            }
        }
        
        private CategoryPage _Type;
        public CategoryPage Type
        {
            get { return this._Type; }
            set
            {
                if (_Type != value)
                {
                    _Type = value;
                    NotifyPropertyChanged();
                }
            }
        }
        

        private string _Name;
        public string Name
        {
            get { return this._Name; }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private int _Childrens;
        public int Childrens
        {
            get { return this._Childrens; }
            set
            {
                if (_Childrens != value)
                {
                    _Childrens = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string _UniqueId;
        public string UniqueId
        {
            get { return this._UniqueId; }
            set
            {
                if (_UniqueId != value)
                {
                    _UniqueId = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _ImageUrl;
        public string ImageUrl
        {
            get { return this._ImageUrl; }
            set
            {
                if (_ImageUrl != value)
                {
                    _ImageUrl = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private ObservableCollection<Category> _Items = new ObservableCollection<Category>();
        public ObservableCollection<Category> Items
        {
            get { return this._Items; }
            set
            {
                if (_Items != value)
                {
                    _Items = value;
                    NotifyPropertyChanged();
                }
            }
        }



        private int _Order = 0;
        public int Order
        {
            get { return this._Order; }
            set
            {
                if (_Order != value)
                {
                    _Order = value;
                    NotifyPropertyChanged();
                }
            }
        }



        private bool _IsGroup = false;
        public bool IsGroup
        {
            get { return this._IsGroup; }
            set
            {
                if (_IsGroup != value)
                {
                    _IsGroup = value;
                    NotifyPropertyChanged();
                }
            }
        }



        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
