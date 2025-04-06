using Event_Ease.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class VenueController : Controller
{
    // Portions of the code in this file were written with the assistance of GitHub Copilot.
    //My CRUD methods were written with the assistace of lecturer videos
    private readonly ApplicationDbContext _context;

    public VenueController(ApplicationDbContext context)
    {
        _context = context;
    }

    // This method will return a list of all venues
    public async Task<IActionResult> Index()
    {
        var venues = await _context.Venue.ToListAsync();
        return View(venues);
    }

    // This method will return a form to create a new venue
    public IActionResult Create()
    {
        return View();
    }

   
    [HttpPost]
    public async Task<IActionResult> Create(Venue venue)
    {
        if (ModelState.IsValid)
        {
            _context.Add(venue);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        return View(venue);
    }

    // This method will return a form to delete a venue
    public async Task<IActionResult> Delete(int id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var venue = await _context.Venue
            .FirstOrDefaultAsync(v => v.VenueId == id);
        if (venue == null)
        {
            return NotFound();
        }
        return View(venue);
    }

    // This method will handle the deletion of the venue
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var venue = await _context.Venue.FindAsync(id);
        if (venue != null)
        {
            _context.Venue.Remove(venue);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    // GET: Venue/Edit/{id}
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var venue = await _context.Venue.FindAsync(id);
        if (venue == null)
        {
            return NotFound();
        }
        return View(venue);
    }

    // POST: Venue/Edit/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("VenueId,VenueName,Location,Capacity,ImageUrl")] Venue venue)
    {
        if (id != venue.VenueId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(venue);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VenueExists(venue.VenueId))
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
        return View(venue);
    }

    

    // This method will return the details of a venue
    // '?' means that the parameter is nullable, which means it can be null or have a value of type int.

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();  // it will return a 404 Not Found error if the id is null
        }

        //This will  find the venue with the specified id and include the events associated with the venue
        var venue = await _context.Venue
            .Include(v => v.Events)
            .FirstOrDefaultAsync(v => v.VenueId == id);

        if (venue == null)
        {
            return NotFound();  // it will return a 404 Not Found error if the venue is null
        }  

        return View(venue);
    }

    // Check if the venue exists in the database
    private bool VenueExists(int id)
    {
        return _context.Venue.Any(e => e.VenueId == id);
    }
}
