using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ClinicalOffice.WPF.Dialogs
{
    public class DialogTitleControl : Border
    {
        static DialogTitleControl()
        {
            DefaultStyleKeyProperty.
                OverrideMetadata(typeof(DialogTitleControl), new FrameworkPropertyMetadata(typeof(DialogTitleControl)));
        }
        public DialogTitleControl()
        {
            var t = new DataTemplate() { DataType = typeof(string) };
            var grid = new FrameworkElementFactory(typeof(Grid));
            var textblock = new FrameworkElementFactory(typeof(TextBlock));
            textblock.SetBinding(TextBlock.TextProperty, new Binding());
            grid.AppendChild(textblock);
            t.VisualTree = grid;
            
        }
    }
}
