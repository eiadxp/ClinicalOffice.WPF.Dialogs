using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace ClinicalOffice.WPF.Dialogs
{
    public class DialogPartsControl : UserControl
    {
        Grid _MainGrid;
        /// <summary>
        /// This will hold the content of the dialog title.
        /// </summary>
        DialogTitleControl _DialogTitleContent;
        public DialogTitleControl DialogTitleControl { get => _DialogTitleContent; }
        /// <summary>
        /// This will hold the buutons grid and will allow styling and templating.
        /// </summary>
        DialogButtonsControl _DialogButtonsBar;
        public DialogButtonsControl DialogButtonsControl { get => _DialogButtonsBar; }
        /// <summary>
        /// This will hold the actual dialog contents.
        /// </summary>
        DialogContentControl _DialogContent;
        public DialogContentControl DialogContentControl { get => _DialogContent; }
        /// <summary>
        /// This is used to create a background for the dialog.
        /// usefull for back ground color and effects like DropShadowEffect.
        /// </summary>
        Border _DialogBackGround;
        public Border DialogBackground { get => _DialogBackGround; }
        public DialogBase Dialog { get; set; }

        static DialogPartsControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogPartsControl), new FrameworkPropertyMetadata(typeof(DialogPartsControl)));
            VerticalAlignmentProperty.OverrideMetadata(typeof(DialogPartsControl), new FrameworkPropertyMetadata(VerticalAlignment.Center));
            HorizontalAlignmentProperty.OverrideMetadata(typeof(DialogPartsControl), new FrameworkPropertyMetadata(HorizontalAlignment.Stretch));
            MarginProperty.OverrideMetadata(typeof(DialogPartsControl), new FrameworkPropertyMetadata(new Thickness(15)));
            FocusableProperty.OverrideMetadata(typeof(DialogPartsControl), new FrameworkPropertyMetadata(true));
        }
        public DialogPartsControl(DialogBase dialog)
        {
            Dialog = dialog ?? throw new ArgumentNullException(nameof(dialog));

            _MainGrid = new Grid() { VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Stretch };
            _MainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            _MainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            _MainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });

            _DialogTitleContent = new DialogTitleControl();
            Grid.SetRow(_DialogTitleContent, 0);
            Panel.SetZIndex(_DialogTitleContent, 1);

            _DialogContent = new DialogContentControl();
            Grid.SetRow(_DialogContent, 1);
            Panel.SetZIndex(_DialogContent, 1);

            _DialogButtonsBar = new DialogButtonsControl()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
            Grid.SetIsSharedSizeScope(_DialogButtonsBar, true);
            Grid.SetRow(_DialogButtonsBar, 2);
            Panel.SetZIndex(_DialogButtonsBar, 1);

            _DialogBackGround = new Border();
            BindingOperations.SetBinding(this, BackgroundProperty,
                                         new Binding(nameof(Background)) { Source = _DialogBackGround, Mode = BindingMode.TwoWay });
            Grid.SetRowSpan(_DialogBackGround, 3);
            Panel.SetZIndex(_DialogBackGround, 0);

            _MainGrid.Children.Add(_DialogTitleContent);
            _MainGrid.Children.Add(_DialogContent);
            _MainGrid.Children.Add(_DialogButtonsBar);
            _MainGrid.Children.Add(_DialogBackGround);

            Content = _MainGrid;

            CommandBindings.Add(new CommandBinding(DialogCommands.Ok, OkCommandExecuted));
            CommandBindings.Add(new CommandBinding(DialogCommands.Cancel, CancelCommandExecuted));
            CommandBindings.Add(new CommandBinding(DialogCommands.Yes, YesCommandExecuted));
            CommandBindings.Add(new CommandBinding(DialogCommands.No, NoCommandExecuted));
            CommandBindings.Add(new CommandBinding(DialogCommands.ReturnKey, ReturnExecuted));
            CommandBindings.Add(new CommandBinding(DialogCommands.EscapeKey, EscapeExecuted));
            InputBindings.Add(new KeyBinding(DialogCommands.ReturnKey, Key.Return, ModifierKeys.None));
            InputBindings.Add(new KeyBinding(DialogCommands.EscapeKey, Key.Escape, ModifierKeys.None));
        }
        void OkCommandExecuted(object sender, ExecutedRoutedEventArgs e) { Dialog.OkCommandExecuted(); }
        void CancelCommandExecuted(object sender, ExecutedRoutedEventArgs e) { Dialog.CancelCommandExecuted(); }
        void YesCommandExecuted(object sender, ExecutedRoutedEventArgs e) { Dialog.YesCommandExecuted(); }
        void NoCommandExecuted(object sender, ExecutedRoutedEventArgs e) { Dialog.NoCommandExecuted(); }
        void ReturnExecuted(object sender, ExecutedRoutedEventArgs e) { Dialog.ReturnKeyCommandExecuted(); }
        void EscapeExecuted(object sender, ExecutedRoutedEventArgs e) { Dialog.EscapeKeyCommandExecuted(); }
    }
}
