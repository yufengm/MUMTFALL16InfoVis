using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.CollaborationWindow.Layers.Glow_Layer
{
    class GlowGroup
    {
        List<string> group = new List<string>();
        /// <summary>
        /// Add a card to glow group
        /// </summary>
        /// <param name="cardID"></param>
        internal void AddCard(string cardID) {
            if (!group.Contains(cardID)) {
                group.Add(cardID);
            }
        }

        /// <summary>
        /// Remove a card from glow group
        /// </summary>
        /// <param name="cardID"></param>
        internal void RemoveCard(string cardID) {
            if (group.Contains(cardID)) {
                group.Remove(cardID);
            }
        }
        /// <summary>
        /// Get all card ids within a group.
        /// </summary>
        /// <returns></returns>
        internal List<string> GetCardID()
        {
            return group;
        }

        /// <summary>
        /// Check if the group contains a card
        /// </summary>
        /// <param name="cardID"></param>
        /// <returns></returns>
        internal bool HasCard(string cardID) {
            return group.Contains(cardID);
        }
    }
}
