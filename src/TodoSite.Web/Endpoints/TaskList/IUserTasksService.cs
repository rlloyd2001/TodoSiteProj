using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TodoSite.TaskList
{
    public interface IUserTasksService
    {
        void DeleteTask(UserTasksViewModel model, int index);
        void UpdateTask(UserTasksViewModel model, TaskInputModel inputModel);
        void AddTask(UserTasksViewModel model, TaskModel taskModel);
    }
}