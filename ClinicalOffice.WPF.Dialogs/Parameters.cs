using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ClinicalOffice.WPF.Dialogs
{
    internal static class Parameters
    {
        public static Brush TitleBackground { get; set; } = Brushes.AliceBlue;
        [AttachedPropertyBrowsableForTypeAttribute(typeof(Application))]
        public static void SetTitleBackground(object obj, Brush brush) { TitleBackground = brush; }
        public static Brush ButtonsBarBackground { get; set; } = Brushes.CornflowerBlue;
        public static Brush BorderBackground { get; set; } = Brushes.SteelBlue;
        public static Brush ButtonsBackground { get; set; } = new SolidColorBrush(Colors.White) { Opacity = 0.5 };
    }
    public class DialogParameters
    {
        public Brush TitleBackground { get => Parameters.TitleBackground; set => Parameters.TitleBackground = value; }
        public Brush ButtonsBarBackground { get => Parameters.ButtonsBarBackground; set => Parameters.ButtonsBarBackground = value; }
        public Brush BorderBackground { get => Parameters.BorderBackground; set => Parameters.BorderBackground = value; }
        public Brush ButtonsBackground { get => Parameters.ButtonsBackground; set => Parameters.ButtonsBackground = value; }
    }
}
