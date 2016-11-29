using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DocProcess
{
    class IrregularMarker
    {
        /// <summary>
        /// Mark the non-work type
        /// </summary>
        /// <param name="token"></param>
        internal static void Mark(Token token) {
            if (!Regex.IsMatch(token.StemmedWord, @"^[a-zA-Z]+$") 
                || token.StemmedWord.Length == 0 
                || token.StemmedWord.Equals(" "))
            {
                token.WordType = WordType.IRREGULAR;
            }
        }
    }
}
