using Microsoft.AspNetCore.Mvc;
using CacauShowApiSeuRa.Data;
using CacauShowApiSeuRa.Models;

namespace CacauShowApiSeuRa.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LotesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LotesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Lotes.ToList());
        }

        [HttpPost]
        public IActionResult Create([FromBody] LoteProducao lote)
        {
            var produto = _context.Produtos.Find(lote.ProdutoId);

            if (produto == null)
                return BadRequest("Produto não existe.");

            if (lote.DataFabricacao > DateTime.Now)
                return Conflict("Lote inválido: Data de fabricação não pode ser maior que a data atual.");

            _context.Lotes.Add(lote);
            _context.SaveChanges();

            return Ok(lote);
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateStatus(int id, [FromBody] string novoStatus)
        {
            var lote = _context.Lotes.Find(id);

            if (lote == null)
                return NotFound("Lote não encontrado.");

            if (lote.Status == "Descartado" &&
                (novoStatus == "Qualidade Aprovada" || novoStatus == "Distribuído"))
            {
                return BadRequest("Regra violada: Lote descartado não pode mudar para esse status.");
            }

            lote.Status = novoStatus;
            _context.SaveChanges();

            return Ok(lote);
        }
    }
}