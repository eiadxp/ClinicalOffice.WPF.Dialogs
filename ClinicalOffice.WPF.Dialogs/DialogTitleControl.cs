﻿using System.Windows;
using System.Windows.Controls;

namespace ClinicalOffice.WPF.Dialogs
{
    public class DialogTitleControl : UserControl
    {
        static DialogTitleControl()
        {
            DefaultStyleKeyProperty.
                OverrideMetadata(typeof(DialogTitleControl), new FrameworkPropertyMetadata(typeof(DialogTitleControl)));
        }
    }
}