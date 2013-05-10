using System.Collections.Generic;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Runtime;
using TodoSite.TaskList;

namespace TodoSite
{
    public class UsersViewModel
    {
        public IList<UserModel> Users { get; set; }
    }

    public class UsersListInputModel
    {
    }

    public class UserLoginController
    {
        private readonly IUsersService _usersService;
        private readonly ISessionState _sessionState;

        public UserLoginController(IUsersService usersService, ISessionState sessionState)
        {
            _usersService = usersService;
            _sessionState = sessionState;
        }

        public UsersViewModel ShowUsers(UsersListInputModel input)
        {
            var users = _usersService.GetUsers();
            return new UsersViewModel
            {
                Users = users
            };
        }
        
        public FubuContinuation post_UserLogin(UserInputModel userInputModel)
        {
            var user = _usersService.GetUserByFirstAndLastName(userInputModel.FirstName, userInputModel.LastName);

            if (user == null)
            {
                user = _usersService.InsertUser(
                    new UserModel() 
                    { 
                        FirstName = userInputModel.FirstName,
                        LastName = userInputModel.LastName
                    });
            }

            var usersTasksVm = new UserTasksViewModel
            { 
                CurrentUser = user
            };
            usersTasksVm.LoginTime = userInputModel.CurrentTime;
            _sessionState.Set(usersTasksVm);

            return FubuContinuation.RedirectTo(new UserInputValid(), "GET");
        }

        public UserTasksViewModel get_UserLogin(UserLoginController.UserInputValid valid)
        {
            return _sessionState.Get<UserTasksViewModel>();
        }

        public class UserInputValid { }

        public UserInputModel get_Home(UserInputModel userInputModel)
        {
            return userInputModel;
        }

        public UserInputModel Home()
        {
            return new UserInputModel();
        }
    }
}