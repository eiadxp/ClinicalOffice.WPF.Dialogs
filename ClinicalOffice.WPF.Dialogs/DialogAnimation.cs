namespace ClinicalOffice.WPF.Dialogs
{
    public enum DialogAnimation
    {
        /// <summary>
        /// No animation applied.
        /// </summary>
        None,
        /// <summary>
        /// Use custom animations in <see cref="DialogBase.DialogCustomAnimationIn"/> or <see cref="DialogBase.DialogCustomAnimationOut"/>
        /// </summary>
        Custom,
        /// <summary>
        /// Get animation from <see cref="DialogParameters.DialogCustomAnimationIn"/> or <see cref="DialogParameters.DialogCustomAnimationOut"/>
        /// </summary>
        /// <remarks>
        /// Setting <see cref="DialogParameters.DialogCustomAnimationIn"/> or <see cref="DialogParameters.DialogCustomAnimationOut"/>
        /// to <see cref="DialogAnimation.Global"/> has same effect of <see cref="DialogAnimation.None"/>.
        /// </remarks>
        Global,
        /// <summary>
        /// Fading in and out the dialog.
        /// </summary>
        Fade,
        /// <summary>
        /// Zoom the dialog from and to the upper left corner.
        /// </summary>
        Zoom,
        /// <summary>
        /// Zoom the dialog from and to the center.
        /// </summary>
        ZoomCenter
    }
}
