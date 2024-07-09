namespace Core_Banco.Models
{
    public class Usuario
    {
        public int UsuarioID { get; set; }
        public string NombreUsuario { get; set; }
        public string Contraseña { get; set; }
        public int PerfilID { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime UltimoAcceso { get; set; }
        public int ClienteID { get; set; }
        // Navegación hacia Perfil
        public Perfil Perfil { get; set; }
    }
}

public class CreateUserDto
{
    public string NombreUsuario { get; set; }
    public string Contraseña { get; set; }
    public int PerfilID { get; set; }
    public int ClienteID { get; set; }
}

public class UpdateUserDto
{
    public string NombreUsuario { get; set; }
    public string Contraseña { get; set; }
    public int PerfilID { get; set; }
    public int ClienteID { get; set; }
}