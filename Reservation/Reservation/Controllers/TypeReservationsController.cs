using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Reservation.Data;
using Reservation.Models;

namespace Reservation.Controllers
{
    [Authorize(Roles = "Admin")]

    public class TypeReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TypeReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // TypeReservations
        public async Task<IActionResult> Index()
        {
            return View(await _context.TypeReservations.ToListAsync());
        }

        // Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeReservations = await _context.TypeReservations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (typeReservations == null)
            {
                return NotFound();
            }

            return View(typeReservations);
        }

        // Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,AccessNumber")] TypeReservations typeReservations)
        {
            if (ModelState.IsValid)
            {
                _context.Add(typeReservations);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(typeReservations);
        }

        // Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeReservations = await _context.TypeReservations.FindAsync(id);
            if (typeReservations == null)
            {
                return NotFound();
            }
            return View(typeReservations);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,AccessNumber")] TypeReservations typeReservations)
        {
            if (id != typeReservations.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(typeReservations);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeReservationsExists(typeReservations.Id))
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
            return View(typeReservations);
        }

        // Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeReservations = await _context.TypeReservations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (typeReservations == null)
            {
                return NotFound();
            }

            return View(typeReservations);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var typeReservations = await _context.TypeReservations.FindAsync(id);
            _context.TypeReservations.Remove(typeReservations);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TypeReservationsExists(int id)
        {
            return _context.TypeReservations.Any(e => e.Id == id);
        }
    }
}
