using CoLocatedCardSystem.CollaborationWindow.TableModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    class ItemCardLayerBase : Canvas
    {
        protected Card attachedCard = null;

        /// <summary>
        /// Load the document to the card
        /// </summary>
        /// <param name="doc"></param>
        internal virtual async Task SetItem(DataRow item)
        {

        }
        public ItemCardLayerBase(Card card)
        {
            attachedCard = card;
        }

        internal virtual async void Init()
        {

        }
    }
}
