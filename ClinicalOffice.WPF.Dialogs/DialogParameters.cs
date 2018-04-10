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
            DependencyProperty.RegisterAttached("TitleBackGround", typeof(Brush), typeof(DialogParameters), new PropertyMetadata(new SolidColorBrush(Colors.White) { Opacity = 0.6 }));
        public static Brush TitleBackground { get => GetTitleBackGround(null); set => SetTitleBackGround(null, value); }

        public static Brush GetContentBackGround(Application obj)
        {
            return (Brush)current.GetValue(ContentBackGroundProperty);
        }
        public static void SetContentBackGround(Application obj, Brush value)
        {
            current.SetValue(ContentBackGroundProperty, value);
        }
        public static readonly DependencyProperty ContentBackGroundProperty =
            DependencyProperty.RegisterAttached("ContentBackGround", typeof(Brush), typeof(DialogParameters), new PropertyMetadata(new SolidColorBrush(Colors.White) { Opacity = 0.8 }));
        public static Brush ContentBackground { get => GetContentBackGround(null); set => SetContentBackGround(null, value); }

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
            DependencyProperty.RegisterAttached("DialogBackground", typeof(Brush), typeof(DialogParameters), new PropertyMetadata(new SolidColorBrush(Colors.Blue)));
        public static Brush DialogBackground { get => GetDialogBackground(null); set => SetDialogBackground(null, value); }
    }
    public class Parameters : DependencyObject
    {
        //Parameters() { }
        public static Parameters Current { get; } = new Parameters();

        public Brush DialogBackground
        {
            get { return (Brush)GetValue(DialogBackgroundProperty); }
            set { SetValue(DialogBackgroundProperty, value); }
        }
        public static readonly DependencyProperty DialogBackgroundProperty =
            DependencyProperty.Register("DialogBackground", typeof(Brush), typeof(Parameters), new PropertyMetadata(Brushes.Blue));

        public Brush TitleBackground
        {
            get { return (Brush)GetValue(TitleBackgroundProperty); }
            set { SetValue(TitleBackgroundProperty, value); }
        }
        public static readonly DependencyProperty TitleBackgroundProperty =
            DependencyProperty.Register("TitleBackground", typeof(Brush), typeof(Parameters), new PropertyMetadata(new SolidColorBrush(Colors.White) { Opacity = .6 }));
    }
}
