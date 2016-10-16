using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Media;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    static class CardLayoutGenerator
    {
        static double totalLenght = 600 * Screen.SCALE_FACTOR;
        static double margin = 80 * Screen.SCALE_FACTOR;
        static Dictionary<User, Point> trans = new Dictionary<User, Point>();
        static Dictionary<User, double> rotates = new Dictionary<User, double>();
        static Dictionary<User, Point> vector = new Dictionary<User, Point>();
        static CardLayoutGenerator()
        {
            trans.Add(User.ALEX, new Point(margin, (Screen.HEIGHT - totalLenght) / 2));
            trans.Add(User.BEN, new Point((Screen.WIDTH - totalLenght) / 2, Screen.HEIGHT - margin));
            trans.Add(User.CHRIS, new Point(Screen.WIDTH - margin, (Screen.HEIGHT + totalLenght) / 2));
            trans.Add(User.DANNY, new Point((Screen.WIDTH + totalLenght) / 2, margin));
            rotates.Add(User.ALEX, 90);
            rotates.Add(User.BEN, 0);
            rotates.Add(User.CHRIS, 270);
            rotates.Add(User.DANNY, 180);
            vector.Add(User.ALEX, new Point(0, 1));
            vector.Add(User.BEN, new Point(1, 0));
            vector.Add(User.CHRIS, new Point(0, -1));
            vector.Add(User.DANNY, new Point(-1, 0));
        }
        /// <summary>
        /// Put the cards in stack along the menubar of each user.
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="user"></param>
        public static void ApplyLayout(Card[] cards, User user)
        {
            for (int i = 0; i < cards.Length; i++)
            {
                cards[i].MoveTo(trans[user]);
                cards[i].Rotate(rotates[user]);
            }
            GenStack(cards, user);
        }

        private static void GenStack(Card[] cards, User user)
        {
            int cardNum = cards.Length;
            Random rand = new Random();
            if (cardNum == 1)
            {
                cards[0].MoveTo(new Point(totalLenght / 2, 0));
            }
            else
            {
                double interval = totalLenght / (cardNum - 1);
                for (int i = 0; i < cardNum; i++)
                {
                    Point mv = new Point(vector[user].X * interval * i, vector[user].Y * interval * i);
                    cards[i].MoveBy(mv);
                    cards[i].Rotate(rand.Next(5) - 2.5);
                }
            }
        }
    }
}
