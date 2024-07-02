using System;
using System.Collections.Generic;

namespace Core_Banco.Models
{
    public class Cliente
    {
        public int ClienteId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string DocumentoIdentidad { get; set; }
        public DateTime FechaRegistro { get; set; }

        // Relación uno a muchos con Cuenta
        public ICollection<Cuenta> Cuentas { get; set; }
    }
}
