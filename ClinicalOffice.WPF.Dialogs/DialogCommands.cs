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
        public static RoutedUICommand Ok { get; } = new RoutedUICommand();
        public static RoutedUICommand Cancel { get; } = new RoutedUICommand();
        public static RoutedUICommand Yes { get; } = new RoutedUICommand();
        public static RoutedUICommand No { get; } = new RoutedUICommand();
        public static RoutedUICommand ReturnKey { get; } = new RoutedUICommand();
        public static RoutedUICommand EscapeKey { get; } = new RoutedUICommand();
    }
}
