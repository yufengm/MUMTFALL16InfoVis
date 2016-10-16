using DataAttribute = CoLocatedCardSystem.CollaborationWindow.TableModule.DataAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    class AttributeCardList
    {
        Dictionary<string, AttributeCard> list = new Dictionary<string, AttributeCard>();//Key: random card id.

        /// <summary>
        /// Add a new card to the user.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="cardInfo"></param>
        ///
        /// <returns></returns>
        internal async Task AddCard(DataAttribute attribute, User user, AttributeCardController attributeController)
        {
            string cardID = Guid.NewGuid().ToString();
            AttributeCard card = new AttributeCard(attributeController);
            await card.Init(cardID, user, attribute);
            list.Add(cardID, card);
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
        internal AttributeCard GetCard(string cardID)
        {
            if (!list.Keys.Contains(cardID)){
                return null;
            }
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