using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TodoSite
{
    public class UserTasksModel
    {
        public UserTasksModel() { }
        public UserTasksModel(UserModel user)
        {
            this.user = user;
            this.list = new List<TaskModel>();
        }

        private UserModel user;
        private List<TaskModel> list;

        public UserModel User { get { return user; } set { user = value; } }

        public List<TaskModel> List { get { return list; } set { list = value; } }
    }
}