using System;
using System.Collections.Generic;
using System.Linq;

namespace TodoSite.TaskList
{
    public class UserTasksViewModel
    {
        public UserTasksModel CurrentUser { get; set; }
        public DateTime LoginTime { get; set; }

        public UserTasksViewModel()
        {
            EditTaskIndex = -1;
            AddedTaskIndex = -1;
        }

        public UserModel User
        {
            get { return CurrentUser.User; }
        }

        public IList<TaskModel> Tasks
        {
            get { return CurrentUser.List; }
        }

        public IList<TaskInputModel> TasksWithIndex
        {
            get
            {
                return Tasks.Select((task, index) => new TaskInputModel
                {
                    Date = task.Date,
                    Description = task.Description,
                    Index = index
                }).ToList();
            }
        }

        public int EditTaskIndex { get; set; }
        public int AddedTaskIndex { get; set; }
   }
}