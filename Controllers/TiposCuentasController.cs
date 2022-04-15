using Dapper;
using gestorPresupuestos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace gestorPresupuestos.Controllers
{
    public class TiposCuentasController: Controller
    {
        private readonly string connectionString;
        public TiposCuentasController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Crear(TipoCuenta tipoCuenta)
        {
            //Si el user ingresa un campo invalido, lo mantemos en la misma pantalla.
            if (!ModelState.IsValid)
            {
                return View(tipoCuenta);
            }
            else
            {
                return View();
            }
        }
    }
}
