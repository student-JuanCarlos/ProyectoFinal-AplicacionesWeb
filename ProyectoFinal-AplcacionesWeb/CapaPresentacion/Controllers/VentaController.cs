using CapaEntidad;
using CapaNegocio;
using Microsoft.AspNetCore.Mvc;

namespace CapaPresentacion.Controllers
{
    public class VentaController : Controller
    {
        VentaBL ventaBL = new VentaBL();

        public IActionResult Index(string Busqueda, string NombreUsuario, bool? Estado)
        {
            if(HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            var listadoVentas = ventaBL.ListadoVentaConFiltro(Busqueda, NombreUsuario, Estado);

            return View(listadoVentas);
        }

        [HttpPost]
        public JsonResult GuardarVenta(Venta venta, List<DetalleVenta> detalles) 
        {
            bool resultado = true;
            string mensaje = "";

            try
            {
                ventaBL.RegistroVenta(venta, detalles);
            }
            catch(Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }

            return Json(new { resultado, mensaje });
        }

        [HttpPost]
        public JsonResult AnularVenta(int idVenta, int IdUsuario)
        {
            bool resultado = true;
            string mensaje = "";

            try
            {
                ventaBL.AnularVenta(idVenta, IdUsuario);
            }
            catch(Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }

            return Json(new { resultado, mensaje });
        }

        [HttpGet]
        public IActionResult DetalleVenta(int id)
        {
            if(HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            var ventaBuscada = ventaBL.DetalleVenta(id);
            return View(ventaBuscada);
        }
    }

}
