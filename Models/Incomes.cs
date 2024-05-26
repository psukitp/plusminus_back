namespace plusminus.Models
{
    public class Incomes
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateOnly Date { get; set; }
        public int CategoryId { get; set; }
        public int Amount { get; set; }

        virtual public CategoryIncomes Category { get; set; } = null!;
        virtual public User User { get; set; } = null!;
    }
}
