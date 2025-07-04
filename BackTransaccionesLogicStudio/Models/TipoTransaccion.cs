using System;
using System.Collections.Generic;

namespace BackTransaccionesLogicStudio.Models;

public partial class TipoTransaccion
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public bool Activo { get; set; }

    public virtual ICollection<TransaccionCabecera> TransaccionCabeceras { get; set; } = new List<TransaccionCabecera>();
}
