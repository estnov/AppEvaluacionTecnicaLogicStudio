﻿namespace BackTestLogicStudio.Models.Dtos
{
    public class ProductUpdateDto
    {
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public decimal? Precio { get; set; }
        public string? Imagen { get; set; }
        public int? Stock { get; set; }
        public int? IdCategoria { get; set; }
    }
}
