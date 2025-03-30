using System;

namespace dotidentity.Models
{
    public class ReservaFormModel
    {
        public DateTime DataHora { get; set; }
        public string Observacoes { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
    }
}
