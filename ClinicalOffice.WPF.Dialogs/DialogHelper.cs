using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ClinicalOffice.WPF.Dialogs
{
    public static class DialogHelper
    {
        static Dispatcher Dispatcher {get => Application.Current.Dispatcher;}
        public static Task<DialogResult> ShowMessage(ContentControl parent, string text, string caption, UIElement icon, Brush theme)
        {
            if (!Dispatcher.CheckAccess()) return Dispatcher.Invoke(() => ShowMessage(parent, text, caption, icon, theme));
            var w = new DialogMessage();
            w.Text = text;
            w.DialogTitle = caption;
            if (theme != null) w.DialogBackGround = theme;
            w.Icon = icon;
            return w.ShowDialogAsync(parent);
        }
    }
}
