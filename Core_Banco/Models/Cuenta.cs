namespace Core_Banco.Models
{
    public class Cuenta
    {
        public int CuentaID { get; set; }
        public int ClienteID { get; set; }
        public int TipoCuentaID { get; set; }
        public decimal Balance { get; set; }
        public DateTime FechaCreacion { get; set; }

        public Cliente Cliente { get; set; }
        public TipoCuenta TipoCuenta { get; set; }
        public ICollection<Transaccion> Transacciones { get; set; }
    }
}
