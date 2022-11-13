namespace gestorPresupuestos.Models.Submenus
{
    public class ResultadoPorMes
    {
        public int mes { get; set; }
        public DateTime fechaReferencia { get; set; }
        public decimal monto { get; set; }
        public decimal ingresos { get; set; }
        public decimal egresos { get; set; }
        public TipoOperacion tipoOperacionId { get; set; }
    }
}