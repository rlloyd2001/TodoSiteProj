using System.Collections.Generic;

namespace TodoSite
{
    public interface IUsersService
    {
        UserTasksModel InsertUser(UserModel input);
        UserTasksModel GetUserByFirstAndLastName(string firstName, string lastName);
        IList<UserModel> GetUsers();
        void Update(UserTasksModel model);
    }
}