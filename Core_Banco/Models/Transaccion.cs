namespace Core_Banco.Models
{
    public class Transaccion
    {
        public int TransaccionID { get; set; }
        public int CuentaID { get; set; }
        public int TipoTransaccionID { get; set; }

        public int BeneficiarioID { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaTransaccion { get; set; }
        public Cuenta Cuenta { get; set; }
        public TipoTransaccion TipoTransaccion { get; set; }
        public Beneficiario Beneficiario { get; set; }
    }
}
