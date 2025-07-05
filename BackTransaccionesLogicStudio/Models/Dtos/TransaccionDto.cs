namespace BackTransaccionesLogicStudio.Models.Dtos
{
    public class TransaccionDto
    {
        public int Id { get; set; }

        public int IdTipoTransaccion { get; set; }

        public DateTime Fecha { get; set; }

        public List<DetalleTransaccionDto> TransaccionDetalles { get; set; }
    }

    public class DetalleTransaccionDto
    {
        public int Id { get; set; }

        public int IdTransaccionCabecera { get; set; }

        public int IdProducto { get; set; }

        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        public decimal PrecioTotal { get; set; }

        public string? Detalle { get; set; }
    }
}
