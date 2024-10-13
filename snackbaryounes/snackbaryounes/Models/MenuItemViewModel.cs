using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace snackbaryounes.Models
{
    public class MenuItemViewModel
    {
        // Constructor to initialize from MenuItem and allow SelectList population
        public MenuItemViewModel(MenuItem menuItem)
        {
            MenuItemId = menuItem.MenuItemId;
            Name = menuItem.Name;
            Description = menuItem.Description;
            Price = menuItem.Price;
            Status = menuItem.Status;
            ProductCategoryId = menuItem.ProductCategoryId;
            OrderItems = menuItem.OrderItems;
            ProductCategory = menuItem.ProductCategory;
        }

        public int MenuItemId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public bool Status { get; set; }

        public int? ProductCategoryId { get; set; }

        public virtual List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public virtual ProductCategory? ProductCategory { get; set; }

    }
}
