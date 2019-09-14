using NUnit.Framework;
using Autorepair.Managers.Jobs;
using System.Collections.Generic;
using Autorepair.Data;
using Autorepair.Test.Data;

namespace ManagersTests
{
    public class TyresManagerTest
    {
        private TyresManager tyresManager;
        private List<JobData> jobs;

        [SetUp]
        public void Setup()
        {
            tyresManager = new TyresManager();
            jobs = new List<JobData>();
        }

        [Test]
        public void CheckTyres_ReturnsFalse_WhenCountIsOdd()
        {
            jobs.Add(new JobData());
            jobs.Add(new JobData());
            jobs.Add(new JobData());

            Assert.False(tyresManager.CheckTyres(jobs));
        }

        [Test]
        public void CheckTyres_ReturnsFalse_WhenTyresAreNotPaired()
        {
            var tyreJob1 = new JobData();
            var tyreJob2 = new JobData();

            tyreJob1.Position = TestConstants.TyrePosition.FRONT_LEFT;
            tyreJob2.Position = TestConstants.TyrePosition.BACK_RIGHT;

            jobs.Add(tyreJob1);
            jobs.Add(tyreJob2);

            Assert.False(tyresManager.CheckTyres(jobs));
        }

        [Test]
        public void CheckTyres_ReturnsTrue_WhenTyresArePaired()
        {
            var tyreJob1 = new JobData();
            var tyreJob2 = new JobData();

            tyreJob1.Position = TestConstants.TyrePosition.FRONT_LEFT;
            tyreJob2.Position = TestConstants.TyrePosition.FRONT_RIGHT;

            jobs.Add(tyreJob1);
            jobs.Add(tyreJob2);

            Assert.True(tyresManager.CheckTyres(jobs));
        }
    }
}