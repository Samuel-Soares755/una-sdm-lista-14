namespace CacauShowApiSeuRa.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public int UnidadeId { get; set; } // Franquia
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorTotal { get; set; }
    }
}