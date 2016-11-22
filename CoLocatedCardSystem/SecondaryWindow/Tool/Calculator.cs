using CoLocatedCardSystem.ClusterModule;
using CoLocatedCardSystem.SecondaryWindow.CloudModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace CoLocatedCardSystem.SecondaryWindow
{
    class Calculator
    {
        internal static double Distance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }
        internal static double Distance(CloudNode node1, CloudNode node2)
        {
            var result = Math.Sqrt(Math.Pow(node1.X + node1.W / 2 - (node2.X + node2.W / 2), 2)
                        + Math.Pow(node1.Y + node1.H / 2 - (node2.Y + node2.H / 2), 2));
            return result;
        }

        internal static Point NodeNodeIntersect(CloudNode node1, CloudNode node2)
        {
            double deltax = 0;
            double deltay = 0;
            double r1x = node1.X,
                r1y = node1.Y,
                r1w = node1.W,
                r1h = node1.H,
                r2x = node2.X,
                r2y = node2.Y,
                r2w = node2.W,
                r2h = node2.H;
            if (node1.Type == NODETYPE.DOC && node2.Type == NODETYPE.DOC)
            {
                var dist = Distance(r1x, r1y, r2x, r2y);
                if (dist < (r1w + r2w) / 2)
                {
                    double ol = (r1w + r2w) / 2 - dist;
                    deltax = Math.Abs(r1x - r2x) * ol / dist;
                    deltay = Math.Abs(r1y - r2y) * ol / dist;
                }
            }
            else
            {
                if (r1x + r1w < r2x)
                {
                    deltax = 0;
                }
                else if (r1x + r1w > r2x && r1x < r2x)
                {
                    deltax = r1x + r1w - r2x;
                }
                else if (r1x + r1w < r2x + r2w && r1x > r2x)
                {
                    deltax = r1w;
                }
                else if (r1x + r1w > r2x + r2w && r1x < r2x + r2w)
                {
                    deltax = r2x + r2w - r1x;
                }
                else if (r1x > r2x + r2w)
                {
                    deltax = 0;
                }
                else if (r1x < r2x && r1x + r1w > r2x + r2w)
                {
                    deltax = r2w;
                }
                else if (r1x == r2x && r1x + r1w == r2x + r2w)
                {
                    deltax = r2w;
                }

                if (r1y + r1h < r2y)
                {
                    deltay = 0;
                }
                else if (r1y + r1h > r2y && r1y < r2y)
                {
                    deltay = r1y + r1h - r2y;
                }
                else if (r1y + r1h < r2y + r2h && r1y > r2y)
                {
                    deltay = r1h;
                }
                else if (r1y + r1h > r2y + r2h && r1y < r2y + r2h)
                {
                    deltay = r2y + r2h - r1y;
                }
                else if (r1y > r2y + r2h)
                {
                    deltay = 0;
                }
                else if (r1y < r2y && r1y + r1h > r2y + r2h)
                {
                    deltay = r2h;
                }
                else if (r1y == r2y && r1y + r1h == r2y + r2h)
                {
                    deltay = r2h;
                }
            }
            return new Point(deltax, deltay);
        }
    }
}
