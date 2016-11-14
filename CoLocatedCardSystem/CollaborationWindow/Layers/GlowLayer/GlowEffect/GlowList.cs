using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CoLocatedCardSystem.CollaborationWindow.Layers.Glow_Layer
{
    class GlowList
    {
        Dictionary<string, Glow> glowEffectList;// A list of the glow objects
        ConcurrentDictionary<string, GlowGroup> glowGroups;//Save info of which cards are connected

        internal void Init() {
            glowEffectList = new Dictionary<string, Glow>();         
            glowGroups = new ConcurrentDictionary<string, GlowGroup>();
        }

        internal void Deinit() {
            glowEffectList.Clear();
            glowGroups.Clear();
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
        internal void RemoveGlowGroup(GlowGroup group)
        {
            if (glowGroups.Keys.Contains(group.Id))
            {
                GlowGroup gg;
                glowGroups.TryRemove(group.Id, out gg);
            }
        }
        /// <summary>
        /// Create a new glow group
        /// </summary>
        /// <param name="group"></param>
        internal void AddGlowGroup(GlowGroup group)
        {
            if (!glowGroups.Keys.Contains(group.Id))
            {
                glowGroups.TryAdd(group.Id,group);
            }
        }

        /// <summary>
        /// Get the glow group that contains a cardID
        /// </summary>
        /// <param name="cardID"></param>
        /// <returns></returns>
        internal GlowGroup GetGroup(string cardID) {
            GlowGroup result = null;
            foreach (GlowGroup group in glowGroups.Values)
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
        internal ConcurrentDictionary<string, GlowGroup> GetGroup()
        {
            return glowGroups;
        }
        /// <summary>
        /// Check if some group contains the cardID
        /// </summary>
        /// <param name="cardID"></param>
        /// <returns></returns>
        internal bool HasGroup(string cardID)
        {
            foreach (GlowGroup group in glowGroups.Values) {
                if (group.HasCard(cardID)) {
                    return true;
                }
            }
            return false;
        }

    }
}
