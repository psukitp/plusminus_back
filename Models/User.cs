namespace plusminus.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SecondName { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        virtual public ICollection<Expenses> Expenses { get; set; } = null!;
        virtual public ICollection<Incomes> Incomes { get; set; } = null!;
    }
}
