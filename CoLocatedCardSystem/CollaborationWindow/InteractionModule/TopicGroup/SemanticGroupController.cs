using CoLocatedCardSystem.CollaborationWindow.DocumentModule;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Input;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    class SemanticGroupController
    {
        CentralControllers controllers;
        CardGroupList cardList;//card clusters
        SemanticGroupList semanticList;//semantic groups
        internal static int PREFERRED_CLOUD_SIZE = 30;

        public CentralControllers Controllers
        {
            get
            {
                return controllers;
            }

            set
            {
                controllers = value;
            }
        }

        internal SemanticGroupList SemanticList
        {
            get
            {
                return semanticList;
            }

            set
            {
                semanticList = value;
            }
        }

        internal SemanticGroupController(CentralControllers ctrls)
        {
            this.controllers = ctrls;
        }

        internal async Task Init()
        {
            cardList = new CardGroupList(this);
            cardList.Init();
            semanticList = new SemanticGroupList();
            string[] docs = controllers.DocumentController.GetDocumentIDs();
            await semanticList.Init(docs, this);
        }

        internal void Deinit()
        {
            cardList.Deinit();
            semanticList.Deinit();
        }

        /// <summary>
        /// Get all the semantic groups
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<SemanticGroup> GetSemanticGroup()
        {
            return semanticList.GetSemanticGroup();
        }
        internal SemanticGroup GetSemanticGroupByDoc(string docID)
        {
            return semanticList.GetSemanticGroupByDoc(docID);
        }

        internal SemanticGroup GetSemanticGroupById(string id)
        {
            return semanticList.GetSemanticGroupById(id);
        }

        /// <summary>
        /// Set the search result. Set the UserActionOnDoc
        /// </summary>
        /// <param name="docIDs"></param>
        /// <param name="owner"></param>
        /// <param name="searched"></param>
        internal void SetSearchResult(string[] docIDs, User owner, bool searched)
        {
            semanticList.SetSearchResult(docIDs, owner, searched);
        }
        /// <summary>
        /// Set the active result. Set the UserActionOnDoc
        /// </summary>
        /// <param name="docIDs"></param>
        /// <param name="owner"></param>
        /// <param name="searched"></param>
        internal void SetActiveCard(string[] docIDs, User owner, bool searched)
        {
            semanticList.SetActiveResult(docIDs, owner, searched);
        }
        /// <summary>
        /// Set the touch result. Set the UserActionOnDoc
        /// </summary>
        /// <param name="documentCard"></param>
        /// <param name="value"></param>
        internal void SetTouchedCard(DocumentCard documentCard, bool value)
        {
            semanticList.SetTouchResult(documentCard.Document.DocID, documentCard.Owner, value);
        }
        /// <summary>
        /// Save the status of the current connected cards
        /// </summary>
        internal async Task<bool> UpdateCurrentStatus()
        {
            bool needUpdate = false;
            try
            {
                foreach (CardGroup gg in GetGroups().Values)
                {
                    if (gg.Count() > 1)
                    {
                        var cardIDs = gg.GetCardID();
                        List<string> docIDs = new List<string>();
                        foreach (string id in cardIDs)
                        {
                            Document doc = controllers.CardController.DocumentCardController.GetDocumentCardById(id).Document;
                            if (!docIDs.Contains(doc.DocID))
                            {
                                docIDs.Add(doc.DocID);
                            }
                        }
                        if (docIDs.Count > 1)
                        {
                            needUpdate = await semanticList.MergeGroup(docIDs.ToArray(), this);
                        }
                    }
                }
                foreach (SemanticGroup sg in semanticList.GetSemanticGroup())
                {
                    if (sg.IsLeaf)
                    {
                        bool splited = await sg.TrySplit(GetGroups().Values);
                        needUpdate = needUpdate ? true : splited;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return needUpdate;
        }

        /// <summary>
        /// Get all card groups
        /// </summary>
        /// <returns></returns>
        internal ConcurrentDictionary<string, CardGroup> GetGroups()
        {
            return cardList.GetCardGroup();
        }
        /// <summary>
        /// Find all groups that intersect with the card
        /// </summary>
        /// <param name="cardID"></param>
        /// <returns></returns>
        internal Task<CardGroup[]> GetAttachedGroups(string cardID)
        {
            return cardList.GetAttachedGroups(cardID);
        }
        /// <summary>
        /// Update the positions of all the connected cards
        /// </summary>
        /// <param name="cardID">the id of the touched cards</param>
        /// <param name="vector">the vector of the manipulation</param>
        internal void UpdateConnectedPosition(string cardID, Point vector)
        {
            cardList.UpdateConnectedPosition(cardID, vector);
        }
        /// <summary>
        /// Update one card when point down
        /// </summary>
        /// <param name="cardID"></param>
        internal void DisconnectOneCardWithGroups(string cardID)
        {
            cardList.DisconnectOneCardWithGroups(cardID);
        }
        /// <summary>
        /// Connect multiple groups
        /// </summary>
        /// <param name="cardID"> The card id that trigger the event</param>
        internal async void ConnectGroupWithGroups(string cardID)
        {
            //cardList.ConnectGroupWithGroups(cardID);
            //bool changed = await UpdateCurrentStatus();
            //if (changed)
            //{
            //    controllers.ConnectionController.UpdateSemanticCloud();
            //}
        }
        /// <summary>
        /// When one card connect to at least 1 group, 
        /// merge the card and all groups into one group and add to list
        /// </summary>
        /// <param name="cardID"></param>
        /// <param name="groups"></param>
        internal async void ConnectOneCardWithGroups(string cardID, CardGroup[] attachedGroups)
        {
            cardList.ConnectOneCardWithGroups(cardID, attachedGroups);
            bool changed = await UpdateCurrentStatus();
            if (changed)
            {
                controllers.ConnectionController.UpdateSemanticCloud();
            }
        }
    }
}
