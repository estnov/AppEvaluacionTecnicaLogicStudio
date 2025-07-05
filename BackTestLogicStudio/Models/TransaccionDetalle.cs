using System;
using System.Collections.Generic;

namespace BackTestLogicStudio.Models;

public partial class TransaccionDetalle
{
    public int Id { get; set; }

    public int IdTransaccionCabecera { get; set; }

    public int IdProducto { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal PrecioTotal { get; set; }

    public string? Detalle { get; set; }

    public virtual Producto IdProductoNavigation { get; set; } = null!;

    public virtual TransaccionCabecera IdTransaccionCabeceraNavigation { get; set; } = null!;
}
