
namespace BookStoreWebForms.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string ImagePath { get; set; }
        public string SellerUserId { get; set; }
        public bool IsActive { get; set; }
    }
}
