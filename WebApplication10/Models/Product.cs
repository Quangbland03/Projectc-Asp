using System.ComponentModel.DataAnnotations;

namespace WebApplication10.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên sách.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tác giả.")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mô tả.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá.")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập đường dẫn hình ảnh.")]
        public string ImagePath { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục.")]
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
    }
}
