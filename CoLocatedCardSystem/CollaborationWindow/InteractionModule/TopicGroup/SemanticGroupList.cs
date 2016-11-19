using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    class SemanticGroupList
    {
        Dictionary<string, Glow> glowEffectList;// A list of the glow objects. key: card id, value Glow object
        ConcurrentDictionary<string, SemanticGroup> semanticGroups;//Save info of which cards are connected. key: group id, value: semantic group

        internal void Init() {
            glowEffectList = new Dictionary<string, Glow>();         
            semanticGroups = new ConcurrentDictionary<string, SemanticGroup>();
        }

        internal void Deinit() {
            glowEffectList.Clear();
            semanticGroups.Clear();
        }
        /// <summary>
        /// Add a glow object to the list
        /// </summary>
        /// <param name="cardID"></param>
        /// <param name="glow"></param>
        internal void AddGlow(string cardID, Glow glow)
        {
            if (!glowEffectList.Keys.Contains(cardID))
            {
                glowEffectList.Add(cardID, glow);
            }
        }
        /// <summary>
        /// Remove a glow object
        /// </summary>
        /// <param name="cardID"></param>
        internal Glow RemoveGlow(string cardID)
        {
            Glow result = null;
            if (glowEffectList.Keys.Contains(cardID))
            {
                result = glowEffectList[cardID];
                glowEffectList.Remove(cardID);
            }
            return result;
        }

        /// <summary>
        /// Get the glow by id
        /// </summary>
        /// <param name="cardID"></param>
        /// <returns></returns>
        internal Glow GetGlow(string cardID)
        {
            if (glowEffectList.Keys.Contains(cardID))
                return glowEffectList[cardID];
            else
                return null;
        }

        /// <summary>
        /// Delete a glow group. return the removed ids.
        /// </summary>
        /// <param name="group"></param>
        internal void RemoveSemanticGroup(SemanticGroup group)
        {
            if (semanticGroups.Keys.Contains(group.Id))
            {
                SemanticGroup gg;
                semanticGroups.TryRemove(group.Id, out gg);
            }
        }
        /// <summary>
        /// Create a new glow group
        /// </summary>
        /// <param name="group"></param>
        internal void AddSemanticGroup(SemanticGroup group)
        {
            if (!semanticGroups.Keys.Contains(group.Id))
            {
                semanticGroups.TryAdd(group.Id,group);
            }
        }

        /// <summary>
        /// Get the glow group that contains a cardID
        /// </summary>
        /// <param name="cardID"></param>
        /// <returns></returns>
        internal SemanticGroup GetSemanticGroup(string cardID) {
            SemanticGroup result = null;
            foreach (SemanticGroup group in semanticGroups.Values)
            {
                if (group!=null&&group.HasCard(cardID))
                {
                    result = group;
                }
            }
            return result;
        }
        /// <summary>
        /// Get the glow group
        /// </summary>
        /// <returns></returns>
        internal ConcurrentDictionary<string, SemanticGroup> GetSemanticGroup()
        {
            return semanticGroups;
        }
        /// <summary>
        /// Check if some group contains the cardID
        /// </summary>
        /// <param name="cardID"></param>
        /// <returns></returns>
        internal bool HasSemanticGroup(string cardID)
        {
            foreach (SemanticGroup group in semanticGroups.Values) {
                if (group.HasCard(cardID)) {
                    return true;
                }
            }
            return false;
        }

    }
}
