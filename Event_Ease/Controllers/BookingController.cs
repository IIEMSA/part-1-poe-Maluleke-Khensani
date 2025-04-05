using Event_Ease.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Event_Ease.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        //This method will inclue the event and venue in the booking and return the view
        public async Task<IActionResult> Index()
        {
            var bookings = await _context.Booking
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .ToListAsync();
            return View(bookings);
        }

        public IActionResult Create()
        {
            ViewData["EventId"] = new SelectList(_context.Event, "EventId", "EventName");
            ViewData["VenueId"] = new SelectList(_context.Venue, "VenueId", "VenueName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Booking booking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            // If the model state is not vaild, then it will repoopulate the dropdowns
            ViewData["EventId"] = new SelectList(_context.Event, "EventId", "EventName", booking.EventId);
            ViewData["VenueId"] = new SelectList(_context.Venue, "VenueId", "VenueName", booking.VenueId);
            return View(booking);
        }


        // This method will delete the booking and return the view
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var booking = await _context.Booking
                .FirstOrDefaultAsync(b => b.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }


        // The hhtp post is responsible for deleting the booking
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Booking.FindAsync(id);
            _context.Booking.Remove(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking  // Get the booking by id
                .Include(b => b.Venue)     // Include the venue
                .FirstOrDefaultAsync(b => b.BookingId == id);  // Get the booking by id

            if (booking == null)
            {
                return NotFound();
            }

            ViewData["EventId"] = new SelectList(_context.Event, "EventId", "EventName", booking.EventId);
            ViewBag.CurrentVenueName = booking.Venue?.VenueName;

            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Booking booking)
        {
            if (id != booking.BookingId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }

            ViewData["EventId"] = new SelectList(_context.Event, "EventId", "EventName", booking.EventId);
            ViewData["VenueId"] = new SelectList(_context.Venue, "VenueId", "VenueName", booking.VenueId);
            return View(booking);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var booking = await _context.Booking
    .Include(b => b.Event)
    .Include(b => b.Venue)
    .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null)
            {
                return NotFound();
            }
            ViewData["EventId"] = new SelectList(_context.Event, "EventId", "EventName", booking.EventId);
            ViewData["VenueId"] = new SelectList(_context.Venue, "VenueId", "VenueName", booking.VenueId);
            return View(booking);
        }


        private bool BookingExists(int id)
        {
            return _context.Booking.Any(b => b.BookingId == id);
        }


    }
}
