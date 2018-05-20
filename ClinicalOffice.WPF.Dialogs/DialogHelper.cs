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
    /// <summary>
    /// A static class that allows you to quickly create and show dialogs, message boxes, and wait dialogs.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Most  of the functions has an overload that acts as an extension method to <see cref="ContentControl"/> which will be the parent of the dialog.
    /// </para>
    /// <para>
    /// The methods of this class can be grouped into three categories:
    /// <list type="number">
    /// <item><description>Dialog Functions: <c>ShowDialog()</c>, <c>ShowDialogAsync()</c>, and <c>CreateDialog()</c>.</description></item>
    /// <item><description>MessageBox Functions: <c>ShowMessage()</c>, and <c>ShowMessageAsync()</c>.</description></item>
    /// <item><description>Wait dialog Functions: <c>ShowWait()</c>, and <c>CreateWaitDialog()</c>.</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    public static class DialogHelper
    {
        static Dispatcher Dispatcher { get => Application.Current.Dispatcher; }
        static T Invoke<T>(Func<T> function)
        {
            if (Dispatcher.CheckAccess()) return function();
            return Dispatcher.Invoke(() => function());
        }
        static void Invoke(Action action)
        {
            if (Dispatcher.CheckAccess()) action();
            else Dispatcher.Invoke(() => action());
        }

        #region Dialog Functions
        public static DialogBase CreateDialog(object content, object title = null, DialogButtons buttons = DialogButtons.None)
        {
            return Invoke(() => new DialogBase() { DialogTitle = title, DialogButtons = buttons, DialogContent = content });
        }

        public static DialogBase ShowDialog(object content, object title = null, DialogButtons buttons = DialogButtons.None)
        {
            return ShowDialog(Application.Current?.MainWindow, content, title, buttons);
        }
        public static DialogBase ShowDialog(this ContentControl parent, object content, object title = null, DialogButtons buttons = DialogButtons.None)
        {
            var w = CreateDialog(content, title, buttons);
            Invoke(() => w.ShowDialog(parent));
            return w;
        }
        public static DialogBase ShowDialog<TContent>(this ContentControl parent, object title = null, DialogButtons buttons = DialogButtons.None) where TContent : new()
        {
            return ShowDialog(parent, new TContent(), title, buttons);
        }
        public static DialogBase ShowDialog<TContent>(object title = null, DialogButtons buttons = DialogButtons.None) where TContent : new()
        {
            return ShowDialog(Application.Current.MainWindow, new TContent(), title, buttons);
        }
        public static Task<DialogResult> ShowDialogAsync(this ContentControl parent, object content, object title = null, DialogButtons buttons = DialogButtons.None)
        {
            var w = CreateDialog(content, title, buttons);
            return w.ShowDialogAsync(parent);
        }
        public static Task<DialogResult> ShowDialogAsync(object content, object title = null, DialogButtons buttons = DialogButtons.None)
        {
            return ShowDialogAsync(Application.Current.MainWindow, content, title, buttons);
        }
        public static Task<DialogResult> ShowDialogAsync<TConent>(this ContentControl parent, object title = null, DialogButtons buttons = DialogButtons.None) where TConent : new()
        {
            return ShowDialogAsync(parent, new TConent(), title, buttons);
        }
        public static Task<DialogResult> ShowDialogAsync<TConent>(object title = null, DialogButtons buttons = DialogButtons.None) where TConent : new()
        {
            return ShowDialogAsync(Application.Current.MainWindow, new TConent(), title, buttons);
        }
        #endregion
        #region MessageBox Functions
        public static Task<DialogResult> ShowMessageAsync(this ContentControl parent, string text, string caption, UIElement icon = null, Brush theme = null, DialogButtons buttons = DialogButtons.Ok)
        {
            return Invoke(() =>
            {
                var w = new DialogMessage
                {
                    DialogButtons = buttons,
                    Text = text,
                    DialogTitle = caption
                };
                if (theme != null) w.DialogBackGround = theme;
                if (icon != null) w.Icon = icon;
                return w.ShowDialogAsync(parent);
            });
        }
        public static Task<DialogResult> ShowMessageAsync(string text, string caption, UIElement icon = null, Brush theme = null, DialogButtons buttons = DialogButtons.Ok)
        {
            return ShowMessageAsync(Application.Current.MainWindow, text, caption, icon, theme, buttons);
        }
        public static Task<DialogResult> ShowMessageAsync(this ContentControl parent, string text, string caption, DialogMessageType type)
        {
            DialogButtons b = (type == DialogMessageType.Question) ? DialogButtons.YesNo : DialogButtons.Ok;
            return ShowMessageAsync(parent, text, caption, type, b);
        }
        public static Task<DialogResult> ShowMessageAsync(string text, string caption, DialogMessageType type)
        {
            return ShowMessageAsync(Application.Current.MainWindow, text, caption, type);
        }
        public static Task<DialogResult> ShowMessageAsync(this ContentControl parent, string text, string caption, DialogMessageType type, DialogButtons buttons)
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
        public static Task<DialogResult> ShowMessageAsync(string text, string caption, DialogMessageType type, DialogButtons buttons)
        {
            return ShowMessageAsync(Application.Current.MainWindow, text, caption, type, buttons);
        }
        public static DialogMessage ShowMessage(this ContentControl parent, string text, string caption, UIElement icon = null, Brush theme = null, DialogButtons buttons = DialogButtons.Ok)
        {
            return Invoke(() =>
            {
                var w = new DialogMessage
                {
                    DialogButtons = buttons,
                    Text = text,
                    DialogTitle = caption
                };
                if (theme != null) w.DialogBackGround = theme;
                if (icon != null) w.Icon = icon;
                w.ShowDialog(parent);
                return w;
            });
        }
        public static DialogMessage ShowMessage(string text, string caption, UIElement icon = null, Brush theme = null, DialogButtons buttons = DialogButtons.Ok)
        {
            return ShowMessage(Application.Current.MainWindow, text, caption, icon, theme, buttons);
        }
        public static DialogMessage ShowMessage(this ContentControl parent, string text, string caption, DialogMessageType type)
        {
            DialogButtons b = (type == DialogMessageType.Question) ? DialogButtons.YesNo : DialogButtons.Ok;
            return ShowMessage(parent, text, caption, type, b);
        }
        public static DialogMessage ShowMessage(string text, string caption, DialogMessageType type)
        {
            return ShowMessage(Application.Current.MainWindow, text, caption, type);
        }
        public static DialogMessage ShowMessage(this ContentControl parent, string text, string caption, DialogMessageType type, DialogButtons buttons)
        {
            return Invoke(() =>
            {
                var w = new DialogMessage
                {
                    DialogButtons = buttons,
                    Text = text,
                    DialogTitle = caption
                };
                var theme = (Brush)w.TryFindResource(type.ToString() + "Brush");
                if (theme != null) w.DialogBackGround = theme;
                var icon = (UIElement)w.FindResource(type.ToString() + "Icon");
                if (icon != null) w.Icon = icon;
                w.ShowDialog(parent);
                return w;
            });
        }
        public static DialogMessage ShowMessage(string text, string caption, DialogMessageType type, DialogButtons buttons)
        {
            return ShowMessage(Application.Current.MainWindow, text, caption, type, buttons);
        }
        #endregion
        #region Wait Dialog Functions
        public static DialogBase CreateWaitDialog(object content = null, UIElement waitControl = null, string title = null, DialogButtons buttons = DialogButtons.None)
        {
            return Invoke(() =>
            {
                var wait = waitControl ?? new ProgressBar() { IsIndeterminate = true, MinHeight = 24, MinWidth = 200 };
                var cont = (content as UIElement) ?? new ContentControl() { Content = content, Margin = new Thickness(5) };
                var pnl = new StackPanel();
                pnl.Children.Add(cont);
                pnl.Children.Add(wait);
                var w = new DialogBase() { DialogContent = pnl, DialogTitle = title, DialogButtons = buttons };
                return w;
            });
        }

        public static DialogBase ShowWait(this ContentControl parent, string title = null, object content = null, UIElement waitControl = null, DialogButtons buttons = DialogButtons.None)
        {
            return Invoke(() =>
            {
                var w = CreateWaitDialog(content, waitControl, title, buttons);
                w.ShowDialog(parent);
                return w;
            });
        }
        public static DialogBase ShowWait(string title = null, object content = null, UIElement waitControl = null, DialogButtons buttons = DialogButtons.None)
        {
            return ShowWait(Application.Current.MainWindow, title, content, waitControl, buttons);
        }
        public static void ShowWait(this ContentControl parent, Action waitingAction, string title = null, object content = null, UIElement waitControl = null, DialogButtons buttons = DialogButtons.None)
        {
            var w = ShowWait(parent, title, content, waitControl, buttons);
            waitingAction();
            Invoke(() => w.Close());
        }
        public static void ShowWait(Action waitingAction, string title = null, object content = null, UIElement waitControl = null, DialogButtons buttons = DialogButtons.None)
        {
            ShowWait(Application.Current.MainWindow, waitingAction, title, content, waitControl, buttons);
        }
        public static void ShowWait(this ContentControl parent, Task waitingTask, string title = null, object content = null, UIElement waitControl = null, DialogButtons buttons = DialogButtons.None, bool autoStartTask = true)
        {
            var w = ShowWait(parent, title, content, waitControl, buttons);
            waitingTask.ContinueWith((a) => Invoke(() => w.Close()));
            if (autoStartTask)
            {
                try
                {
                    waitingTask.Start();
                }
                catch (Exception)
                {
                }
            }
        }
        public static void ShowWait(Task waitingTask, string title = null, object content = null, UIElement waitControl = null, DialogButtons buttons = DialogButtons.None)
        {
            ShowWait(Application.Current.MainWindow, waitingTask, title, content, waitControl, buttons);
        }
        #endregion
    }
}
