using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FubuCore.Dates;
using FubuPersistence;
using FubuPersistence.Reset;
using StoryTeller;
using StoryTeller.Engine;

namespace TodoSite.Storyteller.Fixtures
{
    class ModelFixture : Fixture
    {
        private ICompleteReset _reset;
        private IUnitOfWork _unitOfWork;
        private IEntityRepository _repository;

        public ModelFixture()
        {
            Title = "The system state";
        }

        public override void SetUp(ITestContext context)
        {
            var clock = (Clock)Retrieve<IClock>();
            clock.Live();

            _reset = Retrieve<ICompleteReset>();
            _reset.ResetState();

            _unitOfWork = Retrieve<IUnitOfWork>();
            _repository = _unitOfWork.Start();
        }

        public override void TearDown()
        {
            _unitOfWork.Commit();
            _reset.CommitChanges(); // doesn't do anything now, but might later when we go to IIS
        }

        [ExposeAsTable("The users are")]
        public void UsersAre(string firstName, string lastName)
        {
            var user = new UserInputModel
            {
                FirstName =  firstName,
                LastName = lastName
            };
        }
    }
}
