using Dapper;
using gestorPresupuestos.Models;
using Microsoft.Data.SqlClient;

namespace gestorPresupuestos.Servicios
{
    public interface ITipoCuentaRepository
    {
        Task Insertar(TipoCuenta tipoCuenta);
        Task<bool> ExisteNombreYUsuarioId(string nombre, int? usuarioId);
        Task<IEnumerable<TipoCuenta>> ObtenerPorUsuarioId(int usuarioId);
        Task ActualizarNombre(TipoCuenta tipoCuenta);
        Task<TipoCuenta> ObtenerPorId(int id, int usuarioId);
        Task BorrarTipoCuenta(int id);
        Task OrdenarTipoCuenta(IEnumerable<TipoCuenta> tipoCuentasOrdenados);
    }
    public class TipoCuentaRepository: ITipoCuentaRepository
    {
        private readonly string connectionString;
        public TipoCuentaRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Insertar(TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection(connectionString);

            var id = await connection.QuerySingleAsync<int>("TIPOCUENTA_INSERTAR", 
                new {usuario_id=tipoCuenta.usuarioId,nombre=tipoCuenta.nombre,},
                commandType: System.Data.CommandType.StoredProcedure);

            tipoCuenta.id = id;
        }

        public async Task InsertarOld(TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection(connectionString);
            //QuerySingle: permite hacer un query, que estoy seguro que solo me va a traer un solo resultado
            //SCOPE_IDENTITY permite traer el id, del registro recien creado
            var id = await connection.QuerySingleAsync<int>($@"INSERT INTO tipo_cuenta" +
                "(nombre, usuario_id, orden)" +
                "VALUES(@nombre,@usuarioId, 0);SELECT SCOPE_IDENTITY();", tipoCuenta);
            tipoCuenta.id = id;
        }

        public async Task<bool> ExisteNombreYUsuarioId(string nombre, int? usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            var existe = await connection.QueryFirstOrDefaultAsync<int>(@$"SELECT 1 FROM tipo_cuenta "+
                "WHERE nombre = @nombre AND usuario_id = @usuarioId;", new {nombre, usuarioId});
            return existe == 1;
        }

        /// <summary>
        /// Method <c>ObtenerPorUsuarioId</c> retorna lista con los tipos de cuentas que tiene un usuario
        /// </summary>
        public async Task<IEnumerable<TipoCuenta>> ObtenerPorUsuarioId(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<TipoCuenta>("SELECT id,nombre,orden FROM tipo_cuenta "+
                "WHERE usuario_id = @usuarioId ORDER BY orden", new { usuarioId });
        }

        public async Task ActualizarNombre(TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection(connectionString);

            //Execute: permite hacer un query que no va a retornar nada
            await connection.ExecuteAsync(@"UPDATE tipo_cuenta SET nombre = @nombre WHERE id = @id",tipoCuenta);
        }

        public async Task<TipoCuenta> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<TipoCuenta>(@"SELECT id, nombre, orden "+
            "FROM tipo_cuenta WHERE id = @id AND usuario_id = @usuarioId", new {id, usuarioId});
        }

        public async Task BorrarTipoCuenta(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE tipo_cuenta WHERE id = @id",new { id });
        }

        public async Task OrdenarTipoCuenta(IEnumerable<TipoCuenta> tipoCuentasOrdenados)
        {
            using var connection = new SqlConnection(connectionString);
            //Con el IEnumerable ejecuta la query varias veces
            await connection.ExecuteAsync("UPDATE tipo_cuenta SET orden = @orden WHERE id = @id", tipoCuentasOrdenados);
        }
    }
}