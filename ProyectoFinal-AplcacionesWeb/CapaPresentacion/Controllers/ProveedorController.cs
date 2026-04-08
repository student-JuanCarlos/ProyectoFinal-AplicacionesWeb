using CapaEntidad;
using CapaNegocio;
using Microsoft.AspNetCore.Mvc;

namespace CapaPresentacion.Controllers
{
    public class ProveedorController : Controller
    {
        ProveedorBL proveedorBL = new ProveedorBL();

        [HttpGet]
        public IActionResult Index(int page = 1, string Busqueda = null)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            var listadoProveedores = proveedorBL.ListadoProveedor(Busqueda);

            int registrosPorPagina = 8;
            int totalProductos = listadoProveedores.Count;
            int cantidadPaginas = Convert.ToInt32(Math.Ceiling((double)totalProductos / registrosPorPagina));

            int paginasPorOmitir = registrosPorPagina * (page - 1);

            ViewBag.paginas = cantidadPaginas;
            ViewBag.paginaActual = page;

            return View(listadoProveedores.Skip(paginasPorOmitir).Take(registrosPorPagina));
        }

        [HttpPost]
        public JsonResult GuardarProveedor(Proveedor proveedor)
        {
            bool resultado = true;
            string mensaje = "";

            try
            {
                proveedorBL.GestionarProveedor(proveedor);
            }
            catch(Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }

            return Json(new { resultado, mensaje });
        }

        [HttpPost]
        public JsonResult CambiarEstadoProveedor(int id)
        {
            bool resultado = true;
            string mensaje = "";

            try
            {
                proveedorBL.CambiarEstadoProveedor(id);
            }
            catch(Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }

            return Json(new { resultado, mensaje });
        }

        public IActionResult DetalleProveedor(int id)
        {
            var proveedorBuscado = proveedorBL.DetalleProveedor(id);
            return Json(proveedorBuscado);
        }
    }
}
