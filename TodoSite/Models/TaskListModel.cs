using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TodoSite
{
    public class TaskListModel
    {
        public TaskListModel()
        {
        }

        public TaskListModel(UserModel user)
        {
            User = user;
            List = new List<TaskModel>();
        }

        public UserModel User { get; set; }

        public List<TaskModel> List { get; set; }
    }
}