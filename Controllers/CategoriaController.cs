using gestorPresupuestos.Models;
using gestorPresupuestos.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace gestorPresupuestos.Controllers
{
    public class CategoriaController : Controller
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

        public async Task<IActionResult> Index()
        {
            var usuarioId = iUsuarioRepository.ObtenerUsuarioId();
            var categorias = await iCategoriaRepository.BuscarPorUsuarioId(usuarioId);

            return View(categorias);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var usuarioId = iUsuarioRepository.ObtenerUsuarioId();
            var categoria = await iCategoriaRepository.ObtenerPorId(id, usuarioId);

            if (categoria is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            else
            {
                return View(categoria);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Categoria categoriaEditar)
        {
            var usuarioId = iUsuarioRepository.ObtenerUsuarioId();

            if (!ModelState.IsValid)
            {
                return View(categoriaEditar);
            }

            var categoria = await iCategoriaRepository.ObtenerPorId(categoriaEditar.id, usuarioId);

            if (categoria is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            else
            {
                categoriaEditar.usuarioId = usuarioId;
                await iCategoriaRepository.Editar(categoriaEditar);
                return RedirectToAction("Index");
            }
        }
    }
}
