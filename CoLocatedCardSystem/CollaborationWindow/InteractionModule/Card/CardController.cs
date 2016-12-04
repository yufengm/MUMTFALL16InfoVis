using CoLocatedCardSystem.CollaborationWindow.DocumentModule;
using CoLocatedCardSystem.CollaborationWindow.TableModule;
using DataAttribute = CoLocatedCardSystem.CollaborationWindow.TableModule.DataAttribute;
using System;
using System.Threading.Tasks;
using Windows.UI.Input;
using Windows.Foundation;
using System.Collections.Generic;
using CoLocatedCardSystem.SecondaryWindow.Tool;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    class CardController
    {
        private CentralControllers controllers;
        DocumentCardController documentCardController = null;
        LiveCardList liveCardList = null;

        internal DocumentCardController DocumentCardController
        {
            get
            {
                return documentCardController;
            }
        }

        internal CentralControllers Controllers
        {
            get
            {
                return controllers;
            }            
        }

        public CardController(CentralControllers ctrls) {
            this.controllers = ctrls;
        }
        internal void Init()
        {
            documentCardController = new DocumentCardController(controllers.CardController);
            liveCardList = new LiveCardList();
        }

        internal void Deinit() {
            documentCardController.Deinit();
            liveCardList.Deinit();
        } 
        /// <summary>
        /// Initialize the doc cards
        /// </summary>
        /// <param name="docs"></param>
        /// <returns></returns>                
        internal void InitDocCard(Document[] docs) {
            if (docs != null)
            {
                documentCardController.Init(docs);
            }
        }

        internal async void RemoveActiveCard(string cardID)
        {
            controllers.SemanticGroupController.DisconnectOneCardWithGroups(cardID);
            CardStatus cs = await GetLiveCardStatus(cardID);
            if (cs.type == typeof(DocumentCard))
            {
                DocumentCard card = documentCardController.GetDocumentCardById(cardID);
                if (card != null)
                {
                    controllers.CardLayerController.UnloadCard(card);
                    card.DehighlightAll();
                }
            }
            controllers.MenuLayerController.EnableCard(cs.owner, cardID);
            liveCardList.RemoveCard(cardID);
        }

        /// <summary>
        /// Update the ZIndex of the card. Move the card to the top.
        /// </summary>
        /// <param name="card"></param>
        internal void MoveCardToTop(Card card)
        {
            controllers.CardLayerController.MoveCardToTop(card);
        }
        /// <summary>
        /// Add the card to talbe
        /// </summary>
        /// <param name="card"></param>
        /// <param name="t"></param>
        internal async void MoveCardToTable(Card card, Type t)
        {
            liveCardList.AddCardToTable(card, t);
            await controllers.CardLayerController.LoadCard(card);
        }
        internal bool IsCardOnTable(string cardID)
        {
            return liveCardList.HasCard(cardID);
        }
        /// <summary>
        /// Get the card status of the a card with an card id. Card status is a primative type.
        /// GUI are accessed through dispatcher
        /// The Card status has position, rotation, scale and card ID information
        /// </summary>
        /// <param name="cardID"></param>
        /// <returns></returns>
        internal Task<CardStatus> GetLiveCardStatus(string cardID)
        {
            return liveCardList.GetStatus(cardID);
        }

        internal Task<IEnumerable<CardStatus>> GetLiveCardStatus()
        {
            return liveCardList.GetStatus();
        }
        /// <summary>
        /// Move the card by vector
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vector"></param>
        internal void MoveCardByVector(string id, Point vector)
        {
            liveCardList.MoveCardByVector(id, vector);
        }
        /// <summary>
        /// Create a touch and pass it to the interaction controller.
        /// </summary>
        /// <param name="p"></param>
        internal void PointerDown(PointerPoint localPoint, PointerPoint globalPoint, Card card, Type type)
        {
            controllers.TouchController.TouchDown(localPoint,globalPoint, card, type);
        }
        /// <summary>
        /// Update the touch point
        /// </summary>
        /// <param name="p"></param>
        internal void PointerMove(PointerPoint localPoint, PointerPoint globalPoint)
        {
            controllers.TouchController.TouchMove(localPoint, globalPoint);
        }
        /// <summary>
        /// Lift the touch layer
        /// </summary>
        /// <param name="p"></param>
        internal void PointerUp(PointerPoint localPoint, PointerPoint globalPoint)
        {
            controllers.TouchController.TouchUp(localPoint, globalPoint);
        }

        internal void ResetCardColor()
        {
            foreach (DocumentCard docCard in documentCardController.GetDocumentCardByDoc())
            {
                if (docCard != null)
                {
                    SemanticGroup sg = controllers.SemanticGroupController.GetSemanticGroupByDoc(docCard.Document.DocID);
                    if (sg != null)
                    {
                        docCard.SetBackground(ColorPicker.HsvToRgb(sg.Hue, 0.5, 0.5));
                        if (IsCardOnTable(docCard.CardID)) {
                            docCard.UpdateTransform();
                        }
                    }
                }
            }
        }
    }
}
