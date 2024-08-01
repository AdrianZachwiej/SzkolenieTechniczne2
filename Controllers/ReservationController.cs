using Microsoft.AspNetCore.Mvc;
using ProjektAdrianZachwiej56233.Models;
using ProjektAdrianZachwiej56233.Services;
using System;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ReservationController : ControllerBase
{
    private readonly ReservationService _reservationService;

    public ReservationController(ReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpPost]
    public async Task<IActionResult> MakeReservation([FromBody] ReservationRequest request)
    {
        try
        {
            if (request == null)
            {
                return BadRequest(new { message = "Invalid reservation data." });
            }

            var reservation = await _reservationService.MakeReservation(
                request.UserId,
                request.TableId,
                request.Date,
                request.FirstName,
                request.LastName,
                request.PhoneNumber
            );

            return CreatedAtAction(nameof(GetReservationById), new { id = reservation.Id }, reservation);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An unexpected error occurred.", error = ex.Message });
        }
    }

    [HttpGet("available-tables")]
    public async Task<IActionResult> GetAvailableTables([FromQuery] DateTime date)
    {
        var tables = await _reservationService.GetAvailableTables(date);
        return Ok(tables);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetReservationById([FromRoute] int id)
    {
        var reservation = await _reservationService.GetReservationById(id);
        if (reservation == null)
        {
            return NotFound(new { message = "Reservation not found." });
        }
        return Ok(reservation);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateReservation([FromRoute] int id, [FromBody] ReservationRequest request)
    {
        try
        {
            if (request == null)
            {
                return BadRequest(new { message = "Invalid reservation data." });
            }

            var updatedReservation = await _reservationService.UpdateReservation(
                id,
                request.UserId,
                request.TableId,
                request.Date,
                request.FirstName,
                request.LastName,
                request.PhoneNumber
            );

            if (updatedReservation == null)
            {
                return NotFound(new { message = "Reservation not found." });
            }
            return Ok(updatedReservation);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An unexpected error occurred.", error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReservation([FromRoute] int id)
    {
        try
        {
            var result = await _reservationService.DeleteReservation(id);
            if (result)
            {
                return NoContent(); 
            }
            return NotFound(new { message = "Reservation not found." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An unexpected error occurred.", error = ex.Message });
        }
    }
}
