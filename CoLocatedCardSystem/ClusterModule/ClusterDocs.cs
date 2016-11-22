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
        ConcurrentDictionary<string, Node> nodeList = new ConcurrentDictionary<string, Node>();
        ConcurrentDictionary<string, SemanticCluster> topicList=new ConcurrentDictionary<string, SemanticCluster>();
    }
}
