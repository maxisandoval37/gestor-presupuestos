using Dapper;
using gestorPresupuestos.Models;
using Microsoft.Data.SqlClient;

namespace gestorPresupuestos.Servicios
{
    public interface ICategoriaRepository
    {
        Task Insertar(Categoria categoria);
        Task<IEnumerable<Categoria>> BuscarPorUsuarioId(int usuarioId);
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
            return await connection.QueryAsync<Categoria>(
                $@"SELECT * FROM categorias " +
                "WHERE usuario_id = @usuarioId ", new { usuarioId });
        }
    }
}
