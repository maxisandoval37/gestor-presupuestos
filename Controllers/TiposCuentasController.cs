using gestorPresupuestos.Models;
using Microsoft.AspNetCore.Mvc;

namespace gestorPresupuestos.Controllers
{
    public class TiposCuentasController: Controller
    {
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Crear(TipoCuenta tipoCuenta)
        {
            return View();
        }
    }
}
