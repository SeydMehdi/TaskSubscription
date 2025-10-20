
using Microsoft.AspNetCore.Mvc;
using TaskSubscription.Application.Subscriptions.Contracts;
using TaskSubscription.Application.Subscriptions.Contracts.Dtos;

namespace TaskSubscription.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private ISubscriptionService service;
        public SubscriptionController(ISubscriptionService service)
        {
            this.service = service;
        }
        [HttpGet]
        [Route("GetSubscriptionList")]
        public async Task<IActionResult> GetSubscriptionList([FromBody] SubscriptionSearchDto dto, CancellationToken token)
        {
            return Ok(await service.GetSubscriptionList(dto, token));
        }

        [HttpGet]
        [Route("GetSubscriptionInfo")]
        public async Task<IActionResult> GetSubscriptionInfo([FromBody] SubscriptionSearchDto dto, CancellationToken token)
        {
            return Ok(await service.GetSubscriptionList(dto, token));
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete([FromBody] SubscriptionDto dto, CancellationToken token)
        {
            return Ok(await service.Delete(dto, token));
        }

        [HttpPost]
        [Route("SaveChanges")]
        public async Task<IActionResult> SaveChanges([FromBody] SubscriptionDto dto, CancellationToken token)
        {
            return Ok(await service.SaveChanges(dto, token));
        }



        [HttpPost]
        [Route("Subscribe")]
        public async Task<IActionResult> Subscribe([FromBody] GetByIdDto dto, CancellationToken token)
        {
            return Ok(await service.SaveChanges(dto, token));
        }
    }
}
