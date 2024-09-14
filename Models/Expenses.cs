namespace plusminus.Models
{
    public class Expenses
    {
        public int Id { get; set; }
        public int UserId{ get; set; }
        public DateOnly Date { get; set; }
        public int CategoryId { get; set; }
        public decimal Amount { get; set; }
        virtual public CategoryExpenses Category { get; set; } = null!;
        virtual public User User{ get; set; } = null!;
    }
}
