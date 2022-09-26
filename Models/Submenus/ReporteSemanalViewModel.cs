using System;

namespace gestorPresupuestos.Models.Submenus
{
    public class ReporteSemanalViewModel
    {
        public decimal ingresos => transaccionesPorSemana.Sum(x => x.ingresos);
        public decimal egresos => transaccionesPorSemana.Sum(x => x.egresos) ;
        public decimal total => ingresos - egresos;
        public DateTime fechaReferencia { get; set; }
        public IEnumerable<ResultadoPorSemana> transaccionesPorSemana { get; set; }
    }
}
