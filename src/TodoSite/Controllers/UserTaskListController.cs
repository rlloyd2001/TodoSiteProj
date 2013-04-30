using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FubuCore;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Runtime;

namespace TodoSite
{
    public class UserTaskListController
    {
        private readonly IUsersService _usersService;
        private readonly ISessionState _session;

        public UserTaskListController(IUsersService usersService, ISessionState session)
        {
            _usersService = usersService;
            _session = session;
        }

        public UserTasksViewModel EditTask(EditTaskInputModel inputModel)
        {
            return WithViewModel(x => x.EditTaskIndex = inputModel.Index);
        }

        public UserTasksViewModel DeleteTask(DeleteTaskInputModel inputModel)
        {
            return WithViewModel(x => x.DeleteTask(inputModel.Index));
        }

        public FubuContinuation post_SaveTask(TaskInputModel inputModel)
        {
            WithViewModel(x => x.UpdateTask(inputModel.Index, inputModel));
            return FubuContinuation.RedirectTo<TaskInputModel>("GET");
        }

        public UserTasksViewModel get_SaveTask(TaskInputModel inputModel)
        {
            return GetUsersTasksViewModel();
        }

        public UserTasksViewModel AddTask()
        {
            return WithViewModel(x => x.AddTask(new TaskModel {Description = "new task", Date = DateTime.Now}));
        }

        private UserTasksViewModel WithViewModel(Action<UserTasksViewModel> action)
        {
            UserTasksViewModel model = GetUsersTasksViewModel();
            action(model);
            _usersService.Update(model.CurrentUser);
            return model;
        }

        private UserTasksViewModel GetUsersTasksViewModel()
        {
            return _session.Get<UserTasksViewModel>(); 
        }
    }
}