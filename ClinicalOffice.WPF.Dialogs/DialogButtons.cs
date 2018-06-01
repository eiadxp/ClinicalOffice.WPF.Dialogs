namespace ClinicalOffice.WPF.Dialogs
{
    public enum DialogButtons
    {
        None,
        Ok,
        OkCancel,
        YesNo,
        YesNoCancel,
        NoClose = 8,
        None_NoClose = None | NoClose,
        Ok_NoClose = Ok | NoClose,
        OkCancel_NoClose = OkCancel | NoClose,
        YesNo_NoClose = YesNo | NoClose,
        YesNoCancel_NoClose = YesNoCancel | NoClose
    }
}
