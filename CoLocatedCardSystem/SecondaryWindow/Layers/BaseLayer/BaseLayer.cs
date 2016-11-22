using CoLocatedCardSystem.ClusterModule;
using CoLocatedCardSystem.SecondaryWindow.CloudModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace CoLocatedCardSystem.SecondaryWindow.Layers
{
    class BaseLayer:Canvas
    {
        BaseLayerController baseLayerController;
        Rectangle background;
        Random rand = new Random();
        internal BaseLayer(BaseLayerController ctrls) {
            this.baseLayerController = ctrls;
        }

        internal void Init(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            background = new Rectangle();
            background.Width = width;
            background.Height = height;
            background.Fill = new SolidColorBrush(MyColor.DarkBlue);
            this.Children.Add(background);
        }       
    }
}
