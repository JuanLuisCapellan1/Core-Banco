namespace Core_Banco.Models
{
    public class Movimiento
    {
        public int MovimientoID { get; set; }
        public int CuentaID { get; set; }
        public int TipoTransaccionID { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaTransaccion { get; set; }

        public Cuenta Cuenta { get; set; }
        public TipoTransaccion TipoTransaccion { get; set; }
    }
}
