using AutoMapper;
using gestorPresupuestos.Models;
using gestorPresupuestos.Models.Reportes;
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
        private readonly ITransaccionRepository iTransaccionRepository;
        private readonly IMapper imapper;
        private Utils utils;

        public CuentaController(
            ITipoCuentaRepository iTipoCuentaRepository,
            IUsuarioRepository iUsuarioRepository,
            ICuentaRepository iCuentaRepository,
            ITransaccionRepository iTransaccionRepository,
            IMapper imapper)
        {
            this.iTipoCuentaRepository = iTipoCuentaRepository;
            this.iUsuarioRepository = iUsuarioRepository;
            this.iCuentaRepository = iCuentaRepository;
            this.iTransaccionRepository = iTransaccionRepository;
            this.imapper = imapper;
            utils = new Utils();
        }

        public ITipoCuentaRepository ITipoCuentaRepository { get; }

        [HttpGet]
        public async Task<IActionResult> Insertar()
        {
            var usuarioId = iUsuarioRepository.ObtenerUsuarioId();
            var modelo = new CuentaCreacionViewModel();
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

            cuenta.nombre = utils.capitalizarStr(cuenta.nombre);
            await iCuentaRepository.Insertar(cuenta);
            return RedirectToAction("Index");
        }

        private async Task<IEnumerable<SelectListItem>> ObtenerTiposCuentas(int usuarioId)
        {
            var tiposCuentas = await iTipoCuentaRepository.ObtenerPorUsuarioId(usuarioId);
            return tiposCuentas.Select(x => new SelectListItem(x.nombre, x.id.ToString()));
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

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var usuarioId = iUsuarioRepository.ObtenerUsuarioId();
            var cuenta = await iCuentaRepository.obtenerPorId(id, usuarioId);

            if (cuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            else
            {
                var modelo = imapper.Map<CuentaCreacionViewModel>(cuenta);//<hasta>(desde)

                modelo.tiposCuentas = await ObtenerTiposCuentas(usuarioId);
                return View(modelo);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Editar(CuentaCreacionViewModel cuentaCreacionViewModel)
        {
            var usuarioId = iUsuarioRepository.ObtenerUsuarioId();
            var cuenta = await iCuentaRepository.obtenerPorId(cuentaCreacionViewModel.id, usuarioId);

            if (cuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            else
            {
                var tipoCuenta = await iTipoCuentaRepository.ObtenerPorId(cuentaCreacionViewModel.tipoCuentaId, usuarioId);
                if (tipoCuenta is null)
                {
                    return RedirectToAction("NoEncontrado", "Home");
                }
                else
                {
                    await iCuentaRepository.Editar(cuentaCreacionViewModel);
                    return RedirectToAction("Index");
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> Borrar (int id)
        {
            var usuarioId = iUsuarioRepository.ObtenerUsuarioId();
            var cuenta = await iCuentaRepository.obtenerPorId(id, usuarioId);

            if (cuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            else
            {
                return View(cuenta);
            }
        }

        [HttpPost]
        public async Task<IActionResult> BorrarCuenta(int id)
        {
            var usuarioId = iUsuarioRepository.ObtenerUsuarioId();
            var cuenta = await iCuentaRepository.obtenerPorId(id, usuarioId);

            if (cuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            else
            {
                await iCuentaRepository.Borrar(id);
                return RedirectToAction("Index");
            }
        }

        //Visualizar los movimientos de la cuenta por mes.
        public async Task<IActionResult> Detalle(int id, int mes, int anio)
        {
            var usuarioId = iUsuarioRepository.ObtenerUsuarioId();
            var cuenta = await iCuentaRepository.obtenerPorId(id, usuarioId);

            if (cuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            else
            {
                DateTime fechaInicio;
                DateTime fechaFin;

                //si la fecha ingresada no es valida agarramos el mes actual
                if (mes <=0 || mes >12 || anio <= 1900)
                {
                    fechaInicio = new DateTime(DateTime.Today.Year, DateTime.Today.Month,1);
                }
                else
                {
                    fechaInicio = new DateTime(anio,mes,1);
                }

                //Establecemos como fecha fin el ultimo dia del mismo mes de la fecha inicio:
                fechaFin = fechaInicio.AddMonths(1).AddDays(-1);

                var getTransaccionesPorCuenta = new ParametroGetTransaccionesPorCuenta()
                {
                    cuentaId = id,
                    usuarioId = usuarioId,
                    fechaInicio = fechaInicio,
                    fechaFin = fechaFin
                };

                var transacciones = await iTransaccionRepository.ObtenerPorCuentaId(getTransaccionesPorCuenta);
                var modelo = new ReporteTransaccionesDetalladas();
                ViewBag.Cuenta = cuenta.nombre;

                //Ordenamos las transacciones por fecha
                var transaccionesPorFecha = transacciones.OrderByDescending(x => x.fechaTransaccion)
                    .GroupBy(x => x.fechaTransaccion)
                    .Select(group => new ReporteTransaccionesDetalladas.TransaccionesPorFecha()
                    {
                        fechaTransaccion = group.Key,
                        Transacciones = group.AsEnumerable()
                    });

                modelo.transaccionesAgrupadas = transaccionesPorFecha;
                modelo.fechaInicio = fechaInicio;
                modelo.fechaFin = fechaFin;

                ViewBag.mesAnterior = fechaInicio.AddMonths(-1).Month;
                ViewBag.anioAnterior = fechaInicio.AddMonths(-1).Year;

                ViewBag.mesPosterior = fechaInicio.AddMonths(1).Month;
                ViewBag.anioPosterior = fechaInicio.AddMonths(1).Year;

                return View(modelo);
            }
        }
    }
}