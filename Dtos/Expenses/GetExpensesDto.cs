namespace plusminus.Dtos.Expenses
{
    public class GetExpensesDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateOnly Date { get; set; }
        public string CategoryName { get; set; } = null!;
        public int Amount { get; set; }
    }
}
