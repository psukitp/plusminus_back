namespace plusminus.Dtos.Incomes
{
    public class AddIncomesDto
    {
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public int CategoryId { get; set; }
        public int Amount { get; set; }
    }
}
