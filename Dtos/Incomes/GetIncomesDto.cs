namespace plusminus.Dtos.Incomes
{
    public class GetIncomesDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateOnly Date { get; set; }
        public string CategoryName { get; set; }
        public int Amount { get; set; }
    }
}
