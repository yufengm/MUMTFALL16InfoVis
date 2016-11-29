using CoLocatedCardSystem.CollaborationWindow.DocumentModule;
using CoLocatedCardSystem.CollaborationWindow.MachineLearningModule;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CoLocatedCardSystem.CollaborationWindow.InteractionModule.SemanticGroup;
using static CoLocatedCardSystem.CollaborationWindow.InteractionModule.Topic;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    class SemanticGroupList
    {
        ConcurrentDictionary<string, SemanticGroup> list = new ConcurrentDictionary<string, SemanticGroup>();// key is the topic id
        internal async Task Init(string[] docs, MLController mlController)
        {
            SemanticGroup root = new SemanticGroup();
            var topics = await mlController.GetTopicToken(docs, 1);
            KeyValuePair<Topic, List<string>> pair = topics.ElementAt(0);
            root.SetTopic(pair.Key);
            root.AddDoc(pair.Value);
            list.TryAdd(root.Id, root);

            await root.GenBinaryTree(docs, mlController, list);
            //var topics = await mlController.GetTopicToken(docs, 2);
            //foreach (KeyValuePair<Topic,List<string>> pair in topics)
            //{
            //    SemanticGroup sg = new SemanticGroup();
            //    sg.SetTopic(pair.Key);
            //    list.TryAdd(pair.Key.Id, sg);
            //    foreach (string doc in pair.Value)
            //    {
            //        list[pair.Key.Id].AddDoc(doc);
            //    }
            //}
        }

        internal IEnumerable<SemanticGroup> GetSemanticGroup()
        {
            return list.Values;
        }

        internal void Deinit()
        {
            throw new NotImplementedException();
        }

        internal void SetSearchResult(string[] docIDs, User owner, bool searched)
        {
            foreach (SemanticGroup sg in list.Values) {
                foreach (string docID in docIDs) {
                    sg.SetDocSearched(docID, owner, searched);
                }
            }
        }
    }
}
