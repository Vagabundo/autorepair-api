using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using Autorepair.Data;
using Autorepair.Data.IRepositories;
using Autorepair.Managers;
using Autorepair.IManagers.Jobs;
using Autorepair.Test.Data;
using Autorepair.Data.Model;

namespace ManagersTests
{
    public class RepairResponseManagerTest
    {

        // Despite the repository is already mocked, I use Moq here to show I know how to use them in real scenario
        private RepairResponseManager responseManager;
        private Mock<IRepairJobsRepository> repositoryMock = new Mock<IRepairJobsRepository>();
        private Mock<IBrakesManager> brakesManagerMock = new Mock<IBrakesManager>();
        private Mock<ITyresManager> tyresManagerMock = new Mock<ITyresManager>();

        private List<JobData> jobs;
        private JobSheetData jobSheet;
        private JobData job;
        private List<ReferencePriceModel> preferences;

        [SetUp]
        public void Setup()
        {
            responseManager = new RepairResponseManager(repositoryMock.Object, brakesManagerMock.Object, tyresManagerMock.Object);
            jobs = new List<JobData>();
            jobSheet = new JobSheetData();
            job = new JobData();
            preferences = new List<ReferencePriceModel>();

            InitApprovedRulesScenario();
        }

        #region CheckRules
        [Test]
        public void CheckRules_ReturnsApprovedAnswer_WhenAllTheRulesAreFollowed()
        {
            Answer answer = responseManager.CheckRules(jobs);

            Assert.True(answer.Cod == TestConstants.AnswerCodes.APPROVED);
        }

        [Test]
        public void CheckRules_AddsErrorMessageAndDeclineAnswer_WhenThereAreMoreTyreJobsThanMax()
        {
            repositoryMock.Setup(rm => rm.GetMaxTyres()).Returns(1);
            jobs.Add(new JobData());
            jobs.Add(new JobData());
            jobs.Add(new JobData());

            Answer answer = responseManager.CheckRules(jobs);

            Assert.True(answer.Cod == TestConstants.AnswerCodes.DECLINED);
            Assert.True(answer.Messages.Count == 1);
            Assert.True(answer.Messages[0].Equals(TestConstants.AnswerMessages.ERROR_TYRE));
        }

        [Test]
        public void CheckRules_AddsErrorMessageAndDeclineAnswer_WhenCheckTyresFails()
        {
            tyresManagerMock.Setup(tm => tm.CheckTyres(jobs)).Returns(false);

            Answer answer = responseManager.CheckRules(jobs);

            Assert.True(answer.Cod == TestConstants.AnswerCodes.DECLINED);
            Assert.True(answer.Messages.Count == 1);
            Assert.True(answer.Messages[0].Equals(TestConstants.AnswerMessages.ERROR_TYRE));
        }

        [Test]
        public void CheckRules_AddsErrorMessageAndDeclineAnswer_WhenCheckBrakesFails()
        {
            brakesManagerMock.Setup(tm => tm.CheckBrakes(jobs, jobs)).Returns(false);

            Answer answer = responseManager.CheckRules(jobs);

            Assert.True(answer.Cod == TestConstants.AnswerCodes.DECLINED);
            Assert.True(answer.Messages.Count == 1);
            Assert.True(answer.Messages[0].Equals(TestConstants.AnswerMessages.ERROR_BRAKES));
        }

        [Test]
        public void CheckRules_AddsErrorMessageAndDeclineAnswer_WhenThereAreMoreExhaustJobsThanMax()
        {
            jobs.Add(new JobData());
            jobs.Add(new JobData());

            Answer answer = responseManager.CheckRules(jobs);

            Assert.True(answer.Cod == TestConstants.AnswerCodes.DECLINED);
            Assert.True(answer.Messages.Count == 1);
            Assert.True(answer.Messages[0].Equals(TestConstants.AnswerMessages.ERROR_EXHAUST));
        }

        #endregion

        #region CheckHoursAndPrice

        [Test]
        public void CheckHoursAndPrice_ReturnsApprovedAnswer_WhenLabourHoursIsNotHigherThanReferenceAndPriceExceedsLessThan10Percent()
        {
            InitApprovedHoursAndPriceScenario();
            jobSheet.Jobs = jobs.ToArray();

            Answer answer = responseManager.CheckHoursAndPrice(jobSheet, new Answer());

            Assert.True(answer.Cod == TestConstants.AnswerCodes.APPROVED);
        }

        [Test]
        public void CheckHoursAndPrice_ReturnsDeclinedAnswer_WhenLabourHoursIsHigherThanReference()
        {
            InitApprovedHoursAndPriceScenario();
            jobSheet.Jobs = jobs.ToArray();
            jobSheet.TotalLabourHours = 3;
            
            Answer answer = responseManager.CheckHoursAndPrice(jobSheet, new Answer());

            Assert.True(answer.Cod == TestConstants.AnswerCodes.DECLINED);
            Assert.True(answer.Messages.Count == 1);
            Assert.True(answer.Messages[0].Equals(TestConstants.AnswerMessages.ERROR_HOUR));
        }

        [Test]
        public void CheckHoursAndPrice_ReturnsReferredAnswer_WhenPriceExceedsBetween10And15Percent()
        {
            InitApprovedHoursAndPriceScenario();
            jobSheet.Jobs = jobs.ToArray();
            jobSheet.TotalCost = 150;
            
            Answer answer = responseManager.CheckHoursAndPrice(jobSheet, new Answer());

            Assert.True(answer.Cod == TestConstants.AnswerCodes.REFERRED);
            Assert.True(answer.Messages.Count == 1);
            Assert.True(answer.Messages[0].Equals(TestConstants.AnswerMessages.REFER));
        }

        [Test]
        public void CheckHoursAndPrice_ReturnsDeclinedAnswer_WhenPriceExceeds15Percent()
        {
            InitApprovedHoursAndPriceScenario();
            jobSheet.Jobs = jobs.ToArray();
            jobSheet.TotalCost = 170;
            
            Answer answer = responseManager.CheckHoursAndPrice(jobSheet, new Answer());

            Assert.True(answer.Cod == TestConstants.AnswerCodes.DECLINED);
            Assert.True(answer.Messages.Count == 1);
            Assert.True(answer.Messages[0].Equals(TestConstants.AnswerMessages.ERROR_PRICE));
        }

        #endregion
        private void InitApprovedRulesScenario()
        {
            repositoryMock.Setup(rm => rm.GetMaxTyres()).Returns(3);
            repositoryMock.Setup(rm => rm.GetTyresJobsId()).Returns(0);
            tyresManagerMock.Setup(tm => tm.CheckTyres(jobs)).Returns(true);

            repositoryMock.Setup(rm => rm.GetBrakeDiscsJobsId()).Returns(0);
            repositoryMock.Setup(rm => rm.GetBrakePadsJobsId()).Returns(0);
            brakesManagerMock.Setup(tm => tm.CheckBrakes(jobs, jobs)).Returns(true);

            repositoryMock.Setup(rm => rm.GetExhaustJobsId()).Returns(0);
            repositoryMock.Setup(rm => rm.GetMaxExhaust()).Returns(1);
        }

        private void InitApprovedHoursAndPriceScenario()
        {
            job.Id = 1;
            jobs.Add(job);

            preferences.Add(new ReferencePriceModel(1, "Brake Discs", 90, 100));
            repositoryMock.Setup(rm => rm.GetMaxPriceForApprove()).Returns(1.4);
            repositoryMock.Setup(rm => rm.GetMaxPriceForRefer()).Returns(1.6);
            repositoryMock.Setup(rm => rm.GetReferencePrices()).Returns(preferences);
        }
    }
}