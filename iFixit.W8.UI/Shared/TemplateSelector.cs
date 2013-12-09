
using iFixit.Domain.Models.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace iFixit.UI.Shared
{

    public class GuidePageSelector : DataTemplateSelector
    {
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            GuideBasePage page = item as GuideBasePage;
            switch (page.Type)
            {
                case GuidesPageTypes.Intro:
                    return IntroPage;

                case GuidesPageTypes.Prerequisites:

                case GuidesPageTypes.Step:
                    return StepPage;

                default:
                    return StepPage;
            }
        }

        public DataTemplate IntroPage { get; set; }
        public DataTemplate StepPage { get; set; }

    }

    public class GuideTemplateSelector : DataTemplateSelectorBase
    {
        public DataTemplate IntroPage { get; set; }
        public DataTemplate StepPage { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            GuideBasePage page = item as GuideBasePage;

            switch (page.Type)
            {
                case GuidesPageTypes.Intro:
                    return IntroPage;

                case GuidesPageTypes.Prerequisites:

                case GuidesPageTypes.Step:
                    return StepPage;

                default:
                    return StepPage;
            }




        }
    }

    public class DevicePageTemplateSelector : DataTemplateSelectorBase
    {
        public DataTemplate Intro { get; set; }
        public DataTemplate GuideListing { get; set; }
        public DataTemplate DevicesListing { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DevicePage page = item as DevicePage;
            switch (page.PageType)
            {
                case DevicePageType.Intro:
                    return Intro;

                case DevicePageType.GuideListing:
                    return GuideListing;

                case DevicePageType.DeviceListing:
                    return DevicesListing;
                default:
                    break;
            }
            return base.SelectTemplate(item, container);
        }
    }

    public abstract class DataTemplateSelectorBase : ContentControl
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



}
