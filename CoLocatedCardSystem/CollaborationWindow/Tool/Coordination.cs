using System;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace CoLocatedCardSystem.CollaborationWindow
{
    class Coordination
    {
        static FrameworkElement baselayer;

        public static FrameworkElement Baselayer
        {
            get
            {
                return baselayer;
            }

            set
            {
                baselayer = value;
            }
        }

        /// <summary>
        /// Check if the point falls in the element, isCentered denotes whether the 0 point of the
        /// object is in the center or the top left corner
        /// </summary>
        /// <param name="point"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool IsIntersect(Point point, Point[] points)
        {
            Point[] polygon = points;
            bool isInside = false;
            for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
            {
                if (((polygon[i].Y > point.Y) != (polygon[j].Y > point.Y)) &&
                (point.X < (polygon[j].X - polygon[i].X) * (point.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X))
                {
                    isInside = !isInside;
                }
            }
            return isInside;
        }
        /// <summary>
        /// Check if a circle is intersected with points
        /// </summary>
        /// <param name="point"></param>
        /// <param name="radius"></param>
        /// <param name="points"></param>
        /// <returns></returns>
        public static bool IsIntersect(Point point, double radius, Point[] points)
        {
            Point[] polygon = points;
            if (polygon != null && polygon.Length > 0)
            {
                for (int i = 0; i < polygon.Length; i++)
                {
                    if (Math.Sqrt(Math.Pow(polygon[i].X - point.X, 2) + Math.Pow(polygon[i].Y - point.Y, 2)) < radius)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Checks if the two polygons are intersecting.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool IsIntersect(Point[] points1, Point[] points2)
        {
            foreach (var polygon in new[] { points1, points2 })
            {
                for (int i1 = 0; i1 < polygon.Length; i1++)
                {
                    int i2 = (i1 + 1) % polygon.Length;
                    var p1 = polygon[i1];
                    var p2 = polygon[i2];

                    var normal = new Point(p2.Y - p1.Y, p1.X - p2.X);

                    double? minA = null, maxA = null;
                    foreach (var p in points1)
                    {
                        var projected = normal.X * p.X + normal.Y * p.Y;
                        if (minA == null || projected < minA)
                            minA = projected;
                        if (maxA == null || projected > maxA)
                            maxA = projected;
                    }

                    double? minB = null, maxB = null;
                    foreach (var p in points2)
                    {
                        var projected = normal.X * p.X + normal.Y * p.Y;
                        if (minB == null || projected < minB)
                            minB = projected;
                        if (maxB == null || projected > maxB)
                            maxB = projected;
                    }

                    if (maxA < minB || maxB < minA)
                        return false;
                }
            }
            return true;
        }
    }
}
