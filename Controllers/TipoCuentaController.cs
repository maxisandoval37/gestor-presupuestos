using gestorPresupuestos.Models;
using gestorPresupuestos.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace gestorPresupuestos.Controllers
{
    public class TipoCuentaController: Controller
    {
        private ITipoCuentaRepository iTiposCuentasRepository;
        public TipoCuentaController(ITipoCuentaRepository tiposCuentasRepository)
        {
            this.iTiposCuentasRepository = tiposCuentasRepository;
        }
        public IActionResult Insertar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Insertar(TipoCuenta tipoCuenta)
        {
            //Si el user ingresa un campo invalido, lo mantemos en la misma pantalla.
            if (!ModelState.IsValid)
            {
                return View(tipoCuenta);
            }
            else
            {
                tipoCuenta.id = 1;
                tipoCuenta.usuarioId = 6;

                var existeTipoCuenta = await iTiposCuentasRepository.ExisteNombreYUsuarioId(
                    tipoCuenta.nombre, tipoCuenta.usuarioId);

                if (!existeTipoCuenta)
                {
                    await iTiposCuentasRepository.Insertar(tipoCuenta);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(nameof(tipoCuenta.nombre),$"El nombre para esa cuenta, ya " +
                        $"se encuentra registrado.");
                    return View(tipoCuenta);
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> VerificarExisteTipoCuenta(string nombre)
        {
            var usuarioId = 6;
            var existeTipoCuenta = await iTiposCuentasRepository.ExisteNombreYUsuarioId(nombre, usuarioId);

            if (existeTipoCuenta)
            {
                return Json($"El nombre {nombre} ya se encuentra registrado");
            }

            return Json(true);
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = 6;
            var tiposCuentas = await iTiposCuentasRepository.ObtenerPorUsuarioId(6);
            return View(tiposCuentas);
        }
    }
}
