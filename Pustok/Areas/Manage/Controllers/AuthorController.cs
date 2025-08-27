using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Data;
using Pustok.Models;

namespace Pustok.Areas.Manage.Controllers;

[Area("Manage")]
public class AuthorController : Controller
{
    private readonly AppDbContext _context;

    public AuthorController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Authors
    public async Task<IActionResult> Index()
    {
        return View(await _context.Authors.ToListAsync());
    }

    // GET: Authors/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var author = await _context.Authors
            .FirstOrDefaultAsync(m => m.Id == id);
        if (author == null)
        {
            return NotFound();
        }

        return View(author);
    }

    // GET: Authors/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Authors/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name")] Author author)
    {
        if (ModelState.IsValid)
        {
            _context.Add(author);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(author);
    }

    // GET: Authors/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var author = await _context.Authors.FindAsync(id);
        if (author == null)
        {
            return NotFound();
        }
        return View(author);
    }

    // POST: Authors/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Author author)
    {
        if (id != author.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(author);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(author.Id))
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
        return View(author);
    }

    // GET: Authors/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var author = await _context.Authors
            .FirstOrDefaultAsync(m => m.Id == id);
        if (author == null)
        {
            return NotFound();
        }

        return View(author);
    }

    // POST: Authors/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var author = await _context.Authors.FindAsync(id);
        if (author != null)
        {
            _context.Authors.Remove(author);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool AuthorExists(int id)
    {
        return _context.Authors.Any(e => e.Id == id);
    }
}
