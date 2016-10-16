using DataAttribute = CoLocatedCardSystem.CollaborationWindow.TableModule.DataAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    class AttributeCardLayer1 : AttributeCardLayerBase
    {
        TextBlock titleTextBlock = new TextBlock();

        public AttributeCardLayer1(AttributeCardController controller, Card card) : base(controller, card)
        {
        }


        /// <summary>
        /// Initialize the attribute layers.
        /// </summary>
        /// <param name="attr"></param>
        /// <returns></returns>
        internal override async Task SetAttribute(DataAttribute attr)
        {
            await base.SetAttribute(attr);

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                titleTextBlock.Text = attr.Name;
                if (attr.Name.Length > 25)
                {
                    titleTextBlock.FontSize = 11;

                }
                if (attr.Name.Length > 50)
                {
                    titleTextBlock.FontSize = 9;
                }
            });
        }

        /// <summary>
        /// Initialize the layer
        /// </summary>
        internal override async void Init()
        {
            this.Width = attachedCard.Width;
            this.Height = attachedCard.Height;
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                //Move the textblock - 1 / 2 width and -1 / 2 height to the center.
                UIHelper.InitializeUI(
                    new Point(-0.5 * attachedCard.Width, -0.5 * attachedCard.Height),
                    0,
                    1,
                    new Size(this.Width, this.Height),
                    this);
                titleTextBlock.Width = attachedCard.Width;
                titleTextBlock.Height = attachedCard.Height;
                titleTextBlock.Foreground = new SolidColorBrush(Colors.Black);
                titleTextBlock.LineHeight = 1;
                titleTextBlock.TextWrapping = TextWrapping.Wrap;
                titleTextBlock.FontSize = 13;
                titleTextBlock.TextAlignment = TextAlignment.Center;
                titleTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
                titleTextBlock.VerticalAlignment = VerticalAlignment.Center;
                titleTextBlock.FontStretch = FontStretch.Normal;
                titleTextBlock.FontWeight = FontWeights.Bold;
                this.Children.Add(titleTextBlock);
            });
        }

    }
}
