using CapaEntidad;
using CapaNegocio;
using Microsoft.AspNetCore.Mvc;

namespace CapaPresentacion.Controllers
{
    public class CategoriaController : Controller
    {

        CategoriaBL categoriaBL = new CategoriaBL();

        [HttpGet]
        public IActionResult Index(string Busqueda)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            var listadoCategorias = categoriaBL.ListadoCategoria(Busqueda);

            return View(listadoCategorias);
        }

        [HttpPost]
        public JsonResult GuardarCategoria(Categoria categoria)
        {
            bool resultado = true;
            string mensaje = "";

            try
            {
                categoriaBL.GestionarCategoria(categoria);
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
