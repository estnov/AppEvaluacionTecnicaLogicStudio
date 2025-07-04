using System;
using System.Collections.Generic;

namespace BackTestLogicStudio.Models;

public partial class Categorium
{
    public int Id { get; set; }

    public string Descripcion { get; set; } = null!;

    public bool Activo { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
