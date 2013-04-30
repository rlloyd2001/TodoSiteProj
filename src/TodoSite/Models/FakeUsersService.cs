using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FubuMVC.Core.Runtime;

namespace TodoSite
{
    public static class StringExtensions
    {
        public static bool EqualsIgnoreCase(this string value, string other)
        {
            return value.Equals(other, StringComparison.InvariantCultureIgnoreCase);
        }
    }
    
    public class FakeUsersService : IUsersService
    {
        private readonly ISessionState _sessionState;
        private readonly IList<UserTasksModel> _actualUsers;
        public const string UsersServiceKey = "TaskMgrAllUsersKey";

        public static IEnumerable<UserTasksModel> CreateUsers()
        {
            yield return new UserTasksModel
            {
                User = new UserModel
                {
                    FirstName = "Frank",
                    LastName = "Oh"
                },
                List = new List<TaskModel>
                {
                    new TaskModel
                    {
                        Description = "Take out garbage",
                        Date = DateTime.Now
                    },
                    new TaskModel
                    {
                        Description = "Wash the car",
                        Date = DateTime.Now
                    }
                }
            };
            yield return new UserTasksModel
            {
                User = new UserModel
                {
                    FirstName = "Billy",
                    LastName = "Jean"
                },
                List = new List<TaskModel>
                {
                    new TaskModel
                    {
                        Description = "Wash car",
                        Date = DateTime.Now
                    }
                }
            };
            yield return new UserTasksModel
            {
                User = new UserModel
                {
                    FirstName = "Darth",
                    LastName = "Vader"
                },
                List = new List<TaskModel>
                {
                    new TaskModel {Description = "Brush Teeth", Date = DateTime.Now},
                    new TaskModel {Description = "Kidnap Princess Leah", Date = DateTime.Now.AddHours(2)},
                    new TaskModel {Description = "Eat Lunch", Date = DateTime.Now.AddHours(3)},
                    new TaskModel {Description = "Light Saber Battle", Date = DateTime.Now.AddHours(5)},
                    new TaskModel {Description = "Attend Comicon", Date = DateTime.Now.AddDays(2)},
                    new TaskModel {Description = "Win Oscar", Date = DateTime.Now.AddYears(1).AddHours(53)}
                }
            };
        }

        public FakeUsersService(ISessionState sessionState)
        {
            _sessionState = sessionState;
            _actualUsers = _sessionState.Get<IList<UserTasksModel>>();
            if (_actualUsers == null)
            {
                _actualUsers = CreateUsers().ToList();
                Update(null);
            }
        }

        public UserTasksModel InsertUser(UserModel input)
        {
            var newUser = new UserTasksModel {User = input};
            _actualUsers.Add(newUser);
            Update(null);
            return newUser;
        }

        public void Update(UserTasksModel model)
        {
            _sessionState.Set<IList<UserTasksModel>>(_actualUsers);
        }

        public UserTasksModel GetUserByFirstAndLastName(string firstName, string lastName)
        {
            return
                _actualUsers.FirstOrDefault(
                    x => x.User.FirstName.EqualsIgnoreCase(firstName) && x.User.LastName.EqualsIgnoreCase(lastName));
        }

        public IList<UserModel> GetUsers()
        {
            return _actualUsers.Select(x => x.User).ToList();
        }
    }
}

