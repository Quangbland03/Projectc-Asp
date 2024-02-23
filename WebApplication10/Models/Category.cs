using System.ComponentModel.DataAnnotations;

namespace WebApplication10.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên danh mục.")]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mô tả danh mục.")]
        public string CategoryDescription { get; set; }
        public virtual ICollection<Product>? Product { get; set; }
    }
}
