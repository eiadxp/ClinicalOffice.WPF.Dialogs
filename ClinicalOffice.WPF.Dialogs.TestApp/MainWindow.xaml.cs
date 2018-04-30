using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClinicalOffice.WPF.Dialogs.TestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShowCustomDialog(object sender, RoutedEventArgs e)
        {
            (new CustomDialog()).ShowDialog(this);
            MessageBox.Show("This message is called directly after showing the dialog.");
        }
        private async void ShowCustomDialogAsync(object sender, RoutedEventArgs e)
        {
            var result = await(new CustomDialog()).ShowDialogAsync(this);
            MessageBox.Show("This message is called directly after showing the dialog.\nDialog result is:\n\n" + result.ToString());
        }
        private void ShowUserControlDialog(object sender, RoutedEventArgs e)
        {
            DialogHelper.ShowDialog<CustomControl>("User Control Dialog", this);
            MessageBox.Show("This message is called directly after showing the dialog.");
        }
        private async void ShowUserControlDialogAsync(object sender, RoutedEventArgs e)
        {
            var result = await DialogHelper.ShowDialogAsync<CustomControl>("User Control Dialog", this);
            MessageBox.Show("This message is called directly after showing the dialog.\nDialog result is:\n\n" + result.ToString());
        }

        private async void ShowInfoMessage(object sender, RoutedEventArgs e)
        {
            await DialogHelper.ShowMessageAsync("Info message default style.", "Info test", DialogMessageType.Info, this);
        }
        private async void ShowWarningMessage(object sender, RoutedEventArgs e)
        {
            await DialogHelper.ShowMessageAsync("Warning message default style.", "Title test", DialogMessageType.Warning, this);
        }
        private async void ShowQuestionMessage(object sender, RoutedEventArgs e)
        {
            await DialogHelper.ShowMessageAsync("Question message default style.", "Title test", DialogMessageType.Question, this);
        }
        private async void ShowErrorMessage(object sender, RoutedEventArgs e)
        {
            await DialogHelper.ShowMessageAsync("Error message default style.", "Title test", DialogMessageType.Error, this);
        }

        private void ShowWaitMessage(object sender, RoutedEventArgs e)
        {
            DialogHelper.ShowWait(Task.Run(() => System.Threading.Thread.Sleep(10000)),
                null,
                "Waiting...", "This message will close automatically when underlaying task finish (10 seconds).", null, DialogButtons.Ok);
        }
    }
}
