using System.ComponentModel.DataAnnotations;

namespace gestorPresupuestos.Models
{
    public class Transaccion
    {
        public int id { get; set; }
        public int usuarioId { get; set; }
        public DateTime fechaTransaccion { get; set; } = DateTime.Today;
        public decimal monto { get; set; }
        [Range(0, maximum: int.MaxValue, ErrorMessage = "La categoría es obligatoria")]
        public int categoriaId { get; set; }
        public string nota { get; set; }
        [Range(0, maximum: int.MaxValue, ErrorMessage = "La cuenta es obligatoria")]
        public int cuentaId { get; set; }
    }
}
