using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iFixit.Domain.Models.UI
{
    public class CustomGridBase : ModelBase
    {
        private int _IndexOf = 0;
        public int IndexOf
        {
            get { return (this._IndexOf == 0) ? 2 : 1; }
            set
            {

                _IndexOf = value;
                NotifyPropertyChanged();

            }
        }
    }

    public class HomeItem : CustomGridBase
    {

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

        private string _BigImageUrl;
        public string BigImageUrl
        {
            get { return this._BigImageUrl; }
            set
            {
                if (_BigImageUrl != value)
                {
                    _BigImageUrl = value;
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


      

    }
}
