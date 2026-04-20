using CapaPresentacion.Models.Extensions;
using CapaPresentacion.Models.VM;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SistemaLogistico.BussinesLogic.Services;
using System.Diagnostics;
using System.Security.Claims;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CapaPresentacion.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UsuarioService usuarioService;
        private readonly DashBoardService dashService;
        private readonly RolService rolService;

        public UsuarioController(UsuarioService usuario, DashBoardService dash, RolService rol)
        {
            usuarioService = usuario;
            dashService = dash;
            rolService = rol;
        }

        [HttpGet]
        public IActionResult Index(int page = 1, string Busqueda = null)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Login", "Usuario");

            var listadoUsuarios = usuarioService.ListadoUsuario(Busqueda);
            ViewBag.Roles = rolService.ListadoRoles().Select(rol => rol.ToViewModel());

            int registrosPorPagina = 8;
            int totalProductos = listadoUsuarios.Count;
            int cantidadPaginas = Convert.ToInt32(Math.Ceiling((double)totalProductos / registrosPorPagina));

            int paginasPorOmitir = registrosPorPagina * (page - 1);

            ViewBag.paginas = cantidadPaginas;
            ViewBag.paginaActual = page;

            return View(listadoUsuarios.Skip(paginasPorOmitir).Take(registrosPorPagina));
        }

        [HttpPost]
        public JsonResult GuardarUsuario(UsuarioVM usuario)
        {
            bool resultado = true;
            string mensaje = "";

            try
            {
                usuarioService.GestionarUsuario(usuario.ToEntity());
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
                usuarioService.CambiarEstado(id);
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
            Debug.WriteLine($">>>>>> Email: '{Email}' | Pass: '{Contraseña}' | PassLen: {Contraseña?.Length}");
            var usuario = usuarioService.LoginUsuario(Email, Contraseña);

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
                var usuario = JsonConvert.DeserializeObject<SistemaLogistico.Entities.Usuario>(json);
                ViewBag.Usuario = usuario;
            }

            var (TotalVentas, TotalIngresos) = dashService.VentasHoy();

            ViewBag.TotalVentas = TotalVentas;
            ViewBag.TotalIngresos = TotalIngresos;
            ViewBag.ProductosBajoStock = dashService.ProductosBajoStock();
            ViewBag.ProductosMasVendidos = dashService.ProductosMasVendidos();
            ViewBag.UltimasVentas = dashService.UltimasVentas();

            return View();
        }

        public IActionResult DetalleUsuario(int id)
        {
            var usuario = usuarioService.DetalleUsuario(id).ToViewModel();
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
                usuarioService.CambiarContraseña(id, Email, contraseñaNueva);
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
