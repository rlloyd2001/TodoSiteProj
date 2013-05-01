using System;
using System.Collections.Generic;
using System.Linq;

namespace TodoSite
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
        public int AddedTaskIndex { get; private set; }

        public void UpdateTask(int index, TaskModel taskModel)
        {
            Tasks[index] = taskModel;
            if (index == EditTaskIndex) EditTaskIndex = -1;
            if (index == AddedTaskIndex) AddedTaskIndex = -1;
        }

        public void AddTask(TaskModel taskModel)
        {
            Tasks.Add(taskModel);
            AddedTaskIndex = Tasks.Count - 1;
            EditTaskIndex = -1;
        }

        public void DeleteTask(int index)
        {
            if (index < 0 || Tasks.Count < index) return;
            Tasks.RemoveAt(index);
            EditTaskIndex = AddedTaskIndex = -1;
        }
    }
}