using gestorPresupuestos.Models;
using gestorPresupuestos.Models.Reportes;

namespace gestorPresupuestos.Servicios
{
    public interface IServicioReporte
    {
        Task<ReporteTransaccionesDetalladas> getReporteTransaccionesDetalladasPorCuenta(int usuarioId, int cuentaId, int mes, int anio, dynamic ViewBag);
        Task<ReporteTransaccionesDetalladas> getReporteTransaccionesDetalladas(int usuarioId, int mes, int anio, dynamic ViewBag);
    }
    public class ServicioReporte: IServicioReporte
    {
        private readonly ITransaccionRepository iTransaccionRepository;
        private readonly HttpContext httpContext;
        public ServicioReporte(ITransaccionRepository iTransaccionRepository, IHttpContextAccessor iHttpContextAccessor)
        {
            this.iTransaccionRepository = iTransaccionRepository;
            this.httpContext = iHttpContextAccessor.HttpContext;
        }
        public async Task<ReporteTransaccionesDetalladas> getReporteTransaccionesDetalladasPorCuenta(
            int usuarioId, int cuentaId, int mes, int anio, dynamic ViewBag)
        {
            (DateTime fechaInicio, DateTime fechaFin) = generarFechaInicioYFin(mes, anio);

            var getTransaccionesPorCuenta = new ParametroGetTransaccionesPorCuenta()
            {
                cuentaId = cuentaId,
                usuarioId = usuarioId,
                fechaInicio = fechaInicio,
                fechaFin = fechaFin
            };

            var transacciones = await iTransaccionRepository.ObtenerPorCuentaId(getTransaccionesPorCuenta);
            var modelo = generarReporteTransaccionesDetalladas(fechaInicio, fechaFin, transacciones);
            
            asignarValoresViewBag(ViewBag, fechaInicio);
            return modelo;
        }

        private (DateTime fechaInicio, DateTime fechaFin) generarFechaInicioYFin(int mes, int anio)
        {
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

            return (fechaInicio, fechaFin);
        }

        public async Task<ReporteTransaccionesDetalladas> getReporteTransaccionesDetalladas(
            int usuarioId, int mes, int anio, dynamic ViewBag)
        {
            (DateTime fechaInicio, DateTime fechaFin) = generarFechaInicioYFin(mes, anio);

            var parametro = new ParametroGetTransaccionesPorUsuario()
            {
                usuarioId = usuarioId,
                fechaInicio = fechaInicio,
                fechaFin = fechaFin
            };

            var transacciones = await iTransaccionRepository.ObtenerPorUsuarioId(parametro);
            var modelo = generarReporteTransaccionesDetalladas(fechaInicio, fechaFin, transacciones);
            asignarValoresViewBag(ViewBag, fechaInicio);

            return modelo;
        }

        private ReporteTransaccionesDetalladas generarReporteTransaccionesDetalladas(DateTime fechaInicio, DateTime fechaFin, IEnumerable<Transaccion> transacciones)
        {
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

            return modelo;
        }

        private void asignarValoresViewBag(dynamic ViewBag, DateTime fechaInicio)
        {
            ViewBag.mesAnterior = fechaInicio.AddMonths(-1).Month;
            ViewBag.anioAnterior = fechaInicio.AddMonths(-1).Year;

            ViewBag.mesPosterior = fechaInicio.AddMonths(1).Month;
            ViewBag.anioPosterior = fechaInicio.AddMonths(1).Year;

            ViewBag.urlRegreso = httpContext.Request.Path + httpContext.Request.QueryString;
        }
    }
}