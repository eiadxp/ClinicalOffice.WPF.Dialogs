using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace ClinicalOffice.WPF.Dialogs
{
    public class DialogTitleControl : UserControl
    {
        static DialogTitleControl()
        {
            DefaultStyleKeyProperty.
                OverrideMetadata(typeof(DialogTitleControl), new FrameworkPropertyMetadata(typeof(DialogTitleControl)));
        }
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);
            Visibility = (newContent == null) ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
