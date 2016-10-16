using CoLocatedCardSystem.CollaborationWindow.DocumentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Input;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    class DocumentCardController:CardController
    {
        DocumentCardList list;

        public DocumentCardController(CentralControllers ctrls) : base(ctrls)
        {
        }

        /// <summary>
        /// Initialize the cardController with a list of documents
        /// </summary>
        /// <param name="documents"></param>
        public void Init(Document[] documents)
        {
            list = new DocumentCardList();
            foreach (User user in UserInfo.GetLiveUsers())
            {
                foreach (Document doc in documents)
                {
                    list.AddCard(doc, user, this);
                }
            }
        }
        /// <summary>
        /// Destroy the card list
        /// </summary>
        public void Deinit()
        {
            list.Clear();
        }
        /// <summary>
        /// Get all documents with the content
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        internal DocumentCard[] GetCardDocumentCardWByContent(User owner, string content)
        {
            DocumentCard[] cards = list.GetCardByContent(owner, content);
            return cards;
        }
        /// <summary>
        /// Return the card that match the specific id.
        /// </summary>
        /// <param name="cardID"></param>
        /// <returns></returns>
        internal DocumentCard GetDocumentCardById(string cardID)
        {
            return list.GetCard(cardID);
        }

        internal void PointerDown_Tile(PointerPoint localPoint, PointerPoint globalPoint, Tile tile, Type type)
        {
            this.Controllers.TouchController.TouchDown(localPoint, globalPoint, tile, type);
        }

        internal void Highlight(string cardID, Token token)
        {
            DocumentCard card = list.GetCard(cardID);
            card.AddHighLightWord(token);
        }

        internal void DeHighLight(string cardID, Token token)
        {
            DocumentCard card = list.GetCard(cardID);
            card.RemoveHighLightWord(token);
        }
    }
}
