namespace DAL.Entities
{
    public class Transaction
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public double Amount { get; set; }

        public TransactionType Type { get; set; }

        public DateTime Date { get; set; }
    }
}

