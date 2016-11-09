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

        ConcurrentBag<ClusterWord> list = new ConcurrentBag<ClusterWord>();

        internal void AddWord(ClusterWord word)
        {
            ClusterWord target = null;
            try
            {
                target = list.Single(a => a.Text == word.Text && a.Group == word.Group);
                target.X = word.X;
                target.Y = word.Y;
                target.Weight = word.Weight;
            }
            catch (Exception ex)
            {
                list.Add(word);
            }
        }

        internal ClusterWord[] GetAllWords() {
            return list.ToArray();
        }
    }
}
