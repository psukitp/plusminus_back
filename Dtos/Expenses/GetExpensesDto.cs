namespace plusminus.Dtos.Expenses
{
    public class GetExpensesDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public int CategoryId { get; set; }
        public int Amount { get; set; }
    }
}
