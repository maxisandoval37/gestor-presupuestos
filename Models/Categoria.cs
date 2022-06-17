using System.ComponentModel.DataAnnotations;

namespace gestorPresupuestos.Models
{
    public class Categoria
    {
        public int id { get; set; }

        [Required (ErrorMessage = "El cambio {0} es requerido")]
        [StringLength(maximumLength:50,ErrorMessage = "El campo {1} no puede tener mas de 50 caracteres")]
        public string nombre { get; set; }
        public TipoOperacion tipoOperacion { get; set; }
        public int usuarioId { get; set; }

    }
}
