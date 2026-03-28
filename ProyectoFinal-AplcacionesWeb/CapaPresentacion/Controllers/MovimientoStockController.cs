using CapaEntidad;
using CapaNegocio;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CapaPresentacion.Controllers
{
    public class MovimientoStockController : Controller
    {

        MovimientoStockBL movimientostockBL = new MovimientoStockBL();
        ProductoBL productoBL = new ProductoBL();

        [HttpGet]
        public IActionResult Index(string TipoDeMovimiento, string Busqueda)
        {
            

            ViewBag.Productos = productoBL.ListadoProducto(null);
            var listadoMovimientos = movimientostockBL.ListadoMovimiento(TipoDeMovimiento, Busqueda);
            return View(listadoMovimientos);
        }

        [HttpPost]
        public JsonResult GuardarMovimiento(MovimientoStock movimientoStock)
        {
            bool resultado = true;
            string mensaje = "";

            try
            {
                var json = HttpContext.Session.GetString("Usuario");
                var usuario = JsonConvert.DeserializeObject<CapaEntidad.Usuario>(json);
                movimientoStock.IdUsuario = usuario.IdUsuario;

                movimientostockBL.AgregarMovimiento(movimientoStock);
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
