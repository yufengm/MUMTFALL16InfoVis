using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;

namespace CoLocatedCardSystem.CollaborationWindow.Layers.Glow_Layer
{
    class GlowInfo
    {
        private Size glowSize = new Size(130 * Screen.SCALE_FACTOR, 97.5 * Screen.SCALE_FACTOR);
        private Point glowPosition = new Point(0, 0);
        private double glowScale = 1;
        private double glowRotation = 0;
        private Color[] glowColors = new Color[] { Colors.Green,Colors.LightGreen, Colors.Yellow, Colors.Orange, Colors.Red };
        internal Size GlowSize
        {
            get
            {
                return glowSize;
            }
        }

        internal Point GlowPosition
        {
            get
            {
                return glowPosition;
            }
        }

        internal double GlowScale
        {
            get
            {
                return glowScale;
            }
        }

        internal double GlowRotation
        {
            get
            {
                return glowRotation;
            }
        }

        public Color[] GlowColors
        {
            get
            {
                return glowColors;
            }
        }

        internal static GlowInfo GetGlowInfo()
        {
            return new GlowInfo();
        }
    }
}
