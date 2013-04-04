using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TodoSite
{
    public class UserTaskListController
    {
        public UsersTasksViewModel EditTask(EditTaskInputModel inputModel)
        {
            UsersTasksViewModel model = GetUsersTasksViewModel();            
            model.EditTaskIndex = inputModel.Index;
            return model;
        }

        public UsersTasksViewModel DeleteTask(DeleteTaskInputModel inputModel)
        {
            UsersTasksViewModel model = GetUsersTasksViewModel();
            model.DeleteTask(inputModel.Index);
            return model;
        }

        public UsersTasksViewModel SaveTask(TaskInputModel inputModel)
        {
            UsersTasksViewModel model = GetUsersTasksViewModel();
            model.UpdateTask(inputModel.Index, inputModel);
            return model;
        }

        public UsersTasksViewModel AddTask()
        {
            UsersTasksViewModel model = GetUsersTasksViewModel();
            model.AddTask(new TaskModel { Description = "new task", Date = DateTime.Now });
            return model;
        }

        private UsersTasksViewModel GetUsersTasksViewModel()
        {
            long userID = 0;
            if (HttpContext.Current.Session[UserLoginController.CurrentUserIdKey] != null) {
                userID = (long)HttpContext.Current.Session[UserLoginController.CurrentUserIdKey];
            }
            UsersTasksViewModel usersTasksVM = new UsersTasksViewModel();
            usersTasksVM.UserID = userID;
            return usersTasksVM;
        }
    }
}