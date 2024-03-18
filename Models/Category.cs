using System.ComponentModel.DataAnnotations;

namespace Web_Asm2.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter a category name.")]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "Please enter a category description.")]
        public string CategoryDescription { get; set; }
        public virtual ICollection<Product>? Product { get; set; }
    }
}
