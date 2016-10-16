using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    /// <summary>
    /// Save the active cards on table
    /// </summary>
    class LiveCardList
    {
        Dictionary<string, CardStatus> statusList = new Dictionary<string, CardStatus>();
        Dictionary<string, Card> cardList = new Dictionary<string, Card>();
        internal void Deinit()
        {
            statusList.Clear();
            cardList.Clear();
            cardList = null;
            statusList = null;
        }
        /// <summary>
        /// add a card to the table.
        /// </summary>
        /// <param name="card"></param>
        /// <param name="t"></param>
        internal void AddCardToTable(Card card, Type t)
        {
            if (!cardList.Keys.Contains(card.CardID))
            {
                CardStatus info = new CardStatus();
                info.type = t;
                info.status = CardPositionStatus.ON_TABLE;
                info.position = card.Position;
                info.rotation = card.Rotation;
                info.scale = card.CardScale;
                info.cardID = card.CardID;
                info.corners = card.Corners;
                statusList.Add(card.CardID, info);
                cardList.Add(card.CardID, card);
            }
        }
        /// <summary>
        /// Get the position status of the card
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        internal async Task<CardStatus> GetStatus(string cardID)
        {
            CardStatus status = null;
            if (!cardList.Keys.Contains(cardID)) {
                return null;
            }
            Card card = cardList[cardID];
            await card.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (card.CardID.Equals(cardID))
                {
                    statusList[cardID].position = card.Position;
                    statusList[cardID].rotation = card.Rotation;
                    statusList[cardID].scale = card.CardScale;
                    statusList[cardID].corners = card.Corners;
                    status = statusList[cardID];
                }
            });
            return status;
        }
        /// <summary>
        /// Get the card status of all the cards.
        /// </summary>
        /// <returns></returns>
        internal async Task<IEnumerable<CardStatus>> GetStatus()
        {
            foreach (string cardID in cardList.Keys)
            {
                Card card = cardList[cardID];
                await card.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    if (card.CardID.Equals(cardID))
                    {
                        statusList[cardID].position = card.Position;
                        statusList[cardID].rotation = card.Rotation;
                        statusList[cardID].scale = card.CardScale;
                        statusList[cardID].corners = card.Corners;
                    }
                });
            }
            return statusList.Values.ToArray();
        }
        internal void MoveCardByVector(string cardID, Point vector)
        {
            cardList[cardID].MoveBy(vector);
        }
        /// <summary>
        /// Check if the cardID is on the table
        /// </summary>
        /// <param name="cardID"></param>
        /// <returns></returns>
        internal bool HasCard(string cardID)
        {
            return cardList.Keys.Contains(cardID);
        }
    }
}
