using System;
using System.Collections.Generic;
using FubuPersistence;
using TodoSite.TaskList;

namespace TodoSite
{
    public class UserTasksModel : IEntity
    {
        public Guid Id { get; set; }
        public UserModel User { get; set; }
        public List<TaskModel> List { get; set; }

        public UserTasksModel()
        {
            List = new List<TaskModel>();
        }
    }
}