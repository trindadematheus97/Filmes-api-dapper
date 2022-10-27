namespace Filmes.Models
{
    public class FilmeResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Ano { get; set; }
        public string ProdutoraNome { get; set; }
        public int ProdutoraId { get; set; }
    }
}
