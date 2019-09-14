using Autorepair.Data;

namespace Autorepair.IManagers
{
    public interface IRepairResponseManager
    {
        Answer ProcessSheet(JobSheetData jobSheet);
    }
}