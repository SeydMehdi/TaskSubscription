

using Microsoft.AspNetCore.Identity;

namespace Payment.Core.Models.Identities
{
    public class AspnetUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public string NationalCode { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
    }
}
