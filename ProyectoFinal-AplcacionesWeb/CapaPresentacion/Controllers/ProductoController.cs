using CapaEntidad;
using CapaNegocio;
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
            if(HttpContext.Session.GetString("Usuario") == null)
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
        public JsonResult GuardarProducto(Producto producto)
        {
            bool resultado = true;
            string mensaje = "";

            try
            {
                productoBL.GestionarProducto(producto);
            }
            catch(Exception ex)
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

        [HttpGet]
        public IActionResult DetalleProducto(int id)
        {

            if(HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            var detalleproducto = productoBL.DetalleProducto(id);

            return View(detalleproducto);
        }

    }
}
