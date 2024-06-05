namespace plusminus.Dtos.Expenses
{
    public class ExpensesByCategory
    {
        public int Id { get; set; }
        public string CategoryName { get; set; } = null!;
        public int Amount { get; set; }
        public string Color { get; set; } = null!;
    }
}
