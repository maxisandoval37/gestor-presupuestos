using System.ComponentModel.DataAnnotations;

namespace gestorPresupuestos.Models
{
    public class Transaccion
    {
        public int id { get; set; }
        public int usuarioId { get; set; }
        public DateTime fechaTransaccion { get; set; } = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy hh:mm tt"));
        public decimal monto { get; set; }
        
        [Range(0, maximum: int.MaxValue, ErrorMessage = "La categoría es obligatoria")]
        [Display(Name = "Categoría")]
        public int categoriaId { get; set; }
        public string nota { get; set; }
        [Range(0, maximum: int.MaxValue, ErrorMessage = "La cuenta es obligatoria")]
        
        [Display(Name = "Cuenta")]
        public int cuentaId { get; set; }

        [Display(Name = "Tipo de Operación")]
        public TipoOperacion tipoOperacionId { get; set; } = TipoOperacion.Ingreso;

        public string cuenta { get; set; }
        public string categoria { get; set; }
    }
}
