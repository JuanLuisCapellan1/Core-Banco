using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
namespace Core_Banco.Models
{
    public class Perfil
    {
        public int PerfilID { get; set; }
        public string NombrePerfil { get; set; }
        public string Descripcion { get; set; }

        // Navegación hacia PerfilesRoles
    }
}

namespace Core_Banco.Models
{
    public class UpdatePerfilDto
    {
        public string NombrePerfil { get; set; }
        public string Descripcion { get; set; }
    }
}

namespace Core_Banco.Models
{
    public class CreatePerfilDto
    {
        public int PerfilID { get; set; }
        public string NombrePerfil { get; set; }
        public string Descripcion { get; set; }
    }
}