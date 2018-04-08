using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Effects;
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
    public class DialogBase : UserControl
    {
        #region Fields
        ManualResetEvent _DialogCloseResetEvent;
        /// <summary>
        /// This grid has three rows for: top margins, dialog parts control, and lower margins
        /// </summary>
        Grid _DialogGrid;
        /// <summary>
        /// This will hold the parent background image.
        /// </summary>
        Border _ParentBackground;
        /// <summary>
        /// This is just a semi transparent color over the background item to give faded effect.
        /// </summary>
        Border _ParentBackgroundOverlayColor;
        /// <summary>
        /// This is a dump control to hold the old content in it and resize it with the dialog.
        /// </summary>
        ContentControl _OldContentContainer;
        /// <summary>
        /// This is the displayed dialog which has a title, buttons, and contents.
        /// </summary>
        DialogPartsControl _DialogPartsControl;
        /// <summary>
        /// These will hold the buttons.
        /// </summary>
        ButtonBase _OkButton, _CancelButton, _YesButton, _NoButton;
        #endregion
        static DialogBase()
        {
            DefaultStyleKeyProperty.
                OverrideMetadata(typeof(DialogBase), new FrameworkPropertyMetadata(typeof(DialogBase)));
            HorizontalContentAlignmentProperty.
                OverrideMetadata(typeof(DialogBase), new FrameworkPropertyMetadata(HorizontalAlignment.Stretch));
            VerticalContentAlignmentProperty.
                OverrideMetadata(typeof(DialogBase), new FrameworkPropertyMetadata(VerticalAlignment.Center));
        }
        public DialogBase()
        {
            CreateControls();
            _DialogCloseResetEvent = new ManualResetEvent(false);
        }
        #region Properties
        public new object Content { get => DialogContent; set => DialogContent = value; }
        public DialogPartsControl DialogPartsControl { get => _DialogPartsControl; }

        [Category("Dialog")]
        public Type DialogButtonType
        {
            get { return (Type)GetValue(DialogButtonTypeProperty); }
            set { SetValue(DialogButtonTypeProperty, value); }
        }
        public static readonly DependencyProperty DialogButtonTypeProperty =
            DependencyProperty.Register("DialogButtonType", typeof(Type), typeof(DialogBase), new PropertyMetadata(null));

        [Category("Dialog")]
        public Effect DialogEffect
        {
            get { return (Effect)GetValue(DialogEffectProperty); }
            set { SetValue(DialogEffectProperty, value); }
        }
        public static readonly DependencyProperty DialogEffectProperty =
            DependencyProperty.Register("DialogEffect", typeof(Effect), typeof(DialogBase), new PropertyMetadata(null));

        [Category("Dialog")]
        public object DialogTitle
        {
            get { return (object)GetValue(DialogTitleProperty); }
            set { SetValue(DialogTitleProperty, value); }
        }
        public static readonly DependencyProperty DialogTitleProperty =
            DependencyProperty.Register("DialogTitle", typeof(object), typeof(DialogBase), new PropertyMetadata(null));

        [Category("Dialog")]
        public object DialogContent
        {
            get { return (object)GetValue(DialogContentProperty); }
            set { SetValue(DialogContentProperty, value); }
        }
        public static readonly DependencyProperty DialogContentProperty =
            DependencyProperty.Register("DialogContent", typeof(object), typeof(DialogBase), new PropertyMetadata(null));

        [Category("Dialog")]
        public DialogButtons DialogButtons
        {
            get => (DialogButtons)GetValue(DialogButtonsProperty);
            set => SetValue(DialogButtonsProperty, value);
        }
        public static readonly DependencyProperty DialogButtonsProperty =
            DependencyProperty.Register("DialogButtons", typeof(DialogButtons), typeof(DialogBase),
                new FrameworkPropertyMetadata(DialogButtons.None, FrameworkPropertyMetadataOptions.AffectsArrange, DialogButtonsChangedCallback));
        static void DialogButtonsChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as DialogBase)?.CreateButtons();

        [Category("Dialog")]
        public DialogResult DialogResult
        {
            get { return (DialogResult)GetValue(DialogResultProperty); }
            set { SetValue(DialogResultProperty, value); }
        }
        public static readonly DependencyProperty DialogResultProperty =
            DependencyProperty.Register("DialogResult", typeof(DialogResult), typeof(DialogBase), new PropertyMetadata(DialogResult.None));

        [Category("Dialog")]
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

        [Category("Dialog")]
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

        [Category("Dialog")]
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

        [Category("Dialog")]
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

        [Category("Dialog")]
        public Style DialogButtonStyle
        {
            get { return (Style)GetValue(DialogButtonStyleProperty); }
            set { SetValue(DialogButtonStyleProperty, value); }
        }
        public static readonly DependencyProperty DialogButtonStyleProperty =
            DependencyProperty.Register("DialogButtonStyle", typeof(Style), typeof(DialogBase), new PropertyMetadata(null));

        [Category("Dialog")]
        public Brush DialogParentOverlay
        {
            get { return (Brush)GetValue(DialogParentOverlayProperty); }
            set { SetValue(DialogParentOverlayProperty, value); }
        }
        public static readonly DependencyProperty DialogParentOverlayProperty =
            DependencyProperty.Register("DialogParentOverlay", typeof(Brush), typeof(DialogBase), new PropertyMetadata(new SolidColorBrush(Colors.Black) { Opacity = .2 }));

        [Category("Dialog")]
        public Effect DialogParentEffect
        {
            get { return (Effect)GetValue(DialogParentEffectProperty); }
            set { SetValue(DialogParentEffectProperty, value); }
        }
        public static readonly DependencyProperty DialogParentEffectProperty =
            DependencyProperty.Register("DialogParentEffect", typeof(Effect), typeof(DialogBase), new PropertyMetadata(new BlurEffect()));
        #endregion
        #region Virtsual methods
        virtual protected ButtonBase OnCreateButton(object content)
        {
            if (DialogButtonType != null && DialogButtonType.IsAssignableFrom(typeof(ButtonBase)))
            {
                try
                {
                    var b = Activator.CreateInstance(DialogButtonType) as ButtonBase;
                    if (b != null)
                    {
                        b.Content = content;
                        return b;
                    }
                }
                catch (Exception)
                {
                }
            }
            return new Button() { Content = content };
        }
        virtual protected bool OnDialogOk() => OnClosing(DialogResult.Ok);
        virtual protected bool OnDialogCancel() => OnClosing(DialogResult.Cancel);
        virtual protected bool OnDialogYes() => OnClosing(DialogResult.Yes);
        virtual protected bool OnDialogNo() => OnClosing(DialogResult.No);
        virtual protected bool OnClosing(DialogResult result) => true;
        #endregion
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            _OldContentContainer.Width = sizeInfo.NewSize.Width;
            _OldContentContainer.Height = sizeInfo.NewSize.Height;
            _OldContentContainer.Measure(sizeInfo.NewSize);
            _OldContentContainer.Arrange(new Rect(_OldContentContainer.DesiredSize));
            SetBackgroundBrush(_OldContentContainer);
        }
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            e.Handled = true;
            if (e.Key == Key.Return)
            {
                if (_OkButton != null) _OkButton.Command?.Execute(null);
                else if (_YesButton != null) _YesButton.Command?.Execute(null);
                else e.Handled = false;
            }
            else if (e.Key == Key.Escape)
            {
                if (_CancelButton != null) _CancelButton.Command?.Execute(null);
                else if (_NoButton != null) _NoButton.Command?.Execute(null);
                else e.Handled = false;
            }
            base.OnPreviewKeyDown(e);
        }
        #region Private methods
        void CreateControls()
        {
            _ParentBackground = new Border();
            BindingOperations.SetBinding(_ParentBackground, EffectProperty,
                                         new Binding(nameof(DialogParentEffect)) { Source = this });
            Grid.SetRowSpan(_ParentBackground, 5);
            Panel.SetZIndex(_ParentBackground, 0);

            _ParentBackgroundOverlayColor = new Border();
            BindingOperations.SetBinding(_ParentBackgroundOverlayColor, BackgroundProperty,
                                         new Binding(nameof(DialogParentOverlay)) { Source = this });
            Grid.SetRowSpan(_ParentBackgroundOverlayColor, 5);
            Panel.SetZIndex(_ParentBackgroundOverlayColor, 1);

            _OldContentContainer = new ContentControl();

            _DialogPartsControl = new DialogPartsControl();
            Grid.SetRow(_DialogPartsControl, 1);
            Panel.SetZIndex(_DialogPartsControl, 3);
            BindingOperations.SetBinding(_DialogPartsControl.DialogTitleControl, ContentProperty,
                                         new Binding(nameof(DialogTitle)) { Source = this });
            BindingOperations.SetBinding(_DialogPartsControl.DialogContentControl, ContentProperty,
                                         new Binding(nameof(DialogContent)) { Source = this });
            BindingOperations.SetBinding(_DialogPartsControl.DialogPartsEffects, EffectProperty,
                                         new Binding(nameof(DialogEffect)) { Source = this });

            CreateButtons();

            _DialogGrid = new Grid();
            _DialogGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            _DialogGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            _DialogGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            _DialogGrid.Children.Add(_ParentBackground);
            _DialogGrid.Children.Add(_ParentBackgroundOverlayColor);
            _DialogGrid.Children.Add(_DialogPartsControl);
            base.Content = _DialogGrid;

            CommandBindings.Add(new CommandBinding(DialogCommands.Ok, OkCommandExecuted));
            CommandBindings.Add(new CommandBinding(DialogCommands.Cancel, CancelCommandExecuted));
            CommandBindings.Add(new CommandBinding(DialogCommands.Yes, YesCommandExecuted));
            CommandBindings.Add(new CommandBinding(DialogCommands.No, NoCommandExecuted));
        }
        void CreateButtons()
        {
            foreach (var button in _DialogPartsControl.DialogButtonsControl.GetButtons())
                BindingOperations.ClearBinding(button, StyleProperty);
            _DialogPartsControl.DialogButtonsControl.ClearButtons();
            _OkButton = null;
            _CancelButton = null;
            _YesButton = null;
            _NoButton = null;
            InputBindings.Clear();
            switch (DialogButtons)
            {
                case DialogButtons.None:
                    break;
                case DialogButtons.Ok:
                    _OkButton = CreateButton(DialogOkContent, DialogCommands.Ok);
                    break;
                case DialogButtons.OkCancel:
                    _OkButton = CreateButton(DialogOkContent, DialogCommands.Ok);
                    _CancelButton = CreateButton(DialogCancelContent, DialogCommands.Cancel);
                    break;
                case DialogButtons.YesNo:
                    _YesButton = CreateButton(DialogYesContent, DialogCommands.Yes);
                    _NoButton = CreateButton(DialogNoContent, DialogCommands.No);
                    break;
                case DialogButtons.YesNoCancel:
                    _YesButton = CreateButton(DialogYesContent, DialogCommands.Yes);
                    _NoButton = CreateButton(DialogNoContent, DialogCommands.No);
                    _CancelButton = CreateButton(DialogCancelContent, DialogCommands.Cancel);
                    break;
                default:
                    break;
            }
        }
        ButtonBase CreateButton(object content, ICommand command)
        {
            var b = OnCreateButton(content);
            b.Command = command;
            BindingOperations.SetBinding(b, StyleProperty,
                new Binding(nameof(DialogButtonStyle)) { Source = this, Mode = BindingMode.OneWay });
            _DialogPartsControl.DialogButtonsControl.AddButton(b);
            return b;
        }
        void OkCommandExecuted(object sender, ExecutedRoutedEventArgs e) { if (OnDialogOk()) Close(); }
        void CancelCommandExecuted(object sender, ExecutedRoutedEventArgs e) { if (OnDialogCancel()) Close(); }
        void YesCommandExecuted(object sender, ExecutedRoutedEventArgs e) { if (OnDialogYes()) Close(); }
        void NoCommandExecuted(object sender, ExecutedRoutedEventArgs e) { if (OnDialogNo()) Close(); }
        public void KeyEventHandler(Object sender, KeyEventArgs e) { }
        void SetBackgroundBrush(ContentControl parent)
        {
            var element = parent?.Content as UIElement;
            if (element == null) return;
            _ParentBackground.Background = new VisualBrush(element) { Stretch = Stretch.Uniform, AlignmentX = AlignmentX.Left, AlignmentY = AlignmentY.Top };
        }
        #endregion
        #region Public Methods
        public void ShowDialog(ContentControl parent)
        {
            _DialogCloseResetEvent.Reset();
            SetBackgroundBrush(parent);
            _OldContentContainer.Content = parent.Content;
            parent.Content = this;
            FocusManager.SetFocusedElement(this, _DialogPartsControl);
        }
        public Task<DialogResult> ShowDialogAsync(ContentControl parent)
        {
            return Task.Run(() =>
            {
                Dispatcher.Invoke(() => ShowDialog(parent));
                _DialogCloseResetEvent.WaitOne();
                return Dispatcher.Invoke(() => DialogResult);
            });
        }
        public void Close()
        {
            var c = Parent as ContentControl;
            if (c != null) c.Content = _OldContentContainer.Content;
            _DialogCloseResetEvent.Set();
        }
        #endregion
    }
}
