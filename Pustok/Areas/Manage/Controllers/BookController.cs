using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pustok.Data;
using Pustok.Models;

namespace Pustok.Areas.Manage.Controllers;

[Area("Manage")]
public class BookController : Controller
{
    private readonly AppDbContext _context;

    public BookController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var books = await _context.Books
            .Include(b => b.Author)
            .Include(b => b.Genre)
            .ToListAsync();
        return View(books);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var book = await _context.Books
            .Include(b => b.Author)
            .Include(b => b.Genre)
            .Include(b => b.BookImages)
            .Include(b => b.BookTags)
            .ThenInclude(bt => bt.Tag)
            .FirstOrDefaultAsync(m => m.Id == id);
        
        if (book == null) return NotFound();

        return View(book);
    }

    public IActionResult Create()
    {
        ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name");
        ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,Description,MainImage,HoverImage,AuthorId,Price,DiscountPrice,InStock,IsFeatured,IsNew,GenreId")] Book book)
    {
        if (ModelState.IsValid)
        {
            _context.Add(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name", book.AuthorId);
        ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", book.GenreId);
        return View(book);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var book = await _context.Books.FindAsync(id);
        if (book == null) return NotFound();
        
        ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name", book.AuthorId);
        ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", book.GenreId);
        return View(book);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,MainImage,HoverImage,AuthorId,Price,DiscountPrice,InStock,IsFeatured,IsNew,GenreId")] Book book)
    {
        if (id != book.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(book);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(book.Id))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name", book.AuthorId);
        ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", book.GenreId);
        return View(book);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var book = await _context.Books
            .Include(b => b.Author)
            .Include(b => b.Genre)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (book == null) return NotFound();

        return View(book);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book != null)
        {
            _context.Books.Remove(book);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool BookExists(int id)
    {
        return _context.Books.Any(e => e.Id == id);
    }
}
