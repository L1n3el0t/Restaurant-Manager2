using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantManager.Data;
using RestaurantManager.Models;
using RestaurantManager.Services;

namespace RestaurantManager.Controllers
{
    public class ReservationsController : Controller
    {
    
        private readonly IReservationService _reservationService;

        public ReservationsController(IReservationService reservationService)
        {
        
            _reservationService = reservationService;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            var reservation = await _reservationService.GetAllReservations();
            return View(reservation);
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _reservationService.GetReservationById(id.Value);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = _reservationService.GetCustomerSelectList();
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomerId,ReservationTime,NumberOfGuests,SpecialRequests")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                await _reservationService.CreateReservation(reservation);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_reservationService.GetCustomerSelectList(), reservation.CustomerId);
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _reservationService.GetReservationById(id.Value);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = _reservationService.GetCustomerSelectList();
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerId,ReservationTime,NumberOfGuests,SpecialRequests")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _reservationService.EditReservation(reservation);
                }
                catch (DbUpdateConcurrencyException)
                {
                    var reservationExists = await _reservationService.GetReservationById(reservation.Id);
                    if (reservationExists == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_reservationService.GetCustomerSelectList(), reservation.CustomerId);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _reservationService.GetReservationById(id.Value);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _reservationService.DeleteReservation(id);
            return RedirectToAction(nameof(Index));
        }

        
    }
}
