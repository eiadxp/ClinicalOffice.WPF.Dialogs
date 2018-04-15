using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ClinicalOffice.WPF.Dialogs
{
    static class DialogAnimationHelper
    {
        public static void CreateInAnimation(this DialogBase dialog, Action completeAction = null)
        {
            var type = dialog.DialogAnimationIn;
            if (type == DialogAnimation.Global) type = DialogParameters.DialogAnimationIn;
            if (type == DialogAnimation.Global) type = DialogAnimation.None;
            var duration = dialog.DialogAnimationDuration;
            if (duration == Duration.Automatic) duration = DialogParameters.DialogAnimationDuration;
            if (duration == Duration.Automatic) duration = new Duration(TimeSpan.FromMilliseconds(300));
            switch (type)
            {
                case DialogAnimation.Fade:
                    InFade(dialog, completeAction, duration);
                    break;
                case DialogAnimation.Zoom:
                    InZoom(dialog, completeAction, duration);
                    break;
                case DialogAnimation.ZoomCenter:
                    InZoomCenter(dialog, completeAction, duration);
                    break;
                case DialogAnimation.None:
                case DialogAnimation.Custom:
                default:
                    completeAction?.Invoke();
                    break;
            }
        }
        public static void CreateOutAnimation(this DialogBase dialog, Action completeAction = null)
        {
            var type = dialog.DialogAnimationOut;
            if (type == DialogAnimation.Global) type = DialogParameters.DialogAnimationOut;
            if (type == DialogAnimation.Global) type = DialogAnimation.None;
            var duration = dialog.DialogAnimationDuration;
            if (duration == Duration.Automatic) duration = DialogParameters.DialogAnimationDuration;
            if (duration == Duration.Automatic) duration = new Duration(TimeSpan.FromMilliseconds(300));
            switch (type)
            {
                case DialogAnimation.Fade:
                    OutFade(dialog, completeAction, duration);
                    break;
                case DialogAnimation.Zoom:
                    OutZoom(dialog, completeAction, duration);
                    break;
                case DialogAnimation.ZoomCenter:
                    OutZoomCenter(dialog, completeAction, duration);
                    break;
                case DialogAnimation.Custom:
                case DialogAnimation.None:
                default:
                    completeAction?.Invoke();
                    break;
            }
        }
        static AnimationClock CreateClock(DialogBase dialog, AnimationTimeline animation, Action completeAction)
        {
            var clock = animation.CreateClock();
            var oldTransform = dialog.RenderTransform;
            var oldOrigin = dialog.RenderTransformOrigin;
            var oldOpacity = dialog.Opacity;
            clock.Completed += (a, b) =>
            {
                dialog.Opacity = oldOpacity;
                dialog.RenderTransform = oldTransform;
                dialog.RenderTransformOrigin = oldOrigin;
                completeAction?.Invoke();
            };
            return clock;
        }

        static void InFade(DialogBase dialog, Action completeAction, Duration duration)
        {
            var fade = new DoubleAnimation() { From = 0, To = 1, Duration = duration };
            var fadeClock = CreateClock(dialog, fade, completeAction);
            dialog.ApplyAnimationClock(DialogBase.OpacityProperty, fadeClock);
        }
        static void OutFade(DialogBase dialog, Action completeAction, Duration duration)
        {
            var fade = new DoubleAnimation() { From = 1, To = 0, Duration = duration };
            var fadeClock = CreateClock(dialog, fade, completeAction);
            dialog.ApplyAnimationClock(DialogBase.OpacityProperty, fadeClock);
        }

        static void InZoom(DialogBase dialog, Action completeAction, Duration duration)
        {
            var zoom = new DoubleAnimation() { From = 0, To = 1, Duration = duration };
            var zoomClock = CreateClock(dialog, zoom, completeAction);
            var trans = new ScaleTransform();
            dialog.RenderTransform = trans;
            trans.ApplyAnimationClock(ScaleTransform.ScaleXProperty, zoomClock);
            trans.ApplyAnimationClock(ScaleTransform.ScaleYProperty, zoomClock);
        }
        static void OutZoom(DialogBase dialog, Action completeAction, Duration duration)
        {
            var zoom = new DoubleAnimation() { From = 1, To = 0, Duration = duration };
            var zoomClock = CreateClock(dialog, zoom, completeAction);
            var trans = new ScaleTransform();
            dialog.RenderTransform = trans;
            trans.ApplyAnimationClock(ScaleTransform.ScaleXProperty, zoomClock);
            trans.ApplyAnimationClock(ScaleTransform.ScaleYProperty, zoomClock);
        }

        static void InZoomCenter(DialogBase dialog, Action completeAction, Duration duration)
        {
            var zoom = new DoubleAnimation() { From = 0, To = 1, Duration = duration };
            var zoomClock = CreateClock(dialog, zoom, completeAction);
            var trans = new ScaleTransform();
            dialog.RenderTransform = trans;
            dialog.RenderTransformOrigin = new Point(.5, .5);
            trans.ApplyAnimationClock(ScaleTransform.ScaleXProperty, zoomClock);
            trans.ApplyAnimationClock(ScaleTransform.ScaleYProperty, zoomClock);
        }
        static void OutZoomCenter(DialogBase dialog, Action completeAction, Duration duration)
        {
            var zoom = new DoubleAnimation() { From = 1, To = 0, Duration = duration };
            var zoomClock = CreateClock(dialog, zoom, completeAction);
            var trans = new ScaleTransform();
            dialog.RenderTransform = trans;
            dialog.RenderTransformOrigin = new Point(.5, .5);
            trans.ApplyAnimationClock(ScaleTransform.ScaleXProperty, zoomClock);
            trans.ApplyAnimationClock(ScaleTransform.ScaleYProperty, zoomClock);
        }
    }
}
