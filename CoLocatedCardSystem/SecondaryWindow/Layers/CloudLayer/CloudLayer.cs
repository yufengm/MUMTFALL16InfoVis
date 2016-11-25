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
using Windows.Foundation;

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
                    Color nodeColor = MyColor.Yellow;
                    if (cnode.User_action[User.NONE].select)
                    {
                        args.DrawingSession.FillCircle(new Vector2(cnode.X + cnode.Weight / 2, cnode.Y + cnode.Weight / 2),
                            cnode.Weight / 2, nodeColor);
                    }
                    DrawArc(args, cnode, User.ALEX);
                    DrawArc(args, cnode, User.BEN);
                    DrawArc(args, cnode, User.CHRIS);
                }
                else if (cnode.Type == CloudNode.NODETYPE.WORD)
                {
                    args.DrawingSession.DrawText(cnode.CloudText, cnode.X, cnode.Y, cnode.User_action[User.NONE].default_color);
                    args.DrawingSession.DrawRectangle(new Rect(cnode.X, cnode.Y, cnode.W, cnode.H), Colors.White);
                }
            }
        }

        internal void UpdateCloudNode(ConcurrentDictionary<string, CloudNode> cloudNodes)
        {
            this.cloudNodes = cloudNodes;
        }

        private void DrawArc(CanvasDrawEventArgs args, CloudNode cnode, User user)
        {
            using (var cpb = new CanvasPathBuilder(args.DrawingSession))
            {
                Color nodeColor = Colors.White;
                if (cnode.User_action[user].searched)
                {
                    nodeColor = cnode.User_action[user].highlight_color;
                }
                else
                {
                    nodeColor = cnode.User_action[user].default_color;
                }
                cpb.BeginFigure(cnode.X + cnode.Weight / 2, cnode.Y + cnode.Weight / 2);
                float startArc = 0;
                float swap = 0;
                switch (user)
                {
                    case User.ALEX:
                        startArc = (float)-Math.PI / 2;
                        swap = (float)(-Math.PI * 2 / 3);
                        break;
                    case User.BEN:
                        startArc = (float)(Math.PI / 6);
                        swap = (float)(Math.PI * 2 / 3);
                        break;
                    case User.CHRIS:
                        startArc = (float)-Math.PI / 2;
                        swap = (float)(Math.PI * 2 / 3);
                        break;
                }
                cpb.AddArc(new Vector2(cnode.X + cnode.Weight / 2, cnode.Y + cnode.Weight / 2),
                    cnode.Weight / 2 - 2, cnode.Weight / 2 - 2,
                    startArc, swap);
                cpb.EndFigure(CanvasFigureLoop.Closed);
                args.DrawingSession.FillGeometry(CanvasGeometry.CreatePath(cpb), nodeColor);
            }
        }
    }
}
