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

        [AttachedPropertyBrowsableForType(typeof(Application))]public static Brush GetTitleBackGround(DependencyObject obj)
        {
            return (Brush)current.GetValue(TitleBackGroundProperty);
        }
        [AttachedPropertyBrowsableForType(typeof(Application))]public static void SetTitleBackGround(DependencyObject obj, Brush value)
        {
            current.SetValue(TitleBackGroundProperty, value);
        }
        public static readonly DependencyProperty TitleBackGroundProperty =
            DependencyProperty.RegisterAttached("TitleBackGround", typeof(Brush), typeof(DialogParameters), new FrameworkPropertyMetadata(Brushes.AliceBlue));
        public static Brush TitleBackground { get => GetTitleBackGround(current); set => SetTitleBackGround(current, value); }

        public static Brush GetButtonsBarBackground(DependencyObject obj)
        {
            return (Brush)current.GetValue(ButtonsBarBackgroundProperty);
        }
        public static void SetButtonsBarBackground(DependencyObject obj, Brush value)
        {
            current.SetValue(ButtonsBarBackgroundProperty, value);
        }
        public static readonly DependencyProperty ButtonsBarBackgroundProperty =
            DependencyProperty.RegisterAttached("ButtonsBarBackground", typeof(Brush), typeof(DialogParameters), new PropertyMetadata(Brushes.CornflowerBlue));
        public static Brush ButtonsBarBackground { get => GetButtonsBarBackground(current); set => SetButtonsBarBackground(current, value); }

        public static Brush GetBorderBackground(DependencyObject obj)
        {
            return (Brush)current.GetValue(BorderBackgroundProperty);
        }
        public static void SetBorderBackground(DependencyObject obj, Brush value)
        {
            current.SetValue(BorderBackgroundProperty, value);
        }
        public static readonly DependencyProperty BorderBackgroundProperty =
            DependencyProperty.RegisterAttached("BorderBackground", typeof(Brush), typeof(DialogParameters), new PropertyMetadata(Brushes.SteelBlue));
        public static Brush BorderBackground { get => GetBorderBackground(current); set => SetBorderBackground(current, value); }

        public static Brush GetButtonsBackground(DependencyObject obj)
        {
            return (Brush)obj.GetValue(ButtonsBackgroundProperty);
        }
        public static void SetButtonsBackground(DependencyObject obj, Brush value)
        {
            obj.SetValue(ButtonsBackgroundProperty, value);
        }
        public static readonly DependencyProperty ButtonsBackgroundProperty =
            DependencyProperty.RegisterAttached("ButtonsBackground", typeof(Brush), typeof(DialogParameters), new PropertyMetadata(new SolidColorBrush(Colors.White) { Opacity = 0.5 }));
        public static Brush ButtonsBackground { get => GetButtonsBackground(current); set => SetButtonsBackground(current, value); }
    }
}
