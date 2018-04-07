using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClinicalOffice.WPF.Dialogs
{
    public static class DialogCommands
    {
        public static RoutedCommand Ok { get; } = new RoutedCommand();
        public static RoutedCommand Cancel { get; } = new RoutedCommand();
        public static RoutedCommand Yes { get; } = new RoutedCommand();
        public static RoutedCommand No { get; } = new RoutedCommand();
    }
}
