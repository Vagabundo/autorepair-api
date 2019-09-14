using System.ComponentModel.DataAnnotations;

namespace Autorepair.Data.Model
{
    public abstract class ModelBase
    {
        [Key]
        public int Id { get; set; }
    }
}