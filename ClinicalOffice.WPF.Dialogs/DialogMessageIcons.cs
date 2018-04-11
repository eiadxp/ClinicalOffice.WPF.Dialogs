using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace ClinicalOffice.WPF.Dialogs
{
    public static class DialogMessageIcons
    {
        internal static ResourceDictionary dic;
        public static readonly UIElement Error, Info, Question, Wrning;

        static DialogMessageIcons()
        {
            dic = XamlReader.Load(Application.GetResourceStream(new Uri("Geometery.xaml", UriKind.Relative)).Stream) as ResourceDictionary;
            Error = (UIElement)dic["ErrorIcon"];
            Info = (UIElement)dic["InfoIcon"];
            Question = (UIElement)dic["QuestionIcon"];
            Wrning = (UIElement)dic["WarningIcon"];
        }
    }
}
