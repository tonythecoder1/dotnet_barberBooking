using Microsoft.AspNetCore.Identity;

namespace dotidentity.Models
{
    public class MyUser : IdentityUser
    {
        // Propriedade adicional, se necessário
        public string? NomeCompleto { get; set; }
        public string OrgId{get; set; }
        public string Member{get; set; } = "Member";
        public Organization? Organization { get; set; }        // navegação
        public ICollection<Reserva> Reservas { get; set; } // relação 1:N


    }

public class Organization
{
    public string Id { get; set; }
    public string Name { get; set; } 
    public ICollection<MyUser> Users { get; set; } = new List<MyUser>();  // navegação reversa


}

}
