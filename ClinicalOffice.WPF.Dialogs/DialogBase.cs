using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace ClinicalOffice.WPF.Dialogs
{
    [ContentProperty(nameof(DialogContent))]
    public class DialogBase : UserControl
    {
        #region Fields
        /// <summary>
        /// This will be reset on ShowDialog and will be set on Close to notify waiting thread that the dialog is closed.
        /// </summary>
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
            /// To enable theming in Theme/Generic.xaml file.
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogBase), new FrameworkPropertyMetadata(typeof(DialogBase)));
            HorizontalContentAlignmentProperty.OverrideMetadata(typeof(DialogBase), new FrameworkPropertyMetadata(HorizontalAlignment.Stretch));
            VerticalContentAlignmentProperty.OverrideMetadata(typeof(DialogBase), new FrameworkPropertyMetadata(VerticalAlignment.Center));

        }
        public DialogBase()
        {
            CreateControls();
            _DialogCloseResetEvent = new ManualResetEvent(false);
        }
        #region Properties
        /// <summary>
        /// This is a shadow property to map the content of this control to DialogContent property.
        /// </summary>
        /// <remarks>
        /// NEVER USE THE base.Content OF THE DIALOG TO SET ITS CONTENT.
        /// You can use either Content or DialogContent properties of DialogBase class to set the contents of the dialog.
        /// </remarks>
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
        public Brush DialogOverlay
        {
            get { return (Brush)GetValue(DialogOverlayProperty); }
            set { SetValue(DialogOverlayProperty, value); }
        }
        public static readonly DependencyProperty DialogOverlayProperty =
            DependencyProperty.Register("DialogOverlay", typeof(Brush), typeof(DialogBase), new PropertyMetadata(Brushes.Black));

        [Category("Dialog")]
        public double DialogOverlayOpacity
        {
            get { return (double)GetValue(DialogOverlayOpacityProperty); }
            set { SetValue(DialogOverlayOpacityProperty, value); }
        }
        public static readonly DependencyProperty DialogOverlayOpacityProperty =
            DependencyProperty.Register("DialogOverlayOpacity", typeof(double), typeof(DialogBase), new PropertyMetadata(0.2));

        [Category("Dialog")]
        public Effect DialogParentEffect
        {
            get { return (Effect)GetValue(DialogParentEffectProperty); }
            set { SetValue(DialogParentEffectProperty, value); }
        }
        public static readonly DependencyProperty DialogParentEffectProperty =
            DependencyProperty.Register("DialogParentEffect", typeof(Effect), typeof(DialogBase), new PropertyMetadata(new BlurEffect()));


        [Category("Dialog")]
        public Brush DialogBackGround
        {
            get { return (Brush)GetValue(DialogBackGroundProperty); }
            set { SetValue(DialogBackGroundProperty, value); }
        }
        public static readonly DependencyProperty DialogBackGroundProperty =
            DependencyProperty.Register("DialogBackGround", typeof(Brush), typeof(DialogBase), new PropertyMetadata(DialogParameters.BorderBackground));

        /// <summary>
        /// An enum that specify the in (show) animation of <see cref="DialogBase" /></summary>
        /// <remarks>The library include some ready to use animation like <see cref="DialogAnimation.Fade" />.</remarks>
        [Category("Dialog")]
        public DialogAnimation DialogAnimationIn
        {
            get { return (DialogAnimation)GetValue(DialogAnimationInProperty); }
            set { SetValue(DialogAnimationInProperty, value); }
        }
        public static readonly DependencyProperty DialogAnimationInProperty =
            DependencyProperty.Register("DialogAnimationIn", typeof(DialogAnimation), typeof(DialogBase), new PropertyMetadata(DialogAnimation.Global));

        [Category("Dialog")]
        public Storyboard DialogCustomAnimationIn
        {
            get { return (Storyboard)GetValue(DialogCustomAnimationInProperty); }
            set { SetValue(DialogCustomAnimationInProperty, value); }
        }
        public static readonly DependencyProperty DialogCustomAnimationInProperty =
            DependencyProperty.Register("DialogCustomAnimationIn", typeof(Storyboard), typeof(DialogBase), new PropertyMetadata(null));

        [Category("Dialog")]
        public DialogAnimation DialogAnimationOut
        {
            get { return (DialogAnimation)GetValue(DialogAnimationOutProperty); }
            set { SetValue(DialogAnimationOutProperty, value); }
        }
        public static readonly DependencyProperty DialogAnimationOutProperty =
            DependencyProperty.Register("DialogAnimationOut", typeof(DialogAnimation), typeof(DialogBase), new PropertyMetadata(DialogAnimation.Global));

        [Category("Dialog")]
        public Storyboard DialogCustomAnimationOut
        {
            get { return (Storyboard)GetValue(DialogCustomAnimationOutProperty); }
            set { SetValue(DialogCustomAnimationOutProperty, value); }
        }
        public static readonly DependencyProperty DialogCustomAnimationOutProperty =
            DependencyProperty.Register("DialogCustomAnimationOut", typeof(Storyboard), typeof(DialogBase), new PropertyMetadata(null));

        /// <summary>
        /// The duration of show and hide animations of the dialog.
        /// </summary>
        /// <remarks>
        /// The default value is <see cref="Duration.Automatic"/> static constant, which indicate that the value will be used from <see cref="DialogParameters.DialogAnimationDuration"/> static property.
        /// However if the value of <see cref="DialogParameters.DialogAnimationDuration"/> static property is also <see cref="Duration.Automatic"/> static constant the value of 100 ms is used
        /// </remarks>
        [Category("Dialog")]
        public Duration DialogAnimationDuration
        {
            get { return (Duration)GetValue(DialogAnimationDurationProperty); }
            set { SetValue(DialogAnimationDurationProperty, value); }
        }
        public static readonly DependencyProperty DialogAnimationDurationProperty =
            DependencyProperty.Register("DialogAnimationDuration", typeof(Duration), typeof(DialogBase), new PropertyMetadata(Duration.Automatic));

        [Category("Dialog")]
        public UIElement DialogFocusedElement
        {
            get { return (UIElement)GetValue(DialogFocusedElementProperty); }
            set { SetValue(DialogFocusedElementProperty, value); }
        }
        public static readonly DependencyProperty DialogFocusedElementProperty =
            DependencyProperty.Register("DialogFocusedElement", typeof(UIElement), typeof(DialogBase), new PropertyMetadata(null));

        [Category("Dialog")]
        public bool DialogAutoClose
        {
            get { return (bool)GetValue(DialogAutoCloseProperty); }
            set { SetValue(DialogAutoCloseProperty, value); }
        }
        public static readonly DependencyProperty DialogAutoCloseProperty =
            DependencyProperty.Register("DialogAutoClose", typeof(bool), typeof(DialogBase), new PropertyMetadata(true));

        [Category("Dialog")]
        public ICommand DialogOkCommand
        {
            get { return (ICommand)GetValue(DialogOkCommandProperty); }
            set { SetValue(DialogOkCommandProperty, value); }
        }
        public static readonly DependencyProperty DialogOkCommandProperty =
            DependencyProperty.Register("DialogOkCommand", typeof(ICommand), typeof(DialogBase), new PropertyMetadata(null));

        [Category("Dialog")]
        public object DialogOkCommandParameter
        {
            get { return (object)GetValue(DialogOkCommandParameterProperty); }
            set { SetValue(DialogOkCommandParameterProperty, value); }
        }
        public static readonly DependencyProperty DialogOkCommandParameterProperty =
            DependencyProperty.Register("DialogOkCommandParameter", typeof(object), typeof(DialogBase), new PropertyMetadata(null));

        [Category("Dialog")]
        public ICommand DialogCancelCommand
        {
            get { return (ICommand)GetValue(DialogCancelCommandProperty); }
            set { SetValue(DialogCancelCommandProperty, value); }
        }
        public static readonly DependencyProperty DialogCancelCommandProperty =
            DependencyProperty.Register("DialogCancelCommand", typeof(ICommand), typeof(DialogBase), new PropertyMetadata(null));

        [Category("Dialog")]
        public object DialogCancelCommandParameter
        {
            get { return (object)GetValue(DialogCancelCommandParameterProperty); }
            set { SetValue(DialogCancelCommandParameterProperty, value); }
        }
        public static readonly DependencyProperty DialogCancelCommandParameterProperty =
            DependencyProperty.Register("DialogCancelCommandParameter", typeof(object), typeof(DialogBase), new PropertyMetadata(null));

        [Category("Dialog")]
        public ICommand DialogYesCommand
        {
            get { return (ICommand)GetValue(DialogYesCommandProperty); }
            set { SetValue(DialogYesCommandProperty, value); }
        }
        public static readonly DependencyProperty DialogYesCommandProperty =
            DependencyProperty.Register("DialogYesCommand", typeof(ICommand), typeof(DialogBase), new PropertyMetadata(null));

        [Category("Dialog")]
        public object DialogYesCommandParameter
        {
            get { return (object)GetValue(DialogYesCommandParameterProperty); }
            set { SetValue(DialogYesCommandParameterProperty, value); }
        }
        public static readonly DependencyProperty DialogYesCommandParameterProperty =
            DependencyProperty.Register("DialogYesCommandParameter", typeof(object), typeof(DialogBase), new PropertyMetadata(null));

        [Category("Dialog")]
        public ICommand DialogNoCommand
        {
            get { return (ICommand)GetValue(DialogNoCommandProperty); }
            set { SetValue(DialogNoCommandProperty, value); }
        }
        public static readonly DependencyProperty DialogNoCommandProperty =
            DependencyProperty.Register("DialogNoCommand", typeof(ICommand), typeof(DialogBase), new PropertyMetadata(null));

        [Category("Dialog")]
        public object DialogNoCommandParameter
        {
            get { return (object)GetValue(DialogNoCommandParameterProperty); }
            set { SetValue(DialogNoCommandParameterProperty, value); }
        }
        public static readonly DependencyProperty DialogNoCommandParameterProperty =
            DependencyProperty.Register("DialogNoCommandParameter", typeof(object), typeof(DialogBase), new PropertyMetadata(null));


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
        virtual protected bool OnDialogOk() => true;
        virtual protected bool OnDialogCancel() => true;
        virtual protected bool OnDialogYes() => true;
        virtual protected bool OnDialogNo() => true;
        virtual protected bool OnClosing(DialogResult result) => true;
        #endregion
        #region Overrides
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            _OldContentContainer.Width = sizeInfo.NewSize.Width;
            _OldContentContainer.Height = sizeInfo.NewSize.Height;
            _OldContentContainer.Measure(sizeInfo.NewSize);
            _OldContentContainer.Arrange(new Rect(_OldContentContainer.DesiredSize));
            SetBackgroundBrush(_OldContentContainer);
        }
        #endregion
        #region Internal methods
        /// <summary>
        /// This method is called from DialogPrtsControl when Ok button is clicked.
        /// It is not related directly to DialogOkCommand in this class.
        /// However the DialogOkCommand will be executed when this method is called through OnDialogOk method.
        /// </summary>
        internal void OkCommandExecuted()
        {
            DialogOkCommand?.Execute(DialogOkCommandParameter);
            if (OnDialogOk() && DialogAutoClose && OnClosing(DialogResult.Ok)) Close();
        }
        /// <summary>
        /// This method is called from DialogPrtsControl when Cancel button is clicked.
        /// It is not related directly to DialogCancelCommand in this class.
        /// However the DialogCancelCommand will be executed when this method is called through OnDialogCancel method.
        /// </summary>
        internal void CancelCommandExecuted()
        {
            DialogCancelCommand?.Execute(DialogCancelCommandParameter);
            if (OnDialogCancel() && DialogAutoClose && OnClosing(DialogResult.Cancel)) Close();
        }
        /// <summary>
        /// This method is called from DialogPrtsControl when Yes button is clicked.
        /// It is not related directly to DialogYesCommand in this class.
        /// However the DialogYesCommand will be executed when this method is called through OnDialogYes method.
        /// </summary>
        internal void YesCommandExecuted()
        {
            DialogYesCommand?.Execute(DialogYesCommandParameter);
            if (OnDialogYes() && DialogAutoClose && OnClosing(DialogResult.Yes)) Close();
        }
        /// <summary>
        /// This method is called from DialogPrtsControl when 'No' button is clicked.
        /// It is not related directly to DialogNoCommand in this class.
        /// However the DialogNoCommand will be executed when this method is called through OnDialogNo method.
        /// </summary>
        internal void NoCommandExecuted()
        {
            DialogNoCommand?.Execute(DialogNoCommandParameter);
            if (OnDialogNo() && DialogAutoClose && OnClosing(DialogResult.No)) Close();
        }
        /// <summary>
        /// This method is called from DialogPrtsControl when Return key is pressed and it will simulate the default button click.
        /// </summary>
        internal void ReturnKeyCommandExecuted()
        {
            if (DialogButtons == DialogButtons.Ok || DialogButtons == DialogButtons.OkCancel) OkCommandExecuted();
            else if (DialogButtons == DialogButtons.YesNo || DialogButtons == DialogButtons.YesNoCancel) YesCommandExecuted();
        }
        /// <summary>
        /// This method is called from DialogPrtsControl when Escape key is pressed and it will simulate the cancel button click.
        /// </summary>
        internal void EscapeKeyCommandExecuted()
        {
            if (DialogButtons == DialogButtons.YesNoCancel || DialogButtons == DialogButtons.OkCancel) CancelCommandExecuted();

        }

        #endregion
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
                                         new Binding(nameof(DialogOverlay)) { Source = this });
            BindingOperations.SetBinding(_ParentBackgroundOverlayColor, OpacityProperty,
                                         new Binding(nameof(DialogOverlayOpacity)) { Source = this });
            Grid.SetRowSpan(_ParentBackgroundOverlayColor, 5);
            Panel.SetZIndex(_ParentBackgroundOverlayColor, 1);

            _OldContentContainer = new ContentControl();

            _DialogPartsControl = new DialogPartsControl(this);
            Grid.SetRow(_DialogPartsControl, 1);
            Panel.SetZIndex(_DialogPartsControl, 3);
            BindingOperations.SetBinding(_DialogPartsControl.DialogTitleControl, DialogTitleControl.ContentProperty,
                                         new Binding(nameof(DialogTitle)) { Source = this });
            BindingOperations.SetBinding(_DialogPartsControl.DialogContentControl, ContentProperty,
                                         new Binding(nameof(DialogContent)) { Source = this });
            BindingOperations.SetBinding(_DialogPartsControl.DialogBackground, EffectProperty,
                                         new Binding(nameof(DialogEffect)) { Source = this });
            BindingOperations.SetBinding(_DialogPartsControl.DialogBackground, BackgroundProperty,
                                         new Binding(nameof(DialogBackGround)) { Source = this });

            CreateButtons();

            _DialogGrid = new Grid();
            _DialogGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            _DialogGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            _DialogGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            _DialogGrid.Children.Add(_ParentBackground);
            _DialogGrid.Children.Add(_ParentBackgroundOverlayColor);
            _DialogGrid.Children.Add(_DialogPartsControl);
            base.Content = _DialogGrid;

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

        void SetBackgroundBrush(ContentControl parent)
        {
            var element = parent?.Content as UIElement;
            if (element == null) return;
            _ParentBackground.Background = new VisualBrush(element) { Stretch = Stretch.Uniform, AlignmentX = AlignmentX.Left, AlignmentY = AlignmentY.Top };
        }
        void InternalClose()
        {
            var c = Parent as ContentControl;
            if (c != null) c.Content = _OldContentContainer.Content;
            _DialogCloseResetEvent.Set();
        }

        void CreateInAnimation()
        {
            var type = DialogAnimationIn;
            if (type == DialogAnimation.Global) type = DialogParameters.DialogAnimationIn;
            if (type == DialogAnimation.Global) type = DialogAnimation.None;
            var duration = DialogAnimationDuration;
            if (duration == Duration.Automatic) duration = DialogParameters.DialogAnimationDuration;
            if (duration == Duration.Automatic) duration = new Duration(TimeSpan.FromMilliseconds(100));
            var oldTransform = RenderTransform;
            var oldOrigfin = RenderTransformOrigin;
            var oldOpacity = Opacity;
            Storyboard story;
            story = null;
            if(type == DialogAnimation.Custom)
            {
                story = DialogCustomAnimationIn ?? DialogParameters.DialogCustomAnimationIn;
                if (story == null) type = DialogAnimation.None;
            }
            else
            {
                story = new Storyboard();
            }
            switch (type)
            {
                case DialogAnimation.Fade:
                    var fade = new DoubleAnimation() { From = 0, To = 1, Duration = duration };
                    Storyboard.SetTarget(fade, this);
                    Storyboard.SetTargetProperty(fade, new PropertyPath(OpacityProperty));
                    story.Children.Add(fade);
                    break;
                case DialogAnimation.Zoom:
                    RenderTransform = new ScaleTransform();
                    var zoom = new DoubleAnimation() { From = 0, To = 1, Duration = duration };
                    Storyboard.SetTarget(zoom, this);
                    Storyboard.SetTargetProperty(zoom, new PropertyPath("(0).(1)", RenderTransformProperty, ScaleTransform.ScaleXProperty));
                    story.Children.Add(zoom);
                    zoom = new DoubleAnimation() { From = 0, To = 1, Duration = duration };
                    Storyboard.SetTarget(zoom, this);
                    Storyboard.SetTargetProperty(zoom, new PropertyPath("(0).(1)", RenderTransformProperty, ScaleTransform.ScaleYProperty));
                    story.Children.Add(zoom);
                    break;
                case DialogAnimation.ZoomCenter:
                    RenderTransformOrigin = new Point(.5, .5);
                    RenderTransform = new ScaleTransform();
                    var zoomCenter = new DoubleAnimation() { From = 0, To = 1, Duration = duration };
                    Storyboard.SetTarget(zoomCenter, this);
                    Storyboard.SetTargetProperty(zoomCenter, new PropertyPath("(0).(1)", RenderTransformProperty, ScaleTransform.ScaleXProperty));
                    story.Children.Add(zoomCenter);
                    zoomCenter = new DoubleAnimation() { From = 0, To = 1, Duration = duration };
                    Storyboard.SetTarget(zoomCenter, this);
                    Storyboard.SetTargetProperty(zoomCenter, new PropertyPath("(0).(1)", RenderTransformProperty, ScaleTransform.ScaleYProperty));
                    story.Children.Add(zoomCenter);
                    break;
                case DialogAnimation.Custom:
                    break;
                case DialogAnimation.None:
                default:
                    return;
                    break;
            }
            void AnimationInCompleted(object sender, EventArgs e)
            {
                if (sender is Timeline a) a.Completed -= AnimationInCompleted;
                RenderTransform = oldTransform;
                RenderTransformOrigin = oldOrigfin;
                Opacity = oldOpacity;
            }
            story.Completed += AnimationInCompleted;
            story.Begin();
        }
        void CreateOutAnimation()
        {
            var type = DialogAnimationOut;
            if (type == DialogAnimation.Global) type = DialogParameters.DialogAnimationOut;
            if (type == DialogAnimation.Global) type = DialogAnimation.None;
            var duration = DialogAnimationDuration;
            if (duration == Duration.Automatic) duration = DialogParameters.DialogAnimationDuration;
            if (duration == Duration.Automatic) duration = new Duration(TimeSpan.FromMilliseconds(100));
            var oldTransform = RenderTransform;
            var oldOrigfin = RenderTransformOrigin;
            var oldOpacity = Opacity;
            Storyboard story;
            if (type== DialogAnimation.Custom)
            {
                story = DialogCustomAnimationOut ?? DialogParameters.DialogCustomAnimationOut;
                if (story == null) type = DialogAnimation.None;
            }
            else
            {
                story = new Storyboard();
            }
            switch (type)
            {
                case DialogAnimation.Fade:
                    var fade = new DoubleAnimation() { From = 1, To = 0, Duration = duration };
                    Storyboard.SetTarget(fade, this);
                    Storyboard.SetTargetProperty(fade, new PropertyPath(OpacityProperty));
                    story.Children.Add(fade);
                    break;
                case DialogAnimation.Zoom:
                    RenderTransform = new ScaleTransform();
                    var zoom = new DoubleAnimation() { From = 1, To = 0, Duration = duration };
                    Storyboard.SetTarget(zoom, this);
                    Storyboard.SetTargetProperty(zoom, new PropertyPath("(0).(1)", RenderTransformProperty, ScaleTransform.ScaleXProperty));
                    story.Children.Add(zoom);
                    zoom = new DoubleAnimation() { From = 1, To = 0, Duration = duration };
                    Storyboard.SetTarget(zoom, this);
                    Storyboard.SetTargetProperty(zoom, new PropertyPath("(0).(1)", RenderTransformProperty, ScaleTransform.ScaleYProperty));
                    story.Children.Add(zoom);
                    break;
                case DialogAnimation.ZoomCenter:
                    RenderTransformOrigin = new Point(.5, .5);
                    RenderTransform = new ScaleTransform();
                    var zoomCenter = new DoubleAnimation() { From = 1, To = 0, Duration = duration };
                    Storyboard.SetTarget(zoomCenter, this);
                    Storyboard.SetTargetProperty(zoomCenter, new PropertyPath("(0).(1)", RenderTransformProperty, ScaleTransform.ScaleXProperty));
                    story.Children.Add(zoomCenter);
                    zoomCenter = new DoubleAnimation() { From = 1, To = 0, Duration = duration };
                    Storyboard.SetTarget(zoomCenter, this);
                    Storyboard.SetTargetProperty(zoomCenter, new PropertyPath("(0).(1)", RenderTransformProperty, ScaleTransform.ScaleYProperty));
                    story.Children.Add(zoomCenter);
                    break;
                case DialogAnimation.Custom:
                    break;
                case DialogAnimation.None:
                default:
                    InternalClose();
                    return;
            }
            void AnimationOutCompleted(object sender, EventArgs e)
            {
                if (sender is Timeline a) a.Completed -= AnimationOutCompleted;
                RenderTransform = oldTransform;
                RenderTransformOrigin = oldOrigfin;
                Opacity = oldOpacity;
                InternalClose();
            }
            story.Completed += AnimationOutCompleted;
            story.Begin();
        }
        #endregion
        #region Public Methods
        public void ShowDialog(ContentControl parent)
        {
            if (parent == null) parent = Application.Current.MainWindow;
            _DialogCloseResetEvent.Reset();
            SetBackgroundBrush(parent);
            _OldContentContainer.Content = parent.Content;
            parent.Content = this;
            CreateInAnimation();
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
            CreateOutAnimation();
        }
        public void CloseCommandExecuted(object target, ExecutedRoutedEventArgs e) { Close(); }
        public void SetTheme(Brush themeBrush)
        {
            _DialogPartsControl.Background = themeBrush;

            _DialogPartsControl.DialogButtonsControl.Background = new SolidColorBrush(Colors.White) { Opacity = .4 };
            _DialogPartsControl.DialogButtonsControl.BorderBrush = new SolidColorBrush(Colors.Transparent);

            _DialogPartsControl.DialogTitleControl.Background = new SolidColorBrush(Colors.White) { Opacity = .6 };
            _DialogPartsControl.DialogTitleControl.BorderBrush = new SolidColorBrush(Colors.Transparent);

            _DialogPartsControl.DialogContentControl.Background = new SolidColorBrush(Colors.White) { Opacity = .8 };
            _DialogPartsControl.DialogContentControl.BorderBrush = new SolidColorBrush(Colors.Transparent);
        }
        #endregion
    }
}
