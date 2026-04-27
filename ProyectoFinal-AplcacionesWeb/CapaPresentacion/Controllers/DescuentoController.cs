using CapaPresentacion.Models.Extensions;
using CapaPresentacion.Models.VM;
using Microsoft.AspNetCore.Mvc;
using SistemaLogistico.BussinesLogic.Services;
using SistemaLogistico.Entities;

namespace CapaPresentacion.Controllers
{
    public class DescuentoController : Controller
    {
        private readonly DescuentoService descuentoService;
        private readonly ProductoService productoService;

        public DescuentoController(DescuentoService descuento, ProductoService producto)
        {
            descuentoService = descuento;
            productoService = producto;
        }

        public IActionResult Index(int page = 1, string Busqueda = null)
        {
            if(HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Login", "Usuario");
            
            var listadoDescuento = descuentoService.ListadoDescuento(Busqueda).Select(d => d.ToViewModel()).ToList();
            ViewBag.Productos = productoService.ListadoProducto(null).Select(p => p.ToViewModel()).ToList();

            int registrosPorPagina = 8;
            int totalDescuentos = listadoDescuento.Count;
            int cantidadPaginas = Convert.ToInt32(Math.Ceiling((double)totalDescuentos / registrosPorPagina));

            int paginasPorOmitir = registrosPorPagina * (page - 1);

            ViewBag.paginas = cantidadPaginas;
            ViewBag.paginaActual = page;

            return View(listadoDescuento.Skip(paginasPorOmitir).Take(registrosPorPagina));
        }

        [HttpPost]
        public JsonResult GestionarDescuento(DescuentoVM descuento)
        {
            bool resultado = true;
            string mensaje = "";

            try
            {
                descuentoService.GestionarDescuento(descuento.ToEntity());
            }
            catch(Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }

            return Json( new { resultado, mensaje});
        }

        [HttpPost]
        public JsonResult CambiarEstadoDescuento(int id)
        {
            bool resultado = true;
            string mensaje = "";

            try
            {
                descuentoService.CambiarEstadoDescuento(id);
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }

            return Json(new { resultado, mensaje });
        }

        public JsonResult DetalleDescuento(int id)
        {
            var descuento = descuentoService.DetalleDescuento(id).ToViewModel();
            
            return Json(descuento);
        }
    }
}
