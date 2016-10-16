using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    class PlotCardList
    {
        Dictionary<string, PlotCard> list = new Dictionary<string, PlotCard>();//Key: random card id.
        internal void AddCard(string cardID, PlotCard pcard)
        {
            if (!list.Keys.Contains(cardID))
            {
                list.Add(cardID, pcard);
            }
            else {
                list[cardID] = pcard;
            }
        }
        /// <summary>
        /// Remove a card based on its id
        /// </summary>
        /// <param name="cardID"></param>
        internal void RemoveCard(string cardID)
        {

        }
        /// <summary>
        /// Delete all cards in the card list
        /// </summary>
        internal void Clear()
        {
            list.Clear();
        }
        /// <summary>
        /// Get the card by cardID
        /// </summary>
        /// <param name="cardID"></param>
        /// <returns></returns>
        internal PlotCard GetCard(string cardID)
        {
            return list[cardID];
        }
        /// <summary>
        /// Return all card instances
        /// </summary>
        /// <returns></returns>
        internal Card[] GetCard()
        {
            return list.Values.ToArray<Card>();
        }
        /// <summary>
        /// Get references to all cards belong to a specific user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        internal Card[] GetCard(User user)
        {
            var cardList = list.Values.Where(a => a.Owner.Equals(user));
            return cardList.ToArray<Card>();
        }
    }
}
