using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using snackbaryounes.Models;

namespace snackbaryounes.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuItemsAPIController : ControllerBase
    {
        private readonly SnackbarDatabaseContext _context;

        public MenuItemsAPIController(SnackbarDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/MenuItemsAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuItem>>> GetMenuItems()
        {
            var menuItems = await _context.MenuItems
                .Include(m => m.ProductCategory)
                .ToListAsync();
            return menuItems;
        }

        // GET: api/MenuItemsAPI/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMenuItem(int id)
        {
            var menuItem = await _context.MenuItems
                .Include(m => m.ProductCategory)
                .FirstOrDefaultAsync(m => m.MenuItemId == id);
            if (menuItem == null) return NotFound();
            return Ok(menuItem);
        }

        [HttpGet("GetProductCategories")]
        public async Task<IActionResult> GetProductCategories()
        {
            var categories = await _context.ProductCategories.ToListAsync();

            if (categories == null || !categories.Any())
            {
                // Handle empty categories gracefully, or log the issue
                return NotFound("No categories found.");
            }

            return Ok(categories);
        }


        // POST: api/MenuItemsAPI
        [HttpPost]
        public async Task<IActionResult> PostMenuItem(MenuItem menuItem)
        {
            if (menuItem == null)
            {
                return BadRequest();
            }

            _context.MenuItems.Add(menuItem);
            await _context.SaveChangesAsync();

            // Retourneer een 201 Created response en de locatie van het nieuwe product
            return CreatedAtAction(nameof(GetMenuById), new { id = menuItem.MenuItemId}, menuItem);
        }

        // PUT: api/MenuItemsAPI/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMenuItem(int id, MenuItem menuItem)
        {
            if (id != menuItem.MenuItemId)
            {
                return BadRequest();
            }

            _context.Entry(menuItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await MenuItemExistsAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        private async Task<bool> MenuItemExistsAsync(int id)
        {
            return await _context.MenuItems.AnyAsync(e => e.MenuItemId == id);
        }

        [HttpGet("exists/{id}")]
        public async Task<IActionResult> MenuItemExists(int id)
        {
            var exists = await Task.FromResult(_context.MenuItems.Any(e => e.MenuItemId == id)); // Zorg ervoor dat het async is
            return Ok(exists);
        }

        [HttpGet("product/{id}")]
        public async Task<IActionResult> GetMenuById(int id)
        {
            var product = await _context.MenuItems
                .Include(p => p.ProductCategory)
                .FirstOrDefaultAsync(p => p.MenuItemId == id);

            if (product == null) return NotFound();
            return Ok(product);
        }

        // DELETE: api/MenuItemsAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            var menuItem = await _context.MenuItems.FindAsync(id);
            if (menuItem == null)
            {
                return NotFound();
            }

            _context.MenuItems.Remove(menuItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
