namespace Autorepair.Data.Model
{
    public class RuleModel : ModelBase
    {
        public int JobId { get; set; }
        public string Rule { get; set; }
        public string Value { get; set; }

        public RuleModel (int id, int jobId, string rule, string value)
        {
            Id = id;
            JobId = jobId;
            Rule = rule;
            Value = value;
        }
    }
}