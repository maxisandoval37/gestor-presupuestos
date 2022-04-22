﻿using gestorPresupuestos.Models;
using gestorPresupuestos.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace gestorPresupuestos.Controllers
{
    public class TipoCuentaController: Controller
    {
        private ITipoCuentaRepository iTiposCuentasRepository;
        private IUsuarioRepository iUsuarioRepository;
        public TipoCuentaController(ITipoCuentaRepository tiposCuentasRepository, IUsuarioRepository usuarioRepository)
        {
            this.iTiposCuentasRepository = tiposCuentasRepository;
            this.iUsuarioRepository = usuarioRepository;
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
                tipoCuenta.usuarioId = iUsuarioRepository.ObtenerUsuarioId();

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
            var usuarioId = iUsuarioRepository.ObtenerUsuarioId();
            var existeTipoCuenta = await iTiposCuentasRepository.ExisteNombreYUsuarioId(nombre, usuarioId);

            if (existeTipoCuenta)
            {
                return Json($"El nombre {nombre} ya se encuentra registrado");
            }

            return Json(true);
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = iUsuarioRepository.ObtenerUsuarioId();
            var tiposCuentas = await iTiposCuentasRepository.ObtenerPorUsuarioId(6);
            return View(tiposCuentas);
        }

        [HttpGet]
        public async Task<ActionResult> Editar(int id)
        {
            var usuarioId = iUsuarioRepository.ObtenerUsuarioId();
            var tipoCuenta = await iTiposCuentasRepository.ObtenerPorId(id, usuarioId);

            if (tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            else
            {
                return View(tipoCuenta);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Editar(TipoCuenta tipoCuenta)
        {
            var usuarioId = iUsuarioRepository.ObtenerUsuarioId();
            var existeTipoCuenta = await iTiposCuentasRepository.ObtenerPorId(tipoCuenta.id,usuarioId);
            
            if (existeTipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            else
            {
                await iTiposCuentasRepository.ActualizarNombre(tipoCuenta);
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<ActionResult> Borrar(int id)
        {
            var usuarioId = iUsuarioRepository.ObtenerUsuarioId();
            var tipoCuenta = await iTiposCuentasRepository.ObtenerPorId(id, usuarioId);

            if (tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            else
            {
                return View(tipoCuenta);
            }
        }

        [HttpPost]
        public async Task<ActionResult> BorrarTipoCuenta(int id)
        {
            var usuarioId = iUsuarioRepository.ObtenerUsuarioId();
            var tipoCuenta = await iTiposCuentasRepository.ObtenerPorId(id, usuarioId);

            if (tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            else
            {
                await iTiposCuentasRepository.BorrarTipoCuenta(id);
                return RedirectToAction("Index");
            }
        }
    }
}
