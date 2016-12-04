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

        internal async Task Init(string[] docs, SemanticGroupController semanticGroupController)
        {
            root = new SemanticGroup(semanticGroupController);
            var topics = await semanticGroupController.Controllers.MlController.GetTopicToken(docs, 1);
            KeyValuePair<Topic, List<string>> pair = topics.ElementAt(0);
            root.SetTopic(pair.Key);
            root.AddDoc(docs);
            AddSemanticGroup(root.Id, root);
            await root.GenBinaryTree(root.DocList, SemanticGroupController.PREFERRED_CLOUD_SIZE);
            ResetIndex();
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
        /// <summary>
        /// Merge the semantic group
        /// </summary>
        /// <param name="docIDs"></param>
        /// <returns></returns>
        internal async Task<bool> MergeGroup(string[] docIDs, SemanticGroupController semanticGroupController)
        {
            if (docIDs != null && docIDs.Length > 1)
            {
                //Find the node that contains both docIDs
                SemanticGroup group = FindCommonParent(docIDs);
                List<SemanticGroup> sgs = new List<SemanticGroup>();
                foreach (string docID in docIDs)
                {
                    SemanticGroup sg = GetSemanticGroupByDoc(docID);
                    if (!sgs.Contains(sg))
                    {
                        sgs.Add(sg);
                    }
                }
                if (sgs.Count < 2)
                {
                    return false;
                }
                //Get all docs in leaf nodes
                List<string> clusteredDocs = new List<string>();
                foreach (SemanticGroup sg in sgs)
                {
                    if (sg.IsLeaf)
                    {
                        foreach (string docID in sg.GetDocs())
                        {
                            clusteredDocs.Add(docID);
                        }
                    }
                }

                //Find the rest docs
                List<string> restDocs = new List<string>();
                foreach (string docID in group.GetDocs())
                {
                    if (!clusteredDocs.Contains(docID))
                    {
                        restDocs.Add(docID);
                    }
                }
                RemoveSemanticGroup(group);

                //Add the common node back
                AddSemanticGroup(group.Id, group);
                if (group.LeftChild != null)
                {
                    group.LeftChild.Deinit();
                }
                if (group.RightChild != null)
                {
                    group.RightChild.Deinit();
                }
                if (restDocs.Count > 0)
                {
                    //Merge the docs into the left node
                    group.LeftChild = new SemanticGroup(semanticGroupController);
                    ConcurrentDictionary<string, UserActionOnDoc> subGroup = group.GetSubDocList(clusteredDocs);
                    group.LeftChild.AddDoc(subGroup);
                    var topics = await semanticGroupController.Controllers.MlController.GetTopicToken(clusteredDocs.ToArray(), 1);
                    KeyValuePair<Topic, List<string>> pair = topics.ElementAt(0);
                    group.LeftChild.SetTopic(pair.Key);
                    group.LeftChild.Parent = group;
                    AddSemanticGroup(group.LeftChild.Id, group.LeftChild);
                    group.LeftChild.IsLeaf = true;

                    //Merge the rest docs into the right node
                    group.RightChild = new SemanticGroup(semanticGroupController);
                    subGroup = group.GetSubDocList(restDocs);
                    group.RightChild.AddDoc(subGroup);
                    topics = await semanticGroupController.Controllers.MlController.GetTopicToken(restDocs.ToArray(), 1);
                    pair = topics.ElementAt(0);
                    group.RightChild.SetTopic(pair.Key);
                    group.RightChild.Parent = group;
                    AddSemanticGroup(group.RightChild.Id, group.RightChild);
                    if (restDocs.Count > SemanticGroupController.PREFERRED_CLOUD_SIZE)
                    {
                        await group.RightChild.GenBinaryTree(group.DocList, SemanticGroupController.PREFERRED_CLOUD_SIZE);
                    }
                    else
                    {
                        group.RightChild.IsLeaf = true;
                    }
                    return true;
                }
                else
                {
                    group.IsLeaf = true;
                    return false;
                }
            }
            return false;
        }

        internal void AddSemanticGroup(string id, SemanticGroup group)
        {
            if (!list.Keys.Contains(id))
            {
                list.TryAdd(id, group);
            }
        }
        internal void ResetIndex()
        {
            var leaves = list.Values.Where(sn => sn.IsLeaf);
            int count = 1;
            foreach (SemanticGroup sg in leaves)
            {
                sg.Index = count;
                count++;
            }
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
