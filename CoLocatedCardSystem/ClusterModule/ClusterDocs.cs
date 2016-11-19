using CoLocatedCardSystem.CollaborationWindow.DocumentModule;
using CoLocatedCardSystem.CollaborationWindow.InteractionModule;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace CoLocatedCardSystem.ClusterModule
{
    class ClusterDocs 
    {

        ConcurrentDictionary<string, ClusterWord> list = new ConcurrentDictionary<string, ClusterWord>();

        internal void AddWord(ClusterWord word)
        {
            ClusterWord target = null;
            foreach (ClusterWord cw in list.Values)
            {
                if (cw.StemmedText == word.StemmedText && cw.Group == word.Group)
                {
                    target = cw;
                }
            }
            if (target != null)
            {
                target.X = word.X;
                target.Y = word.Y;
                target.Weight = word.Weight;
            }
            else
            {
                list.TryAdd(word.Group+word.StemmedText, word);
            }
        }

        internal ClusterWord[] GetAllWords() {
            return list.Values.ToArray();
        }

        internal void RemoveWord(string text, string cardID)
        {
            ClusterWord removedWord;
            list.TryRemove(cardID + text, out removedWord);
        }
    }
}
