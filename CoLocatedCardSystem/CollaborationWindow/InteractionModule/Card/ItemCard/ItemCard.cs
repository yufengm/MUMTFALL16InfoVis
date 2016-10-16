using CoLocatedCardSystem.CollaborationWindow.TableModule;
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
    class ItemCard:Card
    {
        ItemCardLayerBase[] layers;
        int currentLayer;
        DataRow item;
        private const int LAYER_NUMBER = 2;
        internal DataRow Item
        {
            get
            {
                return item;
            }

            set
            {
                item = value;
            }
        }

        public ItemCard(CardController cardController) : base(cardController)
        {
        }
        /// <summary>
        /// Initialize the item card
        /// </summary>
        /// <param name="cardID"></param>
        /// <param name="user"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        internal async Task Init(string cardID, User user, DataRow item)
        {
            base.Init(cardID, user);
            this.item = item;
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
                layers = new ItemCardLayerBase[LAYER_NUMBER];
                layers[0] = new ItemCardLayer1(this);
                layers[1] = new ItemCardLayer2(this);
                foreach (var layer in layers)
                {
                    layer.Init();
                    await layer.SetItem(this.item);
                }
                this.Children.Add(layers[0]);
            });
        }
        
        /// <summary>
        /// Send a new touch to the touch module, with the type of Item Card
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void PointerDown(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint localPoint = e.GetCurrentPoint(this);
            PointerPoint globalPoint = e.GetCurrentPoint(Coordination.Baselayer);
            cardController.PointerDown(localPoint, globalPoint, this, typeof(ItemCard));
            base.PointerDown(sender, e);
        }

        /// <summary>
        /// Check the size of the card, load new layer if necessary
        /// </summary>
        internal override void UpdateSize()
        {
            base.UpdateSize();
            UpdateLayer(this.CardScale);
        }
        private void UpdateLayer(double scale)
        {
            if (scale > 2 && currentLayer == 0)
            {
                ShowLayer(1);
            }
            else if (scale <= 2 && currentLayer == 1)
            {
                ShowLayer(0);
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
