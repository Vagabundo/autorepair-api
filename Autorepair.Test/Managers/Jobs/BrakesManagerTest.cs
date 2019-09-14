using NUnit.Framework;
using Autorepair.Managers.Jobs;
using System.Collections.Generic;
using Autorepair.Data;
using Autorepair.Test.Data;

namespace ManagersTests
{
    public class BrakesManagerTest
    {
        private BrakesManager brakesManager;
        private List<JobData> discJobs;
        private List<JobData> padJobs;

        [SetUp]
        public void Setup()
        {
            brakesManager = new BrakesManager();
            discJobs = new List<JobData>();
            padJobs = new List<JobData>();
        }

        [Test]
        public void CheckBrakes_ReturnsFalse_WhenDiscsAndPadsCountsAreNotEqual()
        {
            discJobs.Add(new JobData());
            discJobs.Add(new JobData());
            padJobs.Add(new JobData());

            Assert.False(brakesManager.CheckBrakes(discJobs, padJobs));
        }

        [Test]
        public void CheckBrakes_ReturnsFalse_WhenDiscsAndPadsAreInDifferentPositions()
        {
            var discJob = new JobData();
            var padJob = new JobData();

            discJob.Position = TestConstants.TyrePosition.FRONT_LEFT;
            padJob.Position = TestConstants.TyrePosition.BACK_RIGHT;

            discJobs.Add(discJob);
            padJobs.Add(padJob);

            Assert.False(brakesManager.CheckBrakes(discJobs, padJobs));
        }

        [Test]
        public void CheckBrakes_ReturnsTrue_WhenDiscsAndPadsAreInTheSamePosition()
        {
            var discJob = new JobData();
            var padJob = new JobData();

            discJob.Position = TestConstants.TyrePosition.FRONT_LEFT;
            padJob.Position = TestConstants.TyrePosition.FRONT_LEFT;

            discJobs.Add(discJob);
            padJobs.Add(padJob);

            Assert.True(brakesManager.CheckBrakes(discJobs, padJobs));
        }
    }
}