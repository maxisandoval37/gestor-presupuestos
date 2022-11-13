namespace gestorPresupuestos.Models.Submenus
{
    public class ReporteMensualViewModel
    {
        public decimal ingresos => transaccionesPorMes.Sum(x => x.ingresos);
        public decimal egresos => transaccionesPorMes.Sum(x => x.egresos);
        public decimal total => ingresos - egresos;
        public int anio { get; set; }
        public IEnumerable<ResultadoPorMes> transaccionesPorMes { get; set; }
    }
}
