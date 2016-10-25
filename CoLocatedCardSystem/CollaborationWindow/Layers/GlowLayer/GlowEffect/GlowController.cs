using CoLocatedCardSystem.CollaborationWindow.InteractionModule;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Input;

namespace CoLocatedCardSystem.CollaborationWindow.Layers.Glow_Layer
{
    class GlowController
    {
        CentralControllers controllers;
        GlowList list;
        internal GlowController(CentralControllers ctrls)
        {
            this.controllers = ctrls;
        }

        internal void Init()
        {
            list = new GlowList();
            list.Init();
        }

        internal void Deinit()
        {
            list.Deinit();
        }
        /// <summary>
        /// Find all groups that intersect with the card
        /// </summary>
        /// <param name="cardID"></param>
        /// <returns></returns>
        internal async Task<GlowGroup[]> GetAttachedGroups(string cardID)
        {
            GlowGroup[] groups = null;
            CardStatus targetCard = await controllers.CardController.GetLiveCardStatus(cardID);
            if (targetCard == null)
            {
                return null;
            }
            List<GlowGroup> tempList = new List<GlowGroup>();
            foreach (GlowGroup gg in list.GetGroup())
            {
                if (await IsIntersect(targetCard, gg))
                {
                    tempList.Add(gg);
                }
            }
            groups = tempList.ToArray();
            return groups;
        }
        private async Task<bool> IsIntersect(CardStatus card, GlowGroup gg)
        {
            if (!gg.HasCard(card.cardID))
            {
                CardStatus intersectedCard = null;
                foreach (string id in gg.GetCardID())
                {
                    intersectedCard = await controllers.CardController.GetLiveCardStatus(id);
                    if (Coordination.IsIntersect(intersectedCard.corners, card.corners))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// When one card connect to at least 1 group, 
        /// merge the card and all groups into one group and add to list
        /// </summary>
        /// <param name="cardID"></param>
        /// <param name="groups"></param>
        internal void ConnectOneCardWithGroups(string cardID, GlowGroup[] groups)
        {
            //if no groups intersected, create a new group
            if (groups == null || groups.Length == 0)
            {
                GlowGroup newgg = new GlowGroup();
                newgg.AddCard(cardID);
                list.AddGlowGroup(newgg);
                controllers.ConnectionController.UpdateViz(null, new GlowGroup[] { newgg });
                return;
            }
            else
            {
                List<string> newCardList = new List<string>();
                //Remove the glow effects from groups
                foreach (GlowGroup gg in groups)
                {
                    foreach (string id in gg.GetCardID())
                    {
                        newCardList.Add(id);
                        list.RemoveGlow(id);
                        RemoveGlowEffect(id);
                    }
                    list.RemoveGlowGroup(gg);
                }
                //Add all cards in the previous glow groups to the new group
                GlowGroup newgg = new GlowGroup();
                newgg.AddCard(cardID);
                AddGlowEffect(cardID, 0);
                foreach (string id in newCardList)
                {
                    newgg.AddCard(id);
                    AddGlowEffect(id, 0);
                }
                list.AddGlowGroup(newgg);
                controllers.ConnectionController.UpdateViz(groups, new GlowGroup[] { newgg });
                return;
            }
        }
        /// <summary>
        /// Connect multiple groups
        /// </summary>
        /// <param name="cardID"> The card id that trigger the event</param>
        internal async void ConnectGroupWithGroups(string cardID)
        {
            GlowGroup group = list.GetGroup(cardID);
            List<GlowGroup> tempList = new List<GlowGroup>();
            list.RemoveGlowGroup(group);
            tempList.Add(group);
            foreach (string id in group.GetCardID())
            {
                GlowGroup[] groups = await GetAttachedGroups(id);
                if (groups != null)
                    foreach (GlowGroup gg in groups)
                    {
                        tempList.Add(gg);
                        list.RemoveGlowGroup(gg);
                    }
            }
            GlowGroup newgg = new GlowGroup();
            foreach (GlowGroup gg in tempList) {
                foreach (string c in gg.GetCardID()) {
                    RemoveGlowEffect(c);
                    newgg.AddCard(c);
                }
            }
            foreach (string c in newgg.GetCardID()) {
                AddGlowEffect(c, 0);
            }
            list.AddGlowGroup(newgg);
            controllers.ConnectionController.UpdateViz(tempList.ToArray(), new GlowGroup[] {newgg});
        }
        /// <summary>
        /// Update one card when point down
        /// </summary>
        /// <param name="cardID"></param>
        internal async void DisconnectOneCardWithGroups(string cardID)
        {
            //Find the group that contains this card
            GlowGroup currentGroup = list.GetGroup(cardID);
            int colorIndex = 0;
            Glow glow = list.GetGlow(cardID);
            if (glow != null) {
                colorIndex = glow.ColorIndex;
            }
            
            //If a group contains the card, remove the card from the group
            //If the group is empty, remove the group from the list
            if (currentGroup != null)
            {
                currentGroup.RemoveCard(cardID);
                RemoveGlowEffect(cardID);
                GlowGroup[] groups = await GetGroupsFromCards(currentGroup.GetCardID());
                foreach (string id in currentGroup.GetCardID())
                {
                    RemoveGlowEffect(id);
                }
                list.RemoveGlowGroup(currentGroup);
                foreach (GlowGroup gg in groups)
                {
                    list.AddGlowGroup(gg);
                    var ids = gg.GetCardID();
                    if (ids.Count > 1)
                    {
                        foreach (string id in ids)
                        {
                            AddGlowEffect(id, colorIndex);
                        }
                    }
                }
                currentGroup.AddCard(cardID);
                controllers.ConnectionController.UpdateViz(new GlowGroup[] { currentGroup }, groups);
            }
        }
        /// <summary>
        /// Based on the connection of the cards, create different groups
        /// </summary>
        /// <param name="cardIDs"></param>
        /// <returns></returns>
        private async Task<GlowGroup[]> GetGroupsFromCards(IEnumerable<string> cardIDs)
        {
            List<String> cardList = new List<string>();
            foreach (String card in cardIDs)
            {
                cardList.Add(card);
            }
            List<GlowGroup> groups = new List<GlowGroup>();
            while (cardList.Count > 0)
            {
                String cardID = cardList[0];
                cardList.Remove(cardID);
                GlowGroup newgg = new GlowGroup();
                newgg.AddCard(cardID);
                //Recursion
                await GetConnectedCards(cardID, cardList, newgg);
                groups.Add(newgg);
            }
            return groups.ToArray();
        }

        private async Task GetConnectedCards(string card, List<string> cards, GlowGroup group)
        {
            if (cards.Count == 0)
            {
                return;
            }
            CardStatus status1 = await controllers.CardController.GetLiveCardStatus(card);
            List<string> tempList = new List<string>();
            foreach (string c in cards) {
                CardStatus status2= await controllers.CardController.GetLiveCardStatus(c);
                if (Coordination.IsIntersect(status1.corners, status2.corners))
                {
                    tempList.Add(c);
                }
            }
            if (tempList.Count == 0)
            {
                return;
            }
            else {
                foreach (string s in tempList) {
                    group.AddCard(s);
                    cards.Remove(s);
                }
                foreach (string s in tempList)
                {
                    await GetConnectedCards(s, cards, group);
                }
            }
        }
        /// <summary>
        /// Add the glow to the glow layer
        /// </summary>
        /// <param name="cardID"></param>
        /// <param name="colorIndex"></param>
        private async void AddGlowEffect(string cardID, int colorIndex)
        {
            CardStatus cardStatus = await controllers.CardController.GetLiveCardStatus(cardID);
            Glow glow = await controllers.GlowLayerController.AddGlow(cardStatus, colorIndex, this);
            list.AddGlow(cardID, glow);
        }
        /// <summary>
        /// Remove the glow effect from the glow layer
        /// </summary>
        /// <param name="cardID"></param>
        internal void RemoveGlowEffect(string cardID)
        {
            Glow glow = list.RemoveGlow(cardID);
            controllers.GlowLayerController.RemoveGlowEffect(cardID);
        }
        /// <summary>
        /// If one color changed, update other connected glow color.
        /// </summary>
        /// <param name="colorIndex"></param>
        internal void UpdateConnectedColor(string cardID, int colorIndex)
        {
            foreach (GlowGroup group in list.GetGroup())
            {
                if (group.HasCard(cardID))
                {
                    foreach (string id in group.GetCardID())
                    {
                        Glow glow = list.GetGlow(id);
                        glow.ColorIndex = colorIndex;
                    }
                }
            }
        }
        /// <summary>
        /// Update the positions of all the connected cards
        /// </summary>
        /// <param name="cardID">the id of the touched cards</param>
        /// <param name="vector">the vector of the manipulation</param>
        internal void UpdateConnectedPosition(string cardID, Point vector)
        {
            GlowGroup group = list.GetGroup(cardID);
            if (group != null)
                foreach (string id in group.GetCardID())
                {
                    Glow glow = list.GetGlow(id);
                    if (glow != null)
                    {
                        glow.MoveBy(vector);
                        controllers.CardController.MoveCardByVector(id, vector);
                    }
                }
        }
        /// <summary>
        /// Create a touch and pass it to the interaction controller.
        /// </summary>
        /// <param name="p"></param>
        internal void PointerDown(PointerPoint localPoint, PointerPoint globalPoint, Glow glow, Type type)
        {
            controllers.TouchController.TouchDown(localPoint, globalPoint, glow, type);
        }

        /// <summary>
        /// Update the touch point
        /// </summary>
        /// <param name="p"></param>
        internal void PointerMove(PointerPoint localPoint, PointerPoint globalPoint)
        {
            controllers.TouchController.TouchMove(localPoint, globalPoint);
        }
        /// <summary>
        /// Lift the touch layer
        /// </summary>
        /// <param name="p"></param>
        internal void PointerUp(PointerPoint localPoint, PointerPoint globalPoint)
        {
            controllers.TouchController.TouchUp(localPoint, globalPoint);
        }
    }
}
