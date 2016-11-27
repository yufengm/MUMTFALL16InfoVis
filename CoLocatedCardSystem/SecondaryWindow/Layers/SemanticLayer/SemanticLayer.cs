using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoLocatedCardSystem.SecondaryWindow.SemanticModule;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using CoLocatedCardSystem.CollaborationWindow;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.UI;

namespace CoLocatedCardSystem.SecondaryWindow.Layers
{
    class SemanticLayer : Canvas
    {
        SemanticLayerController semanticLayerController;

        CanvasControl canvas;
        ConcurrentDictionary<string, SemanticNode> semanticNodes = new ConcurrentDictionary<string, SemanticNode>();
        internal SemanticLayer(SemanticLayerController ctrls)
        {
            this.semanticLayerController = ctrls;
        }
        internal void Init(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            canvas = new CanvasControl();
            canvas.Width = this.Width;
            canvas.Height = this.Height;
            this.Children.Add(canvas);
            canvas.Draw += Canvas_Draw; ;
            canvas.ClearColor = Colors.Transparent;
        }
        internal async void Update()
        {
            await canvas.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                canvas.Invalidate();
            });
        }
        private void Canvas_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            foreach (SemanticNode snode in semanticNodes.Values)
            {
                args.DrawingSession.FillCircle(snode.X, snode.Y, 5, MyColor.Wheat);
                foreach (SemanticNode csnode in snode.Connections)
                {
                    args.DrawingSession.DrawLine(snode.X, snode.Y, csnode.X, csnode.Y, MyColor.Wheat);
                }
            }
        }

        internal void UpdateSemanticNode(ConcurrentDictionary<string, SemanticNode> semanticNodes)
        {
            this.semanticNodes = semanticNodes;
        }
    }
}
