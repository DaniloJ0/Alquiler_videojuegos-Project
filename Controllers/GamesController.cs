using Alquiler_videojuegos.DBContext;
using Alquiler_videojuegos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentGameAPI.Models.querys;

namespace Alquiler_videojuegos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GamesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames()
        {
          if (_context.Games == null)
          {
              return NotFound();
          }
            return await _context.Games.ToListAsync();
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
          if (_context.Games == null)
          {
              return NotFound();
          }
            var game = await _context.Games.FindAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            return game;
        }

        [HttpGet("MenosRentado")]
        public async Task<ActionResult> GetMenosRentado()
        {
            var clientes = _context.Clients;
            var juegos = _context.Games;
            var usuarios = _context.Users;
            var rentas = _context.Rents;
            var primerquery = from c in clientes join us in usuarios on c.IdClient equals us.IdClient select new { IdClient = c.IdClient, IdUser = us.IdUser, age = c.Age };
            var segundoquery = from r in rentas join pr in primerquery on r.IdUser equals pr.IdUser select new { Age = pr.age, IdGame = r.IdGame };
            var tercerquery = await (from g in juegos join se in segundoquery on g.IdGame equals se.IdGame select new { age = se.Age, name = g.Name, rango = (se.Age / 10) * 10 }).OrderBy(x => x.rango).ToArrayAsync();
            if (tercerquery.Length == 0)
                return NoContent();
            List<MenosRentado> resultado = new List<MenosRentado>();

            for (var i = 0; i < 12; i++)
            {
                var auxiliarList = (from x in (tercerquery.Where(x => x.rango == i * 10)) group x by x.name into grp select new { title = grp.Key, frecuencia = grp.Count(), rango = i * 10 }).OrderBy(x => x.frecuencia);
                if (auxiliarList.Count() != 0)
                {
                    var auxiliar = auxiliarList.First();
                    MenosRentado transformar = new MenosRentado(auxiliar.title, auxiliar.rango, auxiliar.frecuencia);
                    resultado.Add(transformar);
                }
            }
            var aLista = resultado.ToList();
            if (aLista == null) { return NotFound(); }
            return Ok(aLista);
        }


        [HttpGet("MasRentado")]
        public async Task<ActionResult> GetMasRentado()
        {
            var juegos = _context.Games;
            var rentas = _context.Rents;
            var query = await (from ord in (from r in
                (from r in rentas group r by r.IdGame into grp select new { IdGame = grp.Key, Rentados = grp.Count() })
                                            join g in juegos on r.IdGame equals g.IdGame
                                            select new { Titulo = g.Name, r.IdGame, frecuencia = r.Rentados })
                               orderby ord.frecuencia descending
                               select (ord)).FirstAsync()
                ;
            if (query == null) return NoContent();

            return Ok(query);
        }


        [HttpGet("InfoGames/{atributo}/{data}")]
        public async Task<ActionResult> GetInfoGames(string atributo, string data)
        {
            var juegos = _context.Games;
            switch (atributo)
            {
                case "ReleaseDate":
                    var query = await juegos.Where(x => x.ReleaseYear == Int32.Parse(data)).ToListAsync();
                    if (query == null) { return NotFound(); }
                    return Ok(query);
                    break;
                case "title":
                    var query2 = await juegos.Where(x => x.Name.Contains(data)).ToListAsync();
                    if (query2 == null) { return NotFound(); }
                    return Ok(query2);
                    break;
                case "director":
                    var query3 = await juegos.Where(x => x.Director.Contains(data)).ToListAsync();
                    if (query3 == null) { return NotFound(); }
                    return Ok(query3);
                    break;
                case "protagonist":
                    var query4 = await juegos.Where(x => x.Protagonist.Contains(data)).ToListAsync();
                    if (query4 == null) { return NotFound(); }
                    return Ok(query4);
                    break;
                default:
                    return BadRequest();
                    break;

            }

        }

        // PUT: api/Games/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, Game game)
        {
            if (id != game.IdGame)
            {
                return BadRequest();
            }

            _context.Entry(game).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Games
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(Game game)
        {
          if (_context.Games == null)
          {
              return Problem("Entity set 'AppDbContext.Games'  is null.");
          }
            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGame", new { id = game.IdGame }, game);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            if (_context.Games == null)
            {
                return NotFound();
            }
            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GameExists(int id)
        {
            return (_context.Games?.Any(e => e.IdGame == id)).GetValueOrDefault();
        }
    }
}
