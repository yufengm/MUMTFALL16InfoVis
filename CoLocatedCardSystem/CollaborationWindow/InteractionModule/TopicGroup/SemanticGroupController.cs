using CoLocatedCardSystem.CollaborationWindow.DocumentModule;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Input;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    class SemanticGroupController
    {
        CentralControllers controllers;
        CardGroupList cardList;//card clusters
        SemanticGroupList semanticList;//semantic groups
        internal static int PREFERRED_CLOUD_SIZE = 30;
        internal SemanticGroupController(CentralControllers ctrls)
        {
            this.controllers = ctrls;
        }

        internal async Task Init()
        {
            cardList = new CardGroupList();
            cardList.Init();
            semanticList = new SemanticGroupList();
            string[] docs = controllers.DocumentController.GetDocumentIDs();
            await semanticList.Init(docs, controllers.MlController);
        }

        internal void Deinit()
        {
            cardList.Deinit();
            semanticList.Deinit();
        }
        /// <summary>
        /// Get all the semantic groups
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<SemanticGroup> GetSemanticGroup()
        {
            return semanticList.GetSemanticGroup();
        }
        internal SemanticGroup GetSemanticGroup(string docID)
        {
            return semanticList.GetSemanticGroupByDoc(docID);
        }
        /// <summary>
        /// Find all groups that intersect with the card
        /// </summary>
        /// <param name="cardID"></param>
        /// <returns></returns>
        internal async Task<CardGroup[]> GetAttachedGroups(string cardID)
        {
            CardGroup[] groups = null;
            CardStatus targetCard = await controllers.CardController.GetLiveCardStatus(cardID);
            if (targetCard == null)
            {
                return null;
            }
            List<CardGroup> tempList = new List<CardGroup>();
            foreach (CardGroup gg in cardList.GetCardGroup().Values)
            {
                if (await IsIntersect(targetCard, gg))
                {
                    tempList.Add(gg);
                }
            }
            groups = tempList.ToArray();
            return groups;
        }

        internal SemanticGroup GetSemanticGroupById(string id)
        {
            return semanticList.GetSemanticGroupById(id);
        }

        /// <summary>
        /// Save the status of the current connected cards
        /// </summary>
        internal async void UpdateCurrentStatus()
        {
            foreach (CardGroup gg in GetGroups().Values)
            {
                if (gg.Count() > 1)
                {
                    var cardIDs = gg.GetCardID();
                    List<string> docIDs = new List<string>();
                    foreach (string id in cardIDs)
                    {
                        Document doc = controllers.CardController.DocumentCardController.GetDocumentCardById(id).Document;
                        if (!docIDs.Contains(doc.DocID))
                        {
                            docIDs.Add(doc.DocID);
                        }

                    }
                    if (docIDs.Count > 1)
                    {
                        await controllers.SemanticGroupController.MergeGroup(docIDs.ToArray());
                        controllers.ConnectionController.UpdateSemanticCloud();
                    }
                }
            }
        }
        /// <summary>
        /// Merge the semantic group
        /// </summary>
        /// <param name="docIDs"></param>
        /// <returns></returns>
        internal async Task MergeGroup(string[] docIDs)
        {
            if (docIDs != null && docIDs.Length > 1)
            {
                //Find the node that contains both docIDs
                SemanticGroup group = semanticList.FindCommonParent(docIDs);
                List<SemanticGroup> sgs = new List<SemanticGroup>();
                foreach (string docID in docIDs)
                {
                    SemanticGroup sg = semanticList.GetSemanticGroupByDoc(docID);
                    if (!sgs.Contains(sg))
                    {
                        sgs.Add(sg);
                    }
                }

                //Get all docs in leaf nodes
                List<string> clusteredDocs = new List<string>();
                foreach (SemanticGroup sg in sgs)
                {
                    if (sg.IsLeaf)
                    {
                        foreach (string docID in sg.GetDocs())
                        {
                            clusteredDocs.Add(docID);
                        }
                    }
                }

                //Find the rest docs
                List<string> restDocs = new List<string>();
                foreach (string docID in group.GetDocs())
                {
                    if (!clusteredDocs.Contains(docID))
                    {
                        restDocs.Add(docID);
                    }
                }
                semanticList.RemoveSemanticGroup(group);

                //Add the common node back
                semanticList.List.TryAdd(group.Id, group);
                if (group.LeftChild != null)
                {
                    group.LeftChild.Deinit();
                }
                if (group.RightChild != null)
                {
                    group.RightChild.Deinit();
                }

                if (restDocs.Count > 0)
                {
                    //Merge the docs into the left node
                    group.LeftChild = new SemanticGroup();
                    ConcurrentDictionary<string, UserActionOnDoc> subGroup = group.GetSubDocList(clusteredDocs);
                    group.LeftChild.AddDoc(subGroup);
                    var topics = await controllers.MlController.GetTopicToken(clusteredDocs.ToArray(), 1);
                    KeyValuePair<Topic, List<string>> pair = topics.ElementAt(0);
                    group.LeftChild.SetTopic(pair.Key);
                    group.LeftChild.Parent = group;
                    semanticList.List.TryAdd(group.LeftChild.Id, group.LeftChild);
                    group.LeftChild.IsLeaf = true;

                    //Merge the rest docs into the right node
                    group.RightChild = new SemanticGroup();
                    subGroup = group.GetSubDocList(restDocs);
                    group.RightChild.AddDoc(subGroup);
                    topics = await controllers.MlController.GetTopicToken(restDocs.ToArray(), 1);
                    pair = topics.ElementAt(0);
                    group.RightChild.SetTopic(pair.Key);
                    group.RightChild.Parent = group;
                    semanticList.List.TryAdd(group.RightChild.Id, group.RightChild);
                    if (restDocs.Count > PREFERRED_CLOUD_SIZE)
                    {
                        await group.RightChild.GenBinaryTree(group.DocList, controllers.MlController, semanticList.List, PREFERRED_CLOUD_SIZE);
                    }
                    else
                    {
                        group.RightChild.IsLeaf = true;
                    }
                }
                else
                {
                    group.IsLeaf = true;
                }
            }
        }

        private async Task<bool> IsIntersect(CardStatus card, CardGroup gg)
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
        /// Get all card groups
        /// </summary>
        /// <returns></returns>
        internal ConcurrentDictionary<string, CardGroup> GetGroups()
        {
            return cardList.GetCardGroup();
        }
        /// <summary>
        /// When one card connect to at least 1 group, 
        /// merge the card and all groups into one group and add to list
        /// </summary>
        /// <param name="cardID"></param>
        /// <param name="groups"></param>
        internal void ConnectOneCardWithGroups(string cardID, CardGroup[] groups)
        {
            //if no groups intersected, create a new group
            if (groups == null || groups.Length == 0)
            {
                CardGroup newgg = new CardGroup();
                newgg.AddCard(cardID);
                cardList.AddCardGroup(newgg);
                return;
            }
            else
            {
                List<string> newCardList = new List<string>();
                //Remove the glow effects from groups
                foreach (CardGroup gg in groups)
                {
                    foreach (string id in gg.GetCardID())
                    {
                        newCardList.Add(id);
                        cardList.RemoveGlow(id);
                        RemoveGlowEffect(id);
                    }
                    cardList.RemoveCardGroup(gg);
                }
                //Add all cards in the previous glow groups to the new group
                CardGroup newgg = new CardGroup();
                newgg.AddCard(cardID);
                AddGlowEffect(cardID, 0);
                foreach (string id in newCardList)
                {
                    newgg.AddCard(id);
                    AddGlowEffect(id, 0);
                }
                cardList.AddCardGroup(newgg);
                UpdateCurrentStatus();
                return;
            }
        }
        /// <summary>
        /// Connect multiple groups
        /// </summary>
        /// <param name="cardID"> The card id that trigger the event</param>
        internal async void ConnectGroupWithGroups(string cardID)
        {
            CardGroup group = cardList.GeCardGroup(cardID);
            List<CardGroup> tempList = new List<CardGroup>();
            cardList.RemoveCardGroup(group);
            tempList.Add(group);
            foreach (string id in group.GetCardID())
            {
                CardGroup[] groups = await GetAttachedGroups(id);
                if (groups != null)
                    foreach (CardGroup gg in groups)
                    {
                        tempList.Add(gg);
                        cardList.RemoveCardGroup(gg);
                    }
            }
            CardGroup newgg = new CardGroup();
            foreach (CardGroup gg in tempList)
            {
                foreach (string c in gg.GetCardID())
                {
                    RemoveGlowEffect(c);
                    newgg.AddCard(c);
                }
            }
            foreach (string c in newgg.GetCardID())
            {
                AddGlowEffect(c, 0);
            }
            cardList.AddCardGroup(newgg);
            UpdateCurrentStatus();
        }
        /// <summary>
        /// Update one card when point down
        /// </summary>
        /// <param name="cardID"></param>
        internal async void DisconnectOneCardWithGroups(string cardID)
        {
            //Find the group that contains this card
            CardGroup currentGroup = cardList.GeCardGroup(cardID);
            int colorIndex = 0;
            Glow glow = cardList.GetGlow(cardID);
            if (glow != null)
            {
                colorIndex = glow.ColorIndex;
            }

            //If a group contains the card, remove the card from the group
            //If the group is empty, remove the group from the list
            if (currentGroup != null)
            {
                currentGroup.RemoveCard(cardID);
                RemoveGlowEffect(cardID);
                CardGroup[] groups = await GetGroupsFromCards(currentGroup.GetCardID());
                foreach (string id in currentGroup.GetCardID())
                {
                    RemoveGlowEffect(id);
                }
                cardList.RemoveCardGroup(currentGroup);
                foreach (CardGroup gg in groups)
                {
                    cardList.AddCardGroup(gg);
                    var ids = gg.GetCardID();
                    if (ids.Length > 1)
                    {
                        foreach (string id in ids)
                        {
                            AddGlowEffect(id, colorIndex);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Set the search result. Set the UserActionOnDoc
        /// </summary>
        /// <param name="docIDs"></param>
        /// <param name="owner"></param>
        /// <param name="searched"></param>
        internal void SetSearchResult(string[] docIDs, User owner, bool searched)
        {
            semanticList.SetSearchResult(docIDs, owner, searched);
        }
        /// <summary>
        /// Set the active result. Set the UserActionOnDoc
        /// </summary>
        /// <param name="docIDs"></param>
        /// <param name="owner"></param>
        /// <param name="searched"></param>
        internal void SetActiveCard(string[] docIDs, User owner, bool searched)
        {
            semanticList.SetActiveResult(docIDs, owner, searched);
        }
        /// <summary>
        /// Set the touch result. Set the UserActionOnDoc
        /// </summary>
        /// <param name="documentCard"></param>
        /// <param name="value"></param>
        internal void SetTouchedCard(DocumentCard documentCard, bool value)
        {
            semanticList.SetTouchResult(documentCard.Document.DocID, documentCard.Owner, value);
        }
        /// <summary>
        /// Based on the connection of the cards, create different groups
        /// </summary>
        /// <param name="cardIDs"></param>
        /// <returns></returns>
        private async Task<CardGroup[]> GetGroupsFromCards(IEnumerable<string> cardIDs)
        {
            List<String> cardList = new List<string>();
            foreach (String card in cardIDs)
            {
                cardList.Add(card);
            }
            List<CardGroup> groups = new List<CardGroup>();
            while (cardList.Count > 0)
            {
                String cardID = cardList[0];
                cardList.Remove(cardID);
                CardGroup newgg = new CardGroup();
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
        private async Task GetConnectedCards(string card, List<string> cards, CardGroup group)
        {
            if (cards.Count == 0)
            {
                return;
            }
            CardStatus status1 = await controllers.CardController.GetLiveCardStatus(card);
            List<string> tempList = new List<string>();
            foreach (string c in cards)
            {
                CardStatus status2 = await controllers.CardController.GetLiveCardStatus(c);
                if (Coordination.IsIntersect(status1.corners, status2.corners))
                {
                    tempList.Add(c);
                }
            }
            if (tempList.Count == 0)
            {
                return;
            }
            else
            {
                foreach (string s in tempList)
                {
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
            cardList.AddGlow(cardID, glow);
        }
        /// <summary>
        /// Remove the glow effect from the glow layer
        /// </summary>
        /// <param name="cardID"></param>
        internal void RemoveGlowEffect(string cardID)
        {
            Glow glow = cardList.RemoveGlow(cardID);
            controllers.GlowLayerController.RemoveGlowEffect(cardID);
        }
        /// <summary>
        /// If one color changed, update other connected glow color.
        /// </summary>
        /// <param name="colorIndex"></param>
        internal void UpdateConnectedColor(string cardID, int colorIndex)
        {
            foreach (CardGroup group in cardList.GetCardGroup().Values)
            {
                if (group.HasCard(cardID))
                {
                    foreach (string id in group.GetCardID())
                    {
                        Glow glow = cardList.GetGlow(id);
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
            CardGroup group = cardList.GeCardGroup(cardID);
            if (group != null)
                foreach (string id in group.GetCardID())
                {
                    Glow glow = cardList.GetGlow(id);
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
