using System.Collections.Generic;
using Autorepair.IManagers.Jobs;
using Autorepair.Data;

namespace Autorepair.Managers.Jobs
{
    public class TyresManager : ITyresManager
    {
        public bool CheckTyres (List<JobData> tyresJobs)
        {
            bool numberCheck = tyresJobs.Count % 2 == 0;

            return numberCheck && CheckTyresByPair(tyresJobs);
        }

        private bool CheckTyresByPair (List<JobData> jobs)
        {
            bool paired = true;

            jobs.ForEach(job => 
            {
                if ((job.Position % 2 == 0 && jobs.FindAll(j => j.Position == job.Position - 1).Count != 1) ||
                    (job.Position % 2 == 1 && jobs.FindAll(j => j.Position == job.Position + 1).Count != 1))
                {
                    paired = false;
                }
            });

            return paired;
        }
    }
}