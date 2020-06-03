using Microsoft.AspNetCore.Mvc;

namespace DataApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {
        private readonly IDetermineIfIdsAreValid _idService;

        public DataController(IDetermineIfIdsAreValid idService)
        {
            _idService = idService;
        }

        [HttpGet]
        public ActionResult<ApiResponse> Get(string id)
        {
            if (_idService.IsValidId(id))
            {
                return this.Ok(new ApiResponse { Id = id, SomeInt = 1 });
            }
            else
            {
                return NotFound();
            }
        }
    }
}
