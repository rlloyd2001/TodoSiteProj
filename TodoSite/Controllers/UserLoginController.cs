using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TodoSite
{
    public class UserLoginController
    {
        public const string CurrentUserIdKey = "TaskMgrUserIdKey";

        public UserModel UserLogin()
        {
            return new UserModel();
        }

        public UserTaskListModel UserLogin(UserModel inputModel)
        {
            UserTaskListModel retModel = new UserTaskListModel(inputModel);
            HttpContext.Current.Session[CurrentUserIdKey] = retModel.UserID;
            return retModel;
        }

        public UserModel Home()
        {
            return new UserModel();
        }
    }
}