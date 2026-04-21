using Microsoft.Extensions.DependencyInjection;
using SistemaLogistico.BussinesLogic.Services;
using SistemaLogistico.BussinesLogic.Utilidades;
using SistemaLogistico.Data.Infraestructure;
using SistemaLogistico.Data.Repository;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SistemaLogistico.BussinesLogic.Inyeccion
{
    public static class InyectorDependencias
    {

        public static void Inyeccion(this IServiceCollection services)
        {
            services.AddScoped<ICategoria, CategoriaRepository>();
            services.AddScoped<CategoriaService>();

            services.AddScoped<IDashBoard, DashBoardRepository>();
            services.AddScoped<DashBoardService>();

            services.AddScoped<IMovimientoStock, MovimientoStockRepository>();
            services.AddScoped<MovimientoStockService>();
            services.AddScoped<DashBoardService>();

            services.AddScoped<IProducto, ProductoReposittory>();
            services.AddScoped<ProductoService>();

            services.AddScoped<IProveedor, ProveedorRepository>();
            services.AddScoped<ProveedorService>();

            services.AddScoped<IUsuario, UsuarioRepository>();
            services.AddScoped<UsuarioService>();

            services.AddScoped<IVenta, VentaRepository>();
            services.AddScoped<VentaService>();

            services.AddScoped<IRol, RolRepository>();
            services.AddScoped<RolService>();

            services.AddScoped<IUtilidades, UtilidadesService>();
        }

    }
}
