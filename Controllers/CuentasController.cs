using gestorPresupuestos.Models;
using gestorPresupuestos.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace gestorPresupuestos.Controllers
{
    public class CuentasController : Controller
    {
        private readonly ITipoCuentaRepository iTipoCuentaRepository;
        private readonly IUsuarioRepository iUsuarioRepository;

        public CuentasController(ITipoCuentaRepository iTipoCuentaRepository, IUsuarioRepository iUsuarioRepository)
        {
            this.iTipoCuentaRepository = iTipoCuentaRepository;
            this.iUsuarioRepository = iUsuarioRepository;
        }

        public ITipoCuentaRepository ITipoCuentaRepository { get; }

        [HttpGet]
        public async Task<IActionResult> Insertar()
        {
            var usuarioId = iUsuarioRepository.ObtenerUsuarioId();
            var tiposCuentas = await iTipoCuentaRepository.ObtenerPorUsuarioId(usuarioId);
            var modelo = new CuentaCreacionViewModel();
            modelo.tiposCuentas = tiposCuentas.Select(x => new SelectListItem(x.nombre,x.id.ToString()));

            return View(modelo);
        }
    }
}
