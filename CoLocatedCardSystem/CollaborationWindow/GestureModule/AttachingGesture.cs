using CoLocatedCardSystem.CollaborationWindow.InteractionModule;
using CoLocatedCardSystem.CollaborationWindow.Layers.Glow_Layer;
using CoLocatedCardSystem.CollaborationWindow.Layers.Menu_Layer;
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
        /// Detect the sorting gesture (Drag and drop the card to the sorting box). 
        /// The touches in the sorting gesture will be removed from the touchList
        /// </summary>
        /// <param name="touchList"></param>
        /// <returns></returns>
        internal override async void Detect(Touch[] touchList, Touch[] targetList)
        {
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
                GlowGroup[] attachedGroups = await gestureController.Controllers.GlowController.GetAttachedGroups(card.CardID);
                if (card.isConnectAllowed() && attachedGroups != null)
                {
                    gestureController.Controllers.GlowController.ConnectOneCardWithGroups(card.CardID, attachedGroups);
                }
            }
        }
    }
}
