using System;
using System.Collections.Concurrent;
using CoLocatedCardSystem.SecondaryWindow.CloudModule;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Microsoft.Graphics.Canvas.UI.Xaml;
using CoLocatedCardSystem.CollaborationWindow;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using System.Numerics;

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
                if (cnode.Type == CloudNode.NODETYPE.DOC)
                {
                    using (var cpb = new CanvasPathBuilder(args.DrawingSession))
                    {
                        Color nodeColor = UIHelper.HsvToRgb(cnode.SemanticNode.H,
                            cnode.SemanticNode.S,
                            cnode.User_search[User.ALEX] ? 1 : 0.5*cnode.SemanticNode.V);
                        cpb.BeginFigure(cnode.X + cnode.Weight / 2, cnode.Y + cnode.Weight / 2);
                        cpb.AddArc(new Vector2(cnode.X + cnode.Weight / 2, cnode.Y + cnode.Weight / 2),
                            cnode.Weight / 2, cnode.Weight / 2,
                            (float)Math.PI / 2, (float)(Math.PI));
                        cpb.EndFigure(CanvasFigureLoop.Closed);
                        args.DrawingSession.FillGeometry(CanvasGeometry.CreatePath(cpb), nodeColor);
                    }
                    using (var cpb = new CanvasPathBuilder(args.DrawingSession))
                    {
                        Color nodeColor = UIHelper.HsvToRgb(cnode.SemanticNode.H,
                            cnode.SemanticNode.S,
                            cnode.User_search[User.CHRIS] ? 1 : 0.5 * cnode.SemanticNode.V);
                        cpb.BeginFigure(cnode.X + cnode.Weight / 2, cnode.Y + cnode.Weight / 2);
                        cpb.AddArc(new Vector2(cnode.X + cnode.Weight / 2, cnode.Y + cnode.Weight / 2),
                            cnode.Weight / 2, cnode.Weight / 2,
                            (float)Math.PI / 2, (float)(-Math.PI));
                        cpb.EndFigure(CanvasFigureLoop.Closed);
                        args.DrawingSession.FillGeometry(CanvasGeometry.CreatePath(cpb), nodeColor);
                    }
                }
                else if (cnode.Type == CloudNode.NODETYPE.WORD)
                {
                    args.DrawingSession.DrawText(cnode.CloudText, cnode.X, cnode.Y, MyColor.Wheat);
                }
            }
        }

        internal void UpdateCloudNode(ConcurrentDictionary<string, CloudNode> cloudNodes)
        {
            this.cloudNodes = cloudNodes;
        }
    }
}
