using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using DataAttribute = CoLocatedCardSystem.CollaborationWindow.TableModule.DataAttribute;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    class AttributeCardLayerBase : Canvas
    {
        protected Card attachedCard = null;
        protected AttributeCardController cardController;

        internal virtual async Task SetAttribute(DataAttribute attr)
        {

        }

        public AttributeCardLayerBase(AttributeCardController controller, Card card)
        {
            this.cardController = controller;
            attachedCard = card;
        }

        internal virtual async void Init()
        {

        }
    }
}
