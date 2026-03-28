using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace CapaPresentacion.Controllers
{
    public class ProductoController : Controller
    {

        ProductoBL productoBL = new ProductoBL();
        CategoriaBL categoriaBL = new CategoriaBL();
        ProveedorBL proveedorBL = new ProveedorBL();

        [HttpGet]
        public IActionResult Index(string Busqueda, int id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            if (id > 0)
                ViewBag.HistorialMovimiento = productoBL.HistorialMovimientoProducto(id);
            else
                ViewBag.HistorialMovimiento = new List<MovimientoStock>();

            ViewBag.Proveedores = proveedorBL.ListadoProveedor(null);
            ViewBag.Categorias = categoriaBL.ListadoCategoria(null);
            var listadoProductos = productoBL.ListadoProducto(Busqueda);
            return View(listadoProductos);
        }

        [HttpPost]
        public JsonResult GuardarProducto(ProductoVM model)
        {
            bool resultado = true;
            string mensaje = "";

            try
            {
                if (model.IdProducto == 0)
                {
                    if (model.Fotografia == null)
                        return Json(new { resultado = false, mensaje = "La fotografía es obligatoria para nuevos productos." });

                    var categoria = categoriaBL.ListadoCategoria(null)
                        .FirstOrDefault(c => c.IdCategoria == model.IdCategoria);

                    string prefijo = categoria.NombreCategoria.Substring(0, 3).ToUpper();
                    model.Codigo = $"{prefijo}-{Guid.NewGuid().ToString().Substring(0, 6).ToUpper()}";
                }

                string nombreImagen = model.FotoActual ?? "";

                if (model.Fotografia != null)
                {
                    if (model.IdProducto != 0 && !string.IsNullOrEmpty(model.FotoActual))
                    {
                        var fotoAnterior = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", model.FotoActual);
                        if (System.IO.File.Exists(fotoAnterior))
                            System.IO.File.Delete(fotoAnterior);
                    }

                    nombreImagen = $"{Guid.NewGuid()}{Path.GetExtension(model.Fotografia.FileName)}";
                    var pathImagen = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/img/productos", nombreImagen);

                    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/img/productos"));

                    using (var stream = new FileStream(pathImagen, FileMode.Create))
                    {
                        model.Fotografia.CopyTo(stream);
                    }
                }

                var producto = new Producto()
                {
                    IdProducto = model.IdProducto,
                    NombreProducto = model.NombreProducto,
                    Fotografia = $"assets/img/productos/{nombreImagen}",
                    Codigo = model.Codigo,
                    IdCategoria = model.IdCategoria,
                    IdProveedor = model.IdProveedor,
                    CostoObtenido = model.CostoObtenido,
                    PrecioVendido = model.PrecioVendido,
                    StockActual = model.StockActual,
                    StockMinimo = model.StockMinimo,
                };

                productoBL.GestionarProducto(producto);
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }

            return Json(new { resultado, mensaje });
        }

        [HttpPost]
        public JsonResult CambiarEstadoProducto(int id)
        {
            bool resultado = true;
            string mensaje = "";

            try
            {
                productoBL.CambiarEstadoProducto(id);
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }

            return Json(new { resultado, mensaje });
        }

        public IActionResult DetalleProducto(int id)
        {
            var detalleproducto = productoBL.DetalleProducto(id);

            return Json(detalleproducto);
        }

        public JsonResult HistorialProducto(int id)
        {
            var historial = productoBL.HistorialMovimientoProducto(id);
            return Json(historial);
        }

    }
}
