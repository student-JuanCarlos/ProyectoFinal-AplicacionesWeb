using CapaEntidad;
using CapaNegocio;
using Microsoft.AspNetCore.Mvc;

namespace CapaPresentacion.Controllers
{
    public class ProveedorController : Controller
    {
        ProveedorBL proveedorBL = new ProveedorBL();

        [HttpGet]
        public IActionResult Index(string Busqueda)
        {
            if(HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            var listadoProveedores = proveedorBL.ListadoProveedor(Busqueda);
            return View(listadoProveedores);
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

        [HttpGet]
        public IActionResult DetalleProveedor(int id)
        {
            if(HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            var proveedorBuscado = proveedorBL.DetalleProveedor(id);
            return View(proveedorBuscado);
        }
    }
}
