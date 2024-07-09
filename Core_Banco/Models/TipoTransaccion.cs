namespace Core_Banco.Models
{
    public class TipoTransaccion
    {
        public int TipoTransaccionID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public ICollection<Transaccion> Transacciones { get; set; }
   
    }

}
