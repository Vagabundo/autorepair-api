using System.Collections.Generic;
using Autorepair.Data;

namespace Autorepair.IManagers.Jobs
{
    public interface IBrakesManager
    {
        bool CheckBrakes (List<JobData> discJobs, List<JobData> padJobs);
    }
}