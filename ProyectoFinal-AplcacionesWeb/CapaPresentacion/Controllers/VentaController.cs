using CapaEntidad;
using CapaEntidad.Request;
using CapaNegocio;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CapaPresentacion.Controllers
{
    public class VentaController : Controller
    {
        VentaBL ventaBL = new VentaBL();
        ProductoBL productoBL = new ProductoBL();

        public IActionResult Index(string Busqueda, string NombreUsuario, bool? Estado)
        {
            if(HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            var json = HttpContext.Session.GetString("Usuario");
            var usuario = JsonConvert.DeserializeObject<CapaEntidad.Usuario>(json);
            ViewBag.Usuario = usuario;

            ViewBag.Productos = productoBL.ListadoConFiltro(null);
            var listadoVentas = ventaBL.ListadoVentaConFiltro(Busqueda, NombreUsuario, Estado);

            return View(listadoVentas);
        }

        [HttpPost]
        public JsonResult GuardarVenta([FromBody] VentaRequest request) 
        {
            bool resultado = true;
            string mensaje = "";

            try
            {
                var json = HttpContext.Session.GetString("Usuario");
                var usuario = JsonConvert.DeserializeObject<CapaEntidad.Usuario>(json);
                request.Venta.IdUsuario = usuario.IdUsuario;

                ventaBL.RegistroVenta(request.Venta, request.Detalles);
            }
            catch(Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }

            return Json(new { resultado, mensaje });
        }

        [HttpPost]
        public JsonResult AnularVenta([FromBody] int IdVenta)
        {
            bool resultado = true;
            string mensaje = "";

            try
            {
                var json = HttpContext.Session.GetString("Usuario");
                var usuario = JsonConvert.DeserializeObject<CapaEntidad.Usuario>(json);

                ventaBL.AnularVenta(IdVenta, usuario.IdUsuario);
            }
            catch(Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }

            return Json(new { resultado, mensaje });
        }

        public IActionResult DetalleVenta(int id)
        {
            var ventaBuscada = ventaBL.DetalleVenta(id);
            return Json(ventaBuscada);
        }

    }

}
