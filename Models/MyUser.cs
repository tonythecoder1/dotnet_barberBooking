using Microsoft.AspNetCore.Identity;

namespace dotidentity.Models
{
    public class MyUser : IdentityUser
    {
        // Propriedade adicional, se necessário
        public string? CustomProperty { get; set; }
    }
}
