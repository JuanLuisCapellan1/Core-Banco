using Core_Banco.Models;

public class Beneficiario
{
    public int BeneficiarioID { get; set; }
    public string Nombre { get; set; }
    public int CuentaID { get; set; }
    public int UsuarioID { get; set; }

    public Cuenta Cuenta { get; set; } // Propiedad de navegación

    // DTO para crear un beneficiario
    public class CreateDto
    {
        public string Nombre { get; set; }
        public int CuentaID { get; set; }
        public int UsuarioID { get; set; }
    }

    // DTO para actualizar un beneficiario
    public class UpdateDto
    {
        public string Nombre { get; set; }
        public int CuentaID { get; set; }
        public int UsuarioID { get; set; }
    }
}
