using System;
using System.Collections.Generic;
using FubuMVC.Core.Runtime;
using FubuTestingSupport;
using NUnit.Framework;
using Rhino.Mocks;
using TodoSite.TaskList;

namespace TodoSite.Web.Tests
{
    public class UserTasksServiceTests : InteractionContext<UserTasksService>
    {
        private UserTasksViewModel _model;
        private TaskModel _task1;
        private TaskModel _task2;
        private TaskInputModel _taskInput;

        protected override void beforeEach()
        {
            _model = new UserTasksViewModel
            {
                CurrentUser = new UserTasksModel
                {
                    User = new UserModel
                    {
                        CurrentTime = DateTime.Now,
                        FirstName = "firstName",
                        LastName = "lastName"
                    },
                    List = new List<TaskModel>()
                },
                EditTaskIndex = 1,
                AddedTaskIndex = 1
            };
            _task1 = new TaskModel();
            _task2 = new TaskModel();
            _model.Tasks.Add(_task1);
            _model.Tasks.Add(_task2);
            _taskInput = new TaskInputModel();
        }

        [Test]
        public void testUnit_DeleteTask()
        {
            var count = _model.Tasks.Count;
            ClassUnderTest.DeleteTask(_model, 0);
            _model.Tasks.Count.ShouldEqual(count - 1);
            _model.Tasks[0].ShouldEqual(_task2);
        }

        [Test]
        public void testUnit_UpdateTask()
        {
            _taskInput.Index = 1;
            ClassUnderTest.UpdateTask(_model, _taskInput);
            _model.Tasks[1].ShouldEqual(_taskInput);
            _model.EditTaskIndex.ShouldEqual(-1);
            _model.AddedTaskIndex.ShouldEqual(-1);
        }

        [Test]
        public void testUnit_AddTask()
        {
            var count = _model.Tasks.Count;
            ClassUnderTest.AddTask(_model, _taskInput);
            _model.Tasks.Count.ShouldEqual(count + 1);
            _model.Tasks[count].ShouldEqual(_taskInput);
            _model.AddedTaskIndex.ShouldEqual(count);
            _model.EditTaskIndex.ShouldEqual(-1);
        }
    }
}