using gestorPresupuestos.Models;
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
        public TransaccionController(
            ITransaccionRepository iTransaccionRepository,
            IUsuarioRepository iUsuarioRepository, 
            ICuentaRepository iCuentaRepository,
            ICategoriaRepository iCategoriaRepository)
        {
            this.iTransaccionRepository = iTransaccionRepository;
            this.iUsuarioRepository = iUsuarioRepository;
            this.iCuentaRepository = iCuentaRepository;
            this.iCategoriaRepository = iCategoriaRepository;
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

        public async Task<IActionResult> Index()
        {
            var usuarioId = iUsuarioRepository.ObtenerUsuarioId();
            var transacciones = await iTransaccionRepository.BuscarPorUsuarioId(usuarioId);

            return View(transacciones);
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
    }
}
