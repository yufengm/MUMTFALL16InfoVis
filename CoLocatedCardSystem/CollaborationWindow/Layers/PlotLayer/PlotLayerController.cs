using CoLocatedCardSystem.CollaborationWindow.InteractionModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace CoLocatedCardSystem.CollaborationWindow.Layers.Plot_Layer
{
    class PlotLayerController
    {
        PlotLayer plotLayer;
        CentralControllers controllers;
        Dictionary<PlotCard, int> zIndexList = new Dictionary<PlotCard, int>();

        internal PlotLayerController(CentralControllers ctrls)
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
            plotLayer = new PlotLayer(this);
            plotLayer.Init(width, height);
        }
        /// <summary>
        /// Destroy the CardLayer
        /// </summary>
        internal void Deinit()
        {
            plotLayer.Deinit();
        }
        /// <summary>
        /// Get the instance of the card layer
        /// </summary>
        /// <returns></returns>
        internal PlotLayer GetPlotLayer()
        {
            return plotLayer;
        }
        /// <summary>
        /// Load one card to the card layer
        /// </summary>
        /// <param name="card"></param>
        internal async Task LoadCard(PlotCard card)
        {
            if (!zIndexList.Keys.Contains(card))
            {
                int index = zIndexList.Count();//There might be cards in the list before load the cards
                await card.LoadUI();
                await plotLayer.AddCard(card);
                zIndexList.Add(card, index++);
                await plotLayer.SetZIndex(card, zIndexList[card]);
            }
        }

        internal CoreDispatcher GetDispatcher()
        {
            return plotLayer.Dispatcher;
        }

        /// <summary>
        /// Load the card list to the card layer
        /// </summary>
        /// <param name="cards"></param>
        internal async void LoadCards(PlotCard[] cards)
        {
            int index = zIndexList.Count();//There might be cards in the list before load the cards
            foreach (PlotCard card in cards)
            {
                await card.LoadUI();
                await plotLayer.AddCard(card);
                zIndexList.Add(card, index++);
                await plotLayer.SetZIndex(card, zIndexList[card]);
            }
        }
        /// <summary>
        /// Update the card zindex in the cardlayer
        /// </summary>
        /// <param name="card"></param>
        internal async void MoveCardToTop(PlotCard card)
        {
            if (zIndexList.Keys.Contains(card))
            {
                int currentIndex = zIndexList[card];
                foreach (PlotCard child in zIndexList.Keys.ToList())
                {
                    if (zIndexList[child] > currentIndex)
                    {
                        zIndexList[child]--;
                        await plotLayer.SetZIndex(child, zIndexList[child]);
                    }
                }
                zIndexList[card] = zIndexList.Count - 1;
                await plotLayer.SetZIndex(card, zIndexList[card]);
            }
        }
    }
}
