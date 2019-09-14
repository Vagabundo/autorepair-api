using System.Collections.Generic;
using Autorepair.IManagers.Jobs;
using Autorepair.Data;

namespace Autorepair.Managers.Jobs
{
    public class BrakesManager : IBrakesManager
    {
        public bool CheckBrakes (List<JobData> discJobs, List<JobData> padJobs)
        {
            bool pairedCount = discJobs.Count == padJobs.Count;

            return pairedCount && CheckDiscsAndPads(discJobs, padJobs);
        }

        private bool CheckDiscsAndPads (List<JobData> discJobs, List<JobData> padJobs)
        {
            bool paired = true;

            discJobs.ForEach(discJob => 
            {
                if (padJobs.FindAll(pj => pj.Position == discJob.Position).Count != 1)
                {
                    paired = false;
                }
            });

            return paired;
        }
    }
}