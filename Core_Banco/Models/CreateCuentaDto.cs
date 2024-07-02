namespace Core_Banco.Models
{
    public class CreateCuentaDto
    {
        public int ClienteID { get; set; }
        // No incluimos Balance aquí ya que será siempre 0 al crear una nueva cuenta
        public DateTime FechaCreacion { get; set; }
    }
}
