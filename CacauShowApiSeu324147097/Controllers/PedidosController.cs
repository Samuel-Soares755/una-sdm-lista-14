using Microsoft.AspNetCore.Mvc;
using CacauShowApiSeuRa.Data;
using CacauShowApiSeuRa.Models;

namespace CacauShowApiSeuRa.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PedidosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Pedidos.ToList());
        }

        [HttpPost]
        public IActionResult Create([FromBody] Pedido pedido)
        {
            var franquia = _context.Franquias.Find(pedido.UnidadeId);
            var produto = _context.Produtos.Find(pedido.ProdutoId);

            if (franquia == null || produto == null)
                return BadRequest("Franquia ou Produto inválido.");

            int totalAtual = _context.Pedidos
                .Where(p => p.UnidadeId == pedido.UnidadeId)
                .Sum(p => p.Quantidade);

            if (totalAtual + pedido.Quantidade > franquia.CapacidadeEstoque)
            {
                return BadRequest("Capacidade logística da loja excedida. Não é possível receber mais produtos.");
            }

            pedido.ValorTotal = pedido.Quantidade * produto.PrecoBase;

            if (produto.Tipo == "Sazonal")
            {
                pedido.ValorTotal += 15;
                Console.WriteLine("Produto sazonal detectado: Adicionando embalagem premium!");
            }

            _context.Pedidos.Add(pedido);
            _context.SaveChanges();

            return Ok(pedido);
        }
    }
}