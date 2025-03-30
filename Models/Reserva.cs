using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotidentity.Models
{
    public class Reserva
    {
        public int Id { get; set; }

        [Required]
        public DateTime DataHora { get; set; }

        public string? Observacoes { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public MyUser Usuario { get; set; }


         public int BarbeiroId { get; set; }
        public Barbeiro Barbeiro { get; set; } = null!;

        public int ServicoId { get; set; }
        public Servico servico { get; set; } = null!;
    }
}
