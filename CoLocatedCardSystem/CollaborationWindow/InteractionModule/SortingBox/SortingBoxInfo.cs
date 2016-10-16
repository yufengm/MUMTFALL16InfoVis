using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;

namespace CoLocatedCardSystem.CollaborationWindow.Layers.InteractionModule
{
    class SortingBoxInfo
    {
        protected Size sortingBoxSize = new Size(160 * Screen.SCALE_FACTOR, 120 * Screen.SCALE_FACTOR);
        protected Color sortingBoxColor = Colors.Gray;
        protected Point sortingBoxPosition = new Point(900 * Screen.SCALE_FACTOR, 500 * Screen.SCALE_FACTOR);
        protected double sortingBoxScale = 1;
        protected double sortingBoxRotation = 1;
        protected static Dictionary<User, SortingBoxInfo> sortingBoxInfoList = new Dictionary<User, SortingBoxInfo>();
        protected static int DISTANCETOEDGE = 300;
        public Size SortingBoxSize
        {
            get
            {
                return sortingBoxSize;
            }
        }

        public Color SortingBoxColor
        {
            get
            {
                return sortingBoxColor;
            }
        }

        public Point SortingBoxPosition
        {
            get
            {
                return sortingBoxPosition;
            }
        }

        public double SortingBoxScale
        {
            get
            {
                return sortingBoxScale;
            }
        }

        public double SortingBoxRotation
        {
            get
            {
                return sortingBoxRotation;
            }

            set
            {
                sortingBoxRotation = value;
            }
        }

        static SortingBoxInfo()
        {
            sortingBoxInfoList.Add(User.ALEX, InitAlex());
            sortingBoxInfoList.Add(User.BEN, InitBen());
            sortingBoxInfoList.Add(User.CHRIS, InitChris());
            sortingBoxInfoList.Add(User.DANNY, InitDanny());
        }
        public static SortingBoxInfo GetSortingBoxInfo(User user)
        {
            return sortingBoxInfoList[user];
        }
        /// <summary>
        /// Initialize Alex's user info
        /// </summary>
        /// <returns></returns>
        private static SortingBoxInfo InitAlex()
        {
            SortingBoxInfo sortingBoxInfo = new SortingBoxInfo();
            sortingBoxInfo.sortingBoxPosition = new Point(DISTANCETOEDGE, Screen.HEIGHT/2);
            sortingBoxInfo.sortingBoxScale = 1;
            sortingBoxInfo.SortingBoxRotation = 90;
            return sortingBoxInfo;
        }
        /// <summary>
        /// Initialize Ben's user info
        /// </summary>
        /// <returns></returns>
        private static SortingBoxInfo InitBen()
        {
            SortingBoxInfo sortingBoxInfo = new SortingBoxInfo();
            sortingBoxInfo.sortingBoxPosition = new Point(Screen.WIDTH/2, Screen.HEIGHT- DISTANCETOEDGE);
            sortingBoxInfo.sortingBoxScale = 1;
            sortingBoxInfo.SortingBoxRotation = 0;
            return sortingBoxInfo;
        }
        /// <summary>
        /// Initialize Chris's user info
        /// </summary>
        /// <returns></returns>
        private static SortingBoxInfo InitChris()
        {
            SortingBoxInfo sortingBoxInfo = new SortingBoxInfo();
            sortingBoxInfo.sortingBoxPosition = new Point(Screen.WIDTH- DISTANCETOEDGE, Screen.HEIGHT/2);
            sortingBoxInfo.sortingBoxScale = 1;
            sortingBoxInfo.SortingBoxRotation = 270;
            return sortingBoxInfo;
        }
        /// <summary>
        /// Initialize Danny's user info
        /// </summary>
        /// <returns></returns>
        private static SortingBoxInfo InitDanny()
        {
            SortingBoxInfo sortingBoxInfo = new SortingBoxInfo();
            sortingBoxInfo.sortingBoxPosition = new Point(Screen.WIDTH/2, DISTANCETOEDGE);
            sortingBoxInfo.sortingBoxScale = 1;
            sortingBoxInfo.sortingBoxRotation = 180;           
            return sortingBoxInfo;
        }
    }
}
