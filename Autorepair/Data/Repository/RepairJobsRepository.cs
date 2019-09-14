using System.Collections.Generic;
using Autorepair.Data.IRepositories;
using Autorepair.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace Autorepair.Data.Repositories
{
    public class RepairJobsRepository : DbContext, IRepairJobsRepository
    {
        public RepairJobsRepository(DbContextOptions<RepairJobsRepository> options)
            : base(options){}

        public DbSet<RuleModel> Rules { get; private set; }
        public DbSet<ReferencePriceModel> ReferencePrices { get; private set; }

        // The code above should work with a real DB, I add all the code below in order to mock the data
        public List<RuleModel> MockedRules { get; set; }
        public List<ReferencePriceModel> MockedReferencePrices { get; set; }

        public List<RuleModel> GetRules()
        {
            return MockedRules;
        }

        public List<ReferencePriceModel> GetReferencePrices()
        {
            return MockedReferencePrices;
        }

        public void initMocks()
        {
            MockedRules = new List<RuleModel>();
            MockedRules.Add(new RuleModel(1, 1, "MAX_TYRES", "4"));
            MockedRules.Add(new RuleModel(2, 5, "MAX_EXHAUST", "1"));
            MockedRules.Add(new RuleModel(3, 0, "MAX_PRICE_APPROVE", "1.10"));
            MockedRules.Add(new RuleModel(4, 0, "MAX_PRICE_REFER", "1.15"));

            MockedReferencePrices = new List<ReferencePriceModel>();
            MockedReferencePrices.Add(new ReferencePriceModel(1, "Tyre", 30, 200));
            MockedReferencePrices.Add(new ReferencePriceModel(2, "Brake Discs", 90, 100));
            MockedReferencePrices.Add(new ReferencePriceModel(3, "Brake Pads", 60, 50));
            MockedReferencePrices.Add(new ReferencePriceModel(4, "Oil", 30, 20));
            MockedReferencePrices.Add(new ReferencePriceModel(5, "Exhaust", 240, 175));
            MockedReferencePrices.Add(new ReferencePriceModel(6, "Labour", 60, 45));
        }

        public int GetTyresJobsId()
        {
            return MockedReferencePrices.Find(job => job.Name.Equals("Tyre")).Id;
        }

        public int GetBrakeDiscsJobsId()
        {
            return MockedReferencePrices.Find(job => job.Name.Equals("Brake Discs")).Id;
        }

        public int GetBrakePadsJobsId()
        {
            return MockedReferencePrices.Find(job => job.Name.Equals("Brake Pads")).Id;
        }

        public int GetOilJobsId()
        {
            return MockedReferencePrices.Find(job => job.Name.Equals("Oil")).Id;
        }

        public int GetExhaustJobsId()
        {
            return MockedReferencePrices.Find(job => job.Name.Equals("Exhaust")).Id;
        }

        public double GetMaxTyres()
        {
            return double.Parse(MockedRules.Find(rule => rule.Rule.Equals("MAX_TYRES")).Value);
        }

        public double GetMaxExhaust()
        {
            return double.Parse(MockedRules.Find(rule => rule.Rule.Equals("MAX_EXHAUST")).Value);
        }

        public double GetLabourPrice()
        {
            return MockedReferencePrices.Find(job => job.Name.Equals("Labour")).Cost;
        }

        public double GetMaxPriceForApprove()
        {
            return double.Parse(MockedRules.Find(rule => rule.Rule.Equals("MAX_PRICE_APPROVE")).Value);
        }

        public double GetMaxPriceForRefer()
        {
            return double.Parse(MockedRules.Find(rule => rule.Rule.Equals("MAX_PRICE_REFER")).Value);
        }
    }
}