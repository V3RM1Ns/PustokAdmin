using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Data;
using Pustok.Models;

namespace Pustok.Areas.Manage.Controllers;

[Area("Manage")]
public class TagController : Controller
{
    private readonly AppDbContext _context;

    public TagController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Set<Tag>().ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var tag = await _context.Set<Tag>()
            .FirstOrDefaultAsync(m => m.Id == id);
        if (tag == null) return NotFound();

        return View(tag);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name")] Tag tag)
    {
        if (ModelState.IsValid)
        {
            _context.Add(tag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(tag);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var tag = await _context.Set<Tag>().FindAsync(id);
        if (tag == null) return NotFound();
        return View(tag);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Tag tag)
    {
        if (id != tag.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(tag);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TagExists(tag.Id))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(tag);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var tag = await _context.Set<Tag>()
            .FirstOrDefaultAsync(m => m.Id == id);
        if (tag == null) return NotFound();

        return View(tag);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var tag = await _context.Set<Tag>().FindAsync(id);
        if (tag != null)
        {
            _context.Set<Tag>().Remove(tag);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TagExists(int id)
    {
        return _context.Set<Tag>().Any(e => e.Id == id);
    }
}
