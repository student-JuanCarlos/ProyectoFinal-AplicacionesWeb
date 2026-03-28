namespace CapaPresentacion.ViewModel
{
    public class ProductoVM
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public IFormFile Fotografia { get; set; }
        public string FotoActual { get; set; }
        public string Codigo { get; set; }
        public int IdCategoria { get; set; }
        public int IdProveedor { get; set; }
        public decimal CostoObtenido { get; set; }
        public decimal PrecioVendido { get; set; }
        public int StockActual { get; set; }
        public int StockMinimo { get; set; }
    }
}
