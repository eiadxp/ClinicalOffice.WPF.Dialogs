using System.Windows;
using System.Windows.Controls;

namespace ClinicalOffice.WPF.Dialogs
{
    public class DialogContentControl : UserControl
    {
        static DialogContentControl()
        {
            DefaultStyleKeyProperty.
                OverrideMetadata(typeof(DialogContentControl), new FrameworkPropertyMetadata(typeof(DialogContentControl)));
        }
    }
}
