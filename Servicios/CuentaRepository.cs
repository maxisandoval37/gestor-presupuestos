using Dapper;
using gestorPresupuestos.Models;
using Microsoft.Data.SqlClient;

namespace gestorPresupuestos.Servicios
{
    public interface ICuentaRepository
    {
        Task<IEnumerable<Cuenta>> BuscarPorUsuarioId(int usuarioId);
        Task Insertar(Cuenta cuenta);
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
    }
}