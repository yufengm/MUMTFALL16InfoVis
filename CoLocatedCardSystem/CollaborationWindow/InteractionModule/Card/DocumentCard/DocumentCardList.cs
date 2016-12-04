using CoLocatedCardSystem.CollaborationWindow.DocumentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    class DocumentCardList
    { 
        Dictionary<string, DocumentCard> list=new Dictionary<string, DocumentCard>();//Key: random card id.

        /// <summary>
        /// Add a new card to the user.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="cardInfo"></param>
        /// <returns></returns>
        internal void AddCard(Document doc,User user, CardController cardController) {
            string cardID = Guid.NewGuid().ToString();
            DocumentCard card = new DocumentCard(cardController.DocumentCardController);
            list.Add(cardID, card);
            card.Init(cardID, user, doc);
        }
        /// <summary>
        /// Remove a card based on its id
        /// </summary>
        /// <param name="cardID"></param>
        internal void RemoveCard(string cardID) {

        }
        /// <summary>
        /// Delete all cards in the card list
        /// </summary>
        internal void Clear() {
            list.Clear();
        }
        /// <summary>
        /// Get the card by cardID
        /// </summary>
        /// <param name="cardID"></param>
        /// <returns></returns>
        internal DocumentCard GetCard(string cardID) {
            DocumentCard card = null;
            if (list.Keys.Contains(cardID)) {
                card = list[cardID];
            }
            return card;
        }
        /// <summary>
        /// Assemble a list of cards which contains a keyword
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        internal DocumentCard[] GetCardByContent(User owner, ProcessedDocument tempPD)
        {
            List<DocumentCard> tempList = new List<DocumentCard>();
            foreach (DocumentCard card in list.Values)
            {
                if (card.Owner == owner && card.Document.HasWord(tempPD))
                {
                    tempList.Add(card);
                }
            }
            return tempList.ToArray();
        }

        internal DocumentCard GetCardByDoc(string docID, User owner)
        {
            var docs=list.Values.Where(c => c.Document.DocID == docID && c.Owner == owner);
            return docs.ElementAt(0) == null ? null : docs.ElementAt(0);
        }
    }
}
