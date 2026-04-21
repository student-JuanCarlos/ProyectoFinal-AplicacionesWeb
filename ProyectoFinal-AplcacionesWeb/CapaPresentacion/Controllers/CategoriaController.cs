using CapaPresentacion.Models.Extensions;
using CapaPresentacion.Models.VM;
using Microsoft.AspNetCore.Mvc;
using SistemaLogistico.BussinesLogic.Services;

namespace CapaPresentacion.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly CategoriaService categoriaService;

        public CategoriaController(CategoriaService categoria)
        {
            categoriaService = categoria;
        }

        [HttpGet]
        public IActionResult Index(string Busqueda)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            var listadoCategorias = categoriaService.ListadoCategoria(Busqueda);

            return View(listadoCategorias.Select(c => c.ToViewModel()));
        }

        [HttpPost]
        public JsonResult GuardarCategoria(CategoriaVM categoria)
        {
            bool resultado = true;
            string mensaje = "";

            try
            {
                categoriaService.GestionarCategoria(categoria.ToEntity());
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
