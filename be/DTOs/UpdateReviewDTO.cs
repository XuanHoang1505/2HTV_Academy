using System.ComponentModel.DataAnnotations;

namespace App.DTOs
{
     public class UpdateReviewDTO
    {
        [Required(ErrorMessage = "Rating là bắt buộc")]
        [Range(1, 5, ErrorMessage = "Rating phải từ 1 đến 5")]
        public int Rating { get; set; }
        
        [MaxLength(1000, ErrorMessage = "Comment không được vượt quá 1000 ký tự")]
        public string Comment { get; set; }
    }
}