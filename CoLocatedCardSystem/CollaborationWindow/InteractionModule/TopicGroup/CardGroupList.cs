using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    /// <summary>
    /// Card glows and list of glow effect
    /// </summary>
    class CardGroupList
    {
        Dictionary<string, Glow> glowEffectList;// A list of the glow objects. key: card id, value Glow object
        ConcurrentDictionary<string, CardGroup> cardGroups;//Save info of which cards are connected. key: group id, value: semantic group

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

    }
}
