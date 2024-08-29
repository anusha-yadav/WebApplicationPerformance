using WebApplication_Performance.Models;
using WebApplicationPerformance.Data.Entities;

namespace WebApplication_Performance.Mapper
{
    public static class ProductMapper
    {
        public static List<Item> GetItems(List<Book> books)
        {
            List<Item> items = new();
            foreach (Book book in books) {
                Item item = new Item()
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    ISBN = book.ISBN,
                };
                items.Add(item);
            }
            return items;
        }
    }
}
