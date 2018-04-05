using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ClinicalOffice.WPF.Dialogs
{
    public class DialogPartsControl : ContentControl
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
        /// This is just a layer under the dialog parts to apply effects on it through DialogEffect property of DialogBase.
        /// Applying effects to the DialogPartsControl will cause a blur to its content.
        /// </summary>
        Border _DialogPartsEffect;
        public Border DialogPartsEffects { get => _DialogPartsEffect; }

        static DialogPartsControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogPartsControl),
                new FrameworkPropertyMetadata(typeof(DialogPartsControl)));
        }
        public DialogPartsControl()
        {
            _MainGrid = new Grid();
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
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            Grid.SetIsSharedSizeScope(_DialogButtonsBar, true);
            Grid.SetRow(_DialogButtonsBar, 2);
            Panel.SetZIndex(_DialogButtonsBar, 1);

            _DialogPartsEffect = new Border() { Background = Brushes.Black };
            Grid.SetRowSpan(_DialogPartsEffect, 3);
            Panel.SetZIndex(_DialogPartsEffect, 0);

            _MainGrid.Children.Add(_DialogTitleContent);
            _MainGrid.Children.Add(_DialogContent);
            _MainGrid.Children.Add(_DialogButtonsBar);
            _MainGrid.Children.Add(_DialogPartsEffect);

            Content = _MainGrid;
        }
    }
}
