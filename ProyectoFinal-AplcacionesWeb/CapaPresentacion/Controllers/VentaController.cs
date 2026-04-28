using CapaPresentacion.Models.Extensions;
using CapaPresentacion.Models.Request;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SistemaLogistico.BussinesLogic.Services;

namespace CapaPresentacion.Controllers
{
    public class VentaController : Controller
    {
        private readonly ProductoService productoService;
        private readonly VentaService ventaService;
        private readonly DescuentoService descuentoService;

        public VentaController(ProductoService producto, VentaService venta, DescuentoService descuento)
        {
            productoService = producto;
            ventaService = venta;
            descuentoService = descuento;
        }

        public IActionResult Index(string Busqueda, string NombreUsuario, bool? Estado)
        {
            if(HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            var json = HttpContext.Session.GetString("Usuario");
            var usuario = JsonConvert.DeserializeObject<CapaPresentacion.Models.VM.UsuarioVM>(json);
            ViewBag.Usuario = usuario;

            ViewBag.Productos = productoService.ListadoConFiltro(null).Select(p => p.ToViewModel()).ToList();
            ViewBag.Descuentos = descuentoService.ListadoDescuentoConFiltro(null).Select(d => d.ToViewModel()).ToList();
            var listadoVentas = ventaService.ListadoVentaConFiltro(Busqueda, NombreUsuario, Estado);

            return View(listadoVentas.Select(v => v.ToViewModel()));
        }

        [HttpPost]
        public JsonResult GuardarVenta([FromBody] VentaRequest request) 
        {
            bool resultado = true;
            string mensaje = "";

            try
            {
                var json = HttpContext.Session.GetString("Usuario");
                var usuario = JsonConvert.DeserializeObject<CapaPresentacion.Models.VM.UsuarioVM>(json);
                request.Venta.IdUsuario = usuario.IdUsuario;

                ventaService.RegistroVenta(request.Venta.ToEntity(), request.Detalles.Select(d => d.ToEntity()).ToList(), request.Descuentos.Select(d => d.ToEntity()).ToList());
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
                var usuario = JsonConvert.DeserializeObject<CapaPresentacion.Models.VM.UsuarioVM>(json);

                ventaService.AnularVenta(IdVenta, usuario.IdUsuario);
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
            var ventaBuscada = ventaService.DetalleVenta(id).ToViewModel();
            return Json(ventaBuscada);
        }

    }

}
