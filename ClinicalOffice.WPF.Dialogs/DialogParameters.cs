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

        public static Brush GetTitleBackGround(Application obj)
        {
            return (Brush)current.GetValue(TitleBackGroundProperty);
        }
        public static void SetTitleBackGround(Application obj, Brush value)
        {
            current.SetValue(TitleBackGroundProperty, value);
        }
        public static readonly DependencyProperty TitleBackGroundProperty =
            DependencyProperty.RegisterAttached("TitleBackGround", typeof(Brush), typeof(DialogParameters), new PropertyMetadata(Brushes.AliceBlue));
        public static Brush TitleBackground { get => GetTitleBackGround(null); set => SetTitleBackGround(null, value); }

        public static Brush GetButtonsBarBackground(Application obj)
        {
            return (Brush)current.GetValue(ButtonsBarBackgroundProperty);
        }
        public static void SetButtonsBarBackground(Application obj, Brush value)
        {
            current.SetValue(ButtonsBarBackgroundProperty, value);
        }
        public static readonly DependencyProperty ButtonsBarBackgroundProperty =
            DependencyProperty.RegisterAttached("ButtonsBarBackground", typeof(Brush), typeof(DialogParameters), new PropertyMetadata(Brushes.CornflowerBlue));
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
            DependencyProperty.RegisterAttached("BorderBackground", typeof(Brush), typeof(DialogParameters), new PropertyMetadata(Brushes.SteelBlue));
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
    }
}
