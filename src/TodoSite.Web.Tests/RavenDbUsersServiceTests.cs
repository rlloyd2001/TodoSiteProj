using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FubuPersistence;
using FubuTestingSupport;
using NUnit.Framework;
using Rhino.Mocks;
using TodoSite.TaskList;

namespace TodoSite.Web.Tests
{
    class RavenDbUsersServiceTests : InteractionContext<RavenDbUsersService>
    {
        [Test]
        public void insert_user_trims_name_input()
        {
            var userInput = new UserModel
            {
                FirstName = "  first  ",
                LastName = "    last     "
            };
            var result = ClassUnderTest.InsertUser(userInput);
            result.User.FirstName.ShouldEqual("first");
            result.User.LastName.ShouldEqual("last");
        }

        [Test]
        public void insert_user_calls_update_on_the_repository()
        {
            var repository = MockFor<IEntityRepository>();
            var input = new UserModel();
            repository.Expect(x => x.Update(Arg<UserTasksModel>.Is.NotNull));
            var result = ClassUnderTest.InsertUser(input);
            result.User.ShouldEqual(input);
            repository.VerifyAllExpectations();
        }

        [Test]
        public void update_user_calls_update_on_the_repository()
        {
            var repository = MockFor<IEntityRepository>();
            var input = new UserTasksModel();
            repository.Expect(x => x.Update(Arg<UserTasksModel>.Is.NotNull));
            ClassUnderTest.Update(input);
            repository.VerifyAllExpectations();
        }

        [Test]
        public void get_users_calls_all_on_repository()
        {
            var user1 = new UserModel();
            var allTaskModels = new List<UserTasksModel>
            {
                new UserTasksModel { User = user1 },
            };

            var repository = MockFor<IEntityRepository>();
            repository.Expect(x => x.All<UserTasksModel>()).Return(allTaskModels.AsQueryable());
            var result = ClassUnderTest.GetUsers();
            repository.VerifyAllExpectations();

            result.ShouldContain(user1);
        }
    }
}
