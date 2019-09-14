using System.Collections.Generic;
using Autorepair.Data;

namespace Autorepair.IManagers.Jobs
{
    public interface ITyresManager
    {
        bool CheckTyres (List<JobData> jobs);
    }
}