
namespace DAL.Entities
{
    public class Budget
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public double Limit { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
