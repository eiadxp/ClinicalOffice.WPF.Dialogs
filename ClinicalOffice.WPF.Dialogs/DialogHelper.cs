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
        static Dispatcher Dispatcher { get => Application.Current.Dispatcher; }
        public static Task<DialogResult> ShowMessageAsync(ContentControl parent, string text, string caption, UIElement icon = null, Brush theme = null, DialogButtons buttons = DialogButtons.Ok)
        {
            if (!Dispatcher.CheckAccess()) return Dispatcher.Invoke(() => ShowMessageAsync(parent, text, caption, icon, theme, buttons));
            var w = new DialogMessage();
            w.DialogButtons = buttons;
            w.Text = text;
            w.DialogTitle = caption;
            if (theme != null) w.DialogBackGround = theme;
            if (icon != null) w.Icon = icon;
            return w.ShowDialogAsync(parent);
        }
        public static Task<DialogResult> ShowMessageAsync(string text, string caption, UIElement icon = null, Brush theme = null, DialogButtons buttons = DialogButtons.Ok)
        {
            return ShowMessageAsync(Application.Current.MainWindow, text, caption, icon, theme, buttons);
        }
        public static Task<DialogResult> ShowMessageAsync(string text, string caption, DialogMessageType type)
        {
            return ShowMessageAsync(Application.Current.MainWindow, text, caption, type);
        }
        public static Task<DialogResult> ShowMessageAsync(ContentControl parent, string text, string caption, DialogMessageType type)
        {
            DialogButtons b = (type == DialogMessageType.Question) ? DialogButtons.YesNo : DialogButtons.Ok;
            return ShowMessageAsync(parent, text, caption, type, b);
        }
        public static Task<DialogResult> ShowMessageAsync(string text, string caption, DialogMessageType type, DialogButtons buttons)
        {
            return ShowMessageAsync(Application.Current.MainWindow, text, caption, type, buttons);
        }
        public static Task<DialogResult> ShowMessageAsync(ContentControl parent, string text, string caption, DialogMessageType type, DialogButtons buttons)
        {
            if (!Dispatcher.CheckAccess()) return Dispatcher.Invoke(() => ShowMessageAsync(parent, text, caption, type, buttons));
            var w = new DialogMessage();
            w.DialogButtons = buttons;
            w.Text = text;
            w.DialogTitle = caption;
            var theme = (Brush)w.TryFindResource(type.ToString() + "Brush");
            if (theme != null) w.DialogBackGround = theme;
            var icon = (UIElement)w.FindResource(type.ToString() + "Icon");
            if (icon != null) w.Icon = icon;
            return w.ShowDialogAsync(parent);
        }
        public static DialogMessage ShowMessage(string text, string caption, UIElement icon = null, Brush theme = null, DialogButtons buttons = DialogButtons.Ok)
        {
            return ShowMessage(Application.Current.MainWindow, text, caption, icon, theme, buttons);
        }
        public static DialogMessage ShowMessage(ContentControl parent, string text, string caption, UIElement icon = null, Brush theme = null, DialogButtons buttons = DialogButtons.Ok)
        {
            if (!Dispatcher.CheckAccess()) return Dispatcher.Invoke(() => ShowMessage(parent, text, caption, icon, theme, buttons));
            var w = new DialogMessage();
            w.DialogButtons = buttons;
            w.Text = text;
            w.DialogTitle = caption;
            if (theme != null) w.DialogBackGround = theme;
            if (icon != null) w.Icon = icon;
            w.ShowDialog(parent);
            return w;
        }
        public static DialogMessage ShowMessage(string text, string caption, DialogMessageType type)
        {
            return ShowMessage(Application.Current.MainWindow, text, caption, type);
        }
        public static DialogMessage ShowMessage(ContentControl parent, string text, string caption, DialogMessageType type)
        {
            DialogButtons b = (type == DialogMessageType.Question) ? DialogButtons.YesNo : DialogButtons.Ok;
            return ShowMessage(parent, text, caption, type, b);
        }
        public static DialogMessage ShowMessage(string text, string caption, DialogMessageType type, DialogButtons buttons)
        {
            return ShowMessage(Application.Current.MainWindow, text, caption, type, buttons);
        }
        public static DialogMessage ShowMessage(ContentControl parent, string text, string caption, DialogMessageType type, DialogButtons buttons)
        {
            if (!Dispatcher.CheckAccess()) return Dispatcher.Invoke(() => ShowMessage(parent, text, caption, type, buttons));
            var w = new DialogMessage();
            w.DialogButtons = buttons;
            w.Text = text;
            w.DialogTitle = caption;
            var theme = (Brush)w.TryFindResource(type.ToString() + "Brush");
            if (theme != null) w.DialogBackGround = theme;
            var icon = (UIElement)w.FindResource(type.ToString() + "Icon");
            if (icon != null) w.Icon = icon;
            w.ShowDialog(parent);
            return w;
        }
    }
}
