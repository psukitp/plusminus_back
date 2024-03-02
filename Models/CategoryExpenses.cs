namespace plusminus.Models
{
    public class CategoryExpenses
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;

        virtual public ICollection<Expenses> Expenses { get; set; } = null!;
    }
}
