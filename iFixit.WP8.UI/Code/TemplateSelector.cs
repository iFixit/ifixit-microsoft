using iFixit.Domain.Models.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;

namespace iFixit.WP8.UI.Code
{

    public class DevicePageTemplateSelector : DataTemplateSelectorEX
    {
        public DataTemplate Intro { get; set; }
        public DataTemplate Guides { get; set; }


        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DevicePage page = item as DevicePage;

            switch (page.PageType)
            {
                case DevicePageType.Intro:
                    return Intro;
                case DevicePageType.GuideListing:

                case DevicePageType.DeviceListing:
                    return Guides;
                default:
                    break;
            }




            return base.SelectTemplate(item, container);
        }
    }

    public class SubCategoryTemplateSelector : DataTemplateSelectorEX
    {
        public DataTemplate Intro { get; set; }
        public DataTemplate Index { get; set; }
        public DataTemplate SubItems { get; set; }


        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            iFixit.Domain.Models.UI.Category page = item as iFixit.Domain.Models.UI.Category;

            switch (page.Type)
            {
                case CategoryPage.Index:
                    return Index;
                case CategoryPage.Intro:
                    return Intro;
                case CategoryPage.Root:

                case CategoryPage.Categories:
                    return SubItems;
                default:
                    break;
            }

            //switch (page.Items.Count)
            //{
            //    case 0:
            //        return Index;
            //    case 1:
            //        return Intro;
            //    default:
            //        return SubItems;
            //}





            return base.SelectTemplate(item, container);
        }
    }

    public class GuidePageTemplateSelector : DataTemplateSelectorEX
    {
        public DataTemplate Intro { get; set; }
        public DataTemplate Prerequisites { get; set; }
        public DataTemplate Step { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            GuideBasePage page = item as GuideBasePage;

            switch (page.Type)
            {
                case GuidesPageTypes.Intro:
                    return Intro;
                case GuidesPageTypes.Prerequisites:
                    return Prerequisites;
                case GuidesPageTypes.Step:
                    return Step;
                default:
                    break;
            }


            return base.SelectTemplate(item, container);
        }
    }

    public abstract class DataTemplateSelectorEX : ContentControl
    {
        public virtual DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return null;
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            ContentTemplate = SelectTemplate(newContent, this);
        }
    }


    public class DetailsTemplateSelector : DataTemplateSelector
    {
#if DEBUG
        /* http://www.telerik.com/help/windows-phone/radslideview-features-usingselectors.html */
#endif
        public DataTemplate Intro
        {
            get;
            set;
        }

        public DataTemplate Step
        {
            get;
            set;
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            RadSlideView view = ElementTreeHelper.FindVisualAncestor<RadSlideView>(container);
            if (view == null)
            {
                return null;
            }

            GuideBasePage page = item as GuideBasePage;

            switch (page.Type)
            {
                case GuidesPageTypes.Intro:
                    return Intro;

                case GuidesPageTypes.Step:
                    return Step;
                default:
                    throw new ArgumentException("Don´t know how to render this");
            }



        }
    }
}
