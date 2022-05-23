using System.ComponentModel.DataAnnotations;

namespace gestorPresupuestos.Models
{
    public class Cuenta
    {
        public int id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 50)]
        public string nombre { get; set; }
        [Display(Name = "Tipo de Cuenta")]
        public int tipoCuentaId { get; set; }
        public decimal balance { get; set; }
        [StringLength(maximumLength: 1000)]
        public string descripcion { get; set; }

    }
}
