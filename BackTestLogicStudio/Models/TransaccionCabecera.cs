using System;
using System.Collections.Generic;

namespace BackTestLogicStudio.Models;

public partial class TransaccionCabecera
{
    public int Id { get; set; }

    public int IdTipoTransaccion { get; set; }

    public DateTime Fecha { get; set; }

    public virtual TipoTransaccion IdTipoTransaccionNavigation { get; set; } = null!;

    public virtual ICollection<TransaccionDetalle> TransaccionDetalles { get; set; } = new List<TransaccionDetalle>();
}
