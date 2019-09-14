using Autorepair.IManagers;
using Autorepair.Managers;
using Autorepair.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Autorepair.Data.Repositories;

namespace Autorepair.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepairController : ControllerBase
    {
        private IRepairResponseManager _repairResponseManager;
        private readonly RepairJobsRepository _dbContext;

        public RepairController (RepairJobsRepository context)
        {
            _dbContext = context;
        }

        public IRepairResponseManager RepairResponseManager
        {
            get
            {
                if (_repairResponseManager == null )
                {
                    _repairResponseManager = new RepairResponseManager(_dbContext);
                }

                return _repairResponseManager;
            }
        }

        // GET api/repair/submit
        [HttpPost("submit")]
        public async Task<ActionResult<Answer>> GetJobSheetAndProcessIt(JobSheetData jobSheet)
        {
            return await Task.Run(() =>
            {
                Answer result = RepairResponseManager.ProcessSheet(jobSheet);

                switch (result.Cod){
                    case Constants.Answer.REFERRED:
                        //code to refer. In example:
                        //ReferManager.SendJobSheet(jobSheet);
                        break;
                    case Constants.Answer.APPROVED:
                        //code to store the jobsheet. In example:
                        //ApprovedJobsManager.SendApproval(jobSheet);
                        break;
                    default:
                        break;
                }

                return result;
            });
        }
    }
}
