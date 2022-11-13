﻿using AutoMapper;
using gestorPresupuestos.Models;
using gestorPresupuestos.Models.Submenus;
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
        private readonly IServicioReporte iServicioReporte;
        private readonly IMapper iMapper;

        public TransaccionController(
            ITransaccionRepository iTransaccionRepository,
            IUsuarioRepository iUsuarioRepository,
            ICuentaRepository iCuentaRepository,
            ICategoriaRepository iCategoriaRepository,
            IServicioReporte iServicioReporte,
            IMapper iMapper)
        {
            this.iTransaccionRepository = iTransaccionRepository;
            this.iUsuarioRepository = iUsuarioRepository;
            this.iCuentaRepository = iCuentaRepository;
            this.iCategoriaRepository = iCategoriaRepository;
            this.iServicioReporte = iServicioReporte;
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
            var modelo = await iServicioReporte.getReporteTransaccionesDetalladas(usuarioId, mes, anio, ViewBag);

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

        public async Task<IActionResult> Semanal(int mes, int anio)
        {
            var usuarioId = iUsuarioRepository.ObtenerUsuarioId();
            IEnumerable<ResultadoPorSemana> transaccionesPorSemana = await 
                iServicioReporte.ObtenerReporteSemanal(usuarioId, mes, anio, ViewBag);

            var grupo = transaccionesPorSemana.GroupBy(x => x.semana).Select(x =>
                new ResultadoPorSemana()
                {
                    semana = x.Key,
                    ingresos = x.Where(x => x.tipoOperacionId == TipoOperacion.Ingreso).Select(x => x.monto).FirstOrDefault(),
                    egresos = x.Where(x => x.tipoOperacionId == TipoOperacion.Egreso).Select(x => x.monto).FirstOrDefault()

                }).ToList();


            if (anio == 0 || mes == 0)
            {
                var hoy = DateTime.Today;
                anio = hoy.Year;
                mes = hoy.Month;
            }

            var fechaReferencia = new DateTime(anio, mes, 1);
            var diasDelMes = Enumerable.Range(1, fechaReferencia.AddMonths(1).AddDays(-1).Day);
            var diasSegmentos = diasDelMes.Chunk(7).ToList();

            for (int i = 0; i < diasSegmentos.Count(); i++)
            {
                var segmento = i + 1;
                var fechaInicio = new DateTime(anio, mes, diasSegmentos[i].First());
                var fechaFin = new DateTime(anio, mes, diasSegmentos[i].Last());

                var grupoSemana = grupo.FirstOrDefault(x => x.semana == segmento);

                if (grupoSemana is null)
                {
                    grupo.Add(new ResultadoPorSemana()
                    {
                        semana = segmento,
                        fechaInicio = fechaInicio,
                        fechaFin = fechaFin
                    });
                }
                else
                {
                    grupoSemana.fechaInicio = fechaInicio;
                    grupoSemana.fechaFin = fechaFin;
                }
            }

            grupo = grupo.OrderByDescending(x => x.semana).ToList();
            var modelo = new ReporteSemanalViewModel();
            modelo.transaccionesPorSemana = grupo;
            modelo.fechaReferencia = fechaReferencia;

            return View(modelo);
        }

        
        //TODO Ver si se implementa ese metodo o se descarta
        private void segmentarDias(int anio, int mes, int diasSegmentar, List<ResultadoPorSemana> grupo)
        {
            if (anio == 0 || mes == 0)
            {
                var hoy = DateTime.Today;
                anio = hoy.Year;
                mes = hoy.Month;
            }

            var fechaReferencia = new DateTime(anio, mes, 1);
            var diasDelMes = Enumerable.Range(1, fechaReferencia.AddMonths(1).AddDays(-1).Day);
            var diasSegmentos = diasDelMes.Chunk(diasSegmentar).ToList();


            for (int i = 0; i < diasSegmentos.Count(); i++)
            {
                var segmento = i + 1;
                var fechaInicio = new DateTime(anio, mes, diasSegmentos[i].First());
                var fechaFin = new DateTime(anio, mes, diasSegmentos[i].Last());

                //TODO ver si se puede extraer
                var grupoSemana = grupo.FirstOrDefault(x => x.semana == segmento);

                if (grupoSemana is null)
                {
                    grupo.Add(new ResultadoPorSemana()
                    {
                        semana = segmento,
                        fechaInicio = fechaInicio,
                        fechaFin = fechaFin
                    });
                }
                else
                {
                    grupoSemana.fechaInicio = fechaInicio;
                    grupoSemana.fechaFin = fechaFin;
                }
            }
        }

        public async Task<IActionResult> Mensual(int anio)
        {
            var usuarioId = iUsuarioRepository.ObtenerUsuarioId();

            if (anio == 0)
            {
                anio = DateTime.Today.Year;
            }

            var transaccionesPorMes = await iTransaccionRepository.ObtenerPorMes(usuarioId,anio);
            var transaccionesAgrupadas = transaccionesPorMes.GroupBy(x => x.mes).
                Select(x => new ResultadoPorMes()
                {
                    mes = x.Key,
                    ingresos = x.Where(x => x.tipoOperacionId == TipoOperacion.Ingreso).
                    Select(x => x.monto).FirstOrDefault(),
                    egresos = x.Where(x => x.tipoOperacionId == TipoOperacion.Egreso).
                    Select(x => x.monto).FirstOrDefault()
                }).ToList();

            for (int mes=1; mes <= 12; mes++)
            {
                var transaccion = transaccionesAgrupadas.FirstOrDefault(x => x.mes == mes);
                var fechaReferencia = new DateTime(anio, mes, 1);

                if (transaccion is null)
                {
                    transaccionesAgrupadas.Add(new ResultadoPorMes()
                    {
                        mes = mes,
                        fechaReferencia = fechaReferencia
                    });
                }
                else
                {
                    transaccion.fechaReferencia = fechaReferencia;
                }
            }

            transaccionesAgrupadas = transaccionesAgrupadas.OrderByDescending(x => x.mes).ToList();
            
            var reporteMensualViewModel = new ReporteMensualViewModel();
            reporteMensualViewModel.anio = anio;
            reporteMensualViewModel.transaccionesPorMes = transaccionesAgrupadas;

            return View(reporteMensualViewModel);
        }

        public IActionResult Excel()
        {
            return View();
        }

        public IActionResult Calendario()
        {
            return View();
        }
    }
}