using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoLocatedCardSystem.SecondaryWindow.CloudModule;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using CoLocatedCardSystem.CollaborationWindow;
using Windows.UI;
using CoLocatedCardSystem.ClusterModule;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.Graphics.Canvas.Brushes;

namespace CoLocatedCardSystem.SecondaryWindow.Layers
{
    class CloudLayer : Canvas
    {
        CloudLayerController cloudLayerController;
        CanvasControl canvas;
        ConcurrentDictionary<string, CloudNode> cloudNodes = new ConcurrentDictionary<string, CloudNode>();
        internal CloudLayer(CloudLayerController ctrls)
        {
            this.cloudLayerController = ctrls;
        }
        internal void Init(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            canvas = new CanvasControl();
            canvas.Width = this.Width;
            canvas.Height = this.Height;
            this.Children.Add(canvas);
            canvas.Draw += Canvas_Draw;
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
            foreach (CloudNode cnode in cloudNodes.Values)
            {
                Color nodeColor = cnode.SemanticNode.Color;
                nodeColor.A = cnode.Alpha;
                if (cnode.Type == NODETYPE.DOC)
                {
                    args.DrawingSession.FillEllipse(
                        cnode.X + cnode.Weight / 2, 
                        cnode.Y + cnode.Weight / 2, 
                        cnode.Weight / 2, 
                        cnode.Weight / 2, nodeColor);
                }
                if (cnode.Type == NODETYPE.WORD)
                {
                    args.DrawingSession.DrawText(cnode.CloudText, cnode.X, cnode.Y, nodeColor);
                }
            }
        }

        internal void UpdateCloudNode(ConcurrentDictionary<string, CloudNode> cloudNodes)
        {
            this.cloudNodes = cloudNodes;
        }
    }
}
