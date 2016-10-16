using CoLocatedCardSystem.CollaborationWindow.InteractionModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.CollaborationWindow.GestureModule
{
    class AttachingListener : GestureListener
    {
        public AttachingListener(GestureListenerController gCtrlers) : base(gCtrlers)
        {

        }
        public override void ContinueGesture(object sender, GestureEventArgs e)
        {
            base.ContinueGesture(sender, e);
        }
        public override void TerminateGesture(object sender, GestureEventArgs e)
        {
            base.TerminateGesture(sender, e);
            Card card = (Card)e.Senders[0];
            GlowGroup[] groupGroup = (GlowGroup[])e.Senders[1];
            System.Diagnostics.Debug.WriteLine("Intersect");
            //controllers.GlowController.AddGroup(card.CardID);
            //int colorIndex = 0;
            //if (gg.Count() == 1)
            //{
            //    AddGlowEffect(gg.GetCardID()[0], colorIndex);
            //}
            //else
            //{
            //    Glow glow = list.GetGlow(intersectedCard.CardID);
            //    colorIndex = glow.ColorIndex;
            //}
            //gg.AddCard(cardID);
            //AddGlowEffect(cardID, colorIndex);
        }
    }
}
