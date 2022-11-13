using Dapper;
using gestorPresupuestos.Models;
using gestorPresupuestos.Models.Reportes;
using gestorPresupuestos.Models.Submenus;
using Microsoft.Data.SqlClient;

namespace gestorPresupuestos.Servicios
{
    public interface ITransaccionRepository
    {
        Task Insertar(Transaccion transaccion);
        Task<IEnumerable<Transaccion>> BuscarPorUsuarioId(int usuarioId);
        Task<Transaccion> BuscarPorId(int id, int usuarioId);
        Task Actualizar(Transaccion transaccion, decimal montoAnterior, int cuentaAnterior);
        Task Borrar(int id);
        Task<IEnumerable<Transaccion>> ObtenerPorCuentaId(ParametroGetTransaccionesPorCuenta modelo);
        Task<IEnumerable<Transaccion>> ObtenerPorUsuarioId(ParametroGetTransaccionesPorUsuario modelo);
        Task<IEnumerable<ResultadoPorSemana>> ObtenerPorSemana(ParametroGetTransaccionesPorUsuario modelo);
        Task<IEnumerable<ResultadoPorMes>> ObtenerPorMes(int usuarioId, int anio);
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
        }

        public async Task<IEnumerable<Transaccion>> BuscarPorUsuarioId(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            //Aplicamos el 'AS' para hacer match entre el nombre del atributo de la tabla y el modelo en c#
            return await connection.QueryAsync<Transaccion>(
                $@"SELECT id, fecha_transaccion AS fechaTransaccion, monto, nota, cuenta_id AS categoriaId, categoria_id AS categoriaId " +
                "FROM transacciones " +
                "WHERE usuario_id = @usuarioId ", new { usuarioId });
        }

        public async Task<Transaccion> BuscarPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            //Aplicamos el 'AS' para hacer match entre el nombre del atributo de la tabla y el modelo en c#
            return await connection.QueryFirstOrDefaultAsync<Transaccion>(
                $@"SELECT transacciones.id, fecha_transaccion AS fechaTransaccion, monto, nota, cuenta_id AS cuentaId, categoria_id AS categoriaId ," +
                "CATE.tipo_operacion_id "+
                "FROM transacciones INNER JOIN categorias cate ON cate.id = transacciones.categoria_id " +
                "WHERE transacciones.id = @id AND transacciones.usuario_id = @usuarioId ", new {id, usuarioId });
        }

        public async Task Actualizar(Transaccion transaccion, decimal monto_anterior, int cuenta_id_anterior)
        {
            using var connection = new SqlConnection(connectionString);

            //hacemos esto para que coincida con los nombres de la db:
            var fecha_transaccion = transaccion.fechaTransaccion;
            var categoria_id = transaccion.categoriaId;
            var cuenta_id = transaccion.cuentaId;

            await connection.ExecuteAsync("TRANSACCION_ACTUALIZAR", new {
                transaccion.id,
                fecha_transaccion,
                transaccion.monto,
                monto_anterior,
                cuenta_id,
                cuenta_id_anterior,
                categoria_id,
                transaccion.nota,
            },commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync("TRANSACCION_BORRAR", 
                new { id }, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Transaccion>> ObtenerPorCuentaId(ParametroGetTransaccionesPorCuenta modelo)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Transaccion>($@"SELECT transacciones.id, fecha_transaccion AS fechaTransaccion, monto, nota, cuenta_id AS cuentaId, categoria_id AS categoriaId,  cate.nombre as categoria, cuentas.nombre as cuenta, cate.tipo_operacion_id  AS tipoOperacionId " +
                "FROM transacciones INNER JOIN categorias cate ON cate.id = transacciones.categoria_id " +
                "INNER JOIN cuentas ON cuentas.id = transacciones.cuenta_id " +
                "WHERE transacciones.cuenta_id = @cuentaId AND transacciones.usuario_id = @usuarioId AND " +
                "fecha_transaccion BETWEEN @fechaInicio AND @fechaFin", modelo);
        }

        public async Task<IEnumerable<Transaccion>> ObtenerPorUsuarioId(ParametroGetTransaccionesPorUsuario modelo)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Transaccion>($@"SELECT transacciones.id, fecha_transaccion AS fechaTransaccion, monto, nota, cuenta_id AS cuentaId, categoria_id AS categoriaId,  cate.nombre as categoria, cuentas.nombre as cuenta, cate.tipo_operacion_id AS tipoOperacionId " +
                "FROM transacciones INNER JOIN categorias cate ON cate.id = transacciones.categoria_id " +
                "INNER JOIN cuentas ON cuentas.id = transacciones.cuenta_id " +
                "WHERE transacciones.usuario_id = @usuarioId AND " +
                "fecha_transaccion BETWEEN @fechaInicio AND @fechaFin "+
                "ORDER BY transacciones.fecha_transaccion DESC", modelo);
        }

        //Inicio queries Submenus:
        public async Task<IEnumerable<ResultadoPorSemana>> ObtenerPorSemana(ParametroGetTransaccionesPorUsuario modelo)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<ResultadoPorSemana>(
                $@"select datediff(d, @fechaInicio, fecha_transaccion) / 7 + 1 as semana, 
                SUM(monto) as monto, cat.tipo_operacion_id  AS tipoOperacionId
                FROM transacciones 
                INNER JOIN categorias cat 
                ON cat.id = transacciones.categoria_id 
                WHERE transacciones.usuario_id = @usuarioId AND 
                fecha_transaccion BETWEEN @fechaInicio and @fechaFin 
                GROUP BY datediff(d, @fechaInicio, fecha_transaccion) / 7, cat.tipo_operacion_id", modelo);
        }

        public async Task<IEnumerable<ResultadoPorMes>> ObtenerPorMes(int usuarioId, int anio)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<ResultadoPorMes>(
                $@"SELECT MONTH(fecha_transaccion) AS mes,
                SUM(monto) AS monto, cat.tipo_operacion_id AS tipoOperacionId
                FROM transacciones
                INNER JOIN categorias cat
                ON cat.Id = transacciones.categoria_id
                WHERE Transacciones.usuario_id = @usuarioId AND YEAR(fecha_transaccion) = @anio
                GROUP BY MONTH(fecha_transaccion), cat.tipo_operacion_id", new { usuarioId,anio});
        }
    }
}