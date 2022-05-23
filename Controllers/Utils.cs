namespace gestorPresupuestos.Controllers
{
    public class Utils
    {
        public string capitalizarStr(string str)
        {
            return str[0].ToString().ToUpper() + str.Substring(1);
        }
    }
}
