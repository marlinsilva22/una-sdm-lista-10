using Microsoft.AspNetCore.Mvc;
using OscarFilmeApi.Data;
using OscarFilmeApi.Models;

namespace OscarFilmeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilmesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FilmesController(AppDbContext context)
        {
            _context = context;
        }

        // POST
        [HttpPost]
        public IActionResult Post(Filme filme)
        {
            if (filme.AnoLancamento < 1929)
            {
                return BadRequest("O Oscar começou em 1929. Ano inválido.");
            }

            _context.Filmes.Add(filme);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = filme.Id }, filme);
        }

        // GET ALL
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Filmes.ToList());
        }

        // GET BY ID
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var filme = _context.Filmes.Find(id);

            if (filme == null)
                return NotFound();

            return Ok(filme);
        }

        // GET VENCEDORES
        [HttpGet("vencedores")]
        public IActionResult GetVencedores()
        {
            var vencedores = _context.Filmes
                .Where(f => f.Venceu)
                .ToList();

            return Ok(vencedores);
        }

        // PUT
        [HttpPut("{id}")]
        public IActionResult Put(int id, Filme filmeAtualizado)
        {
            var filme = _context.Filmes.Find(id);

            if (filme == null)
                return NotFound();

            filme.Titulo = filmeAtualizado.Titulo;
            filme.Diretor = filmeAtualizado.Diretor;
            filme.Categoria = filmeAtualizado.Categoria;
            filme.AnoLancamento = filmeAtualizado.AnoLancamento;
            filme.Venceu = filmeAtualizado.Venceu;

            // DESAFIO
            if (filme.Venceu)
            {
                Console.WriteLine($"Temos um novo vencedor: {filme.Titulo}!");
            }

            _context.SaveChanges();

            return NoContent();
        }

        // DELETE
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var filme = _context.Filmes.Find(id);

            if (filme == null)
                return NotFound();

            _context.Filmes.Remove(filme);
            _context.SaveChanges();

            return NoContent();
        }

        // EXTRA - ESTATÍSTICAS
        [HttpGet("estatisticas")]
        public IActionResult GetEstatisticas()
        {
            var total = _context.Filmes.Count();
            var vencedores = _context.Filmes.Count(f => f.Venceu);

            return Ok(new
            {
                TotalFilmes = total,
                TotalVencedores = vencedores
            });
        }
    }
}