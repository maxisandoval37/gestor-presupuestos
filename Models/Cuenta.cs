using System.ComponentModel.DataAnnotations;

namespace gestorPresupuestos.Models
{
    public class Cuenta
    {
        public int id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 50)]
        [Display(Name = "Nombre")]
        public string nombre { get; set; }
        
        [Display(Name = "Tipo de Cuenta")]
        public int tipoCuentaId { get; set; }
        
        [Display(Name = "Balance")]
        public decimal balance { get; set; }
        
        [StringLength(maximumLength: 1000)]
        [Display(Name = "Descripción")]
        public string descripcion { get; set; }

    }
}
