using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.SecondaryWindow.Tool
{
    class ColorPicker
    {
        static int index = 0;
        static int[] H = new int[] { 345, 131, 59, 302, 174, 18, 309, 150, 44, 330, 204, 52 };

        internal static int GetColorHue()
        {
            int h = H[index];
            index = (index + 1) % H.Length;
            return h;
        }
    }
}
