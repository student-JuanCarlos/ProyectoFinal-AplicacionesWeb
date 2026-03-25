using CapaDatos;
using CapaEntidad;
using CapaNegocio;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CapaPresentacion.Controllers
{
    public class UsuarioController : Controller
    {

        UsuarioBL usuarioBL = new UsuarioBL();
        DashboardBL dashboardBL = new DashboardBL();

        [HttpGet]
        public IActionResult Index(string Busqueda)
        {
            var listadoUsuarios = usuarioBL.ListadoUsuario(Busqueda);
            ViewBag.Roles = new List<Rol>
            {
                new Rol { IdRol = 1, NombreRol = "Administrador" },
                new Rol { IdRol = 2, NombreRol = "Trabajador" }
            };

            return View(listadoUsuarios);
        }

        [HttpPost]
        public JsonResult GuardarUsuario(Usuario usuario)
        {
            bool resultado = true;
            string mensaje = "";

            try
            {
                usuarioBL.GestionarUsuario(usuario);
            }
            catch(Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }

            return Json(new { resultado, mensaje });
        }

        [HttpPost]
        public JsonResult CambiarEstado(int id)
        {
            bool resultado = true;
            string mensaje = "";

            try
            {
                usuarioBL.CambiarEstado(id);
            }
            catch(Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }
            return Json(new { resultado, mensaje });
        }

        [HttpPost]
        public IActionResult Login(string Email, string Contraseña)
        {
            Usuario usuario = usuarioBL.LoginUsuario(Email, Contraseña);

            if (usuario == null)
            {
                ViewBag.Error = "Usuario o Contraseña Incorrectos";
                return View("Login");
            }
            if (!usuario.Estado)
            {
                ViewBag.Error = "Su cuenta ha sido desactivada";
                return View("Login");
            }

            HttpContext.Session.SetString("Usuario", JsonConvert.SerializeObject(usuario));
            HttpContext.Session.SetString("Rol", usuario.rol.NombreRol);

            if (usuario.rol.NombreRol == "Administrador")
            {
                return RedirectToAction("InicioAdministrador", "Usuario");
            }

            if (usuario.rol.NombreRol == "Trabajador")
            {
                return RedirectToAction("RegistroVentas", "Venta");
            }

            ViewBag.Error = "Rol no autorizado";
            return View("Login");
        }

        [HttpGet]
        public IActionResult Registrar()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            return View();
        }

        [HttpGet]
        public IActionResult InicioAdministrador()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Login", "Usuario");

            var (TotalVentas, TotalIngresos) = dashboardBL.VentasHoy();

            ViewBag.TotalVentas = TotalVentas;
            ViewBag.TotalIngresos = TotalIngresos;
            ViewBag.ProductosBajoStock = dashboardBL.ProductosBajoStock();
            ViewBag.ProductosMasVendidos = dashboardBL.ProductosMasVendidos();
            ViewBag.UltimasVentas = dashboardBL.UltimasVentas();

            return View();
        }

        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login", "Usuario");
        }

    }
}
