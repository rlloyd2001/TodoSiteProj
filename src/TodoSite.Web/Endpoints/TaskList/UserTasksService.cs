using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TodoSite.TaskList
{
    public class UserTasksService : IUserTasksService
    {
        public void DeleteTask(UserTasksViewModel model, int index)
        {
            if (index < 0 || model.Tasks.Count < index) return;
            model.Tasks.RemoveAt(index);
            model.EditTaskIndex = model.AddedTaskIndex = -1;
        }

        public void UpdateTask(UserTasksViewModel model, TaskInputModel inputModel)
        {
            var index = inputModel.Index;
            model.Tasks[index] = inputModel;
            if (index == model.EditTaskIndex) model.EditTaskIndex = -1;
            if (index == model.AddedTaskIndex) model.AddedTaskIndex = -1;
        }
        
        public void AddTask(UserTasksViewModel model, TaskModel taskModel)
        {
            model.Tasks.Add(taskModel);
            model.AddedTaskIndex = model.Tasks.Count - 1;
            model.EditTaskIndex = -1;
        }
    }
}