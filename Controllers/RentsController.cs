using Alquiler_videojuegos.DBContext;
using Alquiler_videojuegos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alquiler_videojuegos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Rents/Today

        // GET: api/Rents/Recibo
        [HttpGet("Recibo")]
        public async Task<ActionResult> GetRecibo()
        {
            var clientes = _context.Clients;
            var juegos = _context.Games;
            var usuarios = _context.Users;
            var rentas = _context.Rents;

            var query = await (from recibo in (from ren in (from p in juegos
                                                            join r in rentas on p.IdGame equals r.IdGame
                                                            select new { p.IdGame, r.IdUser, r.IdRent, p.Name, p.RentalPrice, r.RentedDate, r.ExpirationDays })
                                               join us in usuarios on ren.IdUser equals us.IdUser
                                               select new { ren.IdGame, us.IdClient, ren.IdUser, ren.IdRent, ren.Name, ren.RentalPrice, ren.RentedDate, ren.ExpirationDays })
                               join cl in clientes on recibo.IdClient equals cl.IdClient
                               select new { recibo.IdUser, recibo.IdRent, cl.FirstName, cl.LastName, cl.Email, GameTitle = recibo.Name, recibo.RentedDate, recibo.ExpirationDays, recibo.RentalPrice, TotalPrice = recibo.ExpirationDays * recibo.RentalPrice }).ToListAsync()
                        ;

            if (query == null) return NotFound();

            return Ok(query);
        }

        // GET: api/Rents/UserBalance
        [HttpGet("UserInfo/{Userid}")]
        public async Task<ActionResult> UserRentsInfo(int Userid)
        {

            var clientes = _context.Clients;
            var juegos = _context.Games;
            var usuarios = _context.Users;
            var rentas = _context.Rents;
            var query = await (from recibo in (from ren in (from p in juegos
                                                            join r in rentas on p.IdGame equals r.IdGame
                                                            select new { p.IdGame, r.IdUser, r.IdRent, p.Name, p.RentalPrice, r.RentedDate, r.ExpirationDays })
                                               join us in usuarios on ren.IdUser equals us.IdUser
                                               select new { ren.IdGame, us.IdClient, ren.IdUser, ren.IdRent, ren.Name, ren.RentalPrice, ren.RentedDate, ren.ExpirationDays })
                               join cl in clientes on recibo.IdClient equals cl.IdClient
                               select new { recibo.IdUser, recibo.IdRent, GameTitle = recibo.Name, recibo.RentedDate, recibo.ExpirationDays, recibo.RentalPrice, ReturnDate = recibo.RentedDate.AddDays(recibo.ExpirationDays), TotalPrice = recibo.ExpirationDays * recibo.RentalPrice }).Where(x => x.IdUser == Userid).ToListAsync()
                        ;
            if (query == null) { NotFound(); }

            return Ok(query);

        }


        // GET: api/Daily
        [HttpGet("Daily")]
        public async Task<ActionResult> GetDailyRents()
        {


            DateTime fecha = DateTime.Today;

            var clientes = _context.Clients;
            var juegos = _context.Games;
            var usuarios = _context.Users;
            var rentas = _context.Rents;

            var query = await (from recibo in (from ren in (from p in juegos
                                                            join r in rentas on p.IdGame equals r.IdGame
                                                            select new { p.IdGame, r.IdUser, r.IdRent, p.Name, p.RentalPrice, r.RentedDate, r.ExpirationDays })
                                               join us in usuarios on ren.IdUser equals us.IdUser
                                               select new { ren.IdGame, us.IdClient, ren.IdUser, ren.IdRent, ren.Name, ren.RentalPrice, ren.RentedDate, ren.ExpirationDays })
                               join cl in clientes on recibo.IdClient equals cl.IdClient
                               select new { recibo.IdUser, recibo.IdRent, cl.FirstName, cl.LastName, cl.Email, GameTitle = recibo.Name, recibo.RentedDate, recibo.ExpirationDays, recibo.RentalPrice, TotalPrice = recibo.ExpirationDays * recibo.RentalPrice }).Where(x => x.RentedDate == fecha).ToListAsync();
            ;

            if (query == null) { return NotFound(); }

            return Ok(query);
        }

        // GET: api/Rents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rent>>> GetRents()
        {
            return await _context.Rents.ToListAsync();
        }

        // GET: api/Rents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rent>> GetRent(int id)
        {
            var rent = await _context.Rents.FindAsync(id);

            if (rent == null)
            {
                return NotFound();
            }

            return rent;
        }

        // PUT: api/Rents/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRent(int id, Rent rent)
        {
            if (id != rent.IdRent)
            {
                return BadRequest();
            }

            _context.Entry(rent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RentExists(id))
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

        // POST: api/Rents
        [HttpPost]
        public async Task<ActionResult<Rent>> PostRent(Rent rent)
        {
            _context.Rents.Add(rent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRent", new { id = rent.IdRent }, rent);
        }

        // DELETE: api/Rents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRent(int id)
        {
            var rent = await _context.Rents.FindAsync(id);
            if (rent == null)
            {
                return NotFound();
            }

            _context.Rents.Remove(rent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RentExists(int id)
        {
            return _context.Rents.Any(e => e.IdRent == id);
        }
    }
}