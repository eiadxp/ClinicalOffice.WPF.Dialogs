using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace ClinicalOffice.WPF.Dialogs
{
    public class DialogOpenCommand : ICommand
    {
        Type dialogType;
        public Type DialogType
        {
            get => dialogType;
            set
            {
                if (!Object.Equals(value, dialogType))
                {
                    if (value == null) dialogType = value;
                    else
                    {
                        if (!dialogType.IsAssignableFrom(typeof(DialogBase))) throw new InvalidCastException("The type should be derived from DialogBase.");
                        dialogType = value;
                    }
                }
            }
        }
        public DialogBase Dialog { get; set; }
        public ContentControl Parent { get; set; }
        public bool ParameterAsDataContext { get; set; } = true;
        #region ICommand
        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            if (DialogType == null && Dialog == null) throw new NullReferenceException("You should set dialog or dialog type.");
            var w = Dialog ?? (Activator.CreateInstance(DialogType) as DialogBase);
            if (w == null) throw new InvalidOperationException("Can not create dialog.");
            if(ParameterAsDataContext) w.DataContext = parameter;
            w.ShowDialog(Parent);
        }
        #endregion
    }
}
