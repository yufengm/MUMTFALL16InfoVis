using DataAttribute = CoLocatedCardSystem.CollaborationWindow.TableModule.DataAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;
using Windows.UI.Xaml;
using CoLocatedCardSystem.CollaborationWindow.TableModule;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    class AttributeCardLayer3 : AttributeCardLayerBase
    {
        TextBlock textBlock = new TextBlock();

        public AttributeCardLayer3(AttributeCardController controller, Card card) : base(controller, card)
        {
        }

        internal override async Task SetAttribute(DataAttribute attr)
        {
            await base.SetAttribute(attr);
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                var resultCells=cardController.Controllers.TableController.GetValueWithAttribute(attr);
                StringBuilder builder = new StringBuilder();
                foreach (DataCell cell in resultCells) {
                    builder.AppendLine(cell.StringData);
                }
                textBlock.Text = builder.ToString();
                textBlock.FontSize = 4;
            });
        }

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

                textBlock.Width = attachedCard.Width;
                textBlock.LineHeight = 1;
                textBlock.TextWrapping = TextWrapping.Wrap;
                textBlock.TextAlignment = TextAlignment.Left;
                ScrollViewer sv = new ScrollViewer();
                sv.Width = attachedCard.Width;
                sv.Height = attachedCard.Height;
                sv.HorizontalAlignment = HorizontalAlignment.Center;
                sv.VerticalAlignment = VerticalAlignment.Center;
                sv.Content = textBlock;
                this.Children.Add(sv);
            });
        }
    }
}
