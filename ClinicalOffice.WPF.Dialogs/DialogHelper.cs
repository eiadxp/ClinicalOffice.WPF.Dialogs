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

        public static DialogBase CreateDialog(object content, string title = null, DialogButtons buttons = DialogButtons.None)
        {
            return Invoke(() => new DialogBase() { DialogTitle = title, DialogButtons = buttons, DialogContent = content });
        }

        public static DialogBase ShowDialog(object content, string title = null, ContentControl parent = null, DialogButtons buttons = DialogButtons.None)
        {
            var w = CreateDialog(content, title, buttons);
            Invoke(() => w.ShowDialog(parent));
            return w;
        }
        public static DialogBase ShowDialog<TContent>(string title = null, ContentControl parent = null, DialogButtons buttons = DialogButtons.None) where TContent : new()
        {
            return ShowDialog(new TContent(), title, parent, buttons);
        }
        public static Task<DialogResult> ShowDialogAsync(object content, string title = null, ContentControl parent = null, DialogButtons buttons = DialogButtons.None)
        {
            var w = CreateDialog(content, title, buttons);
            return w.ShowDialogAsync(parent);
        }
        public static Task<DialogResult> ShowDialogAsync<TContent>(string title = null, ContentControl parent = null, DialogButtons buttons = DialogButtons.None) where TContent : new()
        {
            return ShowDialogAsync(new TContent(), title, parent, buttons);
        }

        public static Task<DialogResult> ShowMessageAsync(string text, string caption, ContentControl parent = null, UIElement icon = null, Brush theme = null, DialogButtons buttons = DialogButtons.Ok)
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
        public static Task<DialogResult> ShowMessageAsync(string text, string caption, DialogMessageType type, ContentControl parent = null)
        {
            DialogButtons b = (type == DialogMessageType.Question) ? DialogButtons.YesNo : DialogButtons.Ok;
            return ShowMessageAsync(text, caption, type, b, parent);
        }
        public static Task<DialogResult> ShowMessageAsync(string text, string caption, DialogMessageType type, DialogButtons buttons, ContentControl parent = null)
        {
            if (!Dispatcher.CheckAccess()) return Dispatcher.Invoke(() => ShowMessageAsync(text, caption, type, buttons, parent));
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
        public static DialogMessage ShowMessage(string text, string caption, ContentControl parent = null, UIElement icon = null, Brush theme = null, DialogButtons buttons = DialogButtons.Ok)
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
        public static DialogMessage ShowMessage(string text, string caption, DialogMessageType type, ContentControl parent = null)
        {
            DialogButtons b = (type == DialogMessageType.Question) ? DialogButtons.YesNo : DialogButtons.Ok;
            return ShowMessage(text, caption, type, b, parent);
        }
        public static DialogMessage ShowMessage(string text, string caption, DialogMessageType type, DialogButtons buttons, ContentControl parent = null)
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

        static UIElement CreateWaitControl(object content)
        {
            return Invoke(() => 
            {
                var wait = new ProgressBar() { IsIndeterminate = true, MinHeight = 24, MinWidth = 200 };
                var cont = new ContentControl() { Content = content, Margin = new Thickness(5) };
                var pnl = new StackPanel();
                pnl.Children.Add(cont);
                pnl.Children.Add(wait);
                return pnl;
            });
        }

        public static DialogBase ShowWait(UIElement waitControl = null, string title = null, object content = null, ContentControl parent = null, DialogButtons buttons = DialogButtons.None)
        {
            if (waitControl == null) waitControl = CreateWaitControl(content);
            return Invoke(() => ShowDialog(waitControl, title, parent, buttons));
        }
        public static void ShowWait(Action waitingAction, UIElement waitControl = null, string title = null, object content = null, ContentControl parent = null, DialogButtons buttons = DialogButtons.None)
        {
            Invoke(() =>
            {
                if (waitControl == null) waitControl = CreateWaitControl(content);
                var w = ShowDialog(waitControl, title, parent, buttons);
                waitingAction();
                w.Close();
            });
        }
        public static void ShowWait(Task waitingTask, UIElement waitControl = null, string title = null, object content = null, ContentControl parent = null, DialogButtons buttons = DialogButtons.None)
        {
            if (waitControl == null) waitControl = CreateWaitControl(content);
            switch (waitingTask.Status)
            {
                case TaskStatus.Created:
                case TaskStatus.Running:
                case TaskStatus.WaitingForActivation:
                case TaskStatus.WaitingToRun:
                case TaskStatus.WaitingForChildrenToComplete:
                    var w = ShowDialog(waitControl, title, parent, buttons);
                    waitingTask.ContinueWith((a) => Invoke(() => w.Close()));
                    if (waitingTask.Status == TaskStatus.Created || 
                        waitingTask.Status == TaskStatus.WaitingForActivation || 
                        waitingTask.Status == TaskStatus.WaitingToRun)
                            waitingTask.Start();
                    break;
                case TaskStatus.RanToCompletion:
                case TaskStatus.Canceled:
                case TaskStatus.Faulted:
                default:
                    break;
            }
        }
    }
}
