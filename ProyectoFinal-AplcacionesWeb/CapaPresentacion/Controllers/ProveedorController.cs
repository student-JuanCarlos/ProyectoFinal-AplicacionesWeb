using CapaPresentacion.Models.Extensions;
using CapaPresentacion.Models.VM;
using Microsoft.AspNetCore.Mvc;
using SistemaLogistico.BussinesLogic.Services;

namespace CapaPresentacion.Controllers
{
    public class ProveedorController : Controller
    {
        private readonly ProveedorService proveedorService;

        public ProveedorController(ProveedorService proveedor)
        {
            proveedorService = proveedor;
        }

        [HttpGet]
        public IActionResult Index(int page = 1, string Busqueda = null)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            var listadoProveedores = proveedorService.ListadoProveedor(Busqueda);

            int registrosPorPagina = 8;
            int totalProductos = listadoProveedores.Count;
            int cantidadPaginas = Convert.ToInt32(Math.Ceiling((double)totalProductos / registrosPorPagina));

            int paginasPorOmitir = registrosPorPagina * (page - 1);

            ViewBag.paginas = cantidadPaginas;
            ViewBag.paginaActual = page;

            return View(listadoProveedores.Select(p => p.ToViewModel()).Skip(paginasPorOmitir).Take(registrosPorPagina));
        }

        [HttpPost]
        public JsonResult GuardarProveedor(ProveedorVM proveedor)
        {
            bool resultado = true;
            string mensaje = "";

            try
            {
                proveedorService.GestionarProveedor(proveedor.ToEntity());
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
                proveedorService.CambiarEstadoProveedor(id);
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
            var proveedorBuscado = proveedorService.DetalleProveedor(id).ToViewModel();
            return Json(proveedorBuscado);
        }
    }
}
