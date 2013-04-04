using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TodoSite
{
    public class UserLoginController
    {
        public const string CurrentUserIdKey = "TaskMgrUserIdKey";
        
        public UsersTasksViewModel UserLogin(UserInputModel userInputModel)
        {            
            UsersTasksViewModel usersTasksVM = new UsersTasksViewModel();
            usersTasksVM.UserInput = userInputModel;
            HttpContext.Current.Session[CurrentUserIdKey] = usersTasksVM.User.ID;
            return usersTasksVM;
        }

        public UserInputModel Home()
        {
            return new UserInputModel();
        }
    }
}