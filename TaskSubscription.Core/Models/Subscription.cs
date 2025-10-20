using CleanArchitecture.Common.Core.Common;
using Payment.Core.Models.Identities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskSubscription.Core.Models
{
    public class Subscription : BaseModel
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public int PlanId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Active;

        public DateTime? CancelledAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("PlanId")]
        public virtual Plan Plan { get; set; } = null!;


        [ForeignKey("UserId")]
        public virtual AspnetUser User { get; set; } = null!;





    }





}
