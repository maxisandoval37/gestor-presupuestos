using Dapper;
using gestorPresupuestos.Models;
using Microsoft.Data.SqlClient;

namespace gestorPresupuestos.Servicios
{
    public interface ITransaccionRepository
    {
        Task Insertar(Transaccion transaccion);

    }
    public class TransaccionRepository : ITransaccionRepository
    {
        private readonly string connectionString;
        public TransaccionRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Insertar(Transaccion transaccion)
        {
            using var connection = new SqlConnection(connectionString);

            var id = await connection.QuerySingleAsync<int>("TRANSACCION_INSERTAR",
                new
                {
                    usuario_id = transaccion.usuarioId,
                    fecha_transaccion = transaccion.fechaTransaccion,
                    monto = transaccion.monto,
                    categoria_id = transaccion.categoriaId,
                    cuenta_id = transaccion.cuentaId,
                    nota = transaccion.nota
                },
                commandType: System.Data.CommandType.StoredProcedure);

            transaccion.id = id;
            //TODO Crear SP en la DB
        }
    }
}