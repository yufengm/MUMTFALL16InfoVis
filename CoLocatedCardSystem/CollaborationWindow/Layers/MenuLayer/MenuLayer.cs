using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoLocatedCardSystem.CollaborationWindow.InteractionModule;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace CoLocatedCardSystem.CollaborationWindow.Layers.Menu_Layer
{
    class MenuLayer : Canvas
    {
        MenuLayerController menulayerController;
        RecycleBin[] recycleBinList;
        internal MenuLayer(MenuLayerController mlcltr)
        {
            this.menulayerController = mlcltr;
        }
        /// <summary>
        /// Initialize the menu layer.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        internal void Init(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }
        internal void Deinit()
        {
            this.Children.Clear();
        }
        /// <summary>
        /// Add a menubar to the layer
        /// </summary>
        /// <param name="menubar"></param>
        internal async void AddMenuBar(MenuBar menubar)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                this.Children.Add(menubar);
            });
        }
        /// <summary>
        /// Add recycle bin to the layer
        /// </summary>
        /// <param name="position"></param>
        internal async void AddRecycleBin(Point[] position) {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                recycleBinList = new RecycleBin[position.Length];
                for (int i = 0; i < position.Length; i++)
                {
                    recycleBinList[i] = new RecycleBin();
                    recycleBinList[i].Init(position[i].X, position[i].Y);
                    this.Children.Add(recycleBinList[i]);
                }
            });
        }

        internal bool IsIntersectWithDeleteBin(CardStatus status)
        {
            foreach (RecycleBin rb in recycleBinList) {
                if (rb.intersect(status)) {
                    return true;
                }
            }
            return false;
        }
    }
}
