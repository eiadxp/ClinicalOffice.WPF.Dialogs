using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace ClinicalOffice.WPF.Dialogs
{
    [ContentProperty(nameof(Content))]
    public class DialogTitleControl : Border
    {
        public object Content
        {
            get { return (object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(DialogTitleControl), new PropertyMetadata(null));

        public VerticalAlignment VerticalContentAlignment
        {
            get { return (VerticalAlignment)GetValue(VerticalContentAlignmentProperty); }
            set { SetValue(VerticalContentAlignmentProperty, value); }
        }
        public static readonly DependencyProperty VerticalContentAlignmentProperty =
            DependencyProperty.Register("VerticalContentAlignment", typeof(VerticalAlignment), typeof(DialogTitleControl), new PropertyMetadata(VerticalAlignment.Center));

        public HorizontalAlignment HorizontalContentAlignment
        {
            get { return (HorizontalAlignment)GetValue(HorizontalContentAlignmentProperty); }
            set { SetValue(HorizontalContentAlignmentProperty, value); }
        }
        public static readonly DependencyProperty HorizontalContentAlignmentProperty =
            DependencyProperty.Register("HorizontalContentAlignment", typeof(HorizontalAlignment), typeof(DialogTitleControl), new PropertyMetadata(HorizontalAlignment.Center));

        ContentControl _Content;
        static DialogTitleControl()
        {
            DefaultStyleKeyProperty.
                OverrideMetadata(typeof(DialogTitleControl), new FrameworkPropertyMetadata(typeof(DialogTitleControl)));
        }
        public DialogTitleControl()
        {
            _Content = new ContentControl() { VerticalAlignment = VerticalContentAlignment,
                                              HorizontalAlignment = HorizontalContentAlignment
                                            };
            BindingOperations.SetBinding(_Content, ContentControl.ContentProperty, 
                new Binding(nameof(Content)) { Source = this });
            BindingOperations.SetBinding(_Content, ContentControl.VerticalAlignmentProperty, 
                new Binding(nameof(VerticalContentAlignment)) { Source = this });
            BindingOperations.SetBinding(_Content, ContentControl.HorizontalAlignmentProperty, 
                new Binding(nameof(HorizontalContentAlignment)) { Source = this });
            Child = _Content;
        }
    }
}
