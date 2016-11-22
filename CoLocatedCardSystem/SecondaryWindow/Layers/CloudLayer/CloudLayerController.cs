using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoLocatedCardSystem.SecondaryWindow.CloudModule;

namespace CoLocatedCardSystem.SecondaryWindow.Layers
{
    class CloudLayerController
    {
        CloudLayer cloudLayer;
        AwareCloudController controllers;
        internal CloudLayer CloudLayer
        {
            get
            {
                return cloudLayer;
            }

            set
            {
                cloudLayer = value;
            }
        }

        internal AwareCloudController Controllers
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

        internal CloudLayerController(AwareCloudController ctrls)
        {
            this.controllers = ctrls;
        }
        internal void Init(int width, int height)
        {
            cloudLayer = new CloudLayer(this);
            cloudLayer.Init(width, height);
        }

        internal void UpdateCloudNode(ConcurrentDictionary<string, CloudNode> cloudNodes)
        {
            cloudLayer.UpdateCloudNode(cloudNodes);
        }

        internal void Invalidate()
        {
            cloudLayer.Update();
        }
    }
}
