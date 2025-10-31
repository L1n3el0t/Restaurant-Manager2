using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantManager.Data;
using RestaurantManager.Models;

namespace RestaurantManager.Services
{
    public class ReservationService : IReservationService
    {

        private readonly ILogger<ReservationService> _logger;
        private readonly RestaurantManagerContext _context;


        public ReservationService(RestaurantManagerContext context, ILogger<ReservationService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Reservation>> GetAllReservations()
        {
            var restaurantManagerContext = _context.Reservation.Include(r => r.Customer);
            return await restaurantManagerContext.ToListAsync();
        }

        public async Task<Reservation?> GetReservationById(int id)
        {
            var reservation = await _context.Reservation
                .Include(r => r.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                _logger.LogWarning($"Reservation with ID {id} not found.");
            }
            else
            {
                _logger.LogInformation($"Reservation with ID {id} retrieved.");
            }
            return reservation;
        }

        public async Task CreateReservation(Reservation reservation)
        {
            _context.Reservation.Add(reservation);
            _logger.LogInformation($"Reservation for Customer ID {reservation.CustomerId} added.");
            await _context.SaveChangesAsync();
        }

        public async Task EditReservation(Reservation reservation)
        {
            _context.Reservation.Update(reservation);
            _logger.LogInformation($"Reservation with ID {reservation.Id} updated.");
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReservation(int id)
        {
            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservation.Remove(reservation);
                _logger.LogInformation($"Reservation with ID {id} deleted.");
                await _context.SaveChangesAsync();
            }
            else
            {
                _logger.LogWarning($"Reservation with ID {id} not found for deletion.");
            }
        }


        public SelectList GetCustomerSelectList()
            {

            var data = new SelectList(_context.Customer, "Id", "Id");
            return data;

        }
}
}

