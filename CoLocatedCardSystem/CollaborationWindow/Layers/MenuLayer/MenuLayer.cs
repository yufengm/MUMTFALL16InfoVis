using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace CoLocatedCardSystem.CollaborationWindow.Layers.Menu_Layer
{
    class MenuLayer : Canvas
    {
        MenuLayerController menulayerController;
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
        internal void Deinit() {
            this.Children.Clear();
        }
        /// <summary>
        /// Add a menubar to the layer
        /// </summary>
        /// <param name="menubar"></param>
        internal async void AddMenuBar(MenuBar menubar) {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>{
                this.Children.Add(menubar);
            });
        }
    }
}
