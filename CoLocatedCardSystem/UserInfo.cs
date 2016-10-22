using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;

namespace CoLocatedCardSystem
{
    public class UserInfo
    {
        /// <summary>
        /// Use this class to access the default setting of the users.
        /// </summary>
        protected User user = User.ALEX;
        private bool isLive = true;
        protected static Dictionary<User, UserInfo> userList = new Dictionary<User, UserInfo>();

        public User User
        {
            get
            {
                return user;
            }
        }      
   
        public bool IsLive
        {
            get
            {
                return isLive;
            }
        }

        /// <summary>
        /// Get the userinfo of a User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static UserInfo GetUserInfo(User user)
        {
            return userList[user];
        }
        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns></returns>
        public static User[] GetLiveUsers() {
            List<User> users = new List<User>();
            foreach (UserInfo ui in userList.Values) {
                if (ui.isLive) {
                    users.Add(ui.user);
                }
            }
            return users.ToArray();
        }
        static UserInfo()
        {
            userList.Add(User.ALEX, InitAlex());
            userList.Add(User.BEN, InitBen());
            userList.Add(User.CHRIS, InitChris());
            userList.Add(User.DANNY, InitDanny());
        }
        /// <summary>
        /// Initialize Alex's user info
        /// </summary>
        /// <returns></returns>
        private static UserInfo InitAlex()
        {
            UserInfo userInfo = new UserInfo();
            userInfo.user = User.ALEX;
            userInfo.isLive = true;
            return userInfo;
        }
        /// <summary>
        /// Initialize Ben's user info
        /// </summary>
        /// <returns></returns>
        private static UserInfo InitBen()
        {
            UserInfo userInfo = new UserInfo();
            userInfo.user = User.BEN;
            userInfo.isLive = true;
            return userInfo;
        }
        /// <summary>
        /// Initialize Chris's user info
        /// </summary>
        /// <returns></returns>
        private static UserInfo InitChris()
        {
            UserInfo userInfo = new UserInfo();
            userInfo.user = User.CHRIS;
            userInfo.isLive = true;
            return userInfo;
        }
        /// <summary>
        /// Initialize Danny's user info
        /// </summary>
        /// <returns></returns>
        private static UserInfo InitDanny()
        {
            UserInfo userInfo = new UserInfo();
            userInfo.user = User.DANNY;
            userInfo.isLive = false;
            return userInfo;
        }
    }
}
