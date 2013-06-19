using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace iFixit.Domain.Models
{
    public partial class ModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName]String propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                try
                {
                    handler(this, new PropertyChangedEventArgs(propertyName));
                }
                catch
                {


                }

            }
        }
    }
}
