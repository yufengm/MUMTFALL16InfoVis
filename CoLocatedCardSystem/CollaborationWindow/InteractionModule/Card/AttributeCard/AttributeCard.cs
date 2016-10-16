using DataAttribute = CoLocatedCardSystem.CollaborationWindow.TableModule.DataAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml.Input;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    class AttributeCard : Card
    {
        AttributeCardLayerBase[] layers;
        int currentLayer=-1;
        private const int LAYER_NUMBER = 3;

        internal DataAttribute Attribute { get; set; }

        public AttributeCard(CardController cardController) : base(cardController)
        {

        }
        /// <summary>
        /// Initialize the item card
        /// </summary>
        /// <param name="cardID"></param>
        /// <param name="user"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        internal async Task Init(string cardID, User user, DataAttribute attribute)
        {
            base.Init(cardID, user);
            Attribute = attribute;
        }
        /// <summary>
        /// Show the card UI.
        /// </summary>
        /// <returns></returns>
        internal override async Task LoadUI()
        {
            await base.LoadUI();
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                layers = new AttributeCardLayerBase[LAYER_NUMBER];
                layers[0] = new AttributeCardLayer1(cardController as AttributeCardController, this);
                layers[1] = new AttributeCardLayer2(cardController as AttributeCardController, this);
                layers[2] = new AttributeCardLayer3(cardController as AttributeCardController, this);
                currentLayer = 0;
                foreach (var layer in layers)
                {
                    layer.Init();
                    await layer.SetAttribute(Attribute);
                }
                this.Children.Add(layers[currentLayer]);
            });
        }
        protected override void PointerDown(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint localPoint = e.GetCurrentPoint(this);
            PointerPoint globalPoint = e.GetCurrentPoint(Coordination.Baselayer);
            cardController.PointerDown(localPoint, globalPoint, this, typeof(AttributeCard));
            base.PointerDown(sender, e);
        }

        /// <summary>
        /// Check the size of the card, load new layer if necessary
        /// </summary>
        internal override void UpdateSize()
        {
            base.UpdateSize();
            if (currentLayer != -1) { 
            UpdateLayer(this.CardScale);
            }
        }
        private void UpdateLayer(double scale)
        {
            if (scale <= 2 && currentLayer != 0)
            {
                ShowLayer(0);
            }
            else if (scale > 2 && scale <= 3 && currentLayer != 1)
            {
                ShowLayer(1);
            }
            else if (scale > 3 && currentLayer != 2)
            {
                ShowLayer(2);
            }
        }
        private async void ShowLayer(int layerIndex)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                this.Children.Remove(layers[currentLayer]);
                currentLayer = layerIndex;
                this.Children.Add(layers[currentLayer]);
            });
        }
    }
}

