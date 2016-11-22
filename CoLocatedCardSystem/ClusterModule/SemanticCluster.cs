using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.ClusterModule
{
    class SemanticCluster
    {
        string id = "";
        int x = 0;
        int y = 0;
        User owner;
        ConcurrentDictionary<string, Node> nodes = new ConcurrentDictionary<string, Node>();
    }
}
