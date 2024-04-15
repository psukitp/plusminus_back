namespace plusminus.Dtos.Expenses
{
    public class AddExpensesDto
    {
        public DateTime Date { get; set; }
        public int CategoryId { get; set; }
        public int Amount { get; set; }
    }
}
