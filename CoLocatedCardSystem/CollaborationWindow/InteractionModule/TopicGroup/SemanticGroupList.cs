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
        SemanticGroup root;

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

        internal async Task Init(string[] docs, MLController mlController)
        {
            root = new SemanticGroup();
            var topics = await mlController.GetTopicToken(docs, 1);
            KeyValuePair<Topic, List<string>> pair = topics.ElementAt(0);
            root.SetTopic(pair.Key);
            root.AddDoc(docs);
            list.TryAdd(root.Id, root);
            await root.GenBinaryTree(root.DocList, mlController, list, SemanticGroupController.PREFERRED_CLOUD_SIZE);
        }

        internal IEnumerable<SemanticGroup> GetSemanticGroup()
        {
            return list.Values;
        }

        internal SemanticGroup GetSemanticGroupByDoc(string docID)
        {
            foreach (SemanticGroup sg in list.Values)
            {
                if (sg.IsLeaf && sg.HasDoc(docID))
                {
                    return sg;
                }
            }
            return null;
        }

        internal SemanticGroup GetSemanticGroupById(string id)
        {
            if (list.Keys.Contains(id))
            {
                return list[id];
            }
            else
            {
                return null;
            }
        }
        internal void Deinit()
        {
            throw new NotImplementedException();
        }

        internal void SetSearchResult(string[] docIDs, User owner, bool searched)
        {
            //foreach (SemanticGroup sg in list.Values) {
            //    if (sg.IsLeaf)
            //    {
            //        foreach (string docID in docIDs)
            //        {
            //            sg.SetDocSearched(docID, owner, searched);
            //        }
            //    }
            //}
            foreach (string docID in docIDs)
            {
                root.SetDocSearched(docID, owner, searched);
            }
        }

        internal void SetActiveResult(string[] docIDs, User owner, bool searched)
        {
            foreach (SemanticGroup sg in list.Values)
            {
                if (sg.IsLeaf)
                {
                    foreach (string docID in docIDs)
                    {
                        sg.SetDocActive(docID, owner, searched);
                    }
                }
            }
        }

        internal SemanticGroup FindCommonParent(string[] docIDs)
        {
            return root.FindCommonParent(docIDs);
        }

        internal void RemoveSemanticGroup(SemanticGroup group)
        {
            SemanticGroup sg;
            if (group.IsLeaf)
            {
                list.TryRemove(group.Id, out sg);
            }
            else
            {
                RemoveSemanticGroup(group.LeftChild);
                RemoveSemanticGroup(group.RightChild);
                list.TryRemove(group.Id, out sg);
            }
        }

        internal void SetTouchResult(string docID, User owner, bool value)
        {
            foreach (SemanticGroup sg in list.Values)
            {
                if (sg.IsLeaf)
                {
                    sg.SetDocTouched(docID, owner, value);
                }
            }
        }
    }
}
