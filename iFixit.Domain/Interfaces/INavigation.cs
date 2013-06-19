using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iFixit.Domain.Interfaces
{

        public enum NavigationModes { New = 0, Back = 1, Forward = 2, Refresh = 3, Reset = 4 }

        public interface INavigation<T>
        {
            /// <summary>
            /// 
            /// </summary>
            T NavigationType { get; }

            /// <summary>
            /// 
            /// </summary>
            bool CanGoBack { get; }

         
            
            /// <summary>
            /// Go back
            /// </summary>
            void GoBack();

            /// <summary>
            /// Navigate to view 
            /// </summary>
            /// <typeparam name="TDestinationViewModel"></typeparam>
            /// <param name="parameter"></param>
            void Navigate<TDestinationViewModel>(object parameter = null);

            /// <summary>
            /// Navigate to view 
            /// </summary>
            /// <typeparam name="TDestinationViewModel"></typeparam>
            /// <param name="sameContext">this allows to set the transitions animations</param>
            /// <param name="parameter"></param>
            void Navigate<TDestinationViewModel>(bool sameContext, object parameter = null);
        }
    
}
