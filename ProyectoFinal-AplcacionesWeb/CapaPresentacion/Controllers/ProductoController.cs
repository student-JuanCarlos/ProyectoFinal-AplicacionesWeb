using CapaPresentacion.Models.Extensions;
using CapaPresentacion.Models.VM;
using Microsoft.AspNetCore.Mvc;
using SistemaLogistico.BussinesLogic.Services;
using SistemaLogistico.Entities;

namespace CapaPresentacion.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ProductoService productoService;
        private readonly CategoriaService categoriaService;
        private readonly ProveedorService proveedorService;

        public ProductoController(ProductoService producto, CategoriaService categoria, ProveedorService proveedor)
        {
            productoService = producto;
            categoriaService = categoria;
            proveedorService = proveedor;
        }

        [HttpGet]
        public IActionResult Index(int page = 1, string Busqueda = null, int id = 0)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            if (id > 0)
                ViewBag.HistorialMovimiento = productoService.HistorialMovimientoProducto(id).Select(h => h.ToViewModel()).ToList();
            else
                ViewBag.HistorialMovimiento = new List<MovimientoStockVM>();

            ViewBag.Proveedores = proveedorService.ListadoProveedorConFiltro().Select(p => p.ToViewModel());
            ViewBag.Categorias = categoriaService.ListadoCategoria(null).Select(c => c.ToViewModel());
            var listadoProductos = productoService.ListadoProducto(Busqueda).Select(p => p.ToViewModel()).ToList();

            int registrosPorPagina = 8;
            int totalProductos = listadoProductos.Count;
            int cantidadPaginas = Convert.ToInt32(Math.Ceiling((double)totalProductos / registrosPorPagina));

            int paginasPorOmitir = registrosPorPagina * (page - 1);

            ViewBag.paginas = cantidadPaginas;
            ViewBag.paginaActual = page;

            return View(listadoProductos.Skip(paginasPorOmitir).Take(registrosPorPagina));
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

                    var categoria = categoriaService.ListadoCategoria(null)
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

                    var nombreRealImagen = Path.GetFileName(model.Fotografia.FileName);
                    nombreImagen = $"assets/img/productos/{nombreRealImagen}";
                    var pathImagen = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/img/productos", nombreRealImagen);

                    using (var stream = new FileStream(pathImagen, FileMode.Create))
                    {
                        model.Fotografia.CopyTo(stream);
                    }
                }

                model.FotoActual = $"{nombreImagen}";

                productoService.GestionarProducto(model.ToEntity());
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
                productoService.CambiarEstadoProducto(id);
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
            var detalleproducto = productoService.DetalleProducto(id).ToViewModel();

            return Json(detalleproducto);
        }

        public JsonResult HistorialProducto(int id)
        {
            var historial = productoService.HistorialMovimientoProducto(id).Select(h => h.ToViewModel()).ToList();
            return Json(historial);
        }

    }
}
