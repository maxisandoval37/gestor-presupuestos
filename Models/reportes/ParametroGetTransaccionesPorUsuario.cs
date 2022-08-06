namespace gestorPresupuestos.Models.Reportes
{
    public class ParametroGetTransaccionesPorUsuario
    {
        public int usuarioId { get; set; }
        public DateTime fechaInicio { get; set; }

        public DateTime fechaFin { get; set; }
    }
}
