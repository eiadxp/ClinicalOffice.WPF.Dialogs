using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ClinicalOffice.WPF.Dialogs
{
    public static class DialogMessageBrushes
    {
        static ResourceDictionary dic;
        public static readonly Brush ErrorBrush, InfoBrush, QuestionBrush, WarningBrush;
        static DialogMessageBrushes()
        {
            dic = DialogMessageIcons.dic;
            InfoBrush = (Brush)dic["InfoBrush"];
            QuestionBrush = (Brush)dic["QuestionBrush"];
            WarningBrush = (Brush)dic["WarninBrush"];
            ErrorBrush = (Brush)dic["ErrorBrush"];
        }
    }
}
