using gestorPresupuestos.Models;
using gestorPresupuestos.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace gestorPresupuestos.Controllers
{
    public class CuentaController : Controller
    {
        private readonly ITipoCuentaRepository iTipoCuentaRepository;
        private readonly IUsuarioRepository iUsuarioRepository;
        private readonly ICuentaRepository iCuentaRepository;
        private Utils utils;

        public CuentaController(
            ITipoCuentaRepository iTipoCuentaRepository,
            IUsuarioRepository iUsuarioRepository,
            ICuentaRepository iCuentaRepository)
        {
            this.iTipoCuentaRepository = iTipoCuentaRepository;
            this.iUsuarioRepository = iUsuarioRepository;
            this.iCuentaRepository = iCuentaRepository;
        }

        public ITipoCuentaRepository ITipoCuentaRepository { get; }

        [HttpGet]
        public async Task<IActionResult> Insertar()
        {
            var usuarioId = iUsuarioRepository.ObtenerUsuarioId();
            var modelo = new CuentaCreacionViewModel();

            utils = new Utils();
            modelo.tiposCuentas = await ObtenerTiposCuentas(usuarioId);

            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Insertar(CuentaCreacionViewModel cuenta)
        {
            var usuarioId = iUsuarioRepository.ObtenerUsuarioId();
            var tiposCuentas = await iTipoCuentaRepository.ObtenerPorId(cuenta.tipoCuentaId, usuarioId);

            if (tiposCuentas is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            if (!ModelState.IsValid)
            {
                cuenta.tiposCuentas = await ObtenerTiposCuentas(usuarioId);
                return View(cuenta);
            }

            await iCuentaRepository.Insertar(cuenta);
            return RedirectToAction("Index");
        }

        private async Task<IEnumerable<SelectListItem>> ObtenerTiposCuentas(int usuarioId)
        {
            var tiposCuentas = await iTipoCuentaRepository.ObtenerPorUsuarioId(usuarioId);
            return tiposCuentas.Select(x => new SelectListItem(utils.capitalizarStr(x.nombre), x.id.ToString()));
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = iUsuarioRepository.ObtenerUsuarioId();
            var cuentasConTC = await iCuentaRepository.BuscarPorUsuarioId(usuarioId);

            //nos trae los tipos cuentas asociados a una cuenta:
            var modelo = cuentasConTC.
                GroupBy(x => x.tipoCuenta).
                Select(group => new IndiceCuentaViewModel
                {
                    tipoCuenta = group.Key,
                    cuentas = group.AsEnumerable()
                }).ToList();

            return View(modelo);
        }
    }
}