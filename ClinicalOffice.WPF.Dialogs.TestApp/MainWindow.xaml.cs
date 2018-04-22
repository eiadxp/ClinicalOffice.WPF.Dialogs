﻿using System;
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

        private async void ShowCustomDialog(object sender, RoutedEventArgs e)
        {
            var w = new CustomDialog()
            {
                DialogAnimationIn = DialogAnimation.ZoomCenter,
                DialogAnimationOut = DialogAnimation.Zoom,
                DialogAnimationDuration = new Duration(TimeSpan.FromMilliseconds(500))
            };
            await w.ShowDialogAsync(this);
            MessageBox.Show("This message is called directly after showing the dialog.");
            await DialogHelper.ShowMessageAsync("Test message", "Title test", DialogMessageType.Question, this);
            DialogHelper.ShowWait(Task.Run(() => System.Threading.Thread.Sleep(3000)), null, "Loading", null, DialogButtons.Ok);
        }
    }
}
