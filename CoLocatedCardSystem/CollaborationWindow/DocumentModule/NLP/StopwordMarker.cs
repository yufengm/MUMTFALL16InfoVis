using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace CoLocatedCardSystem.CollaborationWindow.DocumentModule
{
    class StopwordMarker
    {
        static string[] stopwords;
        /// <summary>
        /// Load the stopwords from the txt file
        /// </summary>
        internal static async Task<string[]> Load()
        {
            StorageFolder folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            StorageFile file = await folder.GetFileAsync(FilePath.StopWords);
            List<string> stopwordlist = new List<string>();
            var lines = await FileIO.ReadLinesAsync(file);
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
