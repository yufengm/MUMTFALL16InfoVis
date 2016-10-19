using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocProcess
{
    class PunctuationMarker
    {
        /// <summary>
        /// Check if the token is a punctuation, and mark the token
        /// </summary>
        /// <param name="token"></param>
        internal static void Mark(Token token) {
            if (token.OriginalWord.Length == 1)
            {
                if (char.IsPunctuation((token.OriginalWord[0])) ||
                    token.OriginalWord[0] == ' ' ||
                    token.OriginalWord[0] == '\n' ||
                    token.OriginalWord[0] == '\r')
                {

                    token.WordType = WordType.IRREGULAR;
                }
            }
        }
    }
}
