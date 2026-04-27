using Microsoft.AspNetCore.Mvc;
using CacauShowApiSeuRa.Data;
using CacauShowApiSeuRa.Models;

namespace CacauShowApiSeuRa.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Produtos.ToList());
        }

        [HttpPost]
        public IActionResult Create([FromBody] Produto produto)
        {
            _context.Produtos.Add(produto);
            _context.SaveChanges();
            return Ok(produto);
        }
    }
}