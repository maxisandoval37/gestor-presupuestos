namespace gestorPresupuestos.Models
{
    public class TransaccionActualizacionViewModel: TransaccionCreacionViewModel
    {
        public int cuentaAnteriorId { get; set; }
        public decimal montoAnterior { get; set; }

        public string urlRegreso { get; set; }
    }
}