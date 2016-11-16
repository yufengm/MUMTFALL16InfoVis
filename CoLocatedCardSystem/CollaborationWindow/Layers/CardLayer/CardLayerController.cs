using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoLocatedCardSystem.CollaborationWindow.InteractionModule;

namespace CoLocatedCardSystem.CollaborationWindow.Layers.Card_Layer
{
    /// <summary>
    /// A controller to manage the card layer
    /// </summary>
    class CardLayerController
    {
        CardLayer cardLayer;
        CentralControllers controllers;
        Dictionary<Card, int> zIndexList = new Dictionary<Card, int>();

        internal CardLayerController(CentralControllers ctrls)
        {
            this.controllers = ctrls;
        }
        /// <summary>
        /// Initialize the CardLayer
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        internal void Init(int width, int height)
        {
            cardLayer = new CardLayer(this);
            cardLayer.Init(width, height);
        }
        /// <summary>
        /// Destroy the CardLayer
        /// </summary>
        internal void Deinit()
        {
            cardLayer.Deinit();
        }
        /// <summary>
        /// Get the instance of the card layer
        /// </summary>
        /// <returns></returns>
        internal CardLayer GetCardLayer() {
            return cardLayer;
        }
        /// <summary>
        /// Load one card to the card layer
        /// </summary>
        /// <param name="card"></param>
        internal async Task LoadCard(Card card)
        {
            if (!zIndexList.Keys.Contains(card))
            {
                int index = zIndexList.Count();//There might be cards in the list before load the cards
                await card.LoadUI();
                await cardLayer.AddCard(card);
                zIndexList.Add(card, index++);
                await cardLayer.SetZIndex(card, zIndexList[card]);
            }
        }
        /// <summary>
        /// Load the card list to the card layer
        /// </summary>
        /// <param name="cards"></param>
        internal async void LoadCards(Card[] cards)
        {
            int index = zIndexList.Count();//There might be cards in the list before load the cards
            foreach (Card card in cards) {
                await card.LoadUI();
                await cardLayer.AddCard(card);
                zIndexList.Add(card, index++);
                await cardLayer.SetZIndex(card, zIndexList[card]);
            }
        }
        /// <summary>
        /// Remove the card.
        /// </summary>
        /// <param name="card"></param>
        internal void UnloadCard(Card card)
        {
            card.Deinit();
            MoveCardToTop(card);
            cardLayer.RemoveCard(card);
            zIndexList.Remove(card);
        }

        /// <summary>
        /// Update the card zindex in the cardlayer
        /// </summary>
        /// <param name="card"></param>
        internal async void MoveCardToTop(Card card)
        {
            if (zIndexList.Keys.Contains(card))
            {
                int currentIndex = zIndexList[card];
                foreach (Card child in zIndexList.Keys.ToList())
                {
                    if (zIndexList[child] > currentIndex)
                    {
                        zIndexList[child]--;
                        await cardLayer.SetZIndex(child, zIndexList[child]);
                    }
                }
                zIndexList[card] = zIndexList.Count - 1;
                await cardLayer.SetZIndex(card, zIndexList[card]);
            }
        }
    }
}
