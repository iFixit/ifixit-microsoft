using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace iFixit.UI.Shared
{
   

    public class VariableGridView : GridView
    {

        public VariableGridView() { }

        public string ItemRowSpanPropertyPath
        {
            get { return (string)GetValue(ItemRowSpanPropertyPathProperty); }
            set { SetValue(ItemRowSpanPropertyPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemRowSpanPropertyPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemRowSpanPropertyPathProperty =
            DependencyProperty.Register("ItemRowSpanPropertyPath", typeof(string), typeof(VariableGridView), new PropertyMetadata(string.Empty));


        public string ItemColSpanPropertyPath
        {
            get { return (string)GetValue(ItemColSpanPropertyPathProperty); }
            set { SetValue(ItemColSpanPropertyPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemColSpanPropertyPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemColSpanPropertyPathProperty =
            DependencyProperty.Register("ItemColSpanPropertyPath", typeof(string), typeof(VariableGridView), new PropertyMetadata(string.Empty));

        protected override void PrepareContainerForItemOverride(Windows.UI.Xaml.DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            var dataItem = item as Domain.Models.UI.CustomGridBase;
            UIElement uiElement = element as UIElement;

            Binding colBinding = new Binding();
            colBinding.Source = dataItem;
            colBinding.Path = new PropertyPath(this.ItemColSpanPropertyPath);
            BindingOperations.SetBinding(uiElement, VariableSizedWrapGrid.ColumnSpanProperty, colBinding);

            Binding rowBinding = new Binding();
            rowBinding.Source = dataItem;
            rowBinding.Path = new PropertyPath(this.ItemRowSpanPropertyPath);
            BindingOperations.SetBinding(uiElement, VariableSizedWrapGrid.RowSpanProperty, rowBinding);
        }

    }
}
