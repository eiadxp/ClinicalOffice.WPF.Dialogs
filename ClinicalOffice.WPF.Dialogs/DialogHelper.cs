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

        public static DialogBase ShowDialog(ContentControl parent, object content, string title = null, DialogButtons buttons = DialogButtons.None)
        {
            var w = CreateDialog(content, title, buttons);
            Invoke(() => w.ShowDialog(parent));
            return w;
        }
        public static DialogBase ShowDialog(object content, string title = null, DialogButtons buttons = DialogButtons.None)
        {
            return ShowDialog(Application.Current.MainWindow, content, title, buttons);
        }

        public static Task<DialogResult> ShowMessageAsync(ContentControl parent, string text, string caption, UIElement icon = null, Brush theme = null, DialogButtons buttons = DialogButtons.Ok)
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

        static ProgressBar CreateWaitControl() => Invoke(() => new ProgressBar() { IsIndeterminate = true, MinHeight = 24, MinWidth = 200 });
        public static DialogBase ShowWait(ContentControl parent, UIElement waitControl, string text, DialogButtons buttons = DialogButtons.None)
        {
            return Invoke(() => ShowDialog(parent, waitControl, text, buttons));
        }
        public static DialogBase ShowWait(string text, UIElement waitControl, DialogButtons buttons = DialogButtons.None)
        {
            return ShowWait(Application.Current.MainWindow, waitControl, text, buttons);
        }
        public static DialogBase ShowWait(ContentControl parent, string text, DialogButtons buttons = DialogButtons.None)
        {
            return ShowWait(parent, CreateWaitControl(), text, buttons);
        }
        public static DialogBase ShowWait(string text, DialogButtons buttons = DialogButtons.None)
        {
            return ShowWait(Application.Current.MainWindow, text, buttons);
        }
        public static void ShowWait(ContentControl parent, UIElement waitControl, string text, Action waitingAction, DialogButtons buttons = DialogButtons.None)
        {
            Invoke(() =>
            {
                var w = ShowDialog(parent, waitControl, text, buttons);
                waitingAction();
                w.Close();
            });
        }
        public static void ShowWait(UIElement waitControl, string text, Action waitingAction, DialogButtons buttons = DialogButtons.None)
        {
            ShowWait(Application.Current.MainWindow, waitControl, text, waitingAction, buttons);
        }
        public static void ShowWait(ContentControl parent, string text, Action waitingAction, DialogButtons buttons = DialogButtons.None)
        {
            Invoke(() =>
            {
                var w = ShowWait(parent, text, buttons);
                waitingAction();
                w.Close();
            });
        }
        public static void ShowWait(string text, Action waitingAction, DialogButtons buttons = DialogButtons.None)
        {
            ShowWait(Application.Current.MainWindow, text, waitingAction, buttons);
        }
        public static void ShowWait(ContentControl parent, UIElement waitControl, string text, Task waitingTask, DialogButtons buttons = DialogButtons.None)
        {
            switch (waitingTask.Status)
            {
                case TaskStatus.Created:
                case TaskStatus.WaitingForActivation:
                case TaskStatus.WaitingToRun:
                case TaskStatus.Running:
                case TaskStatus.WaitingForChildrenToComplete:
                    var w = ShowDialog(parent, waitControl, text, buttons);
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
        public static void ShowWait(UIElement waitControl, string text, Task waitingTask, DialogButtons buttons = DialogButtons.None)
        {
            ShowWait(Application.Current.MainWindow, waitControl, text, waitingTask, buttons);
        }
        public static void ShowWait(ContentControl parent, string text, Task waitingTask, DialogButtons buttons = DialogButtons.None)
        {
            ShowWait(parent, CreateWaitControl(), text, waitingTask, buttons);
        }
        public static void ShowWait(string text, Task waitingTask, DialogButtons buttons = DialogButtons.None)
        {
            ShowWait(Application.Current.MainWindow, text, waitingTask, buttons);
        }
    }
}
