namespace gestorPresupuestos.Utils
{
    public class StringUtils
    {
        public string capitalizarStr(string str)
        {
            return str[0].ToString().ToUpper() + str.Substring(1);
        }
    }
}