using System;

namespace Core_Banco.Models
{
    public class UpdateCuentaDto
    {
        public int TipoCuentaID { get; set; }
        public decimal Balance { get; set; }

        public DateTime FechaCreacion { get; set; }
    }
}
