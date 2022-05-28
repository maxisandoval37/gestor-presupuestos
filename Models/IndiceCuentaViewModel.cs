namespace gestorPresupuestos.Models
{
    public class IndiceCuentaViewModel
    {
        public string tipoCuenta { get; set; }
        public IEnumerable<Cuenta> cuentas { get; set; }
        public decimal balances => cuentas.Sum(x => x.balance);
    }
}
