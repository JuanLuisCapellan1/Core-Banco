﻿namespace Core_Banco.Models
{
    public class UpdateClienteDto
    {
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? DocumentoIdentidad { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}