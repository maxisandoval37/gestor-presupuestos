namespace gestorPresupuestos.Models.Reportes
{
    public class ReporteTransaccionesDetalladas
    {
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public IEnumerable<TransaccionesPorFecha> transaccionesAgrupadas { get; set; }
        public decimal balanceIngresos => transaccionesAgrupadas.Sum(x => x.balanceIngresos);
        public decimal balanceEgresos => transaccionesAgrupadas.Sum(x => x.balanceEgresos);
        public decimal total => balanceIngresos - balanceEgresos;

        public class TransaccionesPorFecha
        {
            public DateTime fechaTransaccion { get; set; }
            public IEnumerable<Transaccion> Transacciones { get; set; }
            public decimal balanceIngresos =>
                Transacciones.Where(x => x.tipoOperacionId == TipoOperacion.Ingreso).Sum(x => x.monto);
            public decimal balanceEgresos =>
                Transacciones.Where(x => x.tipoOperacionId == TipoOperacion.Egreso).Sum(x => x.monto);
        }
    }
}