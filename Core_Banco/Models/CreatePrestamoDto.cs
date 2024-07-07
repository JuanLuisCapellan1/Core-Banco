namespace Core_Banco.Models
{
    public class CreatePrestamoDto
    {
        public int ClienteId { get; set; }
        public int TipoCuentaID { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public DateTime FechaVencimiento { get; set; }
    }
}
