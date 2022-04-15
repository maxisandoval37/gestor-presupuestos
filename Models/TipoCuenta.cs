using System.ComponentModel.DataAnnotations;

namespace gestorPresupuestos.Models
{
    public class TipoCuenta
    {
        public int id { get; set; }

        [Required(ErrorMessage = "El campo nombre es obligatorio")]
        [StringLength(maximumLength:50,MinimumLength = 3, 
        ErrorMessage = "El campo {0} debe estar entre {2} y {1}")]
        [Display(Name = "Nombre del tipo de cuenta: ")]
        public string nombre { get; set; }
        public int usuarioId { get; set; }
        public int orden { get; set; }

    }
}
