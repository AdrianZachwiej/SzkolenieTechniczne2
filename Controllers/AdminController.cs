using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjektAdrianZachwiej56233.Data;
using ProjektAdrianZachwiej56233.Models;
using System.Linq;
using System.Threading.Tasks;

[Route("admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("reservations")]
    public async Task<IActionResult> Index()
    {
        var reservations = await _context.Reservations.ToListAsync();
        return View(reservations);
    }

    [HttpGet("reservations/edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        var reservation = await _context.Reservations.FindAsync(id);
        if (reservation == null)
        {
            return NotFound();
        }
        return View(reservation);
    }

    [HttpPost("reservations/edit/{id}")]
    public async Task<IActionResult> Edit(int id, Reservation reservation)
    {
        if (id != reservation.Id)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            _context.Update(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(reservation);
    }

    [HttpPost("reservations/delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var reservation = await _context.Reservations.FindAsync(id);
        if (reservation != null)
        {
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
