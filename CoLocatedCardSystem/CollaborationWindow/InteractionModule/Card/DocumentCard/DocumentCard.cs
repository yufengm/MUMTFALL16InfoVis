using CoLocatedCardSystem.CollaborationWindow.DocumentModule;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    public class DocumentCard : Card
    {
        DocumentCardLayerBase[] layers;
        int currentLayer;
        Document document;
        private const int LAYER_NUMBER = 4;
        List<Token> highlightedTokens = new List<Token>();
        public Document Document
        {
            get
            {
                return document;
            }

            set
            {
                document = value;
            }
        }

        internal List<Token> HighlightedTokens
        {
            get
            {
                return highlightedTokens;
            }
        }

        public DocumentCard(CardController cardController) : base(cardController)
        {
        }

        /// <summary>
        /// Initialize a semantic card.
        /// </summary>
        /// <param name="cardID"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="position"></param>
        /// <param name="scale"></param>
        /// <param name="rotation"></param>
        internal void Init(string cardID, User user, Document doc)
        {
            base.Init(cardID, user);
            this.document = doc;
        }
        internal override async Task LoadUI()
        {
            await base.LoadUI();
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                layers = new DocumentCardLayerBase[LAYER_NUMBER];
                layers[0] = new DocumentCardLayer1(cardController as DocumentCardController, this);
                layers[1] = new DocumentCardLayer2(cardController as DocumentCardController, this);
                layers[2] = new DocumentCardLayer3(cardController as DocumentCardController, this);
                layers[3] = new DocumentCardLayer4(cardController as DocumentCardController, this);
                foreach (var layer in layers)
                {
                    layer.Init();
                }
                currentLayer = 0;
                this.Children.Add(layers[0]);
                foreach (var layer in layers)
                {
                    await layer.SetArticle(this.document);
                }
            });
        }
        /// <summary>
        /// Add a high light word
        /// </summary>
        /// <param name="token"></param>
        internal void AddHighLightWord(Token token)
        {
            if (!highlightedTokens.Contains(token))
            {
                highlightedTokens.Add(token);
                (layers[1] as DocumentCardLayer2).HightLight(token);
                (layers[2] as DocumentCardLayer3).HightLight(token);
                (layers[3] as DocumentCardLayer4).HightLight(token);
            }
        }
        /// <summary>
        /// remove the high light word from the list
        /// </summary>
        /// <param name="token"></param>
        internal void RemoveHighLightWord(Token token)
        {
            if (highlightedTokens.Contains(token))
            {
                highlightedTokens.Remove(token);
                (layers[1] as DocumentCardLayer2).DeHightLight(token);
                (layers[2] as DocumentCardLayer3).DeHightLight(token);
                (layers[3] as DocumentCardLayer4).DeHightLight(token);
            }
        }

        /// <summary>
        /// Check the size of the card, load new layer if the size falls in a zoom range
        /// </summary>
        internal override void UpdateSize()
        {
            base.UpdateSize();
            UpdateLayer(this.CardScale);
        }
        private void UpdateLayer(double scale)
        {
            if (scale > 3 && currentLayer != 3)
            {
                ShowLayer(3);
            }
            else if (scale > 2 && scale <= 3 && currentLayer != 2)
            {
                ShowLayer(2);
            }
            else if (scale > 1.3 && scale <= 2 && currentLayer != 1)
            {
                ShowLayer(1);
            }
            else if (scale <= 1.3 && currentLayer != 0)
            {
                ShowLayer(0);
            }
        }
        private async void ShowLayer(int layerIndex)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Children.Remove(layers[currentLayer]);
                currentLayer = layerIndex;
                this.Children.Add(layers[currentLayer]);
            });
        }

        /// <summary>
        /// Send a new touch to the touch module, with the type of Document Card
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void PointerDown(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint localPoint = e.GetCurrentPoint(this);
            PointerPoint globalPoint = e.GetCurrentPoint(Coordination.Baselayer);
            cardController.PointerDown(localPoint, globalPoint, this, typeof(DocumentCard));
            base.PointerDown(sender, e);
        }
    }
}
