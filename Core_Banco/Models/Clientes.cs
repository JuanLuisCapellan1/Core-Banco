using System.ComponentModel.DataAnnotations;

namespace Core_Banco.Models
{
    public class Clientes
    {
        [Key]
        public int ClienteId { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? DocumentoIdentidad { get; set; }
        public DateTime FechaRegistro { get; set; }

    }
}
