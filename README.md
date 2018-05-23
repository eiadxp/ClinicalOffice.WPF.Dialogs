# ClinicalOffice.WPF.Dialogs
A small library to allow developer to create dialogs and show them within WPF ContentControls.
## Using:

 1. Inherits from `DialogBase` class and use the XAML designer to create your dialog, then you can show it using `void DialogBase.ShowDialog(ContentControl parent = null)` method if you do not need the dialog result, or you can use async/await pattern method `Task<DialogResult> DialogBase.ShowDialogAsync(Content parent = null)` method.

 2. Create custom control (like `UserControl`) and show it using one of `DialogHelper.ShowDialog()` methods which return the actual dialog that will hold your control, or one of `DialogHelper.ShowDialogAsync()` methods that use async/await pattern to return dialog result.

## Features:

 - Easy to use in new or current applications. If you already has `UserControl` holding your interface you can directly show it using `DialogHelper` methods.
 - Theme color allows you to give a standard color to your dialog, which is very useful if you are using metro-like styles (`Mahapps.Metro` for example).
 - Customizable title bar and buttons. You can set a style or contents of your buttons (Ok, Cancel, Yes, No, And even the close button), 
 - You can also simulate the buttons action using built in commands in `DialogCommands` class.
 - You can use animation for In/Out (Showing/Closing) of the dialog. Currently we provides three built in animations (Fade, Zoom, Zoom Center), or you can use your own animation.
 - Customizable back ground effects. the default is to blur parent content with a dark overlay and a shadow under the dialog.
 - Built in `MessageBox` functionality using `DialogHelper.ShowMessage, DialogHelper.ShowMessageAsync` methods, with different theme color for each of (Info, Warning, Error , and Question messages).
 - You can set most of the dialog properties for all your dialogs in your `App.xaml` file using styles, or attached properties of `DialogParameters`.
 
 More details on [wiki](https://github.com/eiadxp/ClinicalOffice.WPF.Dialogs/wiki)

## Todo:

 - [ ] Adding some useful events and commands.
 - [ ] Easier implantation for MVVM pattern.

> Written with [StackEdit](https://stackedit.io/).
