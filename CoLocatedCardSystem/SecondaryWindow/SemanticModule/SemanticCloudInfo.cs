using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace CoLocatedCardSystem.SecondaryWindow.SemanticModule
{
    class SemanticCloudInfo
    {
        public Point semanticNodePosition = new Point();
        User owner = User.NONE;
        private static Dictionary<User, SemanticCloudInfo> semanticCloudInfoList = new Dictionary<User, SemanticCloudInfo>();

        internal static Dictionary<User, SemanticCloudInfo> SemanticCloudInfoList
        {
            get
            {
                return semanticCloudInfoList;
            }

            set
            {
                semanticCloudInfoList = value;
            }
        }

        static SemanticCloudInfo()
        {
            semanticCloudInfoList.Add(User.ALEX, InitAlex());
            semanticCloudInfoList.Add(User.BEN, InitBen());
            semanticCloudInfoList.Add(User.CHRIS, InitChris());
            semanticCloudInfoList.Add(User.DANNY, InitDanny());
        }
        private static SemanticCloudInfo InitAlex()
        {
            SemanticCloudInfo info = new SemanticCloudInfo();
            info.semanticNodePosition = new Point(200 * SecondaryScreen.SCALE_FACTOR, SecondaryScreen.HEIGHT / 2);
            info.owner = User.ALEX;
            return info;
        }
        private static SemanticCloudInfo InitBen()
        {
            SemanticCloudInfo info = new SemanticCloudInfo();
            info.semanticNodePosition = new Point(SecondaryScreen.WIDTH / 2, SecondaryScreen.HEIGHT - 200 * SecondaryScreen.SCALE_FACTOR);
            info.owner = User.BEN;
            return info;
        }
        private static SemanticCloudInfo InitChris()
        {
            SemanticCloudInfo info = new SemanticCloudInfo();
            info.semanticNodePosition = new Point(SecondaryScreen.WIDTH - 200 * SecondaryScreen.SCALE_FACTOR, SecondaryScreen.HEIGHT / 2);
            info.owner = User.CHRIS;
            return info;
        }
        private static SemanticCloudInfo InitDanny()
        {
            SemanticCloudInfo info = new SemanticCloudInfo();
            info.semanticNodePosition = new Point(SecondaryScreen.WIDTH / 2, 200 * SecondaryScreen.SCALE_FACTOR);
            info.owner = User.DANNY;
            return info;
        }
    }
}
