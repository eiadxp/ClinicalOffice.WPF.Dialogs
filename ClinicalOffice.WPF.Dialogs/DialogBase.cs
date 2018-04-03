using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClinicalOffice.WPF.Dialogs
{
    public enum DialogButtons
    {
        None,
        Ok,
        OkCancel,
        YesNo,
        YesNoCancel
    }
    public enum DialogResult
    {
        None,
        Ok,
        Cancel,
        Yes,
        No
    }
    [ContentProperty(nameof(DialogContent))]
    public class DialogBase : ContentControl
    {
        /// <summary>
        /// This grid has three rows for: Dialogdialog title, dialog content, and dialog buttons
        /// </summary>
        Grid _DialogGrid;
        /// <summary>
        /// This will hold the content of the dialog title.
        /// </summary>
        DialogTitleControl _DialogTitleContent;
        DialogButtonsControl _DialogButtonsBar;
        /// <summary>
        /// This panel will hold the dialog buttons.
        /// </summary>
        Grid _ButtonsGrid;
        /// <summary>
        /// This will hold the actual dialog contents.
        /// </summary>
        DialogContentControl _DialogContent;
        /// <summary>
        /// These will hold the buttons.
        /// </summary>
        ButtonBase _OkButton, _CancelButton, _YesButton, _NoButton;

        static DialogBase()
        {
            //DefaultStyleKeyProperty.
            //    OverrideMetadata(typeof(DialogBase), new FrameworkPropertyMetadata(typeof(DialogBase)));
            HorizontalContentAlignmentProperty.
                OverrideMetadata(typeof(DialogBase), new FrameworkPropertyMetadata(HorizontalAlignment.Stretch));
            VerticalContentAlignmentProperty.
                OverrideMetadata(typeof(DialogBase), new FrameworkPropertyMetadata(VerticalAlignment.Center));
        }
        public DialogBase()
        {
            CreateControls();
        }
        #region Properties
        public new object Content { get => DialogContent; set => DialogContent = value; }
        public DialogTitleControl DialogTitleControl { get => _DialogTitleContent; }

        public object DialogTitle
        {
            get { return (object)GetValue(DialogTitleProperty); }
            set { SetValue(DialogTitleProperty, value); }
        }
        public static readonly DependencyProperty DialogTitleProperty =
            DependencyProperty.Register("DialogTitle", typeof(object), typeof(DialogBase), new PropertyMetadata(null));

        public object DialogContent
        {
            get { return (object)GetValue(DialogContentProperty); }
            set { SetValue(DialogContentProperty, value); }
        }
        public static readonly DependencyProperty DialogContentProperty =
            DependencyProperty.Register("DialogContent", typeof(object), typeof(DialogBase), new PropertyMetadata(null));

        public DialogButtons DialogButtons
        {
            get => (DialogButtons)GetValue(DialogButtonsProperty);
            set => SetValue(DialogButtonsProperty, value);
        }
        public static readonly DependencyProperty DialogButtonsProperty =
            DependencyProperty.Register("DialogButtons", typeof(DialogButtons), typeof(DialogBase), 
                new FrameworkPropertyMetadata(DialogButtons.None, FrameworkPropertyMetadataOptions.AffectsArrange, DialogButtonsChangedCallback));
        static void DialogButtonsChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as DialogBase)?.CreateButtons();

        public DialogResult DialogResult
        {
            get { return (DialogResult)GetValue(DialogResultProperty); }
            set { SetValue(DialogResultProperty, value); }
        }
        public static readonly DependencyProperty DialogResultProperty =
            DependencyProperty.Register("DialogResult", typeof(DialogResult), typeof(DialogBase), new PropertyMetadata(DialogResult.None));

        public object DialogOkContent
        {
            get { return (object)GetValue(DialogOkContentProperty); }
            set { SetValue(DialogOkContentProperty, value); }
        }
        public static readonly DependencyProperty DialogOkContentProperty =
            DependencyProperty.Register("DialogOkContent", typeof(object), typeof(DialogBase), new PropertyMetadata("Ok"));
        static void DialogOkContentChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dialog = d as DialogBase;
            if (dialog?._OkButton != null) dialog._OkButton.Content = e.NewValue;
        }

        public object DialogCancelContent
        {
            get { return (object)GetValue(DialogCancelContentProperty); }
            set { SetValue(DialogCancelContentProperty, value); }
        }
        public static readonly DependencyProperty DialogCancelContentProperty =
            DependencyProperty.Register("DialogCancelContent", typeof(object), typeof(DialogBase), new PropertyMetadata("Cancel"));
        static void DialogCancelContentChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dialog = d as DialogBase;
            if (dialog?._CancelButton != null) dialog._CancelButton.Content = e.NewValue;
        }

        public object DialogYesContent
        {
            get { return (object)GetValue(DialogYesContentProperty); }
            set { SetValue(DialogYesContentProperty, value); }
        }
        public static readonly DependencyProperty DialogYesContentProperty =
            DependencyProperty.Register("DialogYesContent", typeof(object), typeof(DialogBase), new PropertyMetadata("Yes"));
        static void DialogYesContentChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dialog = d as DialogBase;
            if (dialog?._YesButton != null) dialog._YesButton.Content = e.NewValue;
        }

        public object DialogNoContent
        {
            get { return (object)GetValue(DialogNoContentProperty); }
            set { SetValue(DialogNoContentProperty, value); }
        }
        public static readonly DependencyProperty DialogNoContentProperty =
            DependencyProperty.Register("DialogNoContent", typeof(object), typeof(DialogBase), new PropertyMetadata("No"));
        static void DialogNoContentChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dialog = d as DialogBase;
            if (dialog?._NoButton != null) dialog._NoButton.Content = e.NewValue;
        }

        public Style DialogButtonStyle
        {
            get { return (Style)GetValue(DialogButtonStyleProperty); }
            set { SetValue(DialogButtonStyleProperty, value); }
        }
        public static readonly DependencyProperty DialogButtonStyleProperty =
            DependencyProperty.Register("DialogButtonStyle", typeof(Style), typeof(DialogBase), new PropertyMetadata(null));

        #endregion
        virtual protected ButtonBase OnCreateButton(object content)
        {
            return new Button() { Content = content };
        }
        #region Private methods
        void CreateControls()
        {
            _DialogTitleContent = new DialogTitleControl();
            Grid.SetRow(_DialogTitleContent, 0);
            BindingOperations.SetBinding(_DialogTitleContent, ContentProperty,
                                         new Binding(nameof(DialogTitle)) { Source = this });

            _DialogContent = new DialogContentControl();
            Grid.SetRow(_DialogContent, 1);
            BindingOperations.SetBinding(_DialogContent, ContentProperty, 
                                         new Binding(nameof(DialogContent)) { Source = this });

            _DialogButtonsBar = new DialogButtonsControl()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            Grid.SetIsSharedSizeScope(_DialogButtonsBar, true);
            Grid.SetRow(_DialogButtonsBar, 2);

            _ButtonsGrid = new Grid() { HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Bottom };
            CreateButtons();
            _DialogButtonsBar.Content = _ButtonsGrid;

            _DialogGrid = new Grid();
            _DialogGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            _DialogGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            _DialogGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            _DialogGrid.Children.Add(_DialogTitleContent);
            _DialogGrid.Children.Add(_DialogContent);
            _DialogGrid.Children.Add(_DialogButtonsBar);
            base.Content = _DialogGrid;
        }
        void CreateButtons()
        {
            foreach (var button in _ButtonsGrid.Children.OfType<ButtonBase>())
            {
                button.Click -= Button_Click;
                BindingOperations.ClearBinding(button, StyleProperty);
            }
            _ButtonsGrid.Children.Clear();
            _ButtonsGrid.ColumnDefinitions.Clear();
            _OkButton = null;
            _CancelButton = null;
            _YesButton = null;
            _NoButton = null;
            switch (DialogButtons)
            {
                case DialogButtons.None:
                    break;
                case DialogButtons.Ok:
                    _OkButton = CreateButton(DialogOkContent);
                    break;
                case DialogButtons.OkCancel:
                    _OkButton = CreateButton(DialogOkContent);
                    _CancelButton = CreateButton(DialogCancelContent);
                    break;
                case DialogButtons.YesNo:
                    _YesButton = CreateButton(DialogYesContent);
                    _NoButton = CreateButton(DialogNoContent);
                    break;
                case DialogButtons.YesNoCancel:
                    _YesButton = CreateButton(DialogYesContent);
                    _NoButton = CreateButton(DialogNoContent);
                    _CancelButton = CreateButton(DialogCancelContent);
                    break;
                default:
                    break;
            }
        }
        ButtonBase CreateButton(object content)
        {
            var b = OnCreateButton(content);
            b.Click += Button_Click;
            BindingOperations.SetBinding(b, StyleProperty, 
                new Binding(nameof(DialogButtonStyle)) { Source = this, Mode = BindingMode.OneWay });
            _ButtonsGrid.ColumnDefinitions.Add(new ColumnDefinition() { SharedSizeGroup = "dialogButtons" });
            Grid.SetColumn(b, _ButtonsGrid.ColumnDefinitions.Count - 1);
            _ButtonsGrid.Children.Add(b);
            return b;
        }
        void Button_Click(object sender, RoutedEventArgs e) { }
        #endregion
    }
    public class DialogTitleControl : UserControl
    {
        static DialogTitleControl()
        {
            DefaultStyleKeyProperty.
                OverrideMetadata(typeof(DialogTitleControl), new FrameworkPropertyMetadata(typeof(DialogTitleControl)));
        }
    }
    public class DialogButtonsControl : UserControl
    {
        static DialogButtonsControl()
        {
            //DefaultStyleKeyProperty.
            //    OverrideMetadata(typeof(DialogButtonsControl), new FrameworkPropertyMetadata(typeof(DialogButtonsControl)));
        }
    }
    public class DialogContentControl : UserControl
    {
        static DialogContentControl()
        {
            //DefaultStyleKeyProperty.
            //    OverrideMetadata(typeof(DialogContentControl), new FrameworkPropertyMetadata(typeof(DialogContentControl)));
        }
    }
}
