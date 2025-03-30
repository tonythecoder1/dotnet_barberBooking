using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace dotidentity.Models
{
    public class ReservaFormModel
    {
        [Required]
        public DateTime DataHora { get; set; }

        public string? Observacoes { get; set; }

        [Required]
        public int BarbeiroId { get; set; }

        [Required]
        public int ServicoId { get; set; }

        [Required]
        public string HoraSelecionada { get; set; } = string.Empty;

        // Listas para os dropdowns
        public List<SelectListItem> Barbeiros { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Servicos { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> HorasDisponiveis { get; set; } = new List<SelectListItem>();
    }
}