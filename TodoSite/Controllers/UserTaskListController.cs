using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TodoSite
{
    public class UserTaskListController
    {
        public UserTaskListModel EditTask(EditTaskInputModel inputModel)
        {
            UserTaskListModel model = GetUserTaskListModel();
            model.EditTaskIndex = inputModel.Index;
            return model;
        }

        public UserTaskListModel DeleteTask(DeleteTaskInputModel inputModel)
        {
            UserTaskListModel model = GetUserTaskListModel();
            model.DeleteTask(inputModel.Index);
            return model;
        }

        public UserTaskListModel SaveTask(TaskInputModel inputModel)
        {
            UserTaskListModel model = GetUserTaskListModel();
            model.UpdateTask(inputModel.Index, inputModel);
            return model;
        }

        public UserTaskListModel AddTask()
        {
            UserTaskListModel model = GetUserTaskListModel();
            model.AppendTask("new task", DateTime.Now);
            model.AddTaskIndex = model.List.Count - 1;
            return model;
        }

        private UserTaskListModel GetUserTaskListModel()
        {
            long userID = 0;
            if (HttpContext.Current.Session[UserLoginController.CurrentUserIdKey] != null) {
                userID = (long)HttpContext.Current.Session[UserLoginController.CurrentUserIdKey];
            }
            return new UserTaskListModel(userID);
        }
    }
}