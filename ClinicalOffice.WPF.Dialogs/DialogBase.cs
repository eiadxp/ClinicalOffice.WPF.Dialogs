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
        Border _Overlay;
        /// <summary>
        /// This is a dump control to hold the old content in it and resize it with the dialog.
        /// </summary>
        ContentControl _OldContentContainer;
        /// <summary>
        /// This is the displayed dialog which has a title, buttons, and contents.
        /// </summary>
        DialogPartsControl _DialogParts;
        /// <summary>
        /// These will hold the buttons.
        /// </summary>
        ButtonBase _OkButton, _CancelButton, _YesButton, _NoButton;
        #endregion
        /// <summary>
        /// Changes default values of some properties.
        /// </summary>
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
        public DialogPartsControl DialogParts { get => _DialogParts; }

        /// <summary>
        /// Type used to create dialog buttons.
        /// </summary>
        /// <remarks>
        /// The buttons are created in <see cref="OnCreateButton" /> virtual method.
        /// If the value is not derived from <see cref="ButtonBase" /> of the type does not have a parameterless constructor, the dialog will create a normal <see cref="Button" />.
        /// </remarks>
        [Category("Dialog")]
        public Type DialogButtonType
        {
            get { return (Type)GetValue(DialogButtonTypeProperty); }
            set { SetValue(DialogButtonTypeProperty, value); }
        }
        public static readonly DependencyProperty DialogButtonTypeProperty =
            DependencyProperty.Register("DialogButtonType", typeof(Type), typeof(DialogBase), new PropertyMetadata(null, DialogButtonTypeChangedCallback));
        /// <summary>
        /// This method is called when <see cref="DialogBase.DialogButtonType" /> property is changed to create new buttons.
        /// </summary>
        static void DialogButtonTypeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dialog = d as DialogBase;
            dialog?.CreateButtons();
        }

        /// <summary>
        /// Type used to create dialog close button.
        /// </summary>
        /// <remarks>
        /// The button is created in <see cref="OnCreateCloseButton" /> virtual method.
        /// If the value is not derived from <see cref="ButtonBase" /> of the type does not have a parameterless constructor, the dialog will create a normal <see cref="Button" />.
        /// </remarks>
        [Category("Dialog")]
        public Type DialogCloseButtonType
        {
            get { return (Type)GetValue(DialogCloseButtonTypeProperty); }
            set { SetValue(DialogCloseButtonTypeProperty, value); }
        }
        public static readonly DependencyProperty DialogCloseButtonTypeProperty =
            DependencyProperty.Register("DialogCloseButtonType", typeof(Type), typeof(DialogBase), new PropertyMetadata(null, DialogCloseButtonTypeChangedCallback));
        /// <summary>
        /// This method is called when <see cref="DialogBase.DialogCloseButtonType" /> property is changed to create new buttons.
        /// </summary>
        static void DialogCloseButtonTypeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dialog = d as DialogBase;
           dialog?.CreateCloseButton();
        }

        /// <summary>
        /// Effect applied to the dialog.
        /// </summary>
        /// <remarks>
        /// This effect is applied to the dialog parts together (title, content, and buttons bar).
        /// The default value in code is <c>null</c>, but an effect of <see cref="DropShadowEffect" /> is applied in the theme file Generic.xaml.
        /// </remarks>
        [Category("Dialog")]
        public Effect DialogEffect
        {
            get { return (Effect)GetValue(DialogEffectProperty); }
            set { SetValue(DialogEffectProperty, value); }
        }
        public static readonly DependencyProperty DialogEffectProperty =
            DependencyProperty.Register("DialogEffect", typeof(Effect), typeof(DialogBase), new PropertyMetadata(null));

        /// <summary>
        /// The content of title bar.
        /// </summary>
        /// <remarks>
        /// If the value is <c>null</c> the title bar visibility will be changed to <see cref="Visibility.Collapsed" />.
        /// Use empty string (<see cref="String.Empty" />) if you want an empty title bar.
        /// </remarks>
        [Category("Dialog")]
        public object DialogTitle
        {
            get { return (object)GetValue(DialogTitleProperty); }
            set { SetValue(DialogTitleProperty, value); }
        }
        public static readonly DependencyProperty DialogTitleProperty =
            DependencyProperty.Register("DialogTitle", typeof(object), typeof(DialogBase), new PropertyMetadata(null));

        /// <summary>
        /// The actual content of the dialog.
        /// </summary>
        /// <remarks>Content is displayed between the Title and Buttons bars of the dialog.</remarks>
        [Category("Dialog")]
        public object DialogContent
        {
            get { return (object)GetValue(DialogContentProperty); }
            set { SetValue(DialogContentProperty, value); }
        }
        public static readonly DependencyProperty DialogContentProperty =
            DependencyProperty.Register("DialogContent", typeof(object), typeof(DialogBase), new PropertyMetadata(null));

        /// <summary>
        /// An enum that specify the visible buttons in the dialog buttons bar
        /// </summary>
        /// <remarks>If set to <see cref="DialogButtons.None" /> the buttons bar will be hidden by setting its visibility to <see cref="Visibility.Collapsed" /></remarks>
        [Category("Dialog")]
        public DialogButtons DialogButtons
        {
            get => (DialogButtons)GetValue(DialogButtonsProperty);
            set => SetValue(DialogButtonsProperty, value);
        }
        public static readonly DependencyProperty DialogButtonsProperty =
            DependencyProperty.Register("DialogButtons", typeof(DialogButtons), typeof(DialogBase),
                new FrameworkPropertyMetadata(DialogButtons.None, FrameworkPropertyMetadataOptions.AffectsArrange, DialogButtonsChangedCallback));
        /// <summary>
        /// This method is called when <see cref="DialogBase.DialogButtons" /> property is changed to create new buttons.
        /// </summary>
        static void DialogButtonsChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as DialogBase)?.CreateButtons();

        /// <summary>
        /// Result of dialog.
        /// </summary>
        [Category("Dialog")]
        public DialogResult DialogResult
        {
            get { return (DialogResult)GetValue(DialogResultProperty); }
            set { SetValue(DialogResultProperty, value); }
        }
        public static readonly DependencyProperty DialogResultProperty =
            DependencyProperty.Register("DialogResult", typeof(DialogResult), typeof(DialogBase), new PropertyMetadata(DialogResult.None));

        /// <summary>
        /// OK button content.
        /// </summary>
        /// <value>Default value is string "OK".</value>
        [Category("Dialog")]
        public object DialogOkContent
        {
            get { return (object)GetValue(DialogOkContentProperty); }
            set { SetValue(DialogOkContentProperty, value); }
        }
        public static readonly DependencyProperty DialogOkContentProperty =
            DependencyProperty.Register("DialogOkContent", typeof(object), typeof(DialogBase), new PropertyMetadata("OK", DialogOkContentChangedCallback));
        /// <summary>
        /// This method is called when <see cref="DialogBase.DialogOkContent" /> property is changed to create new buttons.
        /// </summary>
        static void DialogOkContentChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dialog = d as DialogBase;
            if (dialog?._OkButton != null) dialog._OkButton.Content = e.NewValue;
        }

        /// <summary>
        /// Cancel button content.
        /// </summary>
        /// <value>Default value is string "No".</value>
        [Category("Dialog")]
        public object DialogCancelContent
        {
            get { return (object)GetValue(DialogCancelContentProperty); }
            set { SetValue(DialogCancelContentProperty, value); }
        }
        public static readonly DependencyProperty DialogCancelContentProperty =
            DependencyProperty.Register("DialogCancelContent", typeof(object), typeof(DialogBase), new PropertyMetadata("Cancel", DialogCancelContentChangedCallback));
        /// <summary>
        /// This method is called when <see cref="DialogBase.DialogCancelContent" /> property is changed to create new buttons.
        /// </summary>
        static void DialogCancelContentChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dialog = d as DialogBase;
            if (dialog?._CancelButton != null) dialog._CancelButton.Content = e.NewValue;
        }

        /// <summary>
        /// Yes button content.
        /// </summary>
        /// <value>Default value is string "Yes".</value>
        [Category("Dialog")]
        public object DialogYesContent
        {
            get { return (object)GetValue(DialogYesContentProperty); }
            set { SetValue(DialogYesContentProperty, value); }
        }
        public static readonly DependencyProperty DialogYesContentProperty =
            DependencyProperty.Register("DialogYesContent", typeof(object), typeof(DialogBase), new PropertyMetadata("Yes", DialogYesContentChangedCallback));
        /// <summary>
        /// This method is called when <see cref="DialogBase.DialogYesContent" /> property is changed to create new buttons.
        /// </summary>
        static void DialogYesContentChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dialog = d as DialogBase;
            if (dialog?._YesButton != null) dialog._YesButton.Content = e.NewValue;
        }

        /// <summary>
        /// No button content.
        /// </summary>
        /// <value>Default value is string "No".</value>
        [Category("Dialog")]
        public object DialogNoContent
        {
            get { return (object)GetValue(DialogNoContentProperty); }
            set { SetValue(DialogNoContentProperty, value); }
        }
        public static readonly DependencyProperty DialogNoContentProperty =
            DependencyProperty.Register("DialogNoContent", typeof(object), typeof(DialogBase), new PropertyMetadata("No", DialogNoContentChangedCallback));
        /// <summary>
        /// This method is called when <see cref="DialogBase.DialogNoContent" /> property is changed to create new buttons.
        /// </summary>
        static void DialogNoContentChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dialog = d as DialogBase;
            if (dialog?._NoButton != null) dialog._NoButton.Content = e.NewValue;
        }

        /// <summary>
        /// Close button content.
        /// </summary>
        /// <value>Default value is string "X".</value>
        [Category("Dialog")]
        public object DialogCloseContent
        {
            get { return (object)GetValue(DialogCloseContentProperty); }
            set { SetValue(DialogCloseContentProperty, value); }
        }
        public static readonly DependencyProperty DialogCloseContentProperty =
            DependencyProperty.Register("DialogCloseContent", typeof(object), typeof(DialogBase), new PropertyMetadata("X", DialogCloseContentChangedCallback));
        /// <summary>
        /// This method is called when <see cref="DialogBase.DialogCloseContent" /> property is changed to create new buttons.
        /// </summary>
        static void DialogCloseContentChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dialog = d as DialogBase;
            if (dialog?._DialogParts?.DialogCloseButton != null) dialog._DialogParts.DialogCloseButton.Content = e.NewValue;
        }

        /// <summary>
        /// A style that is applied to dialog close button.
        /// </summary>
        [Category("Dialog")]
        public Style DialogCloseButtonStyle
        {
            get { return (Style)GetValue(DialogCloseButonStyleProperty); }
            set { SetValue(DialogCloseButonStyleProperty, value); }
        }
        public static readonly DependencyProperty DialogCloseButonStyleProperty =
            DependencyProperty.Register("DialogCloseButtonStyle", typeof(Style), typeof(DialogBase), new PropertyMetadata(null, DialogCloseButtonStyleChangedCallback));
        /// <summary>
        /// This method is called when <see cref="DialogBase.DialogCloseButtonStyle" /> property is changed to create new buttons.
        /// </summary>
        static void DialogCloseButtonStyleChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dialog = d as DialogBase;
            if (dialog?._DialogParts?.DialogCloseButton != null) dialog._DialogParts.DialogCloseButton.Style = e.NewValue as Style;
        }

        /// <summary>
        /// A style that is applied to dialog buttons to give a custom universal look.
        /// </summary>
        [Category("Dialog")]
        public Style DialogButtonStyle
        {
            get { return (Style)GetValue(DialogButtonStyleProperty); }
            set { SetValue(DialogButtonStyleProperty, value); }
        }
        public static readonly DependencyProperty DialogButtonStyleProperty =
            DependencyProperty.Register("DialogButtonStyle", typeof(Style), typeof(DialogBase), new PropertyMetadata(null));

        /// <summary>
        /// Brush used to paint over the parent content.
        /// </summary>
        /// <remarks>Use this property with <see cref="DialogOverlayOpacity" /> property to control the overlay that cover the parent content.</remarks>
        /// <value>Default value is <see cref="Brushes.Black" />.</value>
        [Category("Dialog")]
        public Brush DialogOverlay
        {
            get { return (Brush)GetValue(DialogOverlayProperty); }
            set { SetValue(DialogOverlayProperty, value); }
        }
        public static readonly DependencyProperty DialogOverlayProperty =
            DependencyProperty.Register("DialogOverlay", typeof(Brush), typeof(DialogBase), new PropertyMetadata(new SolidColorBrush(Colors.Black) { Opacity = 0.2 }));

        /// <summary>
        /// Opacity of overlay that cover the parent content.
        /// </summary>
        /// <remarks>Use this property with <see cref="DialogOverlay" /> property to control the overlay that cover the parent content.</remarks>
        /// <value>Default value is 0.2</value>
        [Category("Dialog")]
        public double DialogOverlayOpacity
        {
            get { return (double)GetValue(DialogOverlayOpacityProperty); }
            set { SetValue(DialogOverlayOpacityProperty, value); }
        }
        public static readonly DependencyProperty DialogOverlayOpacityProperty =
            DependencyProperty.Register("DialogOverlayOpacity", typeof(double), typeof(DialogBase), new PropertyMetadata(1.0));

        /// <summary>
        /// Effects applied to parent content.
        /// </summary>
        /// <remarks>The effect is not applied directly to the parent control content, it is applied to a control that will hold the render result of parent content.</remarks>
        /// <value>Default value is <see cref="BlureEffect" />.</value>
        [Category("Dialog")]
        public Effect DialogParentEffect
        {
            get { return (Effect)GetValue(DialogParentEffectProperty); }
            set { SetValue(DialogParentEffectProperty, value); }
        }
        public static readonly DependencyProperty DialogParentEffectProperty =
            DependencyProperty.Register("DialogParentEffect", typeof(Effect), typeof(DialogBase), new PropertyMetadata(new BlurEffect()));


        /// <summary>
        /// Brush used to paint the dialog background
        /// </summary>
        /// <remarks>
        /// The dialog background cover the area under: dialog title, dialog content, and dialog buttons bar.
        /// It is very common to use this property to set the theme of the dialog and set the remaining dialog parts (title, contents, buttons bar) to semi transparent brushes to give a look of a theme.
        /// </remarks>
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

        /// <summary>
        /// Storyboard used to play in (show) animation when the dialog is shown.
        /// </summary>
        /// <remarks>You do not need to start this storyboard. It will be played automatically after showing the dialog</remarks>
        [Category("Dialog")]
        public Storyboard DialogCustomAnimationIn
        {
            get { return (Storyboard)GetValue(DialogCustomAnimationInProperty); }
            set { SetValue(DialogCustomAnimationInProperty, value); }
        }
        public static readonly DependencyProperty DialogCustomAnimationInProperty =
            DependencyProperty.Register("DialogCustomAnimationIn", typeof(Storyboard), typeof(DialogBase), new PropertyMetadata(null));

        /// <summary>
        /// An enum that specify the out (hide)animation of <see cref="DialogBase" /></summary>
        /// <remarks>The library include some ready to use animation like <see cref="DialogAnimation.Fade" />.</remarks>
        [Category("Dialog")]
        public DialogAnimation DialogAnimationOut
        {
            get { return (DialogAnimation)GetValue(DialogAnimationOutProperty); }
            set { SetValue(DialogAnimationOutProperty, value); }
        }
        public static readonly DependencyProperty DialogAnimationOutProperty =
            DependencyProperty.Register("DialogAnimationOut", typeof(DialogAnimation), typeof(DialogBase), new PropertyMetadata(DialogAnimation.Global));

        /// <summary>
        /// Storyboard used to play out (hide) animation when the dialog is about to close.
        /// </summary>
        /// <remarks>You do not need to start this storyboard. It will be played automatically before hiding the dialog</remarks>
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

        /// <summary>
        /// The element that will got focus when the dialog is first shown.
        /// </summary>
        [Category("Dialog")]
        public UIElement DialogFocusedElement
        {
            get { return (UIElement)GetValue(DialogFocusedElementProperty); }
            set { SetValue(DialogFocusedElementProperty, value); }
        }
        public static readonly DependencyProperty DialogFocusedElementProperty =
            DependencyProperty.Register("DialogFocusedElement", typeof(UIElement), typeof(DialogBase), new PropertyMetadata(null));

        /// <summary>
        /// Indicate weather this dialog will be closed when any of its button clicked.
        /// </summary>
        /// <remarks>Set this property to <c>false</c> if you want to verify dialog content and closed only if it is valid.</remarks>
        /// <value>
        /// if <c>true</c> the dialog will be closed after any of its button is clicked.
        /// If <c>false</c> the dialog will be closed manually only.
        /// </value>
        [Category("Dialog")]
        public bool DialogAutoClose
        {
            get { return (bool)GetValue(DialogAutoCloseProperty); }
            set { SetValue(DialogAutoCloseProperty, value); }
        }
        public static readonly DependencyProperty DialogAutoCloseProperty =
            DependencyProperty.Register("DialogAutoClose", typeof(bool), typeof(DialogBase), new PropertyMetadata(true));

        /// <summary>
        /// Command that will be executed when OK button is pressed.
        /// </summary>
        [Category("Dialog")]
        public ICommand DialogOkCommand
        {
            get { return (ICommand)GetValue(DialogOkCommandProperty); }
            set { SetValue(DialogOkCommandProperty, value); }
        }
        public static readonly DependencyProperty DialogOkCommandProperty =
            DependencyProperty.Register("DialogOkCommand", typeof(ICommand), typeof(DialogBase), new PropertyMetadata(null));

        /// <summary>
        /// Parameter passed to <see cref="DialogOkCommand" /> when executed.
        /// </summary>
        [Category("Dialog")]
        public object DialogOkCommandParameter
        {
            get { return (object)GetValue(DialogOkCommandParameterProperty); }
            set { SetValue(DialogOkCommandParameterProperty, value); }
        }
        public static readonly DependencyProperty DialogOkCommandParameterProperty =
            DependencyProperty.Register("DialogOkCommandParameter", typeof(object), typeof(DialogBase), new PropertyMetadata(null));

        /// <summary>
        /// Command that will be executed when cancel button is pressed.
        /// </summary>
        [Category("Dialog")]
        public ICommand DialogCancelCommand
        {
            get { return (ICommand)GetValue(DialogCancelCommandProperty); }
            set { SetValue(DialogCancelCommandProperty, value); }
        }
        public static readonly DependencyProperty DialogCancelCommandProperty =
            DependencyProperty.Register("DialogCancelCommand", typeof(ICommand), typeof(DialogBase), new PropertyMetadata(null));

        /// <summary>
        /// Parameter passed to <see cref="DialogCancelCommand" /> when executed.
        /// </summary>
        [Category("Dialog")]
        public object DialogCancelCommandParameter
        {
            get { return (object)GetValue(DialogCancelCommandParameterProperty); }
            set { SetValue(DialogCancelCommandParameterProperty, value); }
        }
        public static readonly DependencyProperty DialogCancelCommandParameterProperty =
            DependencyProperty.Register("DialogCancelCommandParameter", typeof(object), typeof(DialogBase), new PropertyMetadata(null));

        /// <summary>
        /// Command that will be executed when yes button is pressed.
        /// </summary>
        [Category("Dialog")]
        public ICommand DialogYesCommand
        {
            get { return (ICommand)GetValue(DialogYesCommandProperty); }
            set { SetValue(DialogYesCommandProperty, value); }
        }
        public static readonly DependencyProperty DialogYesCommandProperty =
            DependencyProperty.Register("DialogYesCommand", typeof(ICommand), typeof(DialogBase), new PropertyMetadata(null));

        /// <summary>
        /// Parameter passed to <see cref="DialogYesCommand" /> when executed.
        /// </summary>
        [Category("Dialog")]
        public object DialogYesCommandParameter
        {
            get { return (object)GetValue(DialogYesCommandParameterProperty); }
            set { SetValue(DialogYesCommandParameterProperty, value); }
        }
        public static readonly DependencyProperty DialogYesCommandParameterProperty =
            DependencyProperty.Register("DialogYesCommandParameter", typeof(object), typeof(DialogBase), new PropertyMetadata(null));

        /// <summary>
        /// Command that will be executed when no button is pressed.
        /// </summary>
        [Category("Dialog")]
        public ICommand DialogNoCommand
        {
            get { return (ICommand)GetValue(DialogNoCommandProperty); }
            set { SetValue(DialogNoCommandProperty, value); }
        }
        public static readonly DependencyProperty DialogNoCommandProperty =
            DependencyProperty.Register("DialogNoCommand", typeof(ICommand), typeof(DialogBase), new PropertyMetadata(null));

        /// <summary>
        /// Parameter passed to <see cref="DialogNoCommand" /> when executed.
        /// </summary>
        [Category("Dialog")]
        public object DialogNoCommandParameter
        {
            get { return (object)GetValue(DialogNoCommandParameterProperty); }
            set { SetValue(DialogNoCommandParameterProperty, value); }
        }
        public static readonly DependencyProperty DialogNoCommandParameterProperty =
            DependencyProperty.Register("DialogNoCommandParameter", typeof(object), typeof(DialogBase), new PropertyMetadata(null));

        /// <summary>
        /// Command that will be executed when close button is pressed.
        /// </summary>
        [Category("Dialog")]
        public ICommand DialogCloseCommand
        {
            get { return (ICommand)GetValue(DialogCloseCommandProperty); }
            set { SetValue(DialogCloseCommandProperty, value); }
        }
        public static readonly DependencyProperty DialogCloseCommandProperty =
            DependencyProperty.Register("DialogCloseCommand", typeof(ICommand), typeof(DialogBase), new PropertyMetadata(null));

        /// <summary>
        /// Parameter passed to <see cref="DialogCloseCommand" /> when executed.
        /// </summary>
        [Category("Dialog")]
        public object DialogCloseCommandParameter
        {
            get { return (object)GetValue(DialogCloseCommandParameterProperty); }
            set { SetValue(DialogCloseCommandParameterProperty, value); }
        }
        public static readonly DependencyProperty DialogCloseCommandParameterProperty =
            DependencyProperty.Register("DialogCloseCommandParameter", typeof(object), typeof(DialogBase), new PropertyMetadata(null));


        #endregion
        #region Virtual methods
        /// <summary>
        /// Creates a dialog button.
        /// </summary>
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
        /// <summary>
        /// Creates a dialog button.
        /// </summary>
        virtual protected ButtonBase OnCreateCloseButton(object content)
        {
            if (DialogCloseButtonType != null && DialogCloseButtonType.IsAssignableFrom(typeof(ButtonBase)))
            {
                try
                {
                    var b = Activator.CreateInstance(DialogCloseButtonType) as ButtonBase;
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
        /// <summary>
        /// Called when OK button is clicked (even via Return button or return command <see cref="DialogCommands.ReturnKey"/>) or OK command is executed via <see cref="DialogCommands.Ok"/>.
        /// </summary>
        /// <remarks>
        ///   <p>If this method returned <c>False</c>, the <see cref="OnClosing(DialogResult)" /> will not called.</p>
        ///   <p>If this method returned <c>True</c>, the <see cref="OnClosing(DialogResult)" /> will be called.</p>
        ///   <p>Basic implantation of this method is to return <c>True</c> only. It is not necessary to call the base method if you override it.</p>
        /// </remarks>
        /// <returns>
        ///   <c>True</c> continue closing.
        ///   <c>False</c> cancel closing.
        /// </returns>
        virtual protected bool OnDialogOk() => true;
        /// <summary>
        /// Called when cancel button is clicked (even via Escape button or return command <see cref="DialogCommands.EscapeKey"/>) or cancel command is executed via <see cref="DialogCommands.Cancel"/>.
        /// </summary>
        /// <remarks>
        ///   <p>If this method returned <c>False</c>, the <see cref="OnClosing(DialogResult)" /> will not called.</p>
        ///   <p>If this method returned <c>True</c>, the <see cref="OnClosing(DialogResult)" /> will be called.</p>
        ///   <p>Basic implantation of this method is to return <c>True</c> only. It is not necessary to call the base method if you override it.</p>
        /// </remarks>
        /// <returns>
        ///   <c>True</c> continue closing.
        ///   <c>False</c> cancel closing.
        /// </returns>
        virtual protected bool OnDialogCancel() => true;
        /// <summary>
        /// Called when yes button is clicked (even via Return button or return command <see cref="DialogCommands.ReturnKey"/>) or yes command is executed via <see cref="DialogCommands.Yes"/>.
        /// </summary>
        /// <remarks>
        ///   <p>If this method returned <c>False</c>, the <see cref="OnClosing(DialogResult)" /> will not called.</p>
        ///   <p>If this method returned <c>True</c>, the <see cref="OnClosing(DialogResult)" /> will be called.</p>
        ///   <p>Basic implantation of this method is to return <c>True</c> only. It is not necessary to call the base method if you override it.</p>
        /// </remarks>
        /// <returns>
        ///   <c>True</c> continue closing.
        ///   <c>False</c> cancel closing.
        /// </returns>
        virtual protected bool OnDialogYes() => true;
        /// <summary>
        /// Called when "no" button is clicked or "no" command is executed via <see cref="DialogCommands.No"/>.
        /// </summary>
        /// <remarks>
        ///   <p>If this method returned <c>False</c>, the <see cref="OnClosing(DialogResult)" /> will not called.</p>
        ///   <p>If this method returned <c>True</c>, the <see cref="OnClosing(DialogResult)" /> will be called.</p>
        ///   <p>Basic implantation of this method is to return <c>True</c> only. It is not necessary to call the base method if you override it.</p>
        /// </remarks>
        /// <returns>
        ///   <c>True</c> continue closing.
        ///   <c>False</c> cancel closing.
        /// </returns>
        virtual protected bool OnDialogNo() => true;
        /// <summary>
        /// Called before closing the dialog to determine weather to close or not.
        /// </summary>
        /// <param name="result">The button that was clicked to close the dialog, otherwise it will be <see cref="DialogResult.None" /> (Like when calling <see cref="TryClose()" />.</param>
        /// <remarks>
        ///   <Para>This method is called if a dialog button is pressed, a keyboard (Return/Escape) is pressed, or if a dialog command from <see cref="DialogCommands" /> is executed.</Para>
        ///   <Para>Using <see cref="DialogBase.Close" /> will close the dialog without calling this method</Para>
        ///   <Para>Use <see cref="DialogBase.TryClose" /> if you want to call this method before closing.</Para>
        ///   <Para>The method will never called if a button is pressed and <see cref="DialogAutoClose" /> if <c>False</c>, but it will be called always when you use <see cref="TryClose()" />.</Para>
        ///   <para>When you override this method, it is important to call the base implantation to set <see cref="DialogResult"/> value, otherwise you should set it in your implantation.</para>
        /// </remarks>
        /// <returns>
        /// If <c>True</c> the dialog the dialog will be closed.
        /// If <c>False</c> the close action will be canceled.
        /// </returns>
        virtual protected bool OnClosing(DialogResult result) { DialogResult = result; return true; }
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
        /// This method is called from <see cref="DialogParts"/> when OK button is clicked.
        /// It is not related directly to DialogOkCommand in this class.
        /// However the DialogOkCommand will be executed when this method is called through OnDialogOk method.
        /// </summary>
        internal void OkCommandExecuted()
        {
            DialogOkCommand?.Execute(DialogOkCommandParameter);
            if (OnDialogOk() && DialogAutoClose && OnClosing(DialogResult.Ok)) Close();
        }
        /// <summary>
        /// This method is called from <see cref="DialogParts" /> when Cancel button is clicked.
        /// It is not related directly to DialogCancelCommand in this class.
        /// However the DialogCancelCommand will be executed when this method is called through OnDialogCancel method.
        /// </summary>
        internal void CancelCommandExecuted()
        {
            DialogCancelCommand?.Execute(DialogCancelCommandParameter);
            if (OnDialogCancel() && DialogAutoClose && OnClosing(DialogResult.Cancel)) Close();
        }
        /// <summary>
        /// This method is called from <see cref="DialogParts"/> when Yes button is clicked.
        /// It is not related directly to DialogYesCommand in this class.
        /// However the DialogYesCommand will be executed when this method is called through OnDialogYes method.
        /// </summary>
        internal void YesCommandExecuted()
        {
            DialogYesCommand?.Execute(DialogYesCommandParameter);
            if (OnDialogYes() && DialogAutoClose && OnClosing(DialogResult.Yes)) Close();
        }
        /// <summary>
        /// This method is called from <see cref="DialogParts"/> when 'No' button is clicked.
        /// It is not related directly to DialogNoCommand in this class.
        /// However the DialogNoCommand will be executed when this method is called through OnDialogNo method.
        /// </summary>
        internal void NoCommandExecuted()
        {
            DialogNoCommand?.Execute(DialogNoCommandParameter);
            if (OnDialogNo() && DialogAutoClose && OnClosing(DialogResult.No)) Close();
        }
        /// <summary>
        /// This method is called from <see cref="DialogParts"/> when close button is clicked.
        /// It is not related directly to <see cref="DialogCloseCommand"/> in this class.
        /// However the <see cref="DialogCloseCommand"/> will be executed when this method is called through <see cref="OnDialogClose"/> method.
        /// </summary>
        internal void CloseCommandExecuted()
        {
            DialogCloseCommand?.Execute(DialogCloseCommandParameter);
            if (DialogAutoClose && OnClosing(DialogResult.None)) Close();
        }
        /// <summary>
        /// This method is called from <see cref="DialogParts"/> when Return key is pressed and it will simulate the default button click.
        /// </summary>
        internal void ReturnKeyCommandExecuted()
        {
            if (DialogButtons == DialogButtons.Ok || DialogButtons == DialogButtons.OkCancel) OkCommandExecuted();
            else if (DialogButtons == DialogButtons.YesNo || DialogButtons == DialogButtons.YesNoCancel) YesCommandExecuted();
        }
        /// <summary>
        /// This method is called from <see cref="DialogParts"/> when Escape key is pressed and it will simulate the cancel button click.
        /// </summary>
        internal void EscapeKeyCommandExecuted()
        {
            if (DialogButtons == DialogButtons.YesNoCancel || DialogButtons == DialogButtons.OkCancel) CancelCommandExecuted();
        }

        #endregion
        #region Private methods
        /// <summary>
        /// Create dialog main controls.
        /// </summary>
        void CreateControls()
        {
            _OldContentContainer = new ContentControl();

            _ParentBackground = new Border();
            BindingOperations.SetBinding(_ParentBackground, EffectProperty,
                                         new Binding(nameof(DialogParentEffect)) { Source = this });
            Grid.SetRowSpan(_ParentBackground, 3);
            Panel.SetZIndex(_ParentBackground, 0);

            _Overlay = new Border();
            BindingOperations.SetBinding(_Overlay, BackgroundProperty,
                                         new Binding(nameof(DialogOverlay)) { Source = this });
            BindingOperations.SetBinding(_Overlay, OpacityProperty,
                                         new Binding(nameof(DialogOverlayOpacity)) { Source = this });
            Grid.SetRowSpan(_Overlay, 3);
            Panel.SetZIndex(_Overlay, 1);

            _DialogParts = new DialogPartsControl(this);
            Grid.SetRow(_DialogParts, 1);
            Panel.SetZIndex(_DialogParts, 3);
            BindingOperations.SetBinding(_DialogParts.DialogTitleControl, DialogTitleControl.ContentProperty,
                                         new Binding(nameof(DialogTitle)) { Source = this });
            BindingOperations.SetBinding(_DialogParts.DialogContentControl, ContentProperty,
                                         new Binding(nameof(DialogContent)) { Source = this });
            BindingOperations.SetBinding(_DialogParts.DialogBackground, EffectProperty,
                                         new Binding(nameof(DialogEffect)) { Source = this });
            BindingOperations.SetBinding(_DialogParts.DialogBackground, BackgroundProperty,
                                         new Binding(nameof(DialogBackGround)) { Source = this });

            RegisterNames();

            CreateButtons();

            CreateCloseButton();

            _DialogGrid = new Grid();
            _DialogGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            _DialogGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            _DialogGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            _DialogGrid.Children.Add(_ParentBackground);
            _DialogGrid.Children.Add(_Overlay);
            _DialogGrid.Children.Add(_DialogParts);
            base.Content = _DialogGrid;
        }
        /// <summary>
        /// Create names for dialog parts to allow animations using target name.
        /// </summary>
        /// <remarks>
        /// <para>Parts names can be found within static members of <see cref="DialogParameters"/> class.</para>
        /// <para>Currently we have the following parts name:</para>
        /// <list type="number">
        /// <item><description><see cref="DialogParameters.DialogParentName"/></description></item>
        /// <item><description><see cref="DialogParameters.DialogOverlayName"/></description></item>
        /// <item><description><see cref="DialogParameters.DialogPartsName"/></description></item>
        /// </list>
        /// </remarks>
        void RegisterNames()
        {
            NameScope.SetNameScope(this, new NameScope());
            RegisterName(_DialogParts, DialogParameters.DialogPartsName);
            RegisterName(_Overlay, DialogParameters.DialogOverlayName);
            RegisterName(_ParentBackground, DialogParameters.DialogParentName);
            RegisterName(_DialogParts.DialogTitleControl, DialogParameters.DialogTitleName);
            RegisterName(_DialogParts.DialogContentControl, DialogParameters.DialogContentName);
            RegisterName(_DialogParts.DialogButtonsControl, DialogParameters.DialogButtonsName);
        }
        /// <summary>
        /// Used to register a <see cref="FrameworkElement"/> name in name scope, and set its Name property to the same value.
        /// </summary>
        /// <param name="e">A <see cref="FrameworkElement"/> to register</param>
        /// <param name="name"> Registration name.</param>
        void RegisterName(FrameworkElement e, string name)
        {
            e.Name = name;
            RegisterName(e.Name, e);
        }
        /// <summary>
        /// Create Dialogs buttons according to the value of <see cref="DialogBase.DialogButtons" />.
        /// </summary>
        void CreateButtons()
        {
            foreach (var button in _DialogParts.DialogButtonsControl.GetButtons())
                UnregisterName(button.Name);
            _DialogParts.DialogButtonsControl.ClearButtons();
            _OkButton = null;
            _CancelButton = null;
            _YesButton = null;
            _NoButton = null;
            InputBindings.Clear();
            DialogParts.DialogButtonsControl.Visibility = Visibility.Visible;
            switch (DialogButtons)
            {
                case DialogButtons.None:
                    DialogParts.DialogButtonsControl.Visibility = Visibility.Collapsed;
                    break;
                case DialogButtons.Ok:
                    _OkButton = CreateButton(DialogOkContent, DialogCommands.Ok, DialogParameters.DialogOKButtonName);
                    break;
                case DialogButtons.OkCancel:
                    _OkButton = CreateButton(DialogOkContent, DialogCommands.Ok, DialogParameters.DialogOKButtonName);
                    _CancelButton = CreateButton(DialogCancelContent, DialogCommands.Cancel, DialogParameters.DialogCancelButtonName);
                    break;
                case DialogButtons.YesNo:
                    _YesButton = CreateButton(DialogYesContent, DialogCommands.Yes, DialogParameters.DialogYesButtonName);
                    _NoButton = CreateButton(DialogNoContent, DialogCommands.No, DialogParameters.DialogNoButtonName);
                    break;
                case DialogButtons.YesNoCancel:
                    _YesButton = CreateButton(DialogYesContent, DialogCommands.Yes, DialogParameters.DialogYesButtonName);
                    _NoButton = CreateButton(DialogNoContent, DialogCommands.No, DialogParameters.DialogNoButtonName);
                    _CancelButton = CreateButton(DialogCancelContent, DialogCommands.Cancel, DialogParameters.DialogCancelButtonName);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Create a dialog button and associate it with its internal command and bind its style property to <see cref="DialogBase.DialogButtonStyle" />.
        /// </summary>
        ButtonBase CreateButton(object content, ICommand command, string name)
        {
            var b = OnCreateButton(content);
            b.Command = command;
            BindingOperations.SetBinding(b, StyleProperty,
                new Binding(nameof(DialogButtonStyle)) { Source = this, Mode = BindingMode.OneWay });
            _DialogParts.DialogButtonsControl.AddButton(b);
            RegisterName(b, name);
            return b;
        }
        /// <summary>
        /// Create close button and register its name under <see cref="DialogParameters.DialogCloseButtonName"/>.
        /// </summary>
        void CreateCloseButton()
        {
            if(DialogParts?.DialogCloseButton != null) UnregisterName(DialogParameters.DialogCloseButtonName);
            _DialogParts.SetCloseButton(OnCreateCloseButton(DialogCloseContent));
            RegisterName(_DialogParts.DialogCloseButton, DialogParameters.DialogCloseButtonName);
        }

        /// <summary>
        /// Creates a visual brush of old content of the parent control, and set it for dialog.
        /// </summary>
        void SetBackgroundBrush(ContentControl parent)
        {
            var element = parent?.Content as UIElement;
            if (element == null) return;
            _ParentBackground.Background = new VisualBrush(element) { Stretch = Stretch.Uniform, AlignmentX = AlignmentX.Left, AlignmentY = AlignmentY.Top };
        }
        /// <summary>
        /// This method actually closes the dialog. It also Set an internal <see cref="System.Threading.ManualResetEvent" /> to inform other threads that its closed.
        /// </summary>
        void InternalClose()
        {
            var c = Parent as ContentControl;
            if (c != null) c.Content = _OldContentContainer.Content;
            _DialogCloseResetEvent.Set();
        }

        /// <summary>
        /// Create animation when dialog is shown.
        /// </summary>
        void CreateInAnimation()
        {
            /// Getting the animation type.
            var type = DialogAnimationIn;
            if (type == DialogAnimation.Global) type = DialogParameters.DialogAnimationIn;
            if (type == DialogAnimation.Global) type = DialogAnimation.None;
            /// Getting duration of animation.
            var duration = DialogAnimationDuration;
            if (duration == Duration.Automatic) duration = DialogParameters.DialogAnimationDuration;
            if (duration == Duration.Automatic) duration = new Duration(TimeSpan.FromMilliseconds(100));
            /// Saving old transformation and opacity for built in animation
            var oldTransform = RenderTransform;
            var oldOrigfin = RenderTransformOrigin;
            var oldOpacity = Opacity;
            /// Saving CacheMode to restore it after animation is done.
            var oldCacheMode = CacheMode;
            var oldBackground = Background;
            Storyboard story;
            story = null;
            if (type == DialogAnimation.Custom)
            {
                story = DialogCustomAnimationIn ?? DialogParameters.DialogCustomAnimationIn;
                if (story == null) type = DialogAnimation.None;
            }
            else
            {
                story = new Storyboard();
            }
            EventHandler AnimationInCompleted = null;
            CacheMode = new BitmapCache() { EnableClearType = false, RenderAtScale = 1 } ;
            Background = _OldContentContainer.Background;
            switch (type)
            {
                case DialogAnimation.Fade:
                    AnimationInCompleted = (sender, e) =>
                    {
                        if (sender is Timeline a) a.Completed -= AnimationInCompleted;
                        Background = oldBackground;
                        CacheMode = oldCacheMode;
                        Opacity = oldOpacity;
                    };
                    var fade = new DoubleAnimation() { From = 0, To = 1, Duration = duration };
                    Storyboard.SetTarget(fade, this);
                    Storyboard.SetTargetProperty(fade, new PropertyPath(OpacityProperty));
                    story.Children.Add(fade);
                    break;
                case DialogAnimation.Zoom:
                    AnimationInCompleted = (sender, e) =>
                    {
                        if (sender is Timeline a) a.Completed -= AnimationInCompleted;
                        Background = oldBackground;
                        CacheMode = oldCacheMode;
                        RenderTransform = oldTransform;
                    };
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
                    AnimationInCompleted = (sender, e) =>
                    {
                        if (sender is Timeline a) a.Completed -= AnimationInCompleted;
                        Background = oldBackground;
                        CacheMode = oldCacheMode;
                        RenderTransform = oldTransform;
                        RenderTransformOrigin = oldOrigfin;
                    };
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
                    AnimationInCompleted = (sender, e) =>
                    {
                        if (sender is Timeline a) a.Completed -= AnimationInCompleted;
                        Background = oldBackground;
                        CacheMode = oldCacheMode;
                    };
                    break;
                case DialogAnimation.None:
                default:
                    return;
            }
            story.Completed += AnimationInCompleted;
            story.Begin();
        }
        /// <summary>
        /// Create animation  before closing the dialog, then it calls <see cref="DialogBase.InternalClose" /> to close the dialog.
        /// </summary>
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
            if (type == DialogAnimation.Custom)
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
        /// <summary>
        /// Show dialog in the parent control and return before closing the dialog.
        /// </summary>
        /// <remarks>This method should be called only from UI thread.</remarks>
        public void ShowDialog(ContentControl parent = null)
        {
            if (parent == null) parent = Application.Current.MainWindow;
            _DialogCloseResetEvent.Reset();
            SetBackgroundBrush(parent);
            _OldContentContainer.Content = parent.Content;
            parent.Content = this;
            CreateInAnimation();
        }
        /// <summary>
        /// Show the dialog and return a task that will end only when the dialog is closed.
        /// </summary>
        /// <remarks>
        /// The returned task will wait for the dialog to be closed. This is useful if used with <c>await</c>.
        /// It is safe to call this method from any thread.
        /// </remarks>
        public Task<DialogResult> ShowDialogAsync(ContentControl parent = null)
        {
            Dispatcher.Invoke(() => ShowDialog(parent));
            return Task.Run(() =>
            {
                _DialogCloseResetEvent.WaitOne();
                return Dispatcher.Invoke(() => DialogResult);
            });
        }
        /// <summary>
        /// Close an opened dialog.
        /// </summary>
        public void Close()
        {
            CreateOutAnimation();
        }
        /// <summary>
        /// Try to close an opened dialog.
        /// </summary>
        /// <remarks>
        /// This method will call <see cref="OnClosing(DialogResult)"/>, and then closes the dialog if the return value is <c>True</c>.
        /// </remarks>
        public void TryClose()
        {
            if (OnClosing(DialogResult.None)) Close();
        }
        /// <summary>
        /// Close an opened dialog
        /// </summary>
        /// <remarks>
        /// This method is useful for binding a custom command to close the dialog.
        /// </remarks>
        /// <param name="target"></param>
        /// <param name="e"></param>
        public void CloseCommandExecuted(object target, ExecutedRoutedEventArgs e) { Close(); }
        /// <summary>
        /// Set a theme brush to the dialog.
        /// </summary>
        /// <param name="themeBrush"> The theme brush</param>
        /// <remarks>
        /// This will set all dialog parts background to themeBrush,
        /// title bar will have a white brush with opacity of 0.6, 
        /// buttons bar will have a white brush with opacity of 0.4., 
        /// and dialog content will have a white brush with opacity of 0.8.
        /// </remarks>
        public void SetTheme(Brush themeBrush)
        {
            _DialogParts.Background = themeBrush;
            //Borders are always same color of the them.
            _DialogParts.DialogButtonsControl.BorderBrush = new SolidColorBrush(Colors.Transparent);
            _DialogParts.DialogTitleControl.BorderBrush = new SolidColorBrush(Colors.Transparent);
            _DialogParts.DialogContentControl.BorderBrush = new SolidColorBrush(Colors.Transparent);
            if (themeBrush == Brushes.Transparent)
            {
                //If you use transparent brush then all parts will be transparent
                _DialogParts.DialogButtonsControl.Background = themeBrush;
                _DialogParts.DialogTitleControl.Background = themeBrush;
                _DialogParts.DialogContentControl.Background = themeBrush;
            }
            else
            {
                //If you did not use transparent brush then each part will have different shade.
                _DialogParts.DialogButtonsControl.Background = new SolidColorBrush(Colors.White) { Opacity = .4 };
                _DialogParts.DialogTitleControl.Background = new SolidColorBrush(Colors.White) { Opacity = .6 };
                _DialogParts.DialogContentControl.Background = new SolidColorBrush(Colors.White) { Opacity = .8 };
            }
        }
        #endregion
    }
}
