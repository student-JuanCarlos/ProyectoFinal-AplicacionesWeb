using CapaPresentacion.Models.Extensions;
using CapaPresentacion.Models.VM;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SistemaLogistico.BussinesLogic.Services;

namespace CapaPresentacion.Controllers
{
    public class MovimientoStockController : Controller
    {
        private readonly MovimientoStockService movimientoService;
        private readonly ProductoService productoService;

        public MovimientoStockController(MovimientoStockService movimiento, ProductoService producto)
        {
            movimientoService = movimiento;
            productoService = producto;
        }

        [HttpGet]
        public IActionResult Index(string TipoDeMovimiento, string Busqueda)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            ViewBag.Productos = productoService.ListadoProducto(null).Select(p => p.ToViewModel());
            var listadoMovimientos = movimientoService.ListadoMovimiento(TipoDeMovimiento, Busqueda);
            return View(listadoMovimientos.Select(m => m.ToViewModelM()));
        }

        [HttpPost]
        public JsonResult GuardarMovimiento(MovimientoStockVM movimientoStock)
        {
            bool resultado = true;
            string mensaje = "";

            try
            {
                var json = HttpContext.Session.GetString("Usuario");
                var usuario = JsonConvert.DeserializeObject<CapaPresentacion.Models.VM.UsuarioVM>(json);
                movimientoStock.IdUsuario = usuario.IdUsuario;

                movimientoService.AgregarMovimiento(movimientoStock.ToEntity());
            }
            catch(Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }

            return Json(new { resultado, mensaje });
        }

    }
}
