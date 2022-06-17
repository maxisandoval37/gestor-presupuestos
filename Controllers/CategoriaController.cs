using gestorPresupuestos.Models;
using gestorPresupuestos.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace gestorPresupuestos.Controllers
{
    public class CategoriaController: Controller
    {
        private readonly ICategoriaRepository iCategoriaRepository;
        private readonly IUsuarioRepository iUsuarioRepository;
        public CategoriaController(ICategoriaRepository iCategoriaRepository, IUsuarioRepository iUsuarioRepository)
        {
            this.iCategoriaRepository = iCategoriaRepository;
            this.iUsuarioRepository = iUsuarioRepository;
        }

        [HttpGet]
        public IActionResult Insertar()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Insertar(Categoria categoria)
        {
            var usuarioId = iUsuarioRepository.ObtenerUsuarioId();

            if (!ModelState.IsValid)
            {
                return View(categoria);
            }

            categoria.usuarioId = usuarioId;
            await iCategoriaRepository.Insertar(categoria);
            return RedirectToAction("Index");
        }
    }
}
