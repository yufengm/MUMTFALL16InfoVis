using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace CoLocatedCardSystem.CollaborationWindow.Layers.Menu_Layer
{
    class MenuBarInfo
    {
        private Size size = new Size(800 * Screen.SCALE_FACTOR, 60 * Screen.SCALE_FACTOR);
        private Point position = new Point(0, 0);
        private Point cardInitPosition = new Point(0, 0);
        private double scale = 1;
        private double rotate = 0;
        CreateSortingBoxButtonAttr sortingBoxButtonInfo = new CreateSortingBoxButtonAttr();
        KeyboardAttr keyboardInfo = new KeyboardAttr();
        InputTextBox inputTextBlockInfo = new InputTextBox();
        DeleteButtonAttr deleteButtonInfo = new DeleteButtonAttr();
        SearchButtonAttr searchButtonInfo = new SearchButtonAttr();
        SearchResultTrayAttr searchResultInfo = new SearchResultTrayAttr();
        protected static Dictionary<User, MenuBarInfo> menubarInfoList = new Dictionary<User, MenuBarInfo>();
        internal Size Size
        {
            get
            {
                return size;
            }
        }
        internal Point Position
        {
            get
            {
                return position;
            }
        }
        internal double Scale
        {
            get
            {
                return scale;
            }
        }
        internal double Rotate
        {
            get
            {
                return rotate;
            }
        }
        internal CreateSortingBoxButtonAttr SortingBoxButtonInfo
        {
            get
            {
                return sortingBoxButtonInfo;
            }
        }
        internal KeyboardAttr KeyboardInfo
        {
            get
            {
                return keyboardInfo;
            }
        }
        internal InputTextBox InputTextBoxInfo
        {
            get
            {
                return inputTextBlockInfo;
            }
        }
        internal DeleteButtonAttr DeleteButtonInfo
        {
            get
            {
                return deleteButtonInfo;
            }
        }
        internal SearchButtonAttr SearchButtonInfo
        {
            get
            {
                return searchButtonInfo;
            }
        }
        internal SearchResultTrayAttr SearchResultInfo
        {
            get
            {
                return searchResultInfo;
            }
        }

        public Point CardInitPosition
        {
            get
            {
                return cardInitPosition;
            }

            set
            {
                cardInitPosition = value;
            }
        }

        static MenuBarInfo()
        {
            menubarInfoList.Add(User.ALEX, InitAlex());
            menubarInfoList.Add(User.BEN, InitBen());
            menubarInfoList.Add(User.CHRIS, InitChris());
            menubarInfoList.Add(User.DANNY, InitDanny());
        }
        /// <summary>
        /// Get the menubar info of the user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static MenuBarInfo GetMenuBarInfo(User user)
        {
            return menubarInfoList[user];
        }
        /// <summary>
        /// Initialize Alex's menu bar
        /// </summary>
        /// <returns></returns>
        private static MenuBarInfo InitAlex()
        {
            MenuBarInfo info = new MenuBarInfo();
            info.position = new Point(info.Size.Height, (Screen.HEIGHT - info.Size.Width) / 2);
            info.cardInitPosition = new Point(info.Size.Height + 260*Screen.SCALE_FACTOR, Screen.HEIGHT / 2);
            info.scale = 1;
            info.rotate = 90;
            return info;
        }
        /// <summary>
        /// Initialize Ben's menu bar
        /// </summary>
        /// <returns></returns>
        private static MenuBarInfo InitBen()
        {
            MenuBarInfo info = new MenuBarInfo();
            info.position = new Point((Screen.WIDTH - info.size.Width) / 2, Screen.HEIGHT - info.size.Height);
            info.cardInitPosition = new Point(Screen.WIDTH / 2, Screen.HEIGHT - info.size.Height - 260 * Screen.SCALE_FACTOR);
            info.scale = 1;
            info.rotate = 0;

            return info;
        }
        /// <summary>
        /// Initialize Chris's menu bar
        /// </summary>
        /// <returns></returns>
        private static MenuBarInfo InitChris()
        {
            MenuBarInfo info = new MenuBarInfo();
            info.position = new Point(Screen.WIDTH - info.size.Height, (Screen.HEIGHT + info.Size.Width) / 2);
            info.cardInitPosition = new Point(Screen.WIDTH - info.size.Height - 260 * Screen.SCALE_FACTOR, Screen.HEIGHT / 2);
            info.scale = 1;
            info.rotate = 270;
            return info;
        }
        /// <summary>
        /// Initialize Danny's menu bar
        /// </summary>
        /// <returns></returns>
        private static MenuBarInfo InitDanny()
        {
            MenuBarInfo info = new MenuBarInfo();
            info.position = new Point((Screen.WIDTH + info.Size.Width) / 2, info.Size.Height);
            info.cardInitPosition = new Point(Screen.WIDTH/ 2, info.Size.Height + 260 * Screen.SCALE_FACTOR);
            info.scale = 1;
            info.rotate = 180;
            return info;
        }

        internal class KeyboardAttr
        {
            Point position = new Point(0 * Screen.SCALE_FACTOR, -250 * Screen.SCALE_FACTOR);
            Size size = new Size(800 * Screen.SCALE_FACTOR, 250 * Screen.SCALE_FACTOR);
            public Point Position
            {
                get
                {
                    return position;
                }
            }
            public Size Size
            {
                get
                {
                    return size;
                }
            }
        }
        internal class InputTextBox
        {
            Point position = new Point(0 * Screen.SCALE_FACTOR, -310 * Screen.SCALE_FACTOR);
            Size size = new Size(800 * Screen.SCALE_FACTOR, 30 * Screen.SCALE_FACTOR);
            public Point Position
            {
                get
                {
                    return position;
                }
            }
            public Size Size
            {
                get
                {
                    return size;
                }
            }
        }
        internal class CreateSortingBoxButtonAttr
        {
            Point position = new Point(50 * Screen.SCALE_FACTOR, 10 * Screen.SCALE_FACTOR);
            Size size = new Size(150 * Screen.SCALE_FACTOR, 60 * Screen.SCALE_FACTOR);
            public Point Position
            {
                get
                {
                    return position;
                }
            }
            public Size Size
            {
                get
                {
                    return size;
                }
            }
        }
        internal class DeleteButtonAttr
        {
            Point position = new Point(250 * Screen.SCALE_FACTOR, 0);
            Size size = new Size(150 * Screen.SCALE_FACTOR, 60 * Screen.SCALE_FACTOR);
            public Point Position
            {
                get
                {
                    return position;
                }
            }
            public Size Size
            {
                get
                {
                    return size;
                }
            }
        }
        internal class SearchButtonAttr
        {
            Point position = new Point(450 * Screen.SCALE_FACTOR, 0);
            Size size = new Size(150 * Screen.SCALE_FACTOR, 60 * Screen.SCALE_FACTOR);
            public Point Position
            {
                get
                {
                    return position;
                }
            }
            public Size Size
            {
                get
                {
                    return size;
                }
            }
        }
        internal class SearchResultTrayAttr
        {
            Point position = new Point(-100 * Screen.SCALE_FACTOR, -130 * Screen.SCALE_FACTOR);
            Size size = new Size(1000 * Screen.SCALE_FACTOR, 130 * Screen.SCALE_FACTOR);
            public Point Position
            {
                get
                {
                    return position;
                }
            }
            public Size Size
            {
                get
                {
                    return size;
                }
            }
        }
    }
}
