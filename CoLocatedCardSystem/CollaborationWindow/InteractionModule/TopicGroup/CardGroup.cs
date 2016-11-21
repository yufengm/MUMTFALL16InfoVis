using CoLocatedCardSystem.CollaborationWindow.DocumentModule;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    class CardGroup
    {
        String id = "";//random group id
        ConcurrentDictionary<string, string> group = new ConcurrentDictionary<string, string>();//The key is the cardID, value is group id
        public string Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public CardGroup()
        {
            id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Add a card to glow group
        /// </summary>
        /// <param name="cardID"></param>
        internal void AddCard(string cardID)
        {
            if (!group.Keys.Contains(cardID))
            {
                group.TryAdd(cardID, id);
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
        internal string[] GetCardID()
        {
            return group.Keys.ToArray();
        }

        /// <summary>
        /// Check if the group contains a card
        /// </summary>
        /// <param name="cardID"></param>
        /// <returns></returns>
        internal bool HasCard(string cardID)
        {
            return group.Keys.Contains(cardID);
        }

        internal int Count()
        {
            return group.Count();
        }
    }
}
