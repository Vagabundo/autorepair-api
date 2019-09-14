namespace Autorepair.Data.Model
{
    public class ReferencePriceModel : ModelBase
    {
        public string Name { get; set; }
        public double Time { get; set; }
        public double Cost { get; set; }

        public ReferencePriceModel (int id, string name, double time, double cost)
        {
            Id = id;
            Name = name;
            Time = time;
            Cost = cost;
        }
    }
}