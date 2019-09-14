using System.Collections.Generic;
using Autorepair.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace Autorepair.Data.IRepositories
{
    public interface IRepairJobsRepository
    {
        List<RuleModel> GetRules();

        List<ReferencePriceModel> GetReferencePrices();

        void initMocks();

        int GetTyresJobsId();

        int GetBrakeDiscsJobsId();

        int GetBrakePadsJobsId();

        int GetOilJobsId();

        int GetExhaustJobsId();

        double GetMaxTyres();

        double GetMaxExhaust();

        double GetLabourPrice();

        double GetMaxPriceForApprove();

        double GetMaxPriceForRefer();
    }
}