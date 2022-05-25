namespace gestorPresupuestos.Models
{
    public class IndiceCuentaViewModel
    {
        public string tipoCuenta { get; set; }
        public IEnumerable<Cuenta> Cuentas { get; set; }
        public decimal balances => Cuentas.Sum(x => x.balance);
    }
}
