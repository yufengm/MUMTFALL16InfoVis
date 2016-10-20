using CoLocatedCardSystem.CollaborationWindow.InteractionModule;
using CoLocatedCardSystem.CollaborationWindow.Layers.Glow_Layer;
using CoLocatedCardSystem.CollaborationWindow.TableModule;
using System.Collections.Generic;

namespace CoLocatedCardSystem.CollaborationWindow.GestureModule
{
    class AttachingListener : GestureListener
    {
        public AttachingListener(GestureListenerController gCtrlers) : base(gCtrlers)
        {

        }
        public override void RegisterGesture(object sender, GestureEventArgs e)
        {
            base.RegisterGesture(sender, e);
            Card card = e.Senders[0] as Card;
            GlowGroup[] groups = e.Senders[1] as GlowGroup[];
            gestureListenerController.Controllers.GlowController.ConnectOneCardWithGroups(card.CardID, groups);
        }
        public override async void TerminateGesture(object sender, GestureEventArgs e)
        {
            base.TerminateGesture(sender, e);
            List<AttributeCard> attrCardList = new List<AttributeCard>();
            if (e.Senders[0] is AttributeCard)
            {
                attrCardList.Add((e.Senders[0] as AttributeCard));
            }
            //GlowGroup[] groups = e.Senders[1] as GlowGroup[];
            //if (groups != null)
            //    foreach (GlowGroup gg in groups)
            //    {
            //        foreach (string cardID in gg.GetCardID())
            //        {
            //            AttributeCard card = gestureListenerController.Controllers.CardController.AttributeCardController.GetCard(cardID);
            //            if (card!=null) {
            //                attrCardList.Add(card);
            //            }
            //        }
            //    }
            //if (attrCardList.Count > 1) {
            //    gestureListenerController.Controllers.CardController.PlotCardController.AddCard(attrCardList.ToArray());
            //}
        }
    }
}
