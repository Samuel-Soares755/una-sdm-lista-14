using Microsoft.AspNetCore.Mvc;
using CacauShowApiSeuRa.Data;

namespace CacauShowApiSeuRa.Controllers
{
    [ApiController]
    [Route("api/intelligence")]
    public class ChocolateIntelligenceController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ChocolateIntelligenceController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("estoque-regional")]
        public IActionResult GetEstoqueRegional()
        {
            Thread.Sleep(2000);

            var resultado = _context.Pedidos
                .Join(_context.Franquias,
                    p => p.UnidadeId,
                    f => f.Id,
                    (p, f) => new { f.Cidade, p.Quantidade })
                .GroupBy(x => x.Cidade)
                .Select(g => new
                {
                    Cidade = g.Key,
                    TotalItens = g.Sum(x => x.Quantidade)
                })
                .ToList();

            return Ok(resultado);
        }
    }
}