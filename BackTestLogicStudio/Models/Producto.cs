using System;
using System.Collections.Generic;

namespace BackTestLogicStudio.Models;

public partial class Producto
{
    public int Id { get; set; }

    public int IdCategoria { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public string Imagen { get; set; } = null!;

    public decimal Precio { get; set; }

    public int Stock { get; set; }

    public virtual Categorium IdCategoriaNavigation { get; set; } = null!;

    public virtual ICollection<TransaccionDetalle> TransaccionDetalles { get; set; } = new List<TransaccionDetalle>();
}
