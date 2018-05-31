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
        #region private methods
        /// <summary>
        /// UI dispatcher used for thread safe access of UI element
        /// </summary>
        static Dispatcher Dispatcher { get => Application.Current.Dispatcher; }
        /// <summary>
        /// Invokes a function in the UI thread to keep thread safe access of UI element, and returns its result.
        /// </summary>
        /// <typeparam name="T">Generic type returned by the function.</typeparam>
        /// <param name="function">Function to be accessed in the UI thread.</param>
        /// <returns>It will returns the result from function.</returns>
        static T Invoke<T>(Func<T> function)
        {
            if (Dispatcher.CheckAccess()) return function();
            return Dispatcher.Invoke(() => function());
        }
        /// <summary>
        /// Invokes a method in the UI thread to keep thread safe access of UI element.
        /// </summary>
        /// <param name="action">Method to be accessed in the UI thread.</param>
        static void Invoke(Action action)
        {
            if (Dispatcher.CheckAccess()) action();
            else Dispatcher.Invoke(() => action());
        }
        #endregion

        #region Dialog Functions
        /// <summary>
        /// Create a <see cref="DialogBase"/> object to represent your content.
        /// </summary>
        /// <param name="content">The content to be presented in the dialog.</param>
        /// <param name="title">Dialog title.</param>
        /// <param name="buttons">Dialog buttons to be displayed (by default no buttons displayed).</param>
        /// <returns>A <see cref="DialogBase"/> to display the content (the dialog is not visible yet).</returns>
        public static DialogBase CreateDialog(object content, object title = null, DialogButtons buttons = DialogButtons.None)
        {
            return Invoke(() => new DialogBase() { DialogTitle = title, DialogButtons = buttons, DialogContent = content });
        }

        /// <summary>
        /// Show a <see cref="DialogBase"/> object to represent your content. The main application window is used as parent.
        /// </summary>
        /// <param name="content">The content to be presented in the dialog.</param>
        /// <param name="title">Dialog title.</param>
        /// <param name="buttons">Dialog buttons to be displayed (by default no buttons displayed).</param>
        /// <returns>A <see cref="DialogBase"/> that displays the content (the dialog is visible).</returns>
        public static DialogBase ShowDialog(object content, object title = null, DialogButtons buttons = DialogButtons.None)
        {
            return ShowDialog(Application.Current?.MainWindow, content, title, buttons);
        }
        /// <summary>
        /// Show a <see cref="DialogBase"/> object to represent your content inside <see cref="ContentControl"/>.
        /// </summary>
        /// <param name="parent">A <see cref="ContentControl"/> used to display the dialog inside it.</param>
        /// <param name="content">The content to be presented in the dialog.</param>
        /// <param name="title">Dialog title.</param>
        /// <param name="buttons">Dialog buttons to be displayed (by default no buttons displayed).</param>
        /// <returns>A <see cref="DialogBase"/> that displays the content (the dialog is visible).</returns>
        public static DialogBase ShowDialog(this ContentControl parent, object content, object title = null, DialogButtons buttons = DialogButtons.None)
        {
            var w = CreateDialog(content, title, buttons);
            Invoke(() => w.ShowDialog(parent));
            return w;
        }
        /// <summary>
        /// Show a <see cref="DialogBase"/> object to represent your content inside another <see cref="ContentControl"/>.
        /// </summary>
        /// <typeparam name="TContent">Generic type of your content (should has a parameterless constructor).</typeparam>
        /// <param name="parent">A <see cref="ContentControl"/> used to display the dialog inside it.</param>
        /// <param name="title">Dialog title.</param>
        /// <param name="buttons">Dialog buttons to be displayed (by default no buttons displayed).</param>
        /// <returns>A <see cref="DialogBase"/> that displays the content (the dialog is visible).</returns>
        public static DialogBase ShowDialog<TContent>(this ContentControl parent, object title = null, DialogButtons buttons = DialogButtons.None) where TContent : new()
        {
            return ShowDialog(parent, new TContent(), title, buttons);
        }
        /// <summary>
        /// Show a <see cref="DialogBase"/> object to represent your content. The main application window is used as parent.
        /// </summary>
        /// <typeparam name="TContent">Generic type of your content (should has a parameterless constructor).</typeparam>
        /// <param name="title">Dialog title.</param>
        /// <param name="buttons">Dialog buttons to be displayed (by default no buttons displayed).</param>
        /// <returns>A <see cref="DialogBase"/> that displays the content (the dialog is visible).</returns>
        public static DialogBase ShowDialog<TContent>(object title = null, DialogButtons buttons = DialogButtons.None) where TContent : new()
        {
            return ShowDialog(Application.Current.MainWindow, new TContent(), title, buttons);
        }
        /// <summary>
        /// Async version of <see cref="ShowDialog(ContentControl, object, object, DialogButtons)"/> that returns the dialog result.
        /// </summary>
        /// <param name="parent">A <see cref="ContentControl"/> used to display the dialog inside it.</param>
        /// <param name="content">The content to be presented in the dialog.</param>
        /// <param name="title">Dialog title.</param>
        /// <param name="buttons">Dialog buttons to be displayed (by default no buttons displayed).</param>
        /// <returns>a <see cref="Task{DialogResult}"/> that can be awaited to get the dialog result after closing.</returns>
        /// <remarks>
        /// When we use this overload in async/await function, the calling function will wait until the dialog is closed and get the dialog result.
        /// </remarks>
        public static Task<DialogResult> ShowDialogAsync(this ContentControl parent, object content, object title = null, DialogButtons buttons = DialogButtons.None)
        {
            var w = CreateDialog(content, title, buttons);
            return w.ShowDialogAsync(parent);
        }
        /// <summary>
        /// Async version of <see cref="ShowDialog(object, object, DialogButtons)"/> that returns the dialog result.
        /// </summary>
        /// <param name="content">The content to be presented in the dialog.</param>
        /// <param name="title">Dialog title.</param>
        /// <param name="buttons">Dialog buttons to be displayed (by default no buttons displayed).</param>
        /// <returns>a <see cref="Task{DialogResult}"/> that can be awaited to get the dialog result after closing.</returns>
        /// <remarks>
        /// When we use this overload in async/await function, the calling function will wait until the dialog is closed and get the dialog result.
        /// </remarks>
        public static Task<DialogResult> ShowDialogAsync(object content, object title = null, DialogButtons buttons = DialogButtons.None)
        {
            return ShowDialogAsync(Application.Current.MainWindow, content, title, buttons);
        }
        /// <summary>
        /// Async version of <see cref="ShowDialog{TContent}(ContentControl, object, DialogButtons)"/> that returns the dialog result.
        /// </summary>
        /// <typeparam name="TContent">Generic type of your content (should has a parameterless constructor).</typeparam>
        /// <param name="parent">A <see cref="ContentControl"/> used to display the dialog inside it.</param>
        /// <param name="title">Dialog title.</param>
        /// <param name="buttons">Dialog buttons to be displayed (by default no buttons displayed).</param>
        /// <returns>a <see cref="Task{DialogResult}"/> that can be awaited to get the dialog result after closing.</returns>
        /// <remarks>
        /// When we use this overload in async/await function, the calling function will wait until the dialog is closed and get the dialog result.
        /// </remarks>
        public static Task<DialogResult> ShowDialogAsync<TConent>(this ContentControl parent, object title = null, DialogButtons buttons = DialogButtons.None) where TConent : new()
        {
            return ShowDialogAsync(parent, new TConent(), title, buttons);
        }
        /// <summary>
        /// Async version of <see cref="ShowDialog{TContent}(object, DialogButtons)"/> that returns the dialog result.
        /// </summary>
        /// <typeparam name="TContent">Generic type of your content (should has a parameterless constructor).</typeparam>
        /// <param name="title">Dialog title.</param>
        /// <param name="buttons">Dialog buttons to be displayed (by default no buttons displayed).</param>
        /// <returns>a <see cref="Task{DialogResult}"/> that can be awaited to get the dialog result after closing.</returns>
        /// <remarks>
        /// When we use this overload in async/await function, the calling function will wait until the dialog is closed and get the dialog result.
        /// </remarks>
        public static Task<DialogResult> ShowDialogAsync<TConent>(object title = null, DialogButtons buttons = DialogButtons.None) where TConent : new()
        {
            return ShowDialogAsync(Application.Current.MainWindow, new TConent(), title, buttons);
        }
        #endregion
        #region MessageBox Functions
        /// <summary>
        /// Display a message box inside a parent <see cref="ContentControl"/>, and returns immediately before closing the message.
        /// </summary>
        /// <param name="parent">A <see cref="ContentControl"/> used to display the message box inside it.</param>
        /// <param name="text">Message text.</param>
        /// <param name="caption">Message box title.</param>
        /// <param name="icon">Optional icon to be displayed before the text.</param>
        /// <param name="theme">Optional theme brush to be used for dialog painting.</param>
        /// <param name="buttons">Optional, buttons displayed for the message box(default is OK button).</param>
        /// <returns>A <see cref="DialogMessage"/> that represent the displayed message box.</returns>
        public static DialogMessage ShowMessage(this ContentControl parent, string text, string caption = "", UIElement icon = null, Brush theme = null, DialogButtons buttons = DialogButtons.Ok)
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
        /// <summary>
        /// Display a message box inside the application main window, and returns immediately before closing the message.
        /// </summary>
        /// <param name="text">Message text.</param>
        /// <param name="caption">Message box title.</param>
        /// <param name="icon">Optional icon to be displayed before the text.</param>
        /// <param name="theme">Optional theme brush to be used for dialog painting.</param>
        /// <param name="buttons">Optional, buttons displayed for the message box(default is OK button).</param>
        /// <returns>A <see cref="DialogMessage"/> that represent the displayed message box.</returns>
        public static DialogMessage ShowMessage(string text, string caption = "", UIElement icon = null, Brush theme = null, DialogButtons buttons = DialogButtons.Ok)
        {
            return ShowMessage(Application.Current.MainWindow, text, caption, icon, theme, buttons);
        }
        /// <summary>
        /// Display a message box inside a parent <see cref="ContentControl"/>, and returns immediately before closing the message.
        /// </summary>
        /// <param name="parent">A <see cref="ContentControl"/> used to display the message box inside it.</param>
        /// <param name="text">Message text.</param>
        /// <param name="caption">Message box title.</param>
        /// <param name="type">Type of the message box (info, warning, error, or question) that will determine the icon, them brush, and buttons of the message.</param>
        /// <returns>A <see cref="DialogMessage"/> that represent the displayed message box.</returns>
        public static DialogMessage ShowMessage(this ContentControl parent, string text, string caption = "", DialogMessageType type = DialogMessageType.Info)
        {
            DialogButtons b = (type == DialogMessageType.Question) ? DialogButtons.YesNo : DialogButtons.Ok;
            return ShowMessage(parent, text, caption, type, b);
        }
        /// <summary>
        /// Display a message box inside the application main window, and returns immediately before closing the message.
        /// </summary>
        /// <param name="text">Message text.</param>
        /// <param name="caption">Message box title.</param>
        /// <param name="type">Type of the message box (info, warning, error, or question) that will determine the icon, them brush, and buttons of the message.</param>
        /// <returns>A <see cref="DialogMessage"/> that represent the displayed message box.</returns>
        public static DialogMessage ShowMessage(string text, string caption = "", DialogMessageType type = DialogMessageType.Info)
        {
            return ShowMessage(Application.Current.MainWindow, text, caption, type);
        }
        /// <summary>
        /// Display a message box inside a parent <see cref="ContentControl"/>, and returns immediately before closing the message.
        /// </summary>
        /// <param name="parent">A <see cref="ContentControl"/> used to display the message box inside it.</param>
        /// <param name="text">Message text.</param>
        /// <param name="caption">Message box title.</param>
        /// <param name="type">Type of the message box (info, warning, error, or question) that will determine the icon and them brush of the message.</param>
        /// <param name="buttons">Optional, buttons displayed for the message box(default is OK button).</param>
        /// <returns>A <see cref="DialogMessage"/> that represent the displayed message box.</returns>
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
        /// <summary>
        /// Display a message box inside the application main window, and returns immediately before closing the message.
        /// </summary>
        /// <param name="text">Message text.</param>
        /// <param name="caption">Message box title.</param>
        /// <param name="type">Type of the message box (info, warning, error, or question) that will determine the icon and them brush of the message.</param>
        /// <param name="buttons">Optional, buttons displayed for the message box(default is OK button).</param>
        /// <returns>A <see cref="DialogMessage"/> that represent the displayed message box.</returns>
        public static DialogMessage ShowMessage(string text, string caption, DialogMessageType type, DialogButtons buttons)
        {
            return ShowMessage(Application.Current.MainWindow, text, caption, type, buttons);
        }
        /// <summary>
        /// Async version of <see cref="ShowMessage(ContentControl, string, string, UIElement, Brush, DialogButtons)"/>.
        /// Display a message box inside a parent <see cref="ContentControl"/>, 
        /// if used in await/async method it will return after closing the message box.
        /// </summary>
        /// <param name="parent">A <see cref="ContentControl"/> used to display the message box inside it.</param>
        /// <param name="text">Message text.</param>
        /// <param name="caption">Message box title.</param>
        /// <param name="icon">Optional icon to be displayed before the text.</param>
        /// <param name="theme">Optional theme brush to be used for dialog painting.</param>
        /// <param name="buttons">Optional, buttons displayed for the message box(default is OK button).</param>
        /// <returns>a <see cref="Task{DialogResult}"/> that can be awaited to get the message result after closing.</returns>
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
        /// <summary>
        /// Async version of <see cref="ShowMessage(string, string, UIElement, Brush, DialogButtons)"/>.
        /// Display a message box inside the application main window/>, 
        /// if used in await/async method it will return after closing the message box.
        /// </summary>
        /// <param name="text">Message text.</param>
        /// <param name="caption">Message box title.</param>
        /// <param name="icon">Optional icon to be displayed before the text.</param>
        /// <param name="theme">Optional theme brush to be used for dialog painting.</param>
        /// <param name="buttons">Optional, buttons displayed for the message box(default is OK button).</param>
        /// <returns>a <see cref="Task{DialogResult}"/> that can be awaited to get the message result after closing.</returns>
        public static Task<DialogResult> ShowMessageAsync(string text, string caption, UIElement icon = null, Brush theme = null, DialogButtons buttons = DialogButtons.Ok)
        {
            return ShowMessageAsync(Application.Current.MainWindow, text, caption, icon, theme, buttons);
        }
        /// <summary>
        /// Async version of <see cref="ShowMessage(ContentControl, string, string, DialogMessageType)"/>.
        /// Display a message box inside a parent <see cref="ContentControl"/>, 
        /// if used in await/async method it will return after closing the message box.
        /// </summary>
        /// <param name="parent">A <see cref="ContentControl"/> used to display the message box inside it.</param>
        /// <param name="text">Message text.</param>
        /// <param name="caption">Message box title.</param>
        /// <param name="type">Type of the message box (info, warning, error, or question) that will determine the icon, them brush, and buttons of the message.</param>
        /// <returns>a <see cref="Task{DialogResult}"/> that can be awaited to get the message result after closing.</returns>
        public static Task<DialogResult> ShowMessageAsync(this ContentControl parent, string text, string caption, DialogMessageType type)
        {
            DialogButtons b = (type == DialogMessageType.Question) ? DialogButtons.YesNo : DialogButtons.Ok;
            return ShowMessageAsync(parent, text, caption, type, b);
        }
        /// <summary>
        /// Async version of <see cref="ShowMessage(string, string, UIElement, Brush, DialogButtons)"/>.
        /// Display a message box inside the application main window, 
        /// if used in await/async method it will return after closing the message box.
        /// </summary>
        /// <param name="text">Message text.</param>
        /// <param name="caption">Message box title.</param>
        /// <param name="type">Type of the message box (info, warning, error, or question) that will determine the icon, them brush, and buttons of the message.</param>
        /// <returns>a <see cref="Task{DialogResult}"/> that can be awaited to get the message result after closing.</returns>
        public static Task<DialogResult> ShowMessageAsync(string text, string caption, DialogMessageType type)
        {
            return ShowMessageAsync(Application.Current.MainWindow, text, caption, type);
        }
        /// <summary>
        /// Async version of <see cref="ShowMessage(ContentControl, string, string, DialogMessageType)"/>.
        /// Display a message box inside a parent <see cref="ContentControl"/>, 
        /// if used in await/async method it will return after closing the message box.
        /// </summary>
        /// <param name="parent">A <see cref="ContentControl"/> used to display the message box inside it.</param>
        /// <param name="text">Message text.</param>
        /// <param name="caption">Message box title.</param>
        /// <param name="type">Type of the message box (info, warning, error, or question) that will determine the icon, them brush, and buttons of the message.</param>
        /// <param name="buttons">Buttons displayed for the message box(default is OK button).</param>
        /// <returns>a <see cref="Task{DialogResult}"/> that can be awaited to get the message result after closing.</returns>
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
        /// <summary>
        /// Async version of <see cref="ShowMessage(ContentControl, string, string, DialogMessageType)"/>.
        /// Display a message box inside the application main window/>, 
        /// if used in await/async method it will return after closing the message box.
        /// </summary>
        /// <param name="text">Message text.</param>
        /// <param name="caption">Message box title.</param>
        /// <param name="type">Type of the message box (info, warning, error, or question) that will determine the icon and them brush of the message.</param>
        /// <param name="buttons">Buttons displayed for the message box(default is OK button).</param>
        /// <returns>a <see cref="Task{DialogResult}"/> that can be awaited to get the message result after closing.</returns>
        public static Task<DialogResult> ShowMessageAsync(string text, string caption, DialogMessageType type, DialogButtons buttons)
        {
            return ShowMessageAsync(Application.Current.MainWindow, text, caption, type, buttons);
        }
        #endregion
        #region Wait Dialog Functions
        /// <summary>
        /// Creates a <see cref="DialogBase"/> that contains a waiting control.
        /// </summary>
        /// <param name="title">Waiting box title message.</param>
        /// <param name="content">A content that will be displayed above the waiting control, usually a string note.</param>
        /// <param name="waitControl">The waiting control to be displayed, if <c>null</c> a progress bar will be displayed with its <see cref="ProgressBar.IsIndeterminate"/> property set to <c>true</c>.</param>
        /// <param name="buttons">Buttons displayed at the button of the dialog.</param>
        /// <returns>A <see cref="DialogBase"/> that holds your contents.</returns>
        /// <remarks>
        /// The function does not display the dialog.
        /// You have to call <see cref="DialogBase.ShowDialog(ContentControl)"/> or <see cref="DialogBase.ShowDialogAsync(ContentControl)"/> to show the dialog.
        /// </remarks>
        public static DialogBase CreateWaitDialog(string title = null, object content = null, UIElement waitControl = null, DialogButtons buttons = DialogButtons.None, Brush theme = null)
        {
            return Invoke(() =>
            {
                var wait = waitControl ?? new ProgressBar() { IsIndeterminate = true, MinHeight = 24, MinWidth = 200 };
                var cont = (content as UIElement) ?? new ContentControl() { Content = content, Margin = new Thickness(5) };
                var pnl = new StackPanel();
                pnl.Children.Add(cont);
                pnl.Children.Add(wait);
                var w = new DialogBase() { DialogContent = pnl, DialogTitle = title, DialogButtons = buttons };
                if (theme != null) w.SetTheme(theme);
                return w;
            });
        }

        /// <summary>
        /// Show a <see cref="DialogBase"/> that contains a waiting control.
        /// </summary>
        /// <param name="parent">The <see cref="ContentControl"/> that contains your dialog.</param>
        /// <param name="title">Waiting box title message.</param>
        /// <param name="content">A content that will be displayed above the waiting control, usually a string note.</param>
        /// <param name="waitControl">The waiting control to be displayed, if <c>null</c> a progress bar will be displayed with its <see cref="ProgressBar.IsIndeterminate"/> property set to <c>true</c>.</param>
        /// <param name="buttons">Buttons displayed at the button of the dialog.</param>
        /// <returns>A <see cref="DialogBase"/> that holds your contents.</returns>
        /// <remarks>
        /// The function shows the dialog and returns immediately before closing the dialog.
        /// </remarks>
        public static DialogBase ShowWait(this ContentControl parent, string title = null, object content = null, UIElement waitControl = null, DialogButtons buttons = DialogButtons.None, Brush theme = null)
        {
            return Invoke(() =>
            {
                var w = CreateWaitDialog(title, content, waitControl, buttons, theme);
                w.ShowDialog(parent);
                return w;
            });
        }
        /// <summary>
        /// Show a <see cref="DialogBase"/> that contains a waiting control inside you application's main window.
        /// </summary>
        /// <param name="title">Waiting box title message.</param>
        /// <param name="content">A content that will be displayed above the waiting control, usually a string note.</param>
        /// <param name="waitControl">The waiting control to be displayed, if <c>null</c> a progress bar will be displayed with its <see cref="ProgressBar.IsIndeterminate"/> property set to <c>true</c>.</param>
        /// <param name="buttons">Buttons displayed at the button of the dialog.</param>
        /// <returns>A <see cref="DialogBase"/> that holds your contents.</returns>
        /// <remarks>
        /// The function shows the dialog and returns immediately before closing the dialog.
        /// </remarks>
        public static DialogBase ShowWait(string title = null, object content = null, UIElement waitControl = null, DialogButtons buttons = DialogButtons.None, Brush theme = null)
        {
            return ShowWait(Application.Current.MainWindow, title, content, waitControl, buttons, theme);
        }
        /// <summary>
        /// Show a <see cref="DialogBase"/> that contains a waiting control, and then execute an actions, and when finished closes the dialog.
        /// </summary>
        /// <param name="parent">The <see cref="ContentControl"/> that contains your dialog.</param>
        /// <param name="waitingAction">The action that will be executed and when finished, the dialog will be closed.</param>
        /// <param name="title">Waiting box title message.</param>
        /// <param name="content">A content that will be displayed above the waiting control, usually a string note.</param>
        /// <param name="waitControl">The waiting control to be displayed, if <c>null</c> a progress bar will be displayed with its <see cref="ProgressBar.IsIndeterminate"/> property set to <c>true</c>.</param>
        /// <param name="buttons">Buttons displayed at the button of the dialog.</param>
        /// <remarks>
        /// The method creates and shows the dialog in the UI thread, then it calls the <c>waitingAction</c> action in the same thread as the calling function, and finally it closes the dialog in the UI thread after returning from the action.
        /// </remarks>
        public static void ShowWait(this ContentControl parent, Action waitingAction, string title = null, object content = null, UIElement waitControl = null, DialogButtons buttons = DialogButtons.None, Brush theme = null)
        {
            var w = ShowWait(parent, title, content, waitControl, buttons, theme);
            waitingAction();
            Invoke(() => w.Close());
        }
        /// <summary>
        /// Show a <see cref="DialogBase"/> that contains a waiting control inside you application's main window, and then execute an actions, and when finished closes the dialog.
        /// </summary>
        /// <param name="waitingAction">The action that will be executed and when finished, the dialog will be closed.</param>
        /// <param name="title">Waiting box title message.</param>
        /// <param name="content">A content that will be displayed above the waiting control, usually a string note.</param>
        /// <param name="waitControl">The waiting control to be displayed, if <c>null</c> a progress bar will be displayed with its <see cref="ProgressBar.IsIndeterminate"/> property set to <c>true</c>.</param>
        /// <param name="buttons">Buttons displayed at the button of the dialog.</param>
        /// <remarks>
        /// The method creates and shows the dialog in the UI thread, then it calls the <c>waitingAction</c> action in the same thread as the calling function, and finally it closes the dialog in the UI thread after returning from the action.
        /// </remarks>
        public static void ShowWait(Action waitingAction, string title = null, object content = null, UIElement waitControl = null, DialogButtons buttons = DialogButtons.None, Brush theme = null)
        {
            ShowWait(Application.Current.MainWindow, waitingAction, title, content, waitControl, buttons, theme);
        }
        /// <summary>
        /// Show a <see cref="DialogBase"/> that contains a waiting control, and then run a task, and when finished closes the dialog.
        /// </summary>
        /// <param name="parent">The <see cref="ContentControl"/> that contains your dialog.</param>
        /// <param name="waitingTask">The task that will be executed and when finished, the dialog will be closed.</param>
        /// <param name="title">Waiting box title message.</param>
        /// <param name="content">A content that will be displayed above the waiting control, usually a string note.</param>
        /// <param name="waitControl">The waiting control to be displayed, if <c>null</c> a progress bar will be displayed with its <see cref="ProgressBar.IsIndeterminate"/> property set to <c>true</c>.</param>
        /// <param name="buttons">Buttons displayed at the button of the dialog.</param>
        /// <remarks>
        /// The method creates and shows the dialog in the UI thread, then it starts the <c>waitingTask</c> task in a new thread, and finally it closes the dialog in the UI thread after returning from the task.
        /// </remarks>
        public static void ShowWait(this ContentControl parent, Task waitingTask, string title = null, object content = null, UIElement waitControl = null, DialogButtons buttons = DialogButtons.None, Brush theme = null, bool autoStartTask = true)
        {
            var w = ShowWait(parent, title, content, waitControl, buttons, theme);
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
        /// <summary>
        /// Show a <see cref="DialogBase"/> that contains a waiting control inside you application's main window, and then run a task, and when finished closes the dialog.
        /// </summary>
        /// <param name="waitingTask">The task that will be executed and when finished, the dialog will be closed.</param>
        /// <param name="title">Waiting box title message.</param>
        /// <param name="content">A content that will be displayed above the waiting control, usually a string note.</param>
        /// <param name="waitControl">The waiting control to be displayed, if <c>null</c> a progress bar will be displayed with its <see cref="ProgressBar.IsIndeterminate"/> property set to <c>true</c>.</param>
        /// <param name="buttons">Buttons displayed at the button of the dialog.</param>
        /// <remarks>
        /// The method creates and shows the dialog in the UI thread, then it starts the <c>waitingTask</c> task in a new thread, and finally it closes the dialog in the UI thread after returning from the task.
        /// </remarks>
        public static void ShowWait(Task waitingTask, string title = null, object content = null, UIElement waitControl = null, DialogButtons buttons = DialogButtons.None, Brush theme = null, bool autoStartTask = true)
        {
            ShowWait(Application.Current.MainWindow, waitingTask, title, content, waitControl, buttons, theme, autoStartTask);
        }
        #endregion
    }
}
