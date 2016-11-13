using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.CollaborationWindow.Layers.Glow_Layer
{
    class GlowGroup
    {
        ConcurrentDictionary<string, string> group = new ConcurrentDictionary<string, string>();
        /// <summary>
        /// Add a card to glow group
        /// </summary>
        /// <param name="cardID"></param>
        internal void AddCard(string cardID) {
            if (!group.Keys.Contains(cardID)) {
                group.TryAdd(cardID, cardID);
            }
        }

        /// <summary>
        /// Remove a card from glow group
        /// </summary>
        /// <param name="cardID"></param>
        internal void RemoveCard(string cardID)
        {
            if (group.Keys.Contains(cardID))
            {
                String removedStr = "";
                group.TryRemove(cardID, out removedStr);
            }
        }
        /// <summary>
        /// Get all card ids within a group.
        /// </summary>
        /// <returns></returns>
        internal ConcurrentDictionary<string, string> GetCardID()
        {
            return group;
        }

        /// <summary>
        /// Check if the group contains a card
        /// </summary>
        /// <param name="cardID"></param>
        /// <returns></returns>
        internal bool HasCard(string cardID) {
            return group.Keys.Contains(cardID);
        }
    }
}
