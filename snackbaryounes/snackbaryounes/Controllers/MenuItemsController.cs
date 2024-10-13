using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using snackbaryounes.Models;
using snackbaryounes.Controllers.API;
using Microsoft.EntityFrameworkCore; // Import the API controller namespace

namespace snackbaryounes.Controllers
{
    public class MenuItemsController : Controller
    {
        private readonly MenuItemsAPIController _apiController;

        public MenuItemsController(MenuItemsAPIController apiController)
        {
            _apiController = apiController;
        }

        // GET: MenuItems
        public async Task<IActionResult> Index()
        {
            var result = await _apiController.GetMenuItems();
            if (result.Value != null)
            {
                return View(result.Value);
            }
            else
            {
                return View(new List<MenuItem>());
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Roep de API aan om de productdetails op te halen
            var productResponse = await _apiController.GetMenuById(id.Value);

            // Controleer of de API-aanroep succesvol was
            if (productResponse is OkObjectResult okResult)
            {
                // Haal het Product-object uit het OkObjectResult
                var product = okResult.Value as MenuItem;

                // Controleer of het product bestaat
                if (product != null)
                {
                    // Maak een ProductViewModel van het opgehaalde Product-object
                    var viewModel = new MenuItemViewModel(product);

                    // Geef de view terug met het ProductViewModel
                    return View(viewModel);
                }
            }

            // Als het product niet gevonden is, geef een foutpagina terug
            return View("Error");
        }

        public async Task<IActionResult> Create()
        {
            var categoriesResponse = await _apiController.GetProductCategories();
            if (categoriesResponse is OkObjectResult okResult)
            {
                var categories = okResult.Value as List<ProductCategory>;
                ViewData["ProductCategoryId"] = new SelectList(categories, "ProductCategoryId", "Name");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MenuItemId,Name,Description,Price,Status,ProductCategoryId")] MenuItem product)
        {
            if (ModelState.IsValid)
            {
                // Stuur de gegevens naar de API om het product op te slaan
                var result = await _apiController.PostMenuItem(product);

                // Controleer of het resultaat een succesvolle 201 Created response is
                if (result is CreatedAtActionResult || result is ObjectResult objectResult && objectResult.StatusCode == 201)
                {
                    // Als het product succesvol is aangemaakt, ga terug naar de Index
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Als er een fout is, geef een foutpagina weer
                    return View("Error");
                }
            }

            // Als het model ongeldig is, haal de categorieën opnieuw op en toon het formulier opnieuw
            var categoriesResponse = await _apiController.GetProductCategories();
            if (categoriesResponse is OkObjectResult okResult)
            {
                var categories = okResult.Value as List<ProductCategory>;
                ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "Name");
            }

            return View(product); // Geef het formulier opnieuw weer met de foutmeldingen
        }


        // GET: MenuItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Retrieve the MenuItem from the API
            var result = await _apiController.GetMenuItem(id.Value);
            if (result is OkObjectResult okProductResult)
            {

                var product = okProductResult.Value as MenuItem;
                if (product == null) return NotFound();

                var categoriesResponse = await _apiController.GetProductCategories();
                if (categoriesResponse is OkObjectResult okCategoriesResult)
                {
                    var categories = okCategoriesResult.Value as List<ProductCategory>;
                    ViewData["ProductCategoryId"] = new SelectList(categories, "ProductCategoryId", "Name", product.ProductCategoryId);
                }

                return View(product);
            }
            return NotFound();

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MenuItemId,Name,Description,Price,Status,ProductCategoryId")] MenuItem product)
        {
            if (id != product.MenuItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Roep de API aan om het product bij te werken
                    var result = await _apiController.PutMenuItem(id, product);

                    if (result is NoContentResult)
                    {
                        // Als de update geslaagd is, ga terug naar de Index
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        // Als de update niet geslaagd is, geef een foutpagina weer
                        return View("Error");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Controleer of het product nog steeds bestaat via de API
                    var exists = await _apiController.MenuItemExists(id);
                    if (exists is OkObjectResult okResult && (bool)okResult.Value)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // Als het model niet geldig is, geef het formulier opnieuw weer met de categorieën
            var categoriesResponse = await _apiController.GetProductCategories();
            if (categoriesResponse is OkObjectResult okCategoriesResult)
            {
                var categories = okCategoriesResult.Value as List<MenuItem>;
                ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "Name", product.ProductCategoryId);
            }

            return View(product);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var productResponse = await _apiController.GetMenuById(id.Value);
            if (productResponse is OkObjectResult okProductResult)
            {
                var product = okProductResult.Value as MenuItem;
                if (product == null) return NotFound();

                return View(product); // De view die bevestigt of je het product wilt verwijderen
            }

            return NotFound();
        }

        // POST: MenuItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _apiController.DeleteMenuItem(id);
            if (result is NoContentResult)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Handle error
                ModelState.AddModelError(string.Empty, "An error occurred while deleting the menu item.");
                return View();
            }
        }
    }
}
