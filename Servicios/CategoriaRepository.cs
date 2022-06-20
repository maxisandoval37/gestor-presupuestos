using Dapper;
using gestorPresupuestos.Models;
using Microsoft.Data.SqlClient;

namespace gestorPresupuestos.Servicios
{
    public interface ICategoriaRepository
    {
        Task Insertar(Categoria categoria);
        Task<IEnumerable<Categoria>> BuscarPorUsuarioId(int usuarioId);
        Task<Categoria> ObtenerPorId(int id, int usuarioId);
        Task Editar(Categoria categoria);
        Task Borrar(int id);
    }
    public class CategoriaRepository: ICategoriaRepository
    {
        private readonly string connectionString;
        public CategoriaRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Insertar(Categoria categoria)
        {
            using var connection = new SqlConnection(connectionString);

            var id = await connection.QuerySingleAsync<int>($@"INSERT INTO categorias" +
            "(nombre, tipo_operacion_id, usuario_id)" +
            "VALUES(@nombre, @tipoOperacionId, @usuarioId);SELECT SCOPE_IDENTITY();", categoria);
            categoria.id = id;
        }

        public async Task<IEnumerable<Categoria>> BuscarPorUsuarioId(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            //Aplicamos el 'AS' para hacer match entre el nombre del atributo de la tabla y el modelo en c#
            return await connection.QueryAsync<Categoria>(
                $@"SELECT id, nombre, tipo_operacion_id AS tipoOperacionId, usuario_id AS usuarioId " +
                "FROM categorias " +
                "WHERE usuario_id = @usuarioId ", new { usuarioId });
        }

        public async Task<Categoria> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Categoria>(
                $@"SELECT id, nombre, tipo_operacion_id AS tipoOperacionId, usuario_id AS usuarioId " +
                "FROM categorias " +
                "WHERE id = @id AND usuario_id = @usuarioId ", new { id, usuarioId });
        }

        public async Task Editar(Categoria categoria)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE categorias SET "+
            "nombre = @nombre, tipo_operacion_id = @tipoOperacionId " +
            "WHERE id = @id", categoria);
        }

        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"DELETE categorias WHERE id = @id", new { id });
        }
    }
}