using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoLocatedCardSystem.CollaborationWindow.TableModule;
using Windows.UI.Xaml.Controls;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;
using Windows.Foundation;
using Windows.UI.Core;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{ 
    class PlotCardController : CardController
    {
        PlotCardList list;
        public PlotCardController(CentralControllers ctrls) : base(ctrls)
        {
        }
        internal void Init() {
            list = new PlotCardList();
        }
        /// <summary>
        /// Destroy the card list
        /// </summary>
        internal void Deinit()
        {
            list.Clear();
        }
        /// <summary>
        /// Add card to layer
        /// </summary>
        /// <param name="attributeCards"></param>
        internal async void AddCard(AttributeCard[] attributeCards)
        {
            List<Point> dataPoints = new List<Point>();
            DataAttribute attr1 = attributeCards[0].Attribute;
            DataAttribute attr2 = attributeCards[1].Attribute;
            var x = this.Controllers.TableController.GetValueWithAttribute(attr1).ToArray();
            var y = this.Controllers.TableController.GetValueWithAttribute(attr2).ToArray();
            for (int i = 0, size = x.Count(); i < size; i++)
            {
                dataPoints.Add(new Point(x[i].Value, y[i].Value));
            }
            CoreDispatcher dispatcher = this.Controllers.PlotLayerController.GetDispatcher();
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                PlotCard pcard = new PlotCard(this);
                pcard.Init(Guid.NewGuid().ToString(), attributeCards[0].Owner, dataPoints);
                list.AddCard(pcard.CardID, pcard);
                await this.Controllers.PlotLayerController.LoadCard(pcard);
                Point posi = new Point((attributeCards[0].Position.X + attributeCards[1].Position.X) / 2,
                    (attributeCards[0].Position.Y + attributeCards[1].Position.Y) / 2);
                pcard.MoveTo(posi);
            });
        }
    }
}
