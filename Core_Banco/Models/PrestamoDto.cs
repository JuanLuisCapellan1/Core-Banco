namespace Core_Banco.Models
{
    public class PrestamoDto
    {
        public int PrestamoId { get; set; }
        public int ClienteId { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string ClienteNombre { get; set; }
        public string ClienteDocumentoIdentidad { get; set; }
    }
}
