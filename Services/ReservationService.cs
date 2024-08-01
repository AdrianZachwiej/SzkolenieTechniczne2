using Microsoft.EntityFrameworkCore;
using ProjektAdrianZachwiej56233.Data;
using ProjektAdrianZachwiej56233.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektAdrianZachwiej56233.Services
{
    public class ReservationService
    {
        private readonly ApplicationDbContext _context;

        public ReservationService(ApplicationDbContext context)
        {
            _context = context;
        }

      
        public async Task<Reservation> MakeReservation(int userId, int tableId, DateTime date, string firstName, string lastName, string phoneNumber)
        {
            if (date <= DateTime.Now)
            {
                throw new ArgumentException("Nie można rezerwować w przeszłość");
            }

            var existingReservation = await _context.Reservations
                .FirstOrDefaultAsync(r => r.TableId == tableId && r.Date.Date == date.Date);

            if (existingReservation != null)
            {
                throw new InvalidOperationException("Stolik jest już zajęty");
            }

            var reservation = new Reservation
            {
                UserId = userId,
                TableId = tableId,
                Date = date,
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
            return reservation;
        }

    
        public async Task<IEnumerable<Table>> GetAvailableTables(DateTime date)
        {
            var reservedTableIds = await _context.Reservations
                .Where(r => r.Date.Date == date.Date)
                .Select(r => r.TableId)
                .ToListAsync();

            var availableTables = await _context.Tables
                .Where(t => t.IsAvailable && !reservedTableIds.Contains(t.Id))
                .ToListAsync();

            return availableTables;
        }

      
        public async Task<Reservation> GetReservationById(int id)
        {
            return await _context.Reservations
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);
        }

    
        public async Task<bool> DeleteReservation(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null) return false;

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
