namespace gestorPresupuestos.Models.Submenus
{
    public class ResultadoPorSemana
    {
        public int semana { get; set; }
        public decimal monto { get; set; }
        public TipoOperacion tipoOperacionId { get; set; }
        public decimal ingresos { get; set; }
        public decimal egresos { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
    }
}
