namespace Core_Banco.Models
{
    // MovimientoDto.cs
    public class MovimientoDto
    {
        public int Id { get; set; }
        public int MovimientoID { get; set; }
        public int CuentaID { get; set; }
        public int TipoTransaccionID { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaTransaccion { get; set; }
        public TipoTransaccionDto TipoTransaccion { get; set; }
    }

    // CreateMovimientoDto.cs
    public class CreateMovimientoDto
    {
        public int CuentaID { get; set; }
        public int TipoTransaccionID { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaTransaccion { get; set; }
    }

    // UpdateMovimientoDto.cs
    public class UpdateMovimientoDto
    {
        public decimal Monto { get; set; }
        public DateTime FechaTransaccion { get; set; }
    }


}
