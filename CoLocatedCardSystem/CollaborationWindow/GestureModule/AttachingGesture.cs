using CoLocatedCardSystem.CollaborationWindow.InteractionModule;
using CoLocatedCardSystem.CollaborationWindow.Layers.Glow_Layer;
using CoLocatedCardSystem.CollaborationWindow.Layers.Menu_Layer;
using CoLocatedCardSystem.CollaborationWindow.InteractionModule;
using CoLocatedCardSystem.CollaborationWindow.TouchModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.CollaborationWindow.GestureModule
{
    class AttachingGesture:Gesture
    {
        public AttachingGesture(GestureController gtCtrler) : base(gtCtrler)
        {
        }

        /// <summary>
        /// Detect the attaching gesture
        /// </summary>
        /// <param name="touchList">all the active touch points</param>
        /// <param name="targetList">the removed touch points (by release the finger)</param>
        internal override async void Detect(Touch[] touchList, Touch[] targetList)
        {
            base.Detect(touchList, targetList);
            Touch removedTouch = targetList[0];
            if (removedTouch.Sender is DocumentCard
                    && removedTouch.GetStatus() == TOUCH_STATUS.RELEASED)
            {
                //If some other touch on the same card, don't perform the action
                for (int i = 0, size = touchList.Length; i < size; i++)
                {
                    if (touchList[i].Sender.Equals(removedTouch.Sender))
                    {
                        return;
                    }
                }
                DocumentCard card = removedTouch.Sender as DocumentCard;
                CardGroup[] attachedGroups = await gestureController.Controllers.SemanticGroupController.GetAttachedGroups(card.CardID);
                if (card.isConnectAllowed() && attachedGroups != null)
                {
                    gestureController.Controllers.SemanticGroupController.ConnectOneCardWithGroups(card.CardID, attachedGroups);
                }
            }
        }
    }
}
