using gestorPresupuestos.Models;
using gestorPresupuestos.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace gestorPresupuestos.Controllers
{
    public class TransaccionController: Controller
    {
        private readonly IUsuarioRepository iUsuarioRepository;
        private readonly ICuentaRepository iCuentaRepository;
        public TransaccionController(IUsuarioRepository iUsuarioRepository, ICuentaRepository iCuentaRepository)
        {
            this.iUsuarioRepository = iUsuarioRepository;
            this.iCuentaRepository = iCuentaRepository;
        }

        public async Task<IActionResult> Insertar()
        {
            var usuarioId = iUsuarioRepository.ObtenerUsuarioId();
            var modelo = new TransaccionCreacionViewModel();
            modelo.Cuentas = await ObtenerCuentas(usuarioId);

            return View(modelo);
        }

        private async Task<IEnumerable<SelectListItem>> ObtenerCuentas(int usuarioId)
        {
            var cuentas = await iCuentaRepository.BuscarPorUsuarioId(usuarioId);
            return cuentas.Select(x => new SelectListItem(x.nombre, x.id.ToString()));
        }
    }
}
