using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Input;
using Windows.UI.Input;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;
using Windows.Foundation;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    class PlotCard : Card
    {
        List<Point> list;
        ScatterPlot plot;
        public PlotCard(CardController cardController) : base(cardController)
        {

        }

        internal void Init(string cardID, User user, IEnumerable<Point> data)
        {
            base.Init(cardID, user);
            list = data.ToList();
            plot = new ScatterPlot();
            plot.SetData(data);
        }

        internal override async Task LoadUI()
        {
            await base.LoadUI();
            this.Width = 400;
            this.Height = 300;
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                SetBackground(Colors.Transparent);
                plot.Init(this.Width, this.Height);
                UIHelper.InitializeUI(new Point(-0.5 * this.Width, -0.5 * this.Height),
                0, 1,
                new Size(plot.Width, plot.Height),
                plot);
                //SetBackground(Colors.Transparent);
                this.Children.Add(plot);
            });
        }

        protected override void PointerDown(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint localPoint = e.GetCurrentPoint(this);
            PointerPoint globalPoint = e.GetCurrentPoint(Coordination.Baselayer);
            cardController.PointerDown(localPoint, globalPoint, this, typeof(PlotCard));
            base.PointerDown(sender, e);
        }

    }
}
