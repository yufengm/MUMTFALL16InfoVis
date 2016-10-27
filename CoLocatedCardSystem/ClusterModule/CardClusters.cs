using CoLocatedCardSystem.CollaborationWindow.DocumentModule;
using CoLocatedCardSystem.CollaborationWindow.InteractionModule;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.ClusterModule

{
    class CardClusters
    {
        ConcurrentBag<ClusterDoc[]> list =new ConcurrentBag<ClusterDoc[]>();
        ConcurrentBag<ClusterDoc[]> bufferlist = new ConcurrentBag< ClusterDoc[]>();

        internal ConcurrentBag<ClusterDoc[]> List
        {
            get
            {
                return list;
            }

            set
            {
                list = value;
            }
        }

        internal ConcurrentBag<ClusterDoc[]> Bufferlist
        {
            get
            {
                return bufferlist;
            }

            set
            {
                bufferlist = value;
            }
        }

        internal void SetCluster(Document[][] docs, CardStatus[][] states)
        {           
            if (docs == null || states == null || docs.Length != states.Length)
            {
                return;
            }
            bufferlist = list;
            list = new ConcurrentBag<ClusterDoc[]>();
            lock (list)
            {
                for (int i = 0, sizei = docs.Length; i < sizei; i++)
                {
                    List<ClusterDoc> temp = new List<ClusterDoc>();
                    for (int j = 0, sizej = docs[i].Length; j < sizej; j++)
                    {
                        ClusterDoc cDoc = new ClusterDoc();
                        cDoc.DocID = docs[i][j].DocID;
                        cDoc.RawDocument = docs[i][j].RawDocument;
                        cDoc.ProcessedDocument = docs[i][j].ProcessedDocument;
                        cDoc.Status = states[i][j];
                        temp.Add(cDoc);
                    }
                    list.Add(temp.ToArray());
                }
            }
        }

    }
}
