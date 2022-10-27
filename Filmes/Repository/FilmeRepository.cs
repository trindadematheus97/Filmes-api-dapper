using Dapper;
using Filmes.Models;
using System.Data.SqlClient;

namespace Filmes.Repository
{
    public class FilmeRepository : IFilmeRepository
    {
        private readonly IConfiguration _configuration; 
        private readonly string _connectionString;
        public FilmeRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SqlConnection");
        }
        public async Task<IEnumerable<FilmeResponse>> BuscaFilmesAsync()
        {
            string sql = @"SELECT
                         f.id Id,
                         f.nome Nome,
                         f.ano Ano,
                         p.nome ProdutoraNome,
                         p.id ProdutoraId
                         FROM filme f
                         JOIN produtora p ON id_produtora = p.id";
            using (var con = new SqlConnection(_connectionString))
            {
                return await con.QueryAsync<FilmeResponse>(sql);
            }
        }
        public async Task<FilmeResponse> BuscaFilmeAsync(int id)
        {
            string sql = @"SELECT
                         f.id Id,
                         f.nome Nome,
                         f.ano Ano,
                         p.nome Produtora 
                         FROM filme f
                         JOIN produtora p ON id_produtora = p.id
                         WHERE f.id = @Id";
            using (var con = new SqlConnection(_connectionString))
            {
                return await con.QueryFirstOrDefaultAsync<FilmeResponse>(sql, new {Id = id});
            }
        }
        public async Task<bool> AdicionaAsync(FilmeRequest request)
        {
            string sql = @"INSERT INTO filme(nome, ano, id_produtora)
                            VALUES (@Nome, @Ano, @ProdutoraId)";

            using var con = new SqlConnection(_connectionString);
            return await con.ExecuteAsync(sql, request) > 0;
        }

        public async Task<bool> AtualizarAsync(FilmeRequest request, int id)
        {
            string sql = @"UPDATE filme SET
                            nome = @Nome,
                            ano = @Ano
                            WHERE id = @Id";

            var parametros = new DynamicParameters();
            parametros.Add("Ano", request.Ano);
            parametros.Add("Nome", request.Nome);
            parametros.Add("Id", id);

            using var con = new SqlConnection(_connectionString);
            return await con.ExecuteAsync(sql, parametros) > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            string sql = @"DELETE FROM filme
                            WHERE id = @Id";
            using var con = new SqlConnection(_connectionString);
            return await con.ExecuteAsync(sql, new { Id = id }) > 0;
        }

       

      
    }
}
