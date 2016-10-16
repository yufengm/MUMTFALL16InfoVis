using CoLocatedCardSystem.CollaborationWindow.InteractionModule;
using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace CoLocatedCardSystem.CollaborationWindow.GestureModule
{
    class SortingListener : GestureListener
    {
        Point vector = new Point();
        public SortingListener(GestureListenerController gCtrlers) : base(gCtrlers)
        {

        }
        public override void RegisterGesture(object sender, GestureEventArgs e)
        {
            base.RegisterGesture(sender, e);
            vector = new Point(e.Touches[0].StartPoint.X - e.Touches[0].CurrentGlobalPoint.X,
                e.Touches[0].StartPoint.Y - e.Touches[0].CurrentGlobalPoint.Y);
        }
        public override void ContinueGesture(object sender, GestureEventArgs e)
        {
            base.ContinueGesture(sender, e);
            Card card = (Card)e.Senders[0];
        }
        public override void TerminateGesture(object sender, GestureEventArgs e)
        {
            base.TerminateGesture(sender, e);
            Card card = (Card)e.Senders[0];
            SortingBox box = (SortingBox)e.Senders[1];
            double dist = 50;
            double vLength = Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            vector.X = vector.X * dist / vLength;
            vector.Y = vector.Y * dist / vLength;
            card.MoveBy(vector);
            this.gestureListenerController.Controllers.SortingBoxController.AddCardToSortingBox(card, box);
        }
    }
}
