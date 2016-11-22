using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.SecondaryWindow.Layers
{
    class BaseLayerController
    {
        BaseLayer baseLayer;
        AwareCloudController controllers;
        internal BaseLayerController(AwareCloudController ctrls) {
            this.controllers = ctrls;
        }

        internal BaseLayer BaseLayer
        {
            get
            {
                return baseLayer;
            }

            set
            {
                baseLayer = value;
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

        internal void Init(int width, int height) {
            baseLayer = new BaseLayer(this);
            baseLayer.Init(width, height);
        }
    }
}
