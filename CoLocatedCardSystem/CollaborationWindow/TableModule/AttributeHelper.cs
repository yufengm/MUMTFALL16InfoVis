using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.CollaborationWindow.TableModule
{
    static class AttributeHelper
    {
        internal static ATTRIBUTETYPE ParseType(string str) {
            if (IsDigitsOnly(str))
            {
                return ATTRIBUTETYPE.Numerical;
            }
            else if (IsDate(str))
            {
                return ATTRIBUTETYPE.Time;
            }
            else
            {
                return ATTRIBUTETYPE.Categorical;
            }
        }

        /// <summary>
        /// Checks to see if the cells in the second row are digits or string values
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static bool IsDigitsOnly(string str)
        {
            double i;
            return double.TryParse(str, out i);
        }
        /// <summary>
        /// Checks to see if the cells in the second row are dates
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static bool IsDate(String str)
        {
            DateTime dateTime;
            return DateTime.TryParse(str, out dateTime);
        }
    }
}
