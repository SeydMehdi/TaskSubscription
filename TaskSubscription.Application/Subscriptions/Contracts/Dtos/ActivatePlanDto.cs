namespace TaskSubscription.Application.Subscriptions.Contracts.Dtos
{
    public class ActivatePlanDto
    {
        public long PlanId { get; set; }
        public Guid UserId { get; set; } // Controller fills this from User.Claims
    }
}
