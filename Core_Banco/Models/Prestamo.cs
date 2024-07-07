using System;

namespace Core_Banco.Models
{
    public class Prestamo
    {
        public int PrestamoId { get; set; }
        public int ClienteId { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public DateTime FechaVencimiento { get; set; }

        public Cliente Cliente { get; set; }
    }
}
