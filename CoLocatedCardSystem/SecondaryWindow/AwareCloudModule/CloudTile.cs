using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;

namespace CoLocatedCardSystem.SecondaryWindow.AwareCloudModule
{
    class CloudTile
    {
        CLOUDTILETYPE type = CLOUDTILETYPE.WORD;
        string text="";
        Rect bound=new Rect();
        Point position=new Point();
        Color color = Colors.Black;
    }
}
