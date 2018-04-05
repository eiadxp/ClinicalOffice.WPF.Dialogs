using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls.Primitives;

namespace ClinicalOffice.WPF.Dialogs
{
    public class DialogButtonsControl : UserControl
    {
        /// <summary>
        /// This grid will hold the dialog buttons.
        /// </summary>
        Grid _ButtonsGrid;
        static DialogButtonsControl()
        {
            DefaultStyleKeyProperty.
                OverrideMetadata(typeof(DialogButtonsControl), new FrameworkPropertyMetadata(typeof(DialogButtonsControl)));
        }
        public DialogButtonsControl()
        {
            Grid.SetIsSharedSizeScope(this, true);
            _ButtonsGrid = new Grid()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom
            };
            Content = _ButtonsGrid;
        }

        public IEnumerable<ButtonBase> GetButtons() => _ButtonsGrid.Children.OfType<ButtonBase>();
        public void ClearButtons() { _ButtonsGrid.Children.Clear(); _ButtonsGrid.ColumnDefinitions.Clear(); }
        public void AddButton(ButtonBase button) {
            _ButtonsGrid.ColumnDefinitions.Add(new ColumnDefinition() { SharedSizeGroup = "dialogButtons" });
            Grid.SetColumn(button, _ButtonsGrid.ColumnDefinitions.Count - 1);
            _ButtonsGrid.Children.Add(button);
        }
    }
}
