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
    class DocumentCard : Card
    {
        DocumentCardLayerBase[] layers;
        int currentLayer;
        Document document;
        private const int LAYER_NUMBER = 3;
        List<Token> highlightedTokens = new List<Token>();
        DocumentCardController documentCardController;
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

        internal DocumentCard(DocumentCardController documentCardController) : base(documentCardController.CardController)
        {
            this.documentCardController = documentCardController;
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
        /// <summary>
        /// Dehighlight all words
        /// </summary>
        internal void DehighlightAll()
        {
            highlightedTokens.Clear();
        }

        /// <summary>
        /// Load the document card ui
        /// </summary>
        /// <returns></returns>
        internal override async Task LoadUI()
        {
            await base.LoadUI();
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                layers = new DocumentCardLayerBase[LAYER_NUMBER];
                layers[0] = new DocumentCardLayer1(this.documentCardController, this);
                layers[1] = new DocumentCardLayer3(this.documentCardController, this);
                layers[2] = new DocumentCardLayer4(this.documentCardController, this);
                //layers[3] = new DocumentCardLayer4(cardController as DocumentCardController, this);
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
        /// Initialize the hightlight words
        /// </summary>
        /// <param name="content"></param>
        internal void InitialHighlight(ProcessedDocument tempPD)
        {
            var tokens = document.GetToken(tempPD);
            foreach (Token tk in tokens)
            {
                if (tk != null)
                    highlightedTokens.Add(tk);
            }
        }

        /// <summary>
        /// Add a high light word
        /// </summary>
        /// <param name="words"></param>
        internal void AddHighLightWord(Token token)
        {
            if (!highlightedTokens.Contains(token))
            {
                highlightedTokens.Add(token);
                cardController.Controllers.ConnectionController.AddWordToken(token, this.document.DocID, this.position.X, this.position.Y);
                foreach (var layer in layers) {
                    layer.HighlightToken(token);
                }
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
                bool sameToken = false;
                foreach (Token tk in highlightedTokens)
                {
                    if (tk != token && tk.StemmedWord == token.StemmedWord)
                    {
                        sameToken = true;
                    }
                }
                if (!sameToken)
                {
                    cardController.Controllers.ConnectionController.RemoveToken(token, this.document.DocID);
                }
                highlightedTokens.Remove(token);
                foreach (var layer in layers)
                {
                    layer.DehighlightToken(token);
                }
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
            if (scale > 3 && currentLayer != 2)
            {
                ShowLayer(2);
            }
            else if (scale > 2 && scale <= 3 && currentLayer != 1)
            {
                ShowLayer(1);
            }
            else if (scale <= 2 && currentLayer != 0)
            {
                ShowLayer(0);
            }
        }
        /// <summary>
        /// Show the ith layer
        /// </summary>
        /// <param name="layerIndex"></param>
        private async void ShowLayer(int layerIndex)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Children.Remove(layers[currentLayer]);
                currentLayer = layerIndex;
                this.Children.Add(layers[currentLayer]);
            });
        }
        internal bool isConnectAllowed() {
            return currentLayer <= 1;
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

        protected override void Card_ManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e)
        {
            base.Card_ManipulationStarting(sender, e);
            if (this.PointerCaptures!=null&&this.PointerCaptures.Count > 1)
            {
                foreach (var layer in layers)
                {
                    layer.DisableTouch();
                }
            }
        }
        protected override void Card_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            base.Card_ManipulationCompleted(sender, e);
            foreach (var layer in layers)
            {
                layer.EnableTouch();
            }
        }
    }
}
