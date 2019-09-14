using System.Collections.Generic;
using System.Linq;
using Autorepair.IManagers;
using Autorepair.IManagers.Jobs;
using Autorepair.Managers.Jobs;
using Autorepair.Data;
using Autorepair.Data.Repositories;
using Autorepair.Data.Model;
using Autorepair.Data.IRepositories;

namespace Autorepair.Managers
{
    public class RepairResponseManager : IRepairResponseManager
    {
        private IBrakesManager _brakesManager;
        private ITyresManager _tyresManager;
        private IRepairJobsRepository _repairDb;

        public IBrakesManager BrakesManager
        {
            get
            {
                if (_brakesManager == null )
                {
                    _brakesManager = new BrakesManager();
                }

                return _brakesManager;
            }
        }

        public ITyresManager TyresManager
        {
            get
            {
                if (_tyresManager == null )
                {
                    _tyresManager = new TyresManager();
                }

                return _tyresManager;
            }
        }

        public IRepairJobsRepository RepairDb { get { return _repairDb; } }

        public RepairResponseManager (RepairJobsRepository db) {
            _repairDb = db;
        }

        public RepairResponseManager (IRepairJobsRepository db, IBrakesManager brakesManager, ITyresManager tyresManager) {
            _repairDb = db;
            _brakesManager = brakesManager;
            _tyresManager = tyresManager;
        }

        public Answer ProcessSheet (JobSheetData jobSheet)
        {
            var jobs = jobSheet.Jobs.ToList();
            Answer answer = CheckRules(jobs);
            
            return answer.Cod != Constants.Answer.APPROVED ? answer : CheckHoursAndPrice(jobSheet, answer);
        }

        public Answer CheckRules (List<JobData> jobs)
        {
            RepairDb.initMocks();
            Answer answer = new Answer();

            List<JobData> tyreJobs = jobs.FindAll(j => j.Id == RepairDb.GetTyresJobsId());
            if (tyreJobs.Count > RepairDb.GetMaxTyres() || !TyresManager.CheckTyres(tyreJobs))
            {
                answer.AddErrorMessage("Tyre jobs do not follow the rules");
            } else if (!BrakesManager.CheckBrakes(jobs.FindAll(j => j.Id == RepairDb.GetBrakeDiscsJobsId()), 
                jobs.FindAll(j => j.Id == RepairDb.GetBrakePadsJobsId())))
            {
                answer.AddErrorMessage("Brake jobs do not follow the rules");
            } else if (jobs.FindAll(j => j.Id == RepairDb.GetExhaustJobsId()).Count > RepairDb.GetMaxExhaust())
            {
                answer.AddErrorMessage("Exhaust jobs do not follow the rules");
            }

            return answer;
        }

        public Answer CheckHoursAndPrice (JobSheetData jobSheet, Answer answer)
        {
            double totalReferenceHours = 0;
            double totalReferencePrice = 0;
            double referenceLabourPrice = RepairDb.GetLabourPrice();
            ReferencePriceModel reference = null;

            jobSheet.Jobs.ToList().ForEach(job =>
            {
                reference = RepairDb.GetReferencePrices().Find(dbRef => dbRef.Id == job.Id);
                totalReferencePrice += reference.Cost + (reference.Time/60 * referenceLabourPrice);
                totalReferenceHours += reference.Time/60;
            });

            if (jobSheet.TotalLabourHours > totalReferenceHours)
            {
                answer.AddErrorMessage("Total hours exceed the refence hours labour");
            } else if (jobSheet.TotalCost < totalReferencePrice * RepairDb.GetMaxPriceForApprove())
            {
                answer.AddApproveMessage("Jobsheet aproved successfully");
            } else if (jobSheet.TotalCost <= totalReferencePrice * RepairDb.GetMaxPriceForRefer())
            {
                answer.AddReferredMessage("Jobsheet has been referred");
            } else
            {
                answer.AddErrorMessage("The total price exceeds the reference price");
            }

            return answer;
        }
    }
}