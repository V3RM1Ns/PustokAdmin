using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Data;
using Pustok.Models;

namespace Pustok.Areas.Manage.Controllers;

[Area("Manage")]
public class SliderController : Controller
{
    private readonly AppDbContext _context;

    public SliderController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Sliders.ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var slider = await _context.Sliders
            .FirstOrDefaultAsync(m => m.Id == id);
        if (slider == null) return NotFound();

        return View(slider);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,Description,Image,ButtonText,ButtonUrl,Order")] Slider slider)
    {
        if (ModelState.IsValid)
        {
            _context.Add(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(slider);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var slider = await _context.Sliders.FindAsync(id);
        if (slider == null) return NotFound();
        return View(slider);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Image,ButtonText,ButtonUrl,Order")] Slider slider)
    {
        if (id != slider.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(slider);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SliderExists(slider.Id))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(slider);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var slider = await _context.Sliders
            .FirstOrDefaultAsync(m => m.Id == id);
        if (slider == null) return NotFound();

        return View(slider);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var slider = await _context.Sliders.FindAsync(id);
        if (slider != null)
        {
            _context.Sliders.Remove(slider);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool SliderExists(int id)
    {
        return _context.Sliders.Any(e => e.Id == id);
    }
}
