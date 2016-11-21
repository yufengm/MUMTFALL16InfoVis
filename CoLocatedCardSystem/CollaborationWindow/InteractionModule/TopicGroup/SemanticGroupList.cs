using CoLocatedCardSystem.CollaborationWindow.DocumentModule;
using CoLocatedCardSystem.CollaborationWindow.MachineLearningModule;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CoLocatedCardSystem.CollaborationWindow.InteractionModule.SemanticGroup;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    class SemanticGroupList
    {
        ConcurrentDictionary<string, SemanticGroup> list = new ConcurrentDictionary<string, SemanticGroup>();

        internal ConcurrentDictionary<string, SemanticGroup> List
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

        internal void Init(Document[] docs, MLController mlController)
        {
            foreach (KeyValuePair<string, Topic> pair in mlController.List)
            {
                SemanticGroup group = new SemanticGroup();
                group.Id = pair.Key;
                foreach (Token tk in pair.Value.List)
                {
                    SemanticAttribute sa = new SemanticAttribute();
                    sa.isHighlighted = false;
                    group.AddToken(tk, sa);
                }
                list.TryAdd(group.Id, group);
            }
            foreach (Document doc in docs)
            {
                Semantic semantic = new Semantic();
                semantic.DocID = doc.DocID;
                semantic.Created = false;
                semantic.Owner = User.NONE;
                string topicID = mlController.GetDefaultTopicIDByIndex(doc.GetDefaultTopicIndex());
                list[topicID].AddSemantic(semantic.DocID, semantic);
            }
        }

        internal void Deinit()
        {
            throw new NotImplementedException();
        }
    }
}
