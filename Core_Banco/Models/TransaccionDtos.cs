﻿public class CreateTransaccionDto
{
    public int CuentaID { get; set; }
    public int TipoTransaccionID { get; set; }
    public decimal Monto { get; set; }
    public DateTime FechaTransaccion { get; set; }
    public int CuentaOrigenID { get; set; } = 0; // Valor predeterminado 0
    public int CuentaDestinoID { get; set; } = 0; // Valor predeterminado 0
}

public class TransaccionDto
{
    public int TransaccionID { get; set; }
    public int CuentaID { get; set; }
    public int TipoTransaccionID { get; set; }
    public decimal Monto { get; set; }
    public DateTime FechaTransaccion { get; set; }
}

public class UpdateTransaccionDto
{
    public int CuentaID { get; set; }
    public int TipoTransaccionID { get; set; }
    public decimal Monto { get; set; }
    public DateTime FechaTransaccion { get; set; }
    public int CuentaOrigenID { get; set; } = 0; // Valor predeterminado 0
    public int CuentaDestinoID { get; set; } = 0; // Valor predeterminado 0
}