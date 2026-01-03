namespace App.DTOs
{
    public class UserReviewDTO
    {
       public int rating { get; set; }
       public string comment { get; set; } = null!;
    }
}