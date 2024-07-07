namespace Core_Banco.Models
{
    public class CreateTipoTransaccionDto
    {

        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }

    public class TipoTransaccionDto
    {
        public int TipoTransaccionID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }

    public class UpdateTipoTransaccionDto
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}
