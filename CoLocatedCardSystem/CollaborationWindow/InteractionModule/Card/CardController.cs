using CoLocatedCardSystem.CollaborationWindow.DocumentModule;
using CoLocatedCardSystem.CollaborationWindow.TableModule;
using DataAttribute = CoLocatedCardSystem.CollaborationWindow.TableModule.DataAttribute;
using System;
using System.Threading.Tasks;
using Windows.UI.Input;
using Windows.Foundation;
using System.Collections.Generic;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    public class CardController
    {
        private CentralControllers controllers;
        AttributeCardController attributeCardController = null;
        ItemCardController itemCardController = null;
        DocumentCardController documentCardController = null;
        PlotCardController plotCardController = null;
        LiveCardList liveCardList = null;

        public AttributeCardController AttributeCardController
        {
            get
            {
                return attributeCardController;
            }
        }
        public ItemCardController ItemCardController
        {
            get
            {
                return itemCardController;
            }
        }
        internal DocumentCardController DocumentCardController
        {
            get
            {
                return documentCardController;
            }
        }
        internal PlotCardController PlotCardController
        {
            get
            {
                return plotCardController;
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
            attributeCardController = new AttributeCardController(controllers);
            itemCardController = new ItemCardController(controllers);
            documentCardController = new DocumentCardController(controllers);
            plotCardController = new PlotCardController(controllers);
            liveCardList = new LiveCardList();
            plotCardController.Init();
        }
        internal void Deinit() {
            attributeCardController.Deinit();
            itemCardController.Deinit();
            documentCardController.Deinit();
            plotCardController.Deinit();
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

        /// <summary>
        /// Initialize the table cards
        /// </summary>
        /// <param name="docs"></param>
        /// <returns></returns>                
        internal async Task InitItemCard(DataRow[] items)
        {
            if (items != null)
            {
                await itemCardController.Init(items);
            }
        }
        /// <summary>
        /// Initialize the attribute cards
        /// </summary>
        /// <param name="docs"></param>
        /// <returns></returns>                
        internal async Task InitAttributeCard(DataAttribute[] attributes)
        {
            if (attributes != null)
            {
                await attributeCardController.Init(attributes);
            }
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
        /// Remove the glow from the card
        /// </summary>
        /// <param name="cardID"></param>
        internal void RemoveGlow(string cardID)
        {
            controllers.GlowController.DisconnectOneCardWithGroups(cardID);
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
    }
}
