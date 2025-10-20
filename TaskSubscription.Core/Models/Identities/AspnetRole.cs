
using Microsoft.AspNetCore.Identity;


namespace Payment.Core.Models.Identities
{
   
    public class AspnetRole : IdentityRole<Guid>
    {
        public string AliasName { get; set; }
        public long? RoleGroupId { get; set; }
    }
}
