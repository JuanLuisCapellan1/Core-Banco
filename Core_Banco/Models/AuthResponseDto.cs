namespace Core_Banco.Models
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public int ClienteID { get; set; } // Agrega esta línea

        public int PerfilID { get; set; }
    }

}