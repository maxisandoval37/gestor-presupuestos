using Dapper;
using gestorPresupuestos.Models;
using Microsoft.Data.SqlClient;

namespace gestorPresupuestos.Servicios
{
    public interface ICuentaRepository
    {
        Task<IEnumerable<Cuenta>> BuscarPorUsuarioId(int usuarioId);
        Task<Cuenta> obtenerPorId(int id, int usuarioId);
        Task Insertar(Cuenta cuenta);
        Task Editar(CuentaCreacionViewModel cuenta);
        Task Borrar(int id);
    }
    public class CuentaRepository : ICuentaRepository
    {
        private readonly string connectionString;
        public CuentaRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Insertar(Cuenta cuenta)
        {
            using var connection = new SqlConnection(connectionString);

            var id = await connection.QuerySingleAsync<int>($@"INSERT INTO cuentas" +
            "(nombre,tipo_cuenta_id, balance, descripcion)" +
            "VALUES(@nombre,@tipoCuentaId, @balance, @descripcion);SELECT SCOPE_IDENTITY();", cuenta);
            cuenta.id = id;
        }

        public async Task<IEnumerable<Cuenta>> BuscarPorUsuarioId(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Cuenta>(
                $@"SELECT cuentas.id,cuentas.nombre,balance,tc.nombre " +
                "AS tipocuenta " +
                "FROM cuentas " +
                "INNER JOIN tipo_cuenta tc " +
                "ON tc.Id = cuentas.tipo_cuenta_id " +
                "WHERE tc.usuario_id = @usuarioId " +
                "ORDER BY tc.orden", new { usuarioId });
        }

        public async Task<Cuenta> obtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Cuenta>(
                $@"SELECT cuentas.id,cuentas.nombre,balance, " +
                "descripcion, tc.id " +
                "FROM cuentas " +
                "INNER JOIN tipo_cuenta tc " +
                "ON tc.Id = cuentas.tipo_cuenta_id " +
                "WHERE tc.usuario_id = @usuarioId and " +
                "cuentas.id = @id", new { id, usuarioId });
        }

        public async Task Editar(CuentaCreacionViewModel cuenta)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE cuentas SET nombre = @nombre, balance = @balance, "+
            "descripcion = @descripcion, tipo_cuenta_id = @tipoCuentaId " +
            "WHERE id = @id", cuenta);
        }

        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"DELETE cuentas WHERE id = @id", new {id});
        }
    }
}