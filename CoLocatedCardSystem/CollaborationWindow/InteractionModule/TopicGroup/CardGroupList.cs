using CoLocatedCardSystem.CollaborationWindow.Layers.Glow_Layer;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    /// <summary>
    /// Card glows and list of glow effect
    /// </summary>
    class CardGroupList
    {
        Dictionary<string, Glow> glowEffectList;// A list of the glow objects. key: card id, value Glow object
        ConcurrentDictionary<string, CardGroup> cardGroups;//Save info of which cards are connected. key: group id, value: semantic group
        SemanticGroupController semanticGroupController;
        internal CardGroupList(SemanticGroupController ctrls) {
            semanticGroupController = ctrls;
        }
        internal void Init() {
            glowEffectList = new Dictionary<string, Glow>();         
            cardGroups = new ConcurrentDictionary<string, CardGroup>();           
        }

        internal void Deinit() {
            glowEffectList.Clear();
            cardGroups.Clear();
        }
        /// <summary>
        /// Add a glow object to the list
        /// </summary>
        /// <param name="cardID"></param>
        /// <param name="glow"></param>
        internal void AddGlow(string cardID, Glow glow)
        {
            if (!glowEffectList.Keys.Contains(cardID))
            {
                glowEffectList.Add(cardID, glow);
            }
        }
        /// <summary>
        /// Remove a glow object
        /// </summary>
        /// <param name="cardID"></param>
        internal Glow RemoveGlow(string cardID)
        {
            Glow result = null;
            if (glowEffectList.Keys.Contains(cardID))
            {
                result = glowEffectList[cardID];
                glowEffectList.Remove(cardID);
            }
            return result;
        }

        /// <summary>
        /// Get the glow by id
        /// </summary>
        /// <param name="cardID"></param>
        /// <returns></returns>
        internal Glow GetGlow(string cardID)
        {
            if (glowEffectList.Keys.Contains(cardID))
                return glowEffectList[cardID];
            else
                return null;
        }

        /// <summary>
        /// Delete a glow group. return the removed ids.
        /// </summary>
        /// <param name="group"></param>
        internal void RemoveCardGroup(CardGroup group)
        {
            if (cardGroups.Keys.Contains(group.Id))
            {
                CardGroup gg;
                cardGroups.TryRemove(group.Id, out gg);
            }
        }
        /// <summary>
        /// Create a new glow group
        /// </summary>
        /// <param name="group"></param>
        internal void AddCardGroup(CardGroup group)
        {
            if (!cardGroups.Keys.Contains(group.Id))
            {
                cardGroups.TryAdd(group.Id,group);
            }
        }

        /// <summary>
        /// Get the glow group that contains a cardID
        /// </summary>
        /// <param name="cardID"></param>
        /// <returns></returns>
        internal CardGroup GeCardGroup(string cardID) {
            CardGroup result = null;
            foreach (CardGroup group in cardGroups.Values)
            {
                if (group!=null&&group.HasCard(cardID))
                {
                    result = group;
                }
            }
            return result;
        }
        /// <summary>
        /// Get the glow group
        /// </summary>
        /// <returns></returns>
        internal ConcurrentDictionary<string, CardGroup> GetCardGroup()
        {
            return cardGroups;
        }
        /// <summary>
        /// Check if some group contains the cardID
        /// </summary>
        /// <param name="cardID"></param>
        /// <returns></returns>
        internal bool HasCardGroup(string cardID)
        {
            foreach (CardGroup group in cardGroups.Values) {
                if (group.HasCard(cardID)) {
                    return true;
                }
            }
            return false;
        }
        #region Card glow & connection method
        /// <summary>
        /// Find all groups that intersect with the card
        /// </summary>
        /// <param name="cardID"></param>
        /// <returns></returns>
        internal async Task<CardGroup[]> GetAttachedGroups(string cardID)
        {
            CardGroup[] groups = null;
            CardStatus targetCard = await semanticGroupController.Controllers.CardController.GetLiveCardStatus(cardID);
            if (targetCard == null)
            {
                return null;
            }
            List<CardGroup> tempList = new List<CardGroup>();
            foreach (CardGroup gg in GetCardGroup().Values)
            {
                if (await IsIntersect(targetCard, gg))
                {
                    tempList.Add(gg);
                }
            }
            groups = tempList.ToArray();
            return groups;
        }
        private async Task<bool> IsIntersect(CardStatus card, CardGroup gg)
        {
            if (!gg.HasCard(card.cardID))
            {
                CardStatus intersectedCard = null;
                foreach (string id in gg.GetCardID())
                {
                    intersectedCard = await semanticGroupController.Controllers.CardController.GetLiveCardStatus(id);
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
        internal void ConnectOneCardWithGroups(string cardID, CardGroup[] groups)
        {
            //if no groups intersected, create a new group
            if (groups == null || groups.Length == 0)
            {
                CardGroup newgg = new CardGroup();
                newgg.AddCard(cardID);
                AddCardGroup(newgg);
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
                        RemoveGlow(id);
                        RemoveGlowEffect(id);
                    }
                    RemoveCardGroup(gg);
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
                AddCardGroup(newgg);
                return;
            }
        }
        /// <summary>
        /// Connect multiple groups
        /// </summary>
        /// <param name="cardID"> The card id that trigger the event</param>
        internal async void ConnectGroupWithGroups(string cardID)
        {
            CardGroup group = GeCardGroup(cardID);
            List<CardGroup> tempList = new List<CardGroup>();
            RemoveCardGroup(group);
            tempList.Add(group);
            foreach (string id in group.GetCardID())
            {
                CardGroup[] groups = await GetAttachedGroups(id);
                if (groups != null)
                    foreach (CardGroup gg in groups)
                    {
                        tempList.Add(gg);
                        RemoveCardGroup(gg);
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
            AddCardGroup(newgg);
        }
        /// <summary>
        /// Update one card when point down
        /// </summary>
        /// <param name="cardID"></param>
        internal async void DisconnectOneCardWithGroups(string cardID)
        {
            //Find the group that contains this card
            CardGroup currentGroup = GeCardGroup(cardID);
            int colorIndex = 0;
            Glow glow = GetGlow(cardID);
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
                RemoveCardGroup(currentGroup);
                foreach (CardGroup gg in groups)
                {
                    AddCardGroup(gg);
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
        /// Based on the connection of the cards, create different groups
        /// </summary>
        /// <param name="cardIDs"></param>
        /// <returns></returns>
        private async Task<CardGroup[]> GetGroupsFromCards(IEnumerable<string> cardIDs)
        {
            List<string> cardList = new List<string>();
            foreach (string card in cardIDs)
            {
                cardList.Add(card);
            }
            List<CardGroup> groups = new List<CardGroup>();
            while (cardList.Count > 0)
            {
                string cardID = cardList[0];
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
            CardStatus status1 = await semanticGroupController.Controllers.CardController.GetLiveCardStatus(card);
            List<string> tempList = new List<string>();
            foreach (string c in cards)
            {
                CardStatus status2 = await semanticGroupController.Controllers.CardController.GetLiveCardStatus(c);
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
            CardStatus cardStatus = await semanticGroupController.Controllers.CardController.GetLiveCardStatus(cardID);
            Glow glow = await semanticGroupController.Controllers.GlowLayerController.AddGlow(cardStatus, colorIndex, semanticGroupController.Controllers.GlowLayerController);
            AddGlow(cardID, glow);
        }
        /// <summary>
        /// Remove the glow effect from the glow layer
        /// </summary>
        /// <param name="cardID"></param>
        internal void RemoveGlowEffect(string cardID)
        {
            Glow glow = RemoveGlow(cardID);
            semanticGroupController.Controllers.GlowLayerController.RemoveGlowEffect(cardID);
        }
        /// <summary>
        /// If one color changed, update other connected glow color.
        /// </summary>
        /// <param name="colorIndex"></param>
        internal void UpdateConnectedColor(string cardID, int colorIndex)
        {
            foreach (CardGroup group in GetCardGroup().Values)
            {
                if (group.HasCard(cardID))
                {
                    foreach (string id in group.GetCardID())
                    {
                        Glow glow = GetGlow(id);
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
            CardGroup group = GeCardGroup(cardID);
            if (group != null)
                foreach (string id in group.GetCardID())
                {
                    Glow glow = GetGlow(id);
                    if (glow != null)
                    {
                        glow.MoveBy(vector);
                        semanticGroupController.Controllers.CardController.MoveCardByVector(id, vector);
                    }
                }
        }
        #endregion
    }
}
