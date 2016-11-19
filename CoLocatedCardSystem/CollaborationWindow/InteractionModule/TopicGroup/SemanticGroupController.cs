﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Input;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    class SemanticGroupController
    {
        CentralControllers controllers;
        SemanticGroupList list;
        internal SemanticGroupController(CentralControllers ctrls)
        {
            this.controllers = ctrls;
        }

        internal void Init()
        {
            list = new SemanticGroupList();
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
        internal async Task<SemanticGroup[]> GetAttachedGroups(string cardID)
        {
            SemanticGroup[] groups = null;
            CardStatus targetCard = await controllers.CardController.GetLiveCardStatus(cardID);
            if (targetCard == null)
            {
                return null;
            }
            List<SemanticGroup> tempList = new List<SemanticGroup>();
            foreach (SemanticGroup gg in list.GetSemanticGroup().Values)
            {
                if (await IsIntersect(targetCard, gg))
                {
                    tempList.Add(gg);
                }
            }
            groups = tempList.ToArray();
            return groups;
        }
        private async Task<bool> IsIntersect(CardStatus card, SemanticGroup gg)
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
        /// Get all groups
        /// </summary>
        /// <returns></returns>
        internal ConcurrentDictionary<string, SemanticGroup> GetGroups()
        {
            return list.GetSemanticGroup();
        }
        /// <summary>
        /// When one card connect to at least 1 group, 
        /// merge the card and all groups into one group and add to list
        /// </summary>
        /// <param name="cardID"></param>
        /// <param name="groups"></param>
        internal void ConnectOneCardWithGroups(string cardID, SemanticGroup[] groups)
        {
            //if no groups intersected, create a new group
            if (groups == null || groups.Length == 0)
            {
                SemanticGroup newgg = new SemanticGroup();
                newgg.AddCard(cardID);
                list.AddSemanticGroup(newgg);
                controllers.ConnectionController.UpdateCurrentStatus();
                return;
            }
            else
            {
                List<string> newCardList = new List<string>();
                //Remove the glow effects from groups
                foreach (SemanticGroup gg in groups)
                {
                    foreach (string id in gg.GetCardID())
                    {
                        newCardList.Add(id);
                        list.RemoveGlow(id);
                        RemoveGlowEffect(id);
                    }
                    list.RemoveSemanticGroup(gg);
                }
                //Add all cards in the previous glow groups to the new group
                SemanticGroup newgg = new SemanticGroup();
                newgg.AddCard(cardID);
                AddGlowEffect(cardID, 0);
                foreach (string id in newCardList)
                {
                    newgg.AddCard(id);
                    AddGlowEffect(id, 0);
                }
                list.AddSemanticGroup(newgg);
                controllers.ConnectionController.UpdateCurrentStatus();
                return;
            }
        }
        /// <summary>
        /// Connect multiple groups
        /// </summary>
        /// <param name="cardID"> The card id that trigger the event</param>
        internal async void ConnectGroupWithGroups(string cardID)
        {
            SemanticGroup group = list.GetSemanticGroup(cardID);
            List<SemanticGroup> tempList = new List<SemanticGroup>();
            list.RemoveSemanticGroup(group);
            tempList.Add(group);
            foreach (string id in group.GetCardID())
            {
                SemanticGroup[] groups = await GetAttachedGroups(id);
                if (groups != null)
                    foreach (SemanticGroup gg in groups)
                    {
                        tempList.Add(gg);
                        list.RemoveSemanticGroup(gg);
                    }
            }
            SemanticGroup newgg = new SemanticGroup();
            foreach (SemanticGroup gg in tempList) {
                foreach (string c in gg.GetCardID()) {
                    RemoveGlowEffect(c);
                    newgg.AddCard(c);
                }
            }
            foreach (string c in newgg.GetCardID()) {
                AddGlowEffect(c, 0);
            }
            list.AddSemanticGroup(newgg);
            controllers.ConnectionController.UpdateCurrentStatus();
        }
        /// <summary>
        /// Update one card when point down
        /// </summary>
        /// <param name="cardID"></param>
        internal async void DisconnectOneCardWithGroups(string cardID)
        {
            //Find the group that contains this card
            SemanticGroup currentGroup = list.GetSemanticGroup(cardID);
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
                SemanticGroup[] groups = await GetGroupsFromCards(currentGroup.GetCardID());
                foreach (string id in currentGroup.GetCardID())
                {
                    RemoveGlowEffect(id);
                }
                list.RemoveSemanticGroup(currentGroup);
                foreach (SemanticGroup gg in groups)
                {
                    list.AddSemanticGroup(gg);
                    var ids = gg.GetCardID();
                    if (ids.Length > 1)
                    {
                        foreach (string id in ids)
                        {
                            AddGlowEffect(id, colorIndex);
                        }
                    }
                }
                controllers.ConnectionController.UpdateCurrentStatus();
            }
        }
        /// <summary>
        /// Based on the connection of the cards, create different groups
        /// </summary>
        /// <param name="cardIDs"></param>
        /// <returns></returns>
        private async Task<SemanticGroup[]> GetGroupsFromCards(IEnumerable<string> cardIDs)
        {
            List<String> cardList = new List<string>();
            foreach (String card in cardIDs)
            {
                cardList.Add(card);
            }
            List<SemanticGroup> groups = new List<SemanticGroup>();
            while (cardList.Count > 0)
            {
                String cardID = cardList[0];
                cardList.Remove(cardID);
                SemanticGroup newgg = new SemanticGroup();
                newgg.AddCard(cardID);
                //Recursion
                await GetConnectedCards(cardID, cardList, newgg);
                groups.Add(newgg);
            }
            return groups.ToArray();
        }
        /// <summary>
        /// Recursive method to put a list of cards into different group
        /// </summary>
        /// <param name="card"></param>
        /// <param name="cards"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        private async Task GetConnectedCards(string card, List<string> cards, SemanticGroup group)
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
            foreach (SemanticGroup group in list.GetSemanticGroup().Values)
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
            SemanticGroup group = list.GetSemanticGroup(cardID);
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