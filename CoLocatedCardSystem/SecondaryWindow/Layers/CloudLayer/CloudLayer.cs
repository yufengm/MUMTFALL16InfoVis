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
using Microsoft.Graphics.Canvas.Text;
using CoLocatedCardSystem.SecondaryWindow.SemanticModule;
using CoLocatedCardSystem.SecondaryWindow.Tool;
using Microsoft.Graphics.Canvas;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using System.IO;

namespace CoLocatedCardSystem.SecondaryWindow.Layers
{
    class CloudLayer : Canvas
    {
        CloudLayerController cloudLayerController;
        CanvasControl canvas;
        ConcurrentDictionary<string, CloudNode> cloudNodes = new ConcurrentDictionary<string, CloudNode>();
        Dictionary<string, CanvasBitmap> loadedImage = new Dictionary<string, CanvasBitmap>();
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
            canvas.CreateResources += Canvas_CreateResources;

        }

        private async void Canvas_CreateResources(CanvasControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            StorageFolder assetsFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            StorageFile file = await assetsFolder.GetFileAsync(FilePath.CSVImage);
            using (var inputStream = await file.OpenReadAsync())
            using (var classicStream = inputStream.AsStreamForRead())
            using (var streamReader = new StreamReader(classicStream))
            {
                while (streamReader.Peek() >= 0)
                {
                    string line = streamReader.ReadLine();
                    string s = line.Split(',')[0];
                    CanvasBitmap bitMap = await CanvasBitmap.LoadAsync(sender, new Uri(@"ms-appx:///Assets/review/" + s));
                    if (!loadedImage.ContainsKey(s))
                    {
                        loadedImage.Add(s, bitMap);
                    }
                }
            }
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
                if (cnode.Type == CloudNode.NODETYPE.PICTURE)
                {
                    if (loadedImage.ContainsKey(cnode.Image))
                    {
                        CanvasBitmap cb = loadedImage[cnode.Image];
                        Size size = cb.Size;
                        Rect imgBound = new Rect();
                        if (size.Width > size.Height)
                        {
                            imgBound.Height = cnode.Weight * 3;
                            imgBound.Width = imgBound.Height * size.Width / size.Height;
                        }
                        else
                        {
                            imgBound.Width = cnode.Weight * 3;
                            imgBound.Height = imgBound.Width * size.Height / size.Width;
                        }
                        cnode.W = (float)imgBound.Height;
                        cnode.H = (float)imgBound.Width;
                        imgBound.X = cnode.X;
                        imgBound.Y = cnode.Y;
                        args.DrawingSession.DrawImage(cb, imgBound);
                    }
                }
            }
            foreach (CloudNode cnode in cloudNodes.Values)
            {
                if (cnode.Type == CloudNode.NODETYPE.WORD)
                {
                    Color nodeColor = Colors.White;
                    SemanticNode semantic = cnode.SemanticNode;
                    nodeColor = ColorPicker.HsvToRgb(semantic.H, 0.75, 0.75);
                    CanvasTextFormat format = new CanvasTextFormat();
                    format.FontSize = cnode.Weight;
                    format.FontStretch = Windows.UI.Text.FontStretch.Expanded;
                    format.HorizontalAlignment = CanvasHorizontalAlignment.Center;
                    args.DrawingSession.DrawText(cnode.CloudText,
                        new Rect(cnode.X, cnode.Y, cnode.W, cnode.H),
                        nodeColor,
                        format);
                }

            }
            foreach (CloudNode cnode in cloudNodes.Values)
            {
                if (cnode.SemanticNode != null)
                {
                    if (cnode.Type == CloudNode.NODETYPE.DOC)
                    {
                        Color nodeColor = MyColor.Yellow;
                        Color ringColor = MyColor.Yellow;
                        foreach (User user in Enum.GetValues(typeof(User)))
                        {
                            if (cnode.UserActionOnDoc != null && cnode.UserActionOnDoc.Active[user])
                            {
                                ringColor = ColorPicker.HsvToRgb(60, 1, 0.6);
                                args.DrawingSession.FillCircle(new Vector2(cnode.X + cnode.Weight / 2, cnode.Y + cnode.Weight / 2),
                                    cnode.Weight / 2, ringColor);
                                break;
                            }
                        }
                        DrawArc(args, cnode, User.ALEX);
                        DrawArc(args, cnode, User.BEN);
                        DrawArc(args, cnode, User.CHRIS);
                        foreach (User user in Enum.GetValues(typeof(User)))
                        {
                            if (cnode.UserActionOnDoc.Touched[user])
                            {
                                ringColor = ColorPicker.HsvToRgb(60, 1, 1);
                                args.DrawingSession.FillCircle(new Vector2(cnode.X + cnode.Weight / 2, cnode.Y + cnode.Weight / 2),
                                    cnode.Weight / 2, ringColor);
                                break;
                            }
                        }
                    }
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
                SemanticNode semantic = cnode.SemanticNode;
                if (semantic.UserActionOnDoc.Searched[user])
                {
                    nodeColor = ColorPicker.HsvToRgb(semantic.H, 0.75, 0.75);
                }
                else
                {
                    nodeColor = ColorPicker.HsvToRgb(semantic.H, 0.5, 0.5);
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
