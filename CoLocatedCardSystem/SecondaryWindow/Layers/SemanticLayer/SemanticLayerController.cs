using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoLocatedCardSystem.SecondaryWindow.SemanticModule;
using System.Collections.Concurrent;
using Windows.UI.Xaml.Shapes;
using CoLocatedCardSystem.CollaborationWindow;
using Windows.Foundation;

namespace CoLocatedCardSystem.SecondaryWindow.Layers
{
    class SemanticLayerController
    {
        SemanticLayer semanticlayer;
        AwareCloudController controllers;
        internal SemanticLayer Semanticlayer
        {
            get
            {
                return semanticlayer;
            }

            set
            {
                semanticlayer = value;
            }
        }
        internal SemanticLayerController(AwareCloudController ctrls)
        {
            this.controllers = ctrls;
        }
        internal void Init(int width, int height)
        {
            semanticlayer = new SemanticLayer(this);
            semanticlayer.Init(width, height);
        }

        internal void UpdateSemanticNode(ConcurrentDictionary<string, SemanticNode> semanticNodes)
        {
            semanticlayer.UpdateSemanticNode(semanticNodes);
        }

        internal void Invalidate()
        {
            semanticlayer.Update();
        }
    }
}
