﻿using Dapper;
using gestorPresupuestos.Models;
using Microsoft.Data.SqlClient;

namespace gestorPresupuestos.Servicios
{
    public interface ITipoCuentaRepository
    {
        Task Insertar(TipoCuenta tipoCuenta);
        Task<bool> ExisteNombreYUsuarioId(string nombre, int? usuarioId);
        Task<IEnumerable<TipoCuenta>> ObtenerPorUsuarioId(int usuarioId);
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
                "WHERE usuario_id = @usuarioId", new { usuarioId });
        }
    }
}
