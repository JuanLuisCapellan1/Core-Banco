using System;
using System.ComponentModel.DataAnnotations;

namespace Core_Banco.Models
{
    public class TipoCuenta
    {
        public int TipoCuentaID { get; set; } // Auto-incremental
        public string Nombre { get; set; }

        public class CreateTipoCuentaDto
        {
            [Required]
            [MaxLength(100)]
            public string Nombre { get; set; }
        }

        public class UpdateTipoCuentaDto
        {
            [Required]
            [MaxLength(100)]
            public string Nombre { get; set; }
        }
    }
}
