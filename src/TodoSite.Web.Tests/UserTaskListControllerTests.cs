using FubuMVC.Core.Runtime;
using FubuTestingSupport;
using NUnit.Framework;
using Rhino.Mocks;
using TodoSite.TaskList;

namespace TodoSite.Web.Tests
{
    public class UserTaskListControllerTests : InteractionContext<UserTaskListController>
    {
        private UserTasksViewModel _model;

        protected override void beforeEach()
        {
            _model = new UserTasksViewModel();
            MockFor<ISessionState>().Expect(x => x.Get<UserTasksViewModel>()).Return(_model);
        }

        [TearDown]
        public void after_each()
        {
            VerifyCallsFor<ISessionState>();
        }

        [Test]
        public void testUnit_AddTask()
        {
            MockFor<IUserTasksService>().Expect(x => x.AddTask(Arg.Is(_model), Arg<TaskModel>.Is.Anything));

            ClassUnderTest.AddTask();

            VerifyCallsFor<IUserTasksService>();
        }

        [Test]
        public void testUnit_EditTask()
        {
            var mockInput = new EditTaskInputModel
            {
                Index = 5
            };
            ClassUnderTest.EditTask(mockInput);
            _model.EditTaskIndex.ShouldEqual(mockInput.Index);
        }

        [Test]
        public void testUnit_DeleteTask()
        {
            MockFor<IUserTasksService>().Expect(x => x.DeleteTask(Arg.Is(_model), Arg<int>.Is.Anything));

            ClassUnderTest.DeleteTask(new DeleteTaskInputModel());

            VerifyCallsFor<IUserTasksService>();
        }

        [Test]
        public void testUnit_post_SaveTask()
        {
            MockFor<IUserTasksService>().Expect(x => x.UpdateTask(Arg.Is(_model), Arg<TaskInputModel>.Is.Anything));

            ClassUnderTest.post_SaveTask(new TaskInputModel());

            VerifyCallsFor<IUserTasksService>();
        }

        [Test]
        public void testUnit_get_SaveTask()
        {
            ClassUnderTest.get_SaveTask(new TaskInputModel());
        }
    }
}