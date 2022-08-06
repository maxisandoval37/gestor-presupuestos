using AutoMapper;
using gestorPresupuestos.Models;
using gestorPresupuestos.Models.Reportes;
using gestorPresupuestos.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace gestorPresupuestos.Controllers
{
    public class TransaccionController : Controller
    {
        private readonly IUsuarioRepository iUsuarioRepository;
        private readonly ICuentaRepository iCuentaRepository;
        private readonly ICategoriaRepository iCategoriaRepository;
        private readonly ITransaccionRepository iTransaccionRepository;
        private readonly IMapper iMapper;

        public TransaccionController(
            ITransaccionRepository iTransaccionRepository,
            IUsuarioRepository iUsuarioRepository,
            ICuentaRepository iCuentaRepository,
            ICategoriaRepository iCategoriaRepository,
            IMapper iMapper)
        {
            this.iTransaccionRepository = iTransaccionRepository;
            this.iUsuarioRepository = iUsuarioRepository;
            this.iCuentaRepository = iCuentaRepository;
            this.iCategoriaRepository = iCategoriaRepository;
            this.iMapper = iMapper;
        }

        public async Task<IActionResult> Insertar()
        {
            var usuarioId = iUsuarioRepository.ObtenerUsuarioId();
            var modelo = new TransaccionCreacionViewModel();
            modelo.Cuentas = await ObtenerCuentas(usuarioId);
            modelo.Categorias = await ObtenerCategorias(usuarioId, modelo.tipoOperacionId);

            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Insertar(TransaccionCreacionViewModel transaccionCM)
        {
            var usuarioId = iUsuarioRepository.ObtenerUsuarioId();

            if (!ModelState.IsValid)
            {
                transaccionCM.Cuentas = await ObtenerCuentas(usuarioId);
                transaccionCM.Categorias = await ObtenerCategorias(usuarioId, transaccionCM.tipoOperacionId);
                return View(transaccionCM);
            }
            else
            {
                var cuenta = await iCuentaRepository.obtenerPorId(transaccionCM.categoriaId, usuarioId);

                if (cuenta is null)
                {
                    return RedirectToAction("NoEncontrado", "Home");
                }

                var categoria = await iCategoriaRepository.ObtenerPorId(transaccionCM.cuentaId, usuarioId);

                if (categoria is null)
                {
                    return RedirectToAction("NoEncontrado", "Home");
                }

                transaccionCM.usuarioId = usuarioId;

                if (transaccionCM.tipoOperacionId == TipoOperacion.Egreso)
                {
                    transaccionCM.monto = Math.Abs(transaccionCM.monto);
                }

                await iTransaccionRepository.Insertar(transaccionCM);
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Index(int mes, int anio)
        {
            var usuarioId = iUsuarioRepository.ObtenerUsuarioId();

            DateTime fechaInicio;
            DateTime fechaFin;

            //si la fecha ingresada no es valida agarramos el mes actual
            if (mes <= 0 || mes > 12 || anio <= 1900)
            {
                fechaInicio = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            }
            else
            {
                fechaInicio = new DateTime(anio, mes, 1);
            }

            //Establecemos como fecha fin el ultimo dia del mismo mes de la fecha inicio:
            fechaFin = fechaInicio.AddMonths(1).AddDays(-1);

            var parametro = new ParametroGetTransaccionesPorUsuario()
            {
                usuarioId = usuarioId,
                fechaInicio = fechaInicio,
                fechaFin = fechaFin
            };

            var transacciones = await iTransaccionRepository.ObtenerPorUsuarioId(parametro);

            var modelo = new ReporteTransaccionesDetalladas();

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

            ViewBag.urlRegreso = HttpContext.Request.Path + HttpContext.Request.QueryString;

            return View(modelo);
        }

        private async Task<IEnumerable<SelectListItem>> ObtenerCuentas(int usuarioId)
        {
            var cuentas = await iCuentaRepository.BuscarPorUsuarioId(usuarioId);
            return cuentas.Select(x => new SelectListItem(x.nombre, x.id.ToString()));
        }

        [HttpPost]
        public async Task<IActionResult> ObtenerCategorias([FromBody] TipoOperacion tipoOperacion)
        {
            var usuarioId = iUsuarioRepository.ObtenerUsuarioId();
            var categorias = await ObtenerCategorias(usuarioId, tipoOperacion);

            //devolvemos response ok, y en el cuerpo la categoria
            return Ok(categorias);
        }

        public async Task<IEnumerable<SelectListItem>> ObtenerCategorias(int usuarioId, TipoOperacion tipoOperacion)
        {
            var categorias = await iCategoriaRepository.ObtenerPorIdYTipoOperacion(usuarioId, tipoOperacion);
            return categorias.Select(x => new SelectListItem(x.nombre, x.id.ToString()));
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id, string urlRegreso)
        {
            var usuarioId = iUsuarioRepository.ObtenerUsuarioId();
            var transaccion = await iTransaccionRepository.BuscarPorId(id, usuarioId);

            if (transaccion is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            else
            {
                var modelo = iMapper.Map<TransaccionActualizacionViewModel>(transaccion);

                if (modelo.tipoOperacionId == TipoOperacion.Egreso)
                {
                    modelo.montoAnterior = modelo.monto * -1;
                }
                else
                {
                    modelo.montoAnterior = modelo.monto;
                }

                modelo.cuentaAnteriorId = transaccion.cuentaId;
                modelo.Categorias = await ObtenerCategorias(usuarioId, transaccion.tipoOperacionId);
                modelo.Cuentas = await ObtenerCuentas(usuarioId);
                modelo.urlRegreso = urlRegreso;

                return View(modelo);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Editar(TransaccionActualizacionViewModel modelo)
        {
            var usuarioId = iUsuarioRepository.ObtenerUsuarioId();

            if (!ModelState.IsValid)
            {
                modelo.Cuentas = await ObtenerCuentas(usuarioId);
                modelo.Categorias = await ObtenerCategorias(usuarioId, modelo.tipoOperacionId);
                return View(modelo);
            }
            else
            {
                var cuenta = await iCuentaRepository.obtenerPorId(modelo.cuentaId, usuarioId);

                if (cuenta is null)
                {
                    return RedirectToAction("NoEncontrado", "Home");
                }

                var categoria = await iCategoriaRepository.ObtenerPorId(modelo.categoriaId, usuarioId);

                if (categoria is null)
                {
                    return RedirectToAction("NoEncontrado", "Home");
                }

                var transaccion = iMapper.Map<Transaccion>(modelo);

                if (modelo.tipoOperacionId == TipoOperacion.Egreso)
                {
                    modelo.montoAnterior = modelo.monto * -1;
                }
                else//si es un ingreso
                {
                    modelo.montoAnterior = modelo.monto;
                }

                await iTransaccionRepository.Actualizar(transaccion, modelo.montoAnterior, modelo.cuentaAnteriorId);

                if (string.IsNullOrEmpty(modelo.urlRegreso))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    //nos permite redireccionar a una url que se encuentra dentro de nuestro dominio:
                    return LocalRedirect(modelo.urlRegreso);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Borrar(int id, string urlRegreso = null)
        {
            var usuarioId = iUsuarioRepository.ObtenerUsuarioId();
            var transaccion = await iTransaccionRepository.BuscarPorId(id, usuarioId);

            if (transaccion is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            else
            {
                await iTransaccionRepository.Borrar(id);
            }

            if (string.IsNullOrEmpty(urlRegreso))
            {
                return RedirectToAction("Index");
            }
            else
            {
                //nos permite redireccionar a una url que se encuentra dentro de nuestro dominio:
                return LocalRedirect(urlRegreso);
            }
        }
    }
}