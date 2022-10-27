using Filmes.Models;
using Filmes.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace Filmes.Controllers
{
    [ApiController]
    [Route("api/controller")]
    public class FilmesController : ControllerBase
    {
        private readonly IFilmeRepository _repository;

        public FilmesController(IFilmeRepository repository)
        {
            _repository = repository;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var filmes = await _repository.BuscaFilmesAsync();
            return filmes.Any() ? Ok(filmes) : BadRequest();
        }

        [HttpGet("id")]
        public async Task<IActionResult> Get(int id)
        {
            var filme = await _repository.BuscaFilmeAsync(id);

            return filme != null
                ? Ok(filme)
                : BadRequest("Filme não encontrado.");
        }

        [HttpPost]
        public async Task<IActionResult> Post(FilmeRequest request)
        {
            if (string.IsNullOrEmpty(request.Nome) || request.Ano <= 0 || request.ProdutoraId <= 0)
            {
                return BadRequest("Informações inválidas.");
            }

            var adicionado = await _repository.AdicionaAsync(request);

            return adicionado
                ? Ok("Filme adicionado com sucesso.")
                : BadRequest("Erro ao adicionar filme.");
        }

        [HttpPut]
        public async Task<IActionResult> Put(FilmeRequest request, int id)
        {
            if (id <= 0) return BadRequest("Filme inválido");

            var filme = await _repository.BuscaFilmeAsync(id);

            if (filme == null) NotFound("Filme não existe");

            if (string.IsNullOrEmpty(request.Nome)) request.Nome = filme.Nome;
            if (request.Ano <= 0) request.Ano = filme.Ano;

            var atualizado = await _repository.AtualizarAsync(request, id);

            return atualizado
               ? Ok("Filme atualizado com sucesso.")
               : BadRequest("Erro ao atualizar filme.");
        }

        [HttpDelete("id")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest("Filme inválido");

            var filme = await _repository.BuscaFilmeAsync(id);

            if (filme == null) NotFound("Filme não existe");

            var deletado = await _repository.DeleteAsync(id);

            return deletado
                ? Ok("Filme deletado com sucesso.")
                : BadRequest("Erro ao deletar filme.");
        }


     }  
}
