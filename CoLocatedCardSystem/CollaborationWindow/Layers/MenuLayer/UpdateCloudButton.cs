using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace CoLocatedCardSystem.CollaborationWindow.Layers.Menu_Layer
{
    class UpdateCloudButton:Button
    {
        Image activeImage;
        public void Init(Uri activeImgUri)
        {
            this.activeImage = new Image();
            activeImage.Source = new BitmapImage(activeImgUri);
            activeImage.VerticalAlignment = VerticalAlignment.Center;
            this.Content = activeImage;          
        }
    }
}
