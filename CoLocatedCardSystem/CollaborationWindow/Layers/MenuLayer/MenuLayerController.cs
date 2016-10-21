using CoLocatedCardSystem.CollaborationWindow.InteractionModule;
using CoLocatedCardSystem.CollaborationWindow.Layers.Card_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Input;

namespace CoLocatedCardSystem.CollaborationWindow.Layers.Menu_Layer
{
    class MenuLayerController
    {
        MenuLayer menuLayer;
        CentralControllers controllers;
        Dictionary<User, MenuBar> list = new Dictionary<User, MenuBar>();
        public MenuLayerController(CentralControllers ctrls) {
            this.controllers = ctrls;
        }
        /// <summary>
        /// Initialize the menu controller and the menu layer.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        internal void Init(int width, int height) {
            menuLayer = new MenuLayer(this);
            menuLayer.Init(width, height);
            foreach (User user in UserInfo.GetLiveUsers()) {
                MenuBar bar = new MenuBar(this);
                bar.Init(user);
                list.Add(user, bar);
                menuLayer.AddMenuBar(bar);
            }
        }
        /// <summary>
        /// Destroy the menu layer
        /// </summary>
        internal void Deinit() {
            foreach (MenuBar bar in list.Values)
            {
                bar.Deinit();
            }
            list.Clear();
            menuLayer.Deinit();
        }

        internal MenuLayer GetMenuLayer() {
            return menuLayer;
        }
        /// <summary>
        /// Ask the interaction module to create a sorting box
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="content"></param>
        internal void CreateSortingBox(User owner, string content)
        {
            if (content.Length > 0)
            {
                controllers.SortingBoxController.CreateSortingBox(owner, content);
            }
        }
        /// <summary>
        /// Search the document and add show the search results.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="content"></param>
        internal void SearchDocumentCard(User owner, string content)
        {
            DocumentCard[] cards = 
                controllers.CardController.DocumentCardController.GetCardDocumentCardWByContent(owner, content);
            foreach (DocumentCard card in cards) {
                card.InitialHighlight(content);
            }
            list[owner].ShowCardsInSearchResultTray(cards);
        }
        /// <summary>
        /// Get all menu bars
        /// </summary>
        /// <returns></returns>
        internal MenuBar[] GetAllMenuBars()
        {
            return list.Values.ToArray();
        }
        /// <summary>
        /// Add the card to the card layer.
        /// </summary>
        /// <param name="resultCard"></param>
        internal void AddPulledCard(ResultCard resultCard)
        {
            DocumentCard card = controllers.CardController.DocumentCardController.GetDocumentCardById(resultCard.CardID);
            MenuBarInfo info = MenuBarInfo.GetMenuBarInfo(card.Owner);
            card.MoveBy(info.CardInitPosition);
            card.Rotate(info.Rotate);
            controllers.CardController.MoveCardToTable(card, typeof(DocumentCard));     
        }
        /// <summary>
        /// Check if the card is on the table.
        /// </summary>
        /// <param name="documentCard"></param>
        /// <returns></returns>
        internal bool IsCardOnTable(DocumentCard documentCard)
        {
            return controllers.CardController.IsCardOnTable(documentCard.CardID);
        }
        /// <summary>
        /// Pass the touch poitn to the touch module
        /// </summary>
        /// <param name="localPoint"></param>
        /// <param name="globalPoint"></param>
        /// <param name="sender"></param>
        /// <param name="type"></param>
        internal void PointerDown(PointerPoint localPoint, PointerPoint globalPoint, object sender, Type type)
        {
            controllers.TouchController.TouchDown(localPoint, globalPoint, sender, type);
        }
        /// <summary>
        /// update the touch point
        /// </summary>
        /// <param name="localPoint"></param>
        /// <param name="globalPoint"></param>
        internal void PointerMove(PointerPoint localPoint, PointerPoint globalPoint)
        {
            controllers.TouchController.TouchMove(localPoint, globalPoint);
        }
        /// <summary>
        /// End the touch Point
        /// </summary>
        /// <param name="localPoint"></param>
        /// <param name="globalPoint"></param>
        internal void PointerUp(PointerPoint localPoint, PointerPoint globalPoint)
        {
            controllers.TouchController.TouchUp(localPoint, globalPoint);
        }
    }
}
