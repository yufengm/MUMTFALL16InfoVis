using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    class CardInfo
    {
        protected Size cardSize = new Size(120 * Screen.SCALE_FACTOR, 90 * Screen.SCALE_FACTOR);
        protected Color cardColor = Colors.Red;
        protected Point cardPosition = new Point(0, 0);
        protected double cardScale = 1;
        protected double cardRotation = 0;
        protected static Dictionary<User, CardInfo> cardInfoList = new Dictionary<User, CardInfo>();
        public Color CardColor
        {
            get
            {
                return cardColor;
            }
        }
        public Point CardPosition
        {
            get
            {
                return cardPosition;
            }
        }
        public double CardScale
        {
            get
            {
                return cardScale;
            }
        }
        public double CardRotation
        {
            get
            {
                return cardRotation;
            }
        }
        public Size CardSize
        {
            get
            {
                return cardSize;
            }
        }
        static CardInfo()
        {
            cardInfoList.Add(User.ALEX, InitAlex());
            cardInfoList.Add(User.BEN, InitBen());
            cardInfoList.Add(User.CHRIS, InitChris());
            cardInfoList.Add(User.DANNY, InitDanny());
        }
        /// <summary>
        /// Get the cardInfo of an user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static CardInfo GetCardInfo(User user) {
            return cardInfoList[user];
        }
        /// <summary>
        /// Intilize Alex's card
        /// </summary>
        /// <returns></returns>
        private static CardInfo InitAlex()
        {
            CardInfo cardInfo = new CardInfo();
            cardInfo.cardColor = Colors.Silver;
            cardInfo.cardPosition = new Point(0, 0);
            cardInfo.cardScale = 1;
            cardInfo.cardRotation = 0;
            return cardInfo;
        }

        /// <summary>
        /// Initialize Ben's card info
        /// </summary>
        /// <returns></returns>
        private static CardInfo InitBen()
        {
            CardInfo cardInfo = new CardInfo();
            cardInfo.cardColor = Colors.Silver;
            cardInfo.cardPosition = new Point(0, 0);
            cardInfo.cardScale = 1;
            cardInfo.cardRotation = 0;
            return cardInfo;
        }
        /// <summary>
        /// Initialize Chris's card info
        /// </summary>
        /// <returns></returns>
        private static CardInfo InitChris()
        {
            CardInfo cardInfo = new CardInfo();
            cardInfo.cardColor = Colors.Silver;
            cardInfo.cardPosition = new Point(0, 0);
            cardInfo.cardScale = 1;
            cardInfo.cardRotation = 0;
            return cardInfo;
        }
        /// <summary>
        /// Initialize Danny's card info
        /// </summary>
        /// <returns></returns>
        private static CardInfo InitDanny()
        {
            CardInfo cardInfo = new CardInfo();
            cardInfo.cardColor = Colors.Silver;
            cardInfo.cardPosition = new Point(0, 0);
            cardInfo.cardScale = 1;
            cardInfo.cardRotation = 0;
            return cardInfo;
        }
    }
}
