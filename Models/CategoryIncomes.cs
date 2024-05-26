namespace plusminus.Models
{
    public class CategoryIncomes
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string Color { get; set; } = string.Empty;

        virtual public ICollection<Incomes> Incomes { get; set; } = null!;
        virtual public User User { get; set; } = null!;
    }
}
