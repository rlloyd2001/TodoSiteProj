using System.Collections.Generic;
using System.Linq;
using FubuPersistence;

namespace TodoSite
{
    public class RavenDbUsersService : IUsersService
    {
        private readonly IEntityRepository _repository;

        public RavenDbUsersService(IEntityRepository repository)
        {
            _repository = repository;
            //var count = _repository.All<UserTasksModel>().Count();

//            if (count != 0) return;
//            foreach (var m in FakeUsersService.CreateUsers())
//            {
//                _repository.Update(m);
//            }
        }

        public UserTasksModel InsertUser(UserModel input)
        {
            input.FirstName = input.FirstName.Trim();
            input.LastName = input.LastName.Trim();
            var newUser = new UserTasksModel {User = input};
            _repository.Update(newUser);
            return newUser;
        }

        public void Update(UserTasksModel model)
        {
            _repository.Update(model);
        }

        public UserTasksModel GetUserByFirstAndLastName(string firstName, string lastName)
        {
            firstName = firstName.Trim();
            lastName = lastName.Trim();
            var m = _repository.FindWhere<UserTasksModel>(x => x.User.FirstName == firstName &&
                x.User.LastName == lastName);
            return m;
        }

        public IList<UserModel> GetUsers()
        {
            var retVal = _repository.All<UserTasksModel>().Select(x => x.User);
            return retVal.ToList();
        }
    }
}

