using CapaDatos;
using CapaEntidad;
using CapaNegocio;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
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
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Login", "Usuario");

            var listadoUsuarios = usuarioBL.ListadoUsuario(Busqueda);
            ViewBag.Roles = new List<Rol>
            {
                new Rol { IdRol = 1, NombreRol = "SuperUser" },
                new Rol { IdRol = 2, NombreRol = "Administrador" },
                new Rol { IdRol = 3, NombreRol = "Trabajador" }
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
        public JsonResult CambiarEstadoUsuario(int id)
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
        public async Task<IActionResult> Login(string Email, string Contraseña)
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

            var claims = new List<Claim>{
                new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                new Claim(ClaimTypes.Role, usuario.rol.NombreRol)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            HttpContext.Session.SetString("Usuario", JsonConvert.SerializeObject(usuario));
            HttpContext.Session.SetString("Rol", usuario.rol.NombreRol);

            if (usuario.rol.NombreRol == "Administrador" || usuario.rol.NombreRol == "SuperUser")
            {
                return RedirectToAction("InicioAdministrador", "Usuario");
            }

            if (usuario.rol.NombreRol == "Trabajador")
            {
                return RedirectToAction("InicioAdministrador", "Usuario");
            }

            ViewBag.Error = "Rol no autorizado";
            return View("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult InicioAdministrador()
        { 
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Login", "Usuario");

            var json = HttpContext.Session.GetString("Usuario");

            if (json != null)
            {
                var usuario = JsonConvert.DeserializeObject<CapaEntidad.Usuario>(json);
                ViewBag.Usuario = usuario;
            }

            var (TotalVentas, TotalIngresos) = dashboardBL.VentasHoy();

            ViewBag.TotalVentas = TotalVentas;
            ViewBag.TotalIngresos = TotalIngresos;
            ViewBag.ProductosBajoStock = dashboardBL.ProductosBajoStock();
            ViewBag.ProductosMasVendidos = dashboardBL.ProductosMasVendidos();
            ViewBag.UltimasVentas = dashboardBL.UltimasVentas();

            return View();
        }

        public IActionResult DetalleUsuario(int id)
        {
            var usuario = usuarioBL.DetalleUsuario(id);
            return Json(usuario);
        }

        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login", "Usuario");
        }

        [HttpPost]
        public IActionResult CambiarContraseña(int id, string Email, string contraseñaNueva)
        {
            bool resultado = true;
            string mensaje = "";

            try
            {
                usuarioBL.CambiarContraseña(id, Email, contraseñaNueva);
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
