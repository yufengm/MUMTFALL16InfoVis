using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoLocatedCardSystem.CollaborationWindow.TouchModule;
using CoLocatedCardSystem.CollaborationWindow.InteractionModule;
using CoLocatedCardSystem.CollaborationWindow.Layers.Glow_Layer;

namespace CoLocatedCardSystem.CollaborationWindow.GestureModule
{
    class DeleteCardGesture : Gesture
    {
        public DeleteCardGesture(GestureController gtCtrler) : base(gtCtrler)
        {
        }

        internal override async void Detect(Touch[] touchList, Touch[] targetList)
        {
            base.Detect(touchList, targetList);
            Touch removedTouch = targetList[0];
            if (removedTouch.Sender is Card
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
                var status = await gestureController.Controllers.CardController.GetLiveCardStatus();
                List<string> tobeRemoved = new List<string>();
                foreach (CardStatus cs in status)
                {
                    bool isIntersect = gestureController.Controllers.MenuLayerController.IsIntersectWithDelete(cs);
                    if (isIntersect && !tobeRemoved.Contains(cs.cardID))
                    {
                        tobeRemoved.Add(cs.cardID);
                    }
                }
                foreach (string cardID in tobeRemoved) {
                    gestureController.Controllers.CardController.RemoveActiveCard(cardID);
                }
            }
        }
    }
}
