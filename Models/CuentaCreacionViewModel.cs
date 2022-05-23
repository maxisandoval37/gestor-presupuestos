using Microsoft.AspNetCore.Mvc.Rendering;

namespace gestorPresupuestos.Models
{
    public class CuentaCreacionViewModel: Cuenta
    {
        public IEnumerable<SelectListItem> tiposCuentas { get; set; }
    }
}
