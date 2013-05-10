using System;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Runtime;

namespace TodoSite.TaskList
{
    public class UserTaskListController
    {
        private readonly IUsersService _usersService;
        private readonly ISessionState _session;
        private IUserTasksService _userTasksService;

        public UserTaskListController(IUsersService usersService, ISessionState session, IUserTasksService userTasksService)
        {
            _userTasksService = userTasksService;
            _usersService = usersService;
            _session = session;
        }

        public UserTasksViewModel EditTask(EditTaskInputModel inputModel)
        {
            return WithViewModel(x => x.EditTaskIndex = inputModel.Index);
        }

        public UserTasksViewModel DeleteTask(DeleteTaskInputModel inputModel)
        {
            return WithViewModel(x => _userTasksService.DeleteTask(x, inputModel.Index));
        }

        public FubuContinuation post_SaveTask(TaskInputModel inputModel)
        {
            WithViewModel(x => _userTasksService.UpdateTask(x, inputModel));
            return FubuContinuation.RedirectTo<TaskInputModel>("GET");
        }

        public UserTasksViewModel get_SaveTask(TaskInputModel inputModel)
        {
            return GetUserTasksViewModel();
        }

        public UserTasksViewModel AddTask()
        {
            return WithViewModel(x => _userTasksService.AddTask(x, new TaskModel {Description = "new task", Date = DateTime.Now}));
        }

        private UserTasksViewModel WithViewModel(Action<UserTasksViewModel> action)
        {
            UserTasksViewModel model = GetUserTasksViewModel();
            action(model);
            _usersService.Update(model.CurrentUser);
            return model;
        }

        private UserTasksViewModel GetUserTasksViewModel()
        {
            return _session.Get<UserTasksViewModel>(); 
        }
    }
}