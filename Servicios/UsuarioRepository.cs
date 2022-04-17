namespace gestorPresupuestos.Servicios
{
    public interface IUsuarioRepository
    {
        int ObtenerUsuarioId();
    }
    public class UsuarioRepository: IUsuarioRepository
    {
        public int Id { get; set; }

        public int ObtenerUsuarioId()
        {
            return 6;
        }
    }
}
