using System.ComponentModel.DataAnnotations;

namespace Web_Asm2.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter book title.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please enter author.")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Please enter a description.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please enter price.")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Please enter image path.")]
        public string ImagePath { get; set; }

        [Required(ErrorMessage = "Please select a category.")]
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
    }
}
