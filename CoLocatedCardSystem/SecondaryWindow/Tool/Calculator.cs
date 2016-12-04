using CoLocatedCardSystem.SecondaryWindow.CloudModule;
using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Collections.Generic;

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
            if (node1.Type == CloudNode.NODETYPE.DOC && node2.Type == CloudNode.NODETYPE.DOC)
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

        internal static double Map(double value, double x1, double x2, double y1, double y2)
        {
            if (value < x1) {
                return y1;
            }
            if (value > x1) {
                return y2;
            }
            return (value - x1) * (y2 - y1) / (x2 - x1) + y1;
        }

        internal static Size GetBoundingSize(string cloudText, float weight)
        {
            TextBlock tb = new TextBlock { Text = cloudText, FontSize = weight };
            tb.Padding = new Thickness(0.5, 0.5, 0.5, 0);
            tb.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            Size boundSize = new Size(tb.DesiredSize.Width * 1.2, tb.DesiredSize.Height);
            return boundSize;
        }

        internal static double[] CalAvgVector(IEnumerable<double[]> vectors)
        {
            double[] result = null;
            double count = 0;
            foreach (double[] vector in vectors) {
                count++;
                if (result == null)
                {
                    result = vector;
                }
                else {
                    for (int i = 0; i < result.Length; i++) {
                        result[i] += vector[i];
                    }
                }
            }
            for (int i = 0; i < result.Length; i++)
            {
                result[i] /=count;
            }
            return result;
        }

        internal static double CalDistance(double[] v1, double[] v2)
        {
            double sim = 0.0d;
            int N = 0;
            N = ((v2.Length < v1.Length) ? v2.Length : v1.Length);
            double dot = 0.0d;
            double mag1 = 0.0d;
            double mag2 = 0.0d;
            for (int n = 0; n < N; n++)
            {
                dot += v1[n] * v2[n];
                mag1 += Math.Pow(v1[n], 2);
                mag2 += Math.Pow(v2[n], 2);
            }

            return dot / (Math.Sqrt(mag1) * Math.Sqrt(mag2));
        }
    }
}
