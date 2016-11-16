using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoLocatedCardSystem.CollaborationWindow.InteractionModule;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

namespace CoLocatedCardSystem.CollaborationWindow.Layers.Menu_Layer
{
    class RecycleBin : Canvas
    {
        double radius = 150 * Screen.SCALE_FACTOR;
        ImageBrush closeTrashBin = new ImageBrush();
        ImageBrush openTrashBin = new ImageBrush();
        Ellipse bg;
        Point position;
        public double Radius
        {
            get
            {
                return radius;
            }

            set
            {
                radius = value;
            }
        }

        internal void Init(double x, double y)
        {
            position = new Point(x, y);
            bg = new Ellipse();

            closeTrashBin.ImageSource = new BitmapImage(new Uri(@"ms-appx:///Assets/trashIcon-1.png"));
            openTrashBin.ImageSource = new BitmapImage(new Uri(@"ms-appx:///Assets/trashIcon-2.png"));
            bg.Fill = closeTrashBin;
            UIHelper.InitializeUI(new Point(-radius, -radius),
                0, 1,
                new Size(2 * radius, 2 * radius),
                bg);
            UIHelper.InitializeUI(new Point(x, y),
                0, 1,
                new Size(2 * radius, 2 * radius),
                this);
            this.Children.Add(bg);
        }

        internal async void Open()
        {
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () =>
            {
                bg.Fill = this.openTrashBin;
            });
        }
        internal async void Close()
        {
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () =>
            {
                bg.Fill = this.closeTrashBin;
            });
        }

        internal bool intersect(CardStatus status)
        {
            return Coordination.IsIntersect(position, radius, status.corners);
        }
    }
}
