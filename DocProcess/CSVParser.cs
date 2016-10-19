using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocProcess
{
    class CSVParser
    {
        public static string[] Parse(string csvText)
        {
            List<string> tokens = new List<string>();

            int last = -1;
            int current = 0;
            bool inText = false;

            while (current < csvText.Length)
            {
                switch (csvText[current])
                {
                    case '"':
                        inText = !inText; break;
                    case ',':
                        if (!inText)
                        {
                            tokens.Add(csvText.Substring(last + 1, (current - last)).Trim(' ', ','));
                            last = current;
                        }
                        break;
                    default:
                        break;
                }
                current++;
            }

            if (last != csvText.Length - 1)
            {
                tokens.Add(csvText.Substring(last + 1).Trim());
            }

            return tokens.ToArray();
        }
    }
}
