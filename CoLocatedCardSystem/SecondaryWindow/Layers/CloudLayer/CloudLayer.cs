using System;
using System.Collections.Concurrent;
using CoLocatedCardSystem.SecondaryWindow.CloudModule;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Microsoft.Graphics.Canvas.UI.Xaml;

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
