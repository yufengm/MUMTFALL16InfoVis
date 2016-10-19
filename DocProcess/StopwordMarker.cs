using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DocProcess
{
    class StopwordMarker
    {
        static string[] stopwords;
        /// <summary>
        /// Load the stopwords from the txt file
        /// </summary>
        internal static async Task<string[]> Load()
        {
            string[] lines = System.IO.File.ReadAllLines(@"stop_words.txt");
            List<string> stopwordlist = new List<string>();
            foreach (string line in lines)
            {
                string stemmedStopword = Stemmer.Stem(line.Trim());
                stopwordlist.Add(stemmedStopword);
            }
            stopwords = stopwordlist.ToArray();
            return stopwords;
        }
        /// <summary>
        /// Mark the stop words
        /// </summary>
        /// <param name="token"></param>
        internal static async void Mark(Token token)
        {
            if (stopwords == null)
            {
                stopwords = await Load();
            }
            foreach (String stopword in stopwords)
            {
                if (stopword.Equals(token.StemmedWord))
                {
                    token.WordType = WordType.STOPWORD;
                    break;
                }
            }
        }
    }
}
