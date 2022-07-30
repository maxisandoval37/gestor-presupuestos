namespace gestorPresupuestos.Models.Reportes
{
    public class ParametroGetTransaccionesPorCuenta
    {
        public int usuarioId { get; set; }
        public int cuentaId { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
    }
}
