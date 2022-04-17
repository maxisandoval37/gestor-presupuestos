using gestorPresupuestos.Models;
using gestorPresupuestos.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace gestorPresupuestos.Controllers
{
    public class TiposCuentasController: Controller
    {
        private ITiposCuentasRepository iTiposCuentasRepository;
        public TiposCuentasController(ITiposCuentasRepository tiposCuentasRepository)
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
                    return View();
                }
                else
                {
                    ModelState.AddModelError(nameof(tipoCuenta.nombre),$"El nombre para esa cuenta, ya " +
                        $"se encuentra registrado.");
                    Console.WriteLine("Registro creado correctamente");
                    return View(tipoCuenta);
                }
            }
        }
    }
}
