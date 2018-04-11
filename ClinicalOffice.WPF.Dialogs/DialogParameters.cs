using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ClinicalOffice.WPF.Dialogs
{
    public class DialogParameters : DependencyObject
    {
        static DialogParameters current = new DialogParameters();

        public static Brush GetTitleBackground(Application obj)
        {
            return (Brush)current.GetValue(TitleBackgroundProperty);
        }
        public static void SetTitleBackground(Application obj, Brush value)
        {
            current.SetValue(TitleBackgroundProperty, value);
        }
        public static readonly DependencyProperty TitleBackgroundProperty =
            DependencyProperty.RegisterAttached("TitleBackground", typeof(Brush), typeof(DialogParameters), new PropertyMetadata(new SolidColorBrush(Colors.White) { Opacity = 0.8 }));
        public static Brush TitleBackground { get => GetTitleBackground(null); set => SetTitleBackground(null, value); }

        public static Brush GetContentBackground(Application obj)
        {
            return (Brush)current.GetValue(ContentBackgroundProperty);
        }
        public static void SetContentBackground(Application obj, Brush value)
        {
            current.SetValue(ContentBackgroundProperty, value);
        }
        public static readonly DependencyProperty ContentBackgroundProperty =
            DependencyProperty.RegisterAttached("ContentBackground", typeof(Brush), typeof(DialogParameters), new PropertyMetadata(new SolidColorBrush(Colors.White) { Opacity = 0.9 }));
        public static Brush ContentBackground { get => GetContentBackground(null); set => SetContentBackground(null, value); }

        public static Brush GetButtonsBarBackground(Application obj)
        {
            return (Brush)current.GetValue(ButtonsBarBackgroundProperty);
        }
        public static void SetButtonsBarBackground(Application obj, Brush value)
        {
            current.SetValue(ButtonsBarBackgroundProperty, value);
        }
        public static readonly DependencyProperty ButtonsBarBackgroundProperty =
            DependencyProperty.RegisterAttached("ButtonsBarBackground", typeof(Brush), typeof(DialogParameters), new PropertyMetadata(new SolidColorBrush(Colors.White) { Opacity = 0.2 }));
        public static Brush ButtonsBarBackground { get => GetButtonsBarBackground(null); set => SetButtonsBarBackground(null, value); }

        public static Brush GetBorderBackground(Application obj)
        {
            return (Brush)current.GetValue(BorderBackgroundProperty);
        }
        public static void SetBorderBackground(Application obj, Brush value)
        {
            current.SetValue(BorderBackgroundProperty, value);
        }
        public static readonly DependencyProperty BorderBackgroundProperty =
            DependencyProperty.RegisterAttached("BorderBackground", typeof(Brush), typeof(DialogParameters), new PropertyMetadata(null));
        public static Brush BorderBackground { get => GetBorderBackground(null); set => SetBorderBackground(null, value); }

        public static Brush GetButtonsBackground(Application obj)
        {
            return (Brush)current.GetValue(ButtonsBackgroundProperty);
        }
        public static void SetButtonsBackground(Application obj, Brush value)
        {
            current.SetValue(ButtonsBackgroundProperty, value);
        }
        public static readonly DependencyProperty ButtonsBackgroundProperty =
            DependencyProperty.RegisterAttached("ButtonsBackground", typeof(Brush), typeof(DialogParameters), new PropertyMetadata(new SolidColorBrush(Colors.White) { Opacity = 0.5 }));
        public static Brush ButtonsBackground { get => GetButtonsBackground(null); set => SetButtonsBackground(null, value); }

        public static Brush GetDialogBackground(Application obj)
        {
            return (Brush)current.GetValue(DialogBackgroundProperty);
        }
        public static void SetDialogBackground(Application obj, Brush value)
        {
            current.SetValue(DialogBackgroundProperty, value);
        }
        public static readonly DependencyProperty DialogBackgroundProperty =
            DependencyProperty.RegisterAttached("DialogBackground", typeof(Brush), typeof(DialogParameters), new PropertyMetadata(new SolidColorBrush(Colors.LightBlue)));
        public static Brush DialogBackground { get => GetDialogBackground(null); set => SetDialogBackground(null, value); }
    }
}
