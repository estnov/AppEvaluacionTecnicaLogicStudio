namespace BackTestLogicStudio.Models.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public string? Imagen { get; set; }
        public int Stock { get; set; }
        public int IdCategoria { get; set; }
    }
}
