using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

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
            DependencyProperty.RegisterAttached("DialogBackground", typeof(Brush), typeof(DialogParameters), new PropertyMetadata(Brushes.LightBlue));
        public static Brush DialogBackground { get => GetDialogBackground(null); set => SetDialogBackground(null, value); }

        public static Brush GetDialogOverlay(Application obj)
        {
            return (Brush)current.GetValue(DialogOverlayProperty);
        }
        public static void SetDialogOverlay(Application obj, Brush value)
        {
            current.SetValue(DialogOverlayProperty, value);
        }
        public static readonly DependencyProperty DialogOverlayProperty =
            DependencyProperty.RegisterAttached("DialogOverlay", typeof(Brush), typeof(DialogParameters), new PropertyMetadata(Brushes.Black));
        public static Brush DialogOverlay { get => GetDialogOverlay(null); set => SetDialogOverlay(null, value); }

        public static double GetDialogOverlayOpacity(Application obj)
        {
            return (double)current.GetValue(DialogOverlayOpacityProperty);
        }
        public static void SetDialogOverlayOpacity(Application obj, double value)
        {
            current.SetValue(DialogOverlayOpacityProperty, value);
        }
        public static readonly DependencyProperty DialogOverlayOpacityProperty =
            DependencyProperty.RegisterAttached("DialogOverlayOpacity", typeof(double), typeof(DialogParameters), new PropertyMetadata(0.2));
        public static double DialogOverlayOpacity { get => GetDialogOverlayOpacity(null); set => SetDialogOverlayOpacity(null, value); }

        public static Color GetMessageErrorColor(Application obj)
        {
            return (Color)current.GetValue(MessageErrorColorProperty);
        }
        public static void SetMessageErrorColor(Application obj, Color value)
        {
            current.SetValue(MessageErrorColorProperty, value);
        }
        public static readonly DependencyProperty MessageErrorColorProperty =
            DependencyProperty.RegisterAttached("MessageErrorColor", typeof(Color), typeof(DialogParameters), new PropertyMetadata(Colors.Red));
        public static Color MessageErrorColor { get => GetMessageErrorColor(null); set => SetMessageErrorColor(null, value); }

        public static Color GetMessageInfoColor(Application obj)
        {
            return (Color)current.GetValue(MessageInfoColorProperty);
        }
        public static void SetMessageInfoColor(Application obj, Color value)
        {
            current.SetValue(MessageInfoColorProperty, value);
        }
        public static readonly DependencyProperty MessageInfoColorProperty =
            DependencyProperty.RegisterAttached("MessageInfoColor", typeof(Color), typeof(DialogParameters), new PropertyMetadata(Color.FromRgb(34, 100, 247)));
        public static Color MessageInfoColor { get => GetMessageInfoColor(null); set => SetMessageInfoColor(null, value); }

        public static Color GetMessageWarningColor(Application obj)
        {
            return (Color)current.GetValue(MessageWarningColorProperty);
        }
        public static void SetMessageWarningColor(Application obj, Color value)
        {
            current.SetValue(MessageWarningColorProperty, value);
        }
        public static readonly DependencyProperty MessageWarningColorProperty =
            DependencyProperty.RegisterAttached("MessageWarningColor", typeof(Color), typeof(DialogParameters), new PropertyMetadata(Color.FromRgb(255, 195, 34)));
        public static Color MessageWarningColor { get => GetMessageWarningColor(null); set => SetMessageWarningColor(null, value); }

        public static Color GetMessageQuestionColor(Application obj)
        {
            return (Color)current.GetValue(MessageQuestionColorProperty);
        }
        public static void SetMessageQuestionColor(Application obj, Color value)
        {
            current.SetValue(MessageQuestionColorProperty, value);
        }
        public static readonly DependencyProperty MessageQuestionColorProperty =
            DependencyProperty.RegisterAttached("MessageQuestionColor", typeof(Color), typeof(DialogParameters), new PropertyMetadata(Color.FromRgb(45, 187, 80)));
        public static Color MessageQuestionColor { get => GetMessageQuestionColor(null); set => SetMessageQuestionColor(null, value); }

        public static DialogAnimation GetDialogAnimationIn(Application obj)
        {
            return (DialogAnimation)current.GetValue(DialogAnimationInProperty);
        }
        public static void SetDialogAnimationIn(Application obj, DialogAnimation value)
        {
            current.SetValue(DialogAnimationInProperty, value);
        }
        public static readonly DependencyProperty DialogAnimationInProperty =
            DependencyProperty.RegisterAttached("DialogAnimationIn", typeof(DialogAnimation), typeof(DialogParameters), new PropertyMetadata(DialogAnimation.Fade));
        public static DialogAnimation DialogAnimationIn { get => GetDialogAnimationIn(null); set => SetDialogAnimationIn(null, value); }

        public static Storyboard GetDialogCustomAnimationIn(Application obj)
        {
            return (Storyboard)current.GetValue(DialogCustomAnimationInProperty);
        }
        public static void SetDialogCustomAnimationIn(Application obj, Storyboard value)
        {
            current.SetValue(DialogCustomAnimationInProperty, value);
        }
        public static readonly DependencyProperty DialogCustomAnimationInProperty =
            DependencyProperty.RegisterAttached("DialogCustomAnimationIn", typeof(Storyboard), typeof(DialogParameters), new PropertyMetadata(null));
        public static Storyboard DialogCustomAnimationIn { get => GetDialogCustomAnimationIn(null); set => SetDialogCustomAnimationIn(null, value); }

        public static DialogAnimation GetDialogAnimationOut(Application obj)
        {
            return (DialogAnimation)current.GetValue(DialogAnimationOutProperty);
        }
        public static void SetDialogAnimationOut(Application obj, DialogAnimation value)
        {
            current.SetValue(DialogAnimationOutProperty, value);
        }
        public static readonly DependencyProperty DialogAnimationOutProperty =
            DependencyProperty.RegisterAttached("DialogAnimationOut", typeof(DialogAnimation), typeof(DialogParameters), new PropertyMetadata(DialogAnimation.Fade));
        public static DialogAnimation DialogAnimationOut { get => GetDialogAnimationOut(null); set => SetDialogAnimationOut(null, value); }

        public static Storyboard GetDialogCustomAnimationOut(Application obj)
        {
            return (Storyboard)current.GetValue(DialogCustomAnimationOutProperty);
        }
        public static void SetDialogCustomAnimationOut(Application obj, Storyboard value)
        {
            current.SetValue(DialogCustomAnimationOutProperty, value);
        }
        public static readonly DependencyProperty DialogCustomAnimationOutProperty =
            DependencyProperty.RegisterAttached("DialogCustomAnimationOut", typeof(Storyboard), typeof(DialogParameters), new PropertyMetadata(null));
        public static Storyboard DialogCustomAnimationOut { get => GetDialogCustomAnimationOut(null); set => SetDialogCustomAnimationOut(null, value); }

        public static Duration GetDialogAnimationDuration(Application obj)
        {
            return (Duration)current.GetValue(DialogAnimationDurationProperty);
        }
        public static void SetDialogAnimationDuration(Application obj, Duration value)
        {
            current.SetValue(DialogAnimationDurationProperty, value);
        }
        public static readonly DependencyProperty DialogAnimationDurationProperty =
            DependencyProperty.RegisterAttached("DialogAnimationDuration", typeof(Duration), typeof(DialogParameters), new PropertyMetadata(new Duration(TimeSpan.FromMilliseconds(100))));
        public static Duration DialogAnimationDuration { get => GetDialogAnimationDuration(null); set => SetDialogAnimationDuration(null, value); }

        public static bool GetDialogAutoClose(Application obj)
        {
            return (bool)current.GetValue(DialogAutoCloseProperty);
        }
        public static void SetDialogAutoClose(Application obj, bool value)
        {
            current.SetValue(DialogAutoCloseProperty, value);
        }
        public static readonly DependencyProperty DialogAutoCloseProperty =
            DependencyProperty.RegisterAttached("DialogAutoClose", typeof(bool), typeof(DialogParameters), new PropertyMetadata(true));
        public static bool DialogAutoClose { get => GetDialogAutoClose(null); set => SetDialogAutoClose(null, value); }
    }
}
