using CoLocatedCardSystem.CollaborationWindow.DocumentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    class DocumentCardLayerBase : Canvas
    {
        protected DocumentCard attachedCard = null;
        protected DocumentCardController cardController;
        /// <summary>
        /// Load the document to the card
        /// </summary>
        /// <param name="doc"></param>
        internal virtual async Task SetArticle(Document doc)
        {

        }
        public DocumentCardLayerBase(DocumentCardController cardController, DocumentCard card)
        {
            attachedCard = card;
            this.cardController = cardController;
        }

        internal virtual async void Init()
        {

        }

        internal virtual async void DisableTouch()
        {
        }

        internal virtual async void EnableTouch()
        {
        }

        internal virtual async void HighlightToken(Token token)
        {
        }
        internal virtual async void DehighlightToken(Token token)
        {
        }
    }
}
