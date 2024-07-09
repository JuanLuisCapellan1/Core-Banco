namespace Core_Banco.Models
{
    public class CuentaDto
    {
        public int CuentaId { get; set; }
        public int ClienteId { get; set; }
        public int TipoCuentaID { get; set; }
        public decimal Balance { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string ClienteNombre { get; set; }
        public string ClienteApellido { get; set; }
        public string ClienteDocumentoIdentidad { get; set; }
    }
}
