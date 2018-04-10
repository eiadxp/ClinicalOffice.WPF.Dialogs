﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace ClinicalOffice.WPF.Dialogs
{
    public class DialogPartsControl : Border
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

        static DialogPartsControl()
        {
            VerticalAlignmentProperty.OverrideMetadata(typeof(DialogPartsControl),
                new FrameworkPropertyMetadata(VerticalAlignment.Center));
            HorizontalAlignmentProperty.OverrideMetadata(typeof(DialogPartsControl),
                new FrameworkPropertyMetadata(HorizontalAlignment.Stretch));
            MarginProperty.OverrideMetadata(typeof(DialogPartsControl),
                new FrameworkPropertyMetadata(new Thickness(15)));
        }
        public DialogPartsControl()
        {
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

            Child = _MainGrid;
        }
    }
}
