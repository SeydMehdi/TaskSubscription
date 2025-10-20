using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskSubscription.Application.Plans.Contracts;
using TaskSubscription.Application.Plans.Contracts.Dtos;


namespace TaskPlan.RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanController : ControllerBase
    {
        private IPlanService service;
        public PlanController(IPlanService service)
        {
            this.service = service;
        }
        [HttpGet]
        [Route("GetPlanList")]
        public async Task<IActionResult> GetPlanList([FromBody] PlanSearchDto dto, CancellationToken token)
        {
            return Ok(await service.GetPlanList(dto, token));
        }

        [HttpGet]
        [Route("GetPlanInfo")]
        public async Task<IActionResult> GetPlanInfo([FromBody] PlanSearchDto dto, CancellationToken token)
        {
            return Ok(await service.GetPlanList(dto, token));
        }


        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete([FromBody] PlanDto dto, CancellationToken token)
        {
            return Ok(await service.Delete(dto, token));
        }

        [HttpPost]
        [Route("SaveChanges")]
        public async Task<IActionResult> SaveChanges([FromBody] PlanDto dto, CancellationToken token)
        {
            return Ok(await service.SaveChanges(dto, token));
        }
    }
}

}
}
