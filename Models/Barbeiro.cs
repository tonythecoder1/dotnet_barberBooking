using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotidentity.Models;

namespace dotidentity
{
    public class Barbeiro
    {
         public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
    }
}