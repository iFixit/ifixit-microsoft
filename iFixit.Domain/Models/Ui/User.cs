using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iFixit.Domain.Models.UI
{
    public class User : ModelBase
    {

        private string _Email;
        public string Email
        {
            get { return this._Email; }
            set
            {
                if (_Email != value)
                {
                    _Email = value;
                    NotifyPropertyChanged();
                }
            }
        }
        

        private string _Token;
        public string Token
        {
            get { return this._Token; }
            set
            {
                if (_Token != value)
                {
                    _Token = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string _UserName;
        public string UserName
        {
            get { return this._UserName; }
            set
            {
                if (_UserName != value)
                {
                    _UserName = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private int _UserId;
        public int UserId
        {
            get { return this._UserId; }
            set
            {
                if (_UserId != value)
                {
                    _UserId = value;
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


    }
}
