using System;

namespace Savaglow.Models.Ledger
{
    public class RecurringLedgerItem
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public int RecurringFrequency { get; set; }
        public DateTime RecurringStartDate { get; set; }
        public DateTime? RecurringLastModified { get; set; } = null;
        public User User { get; set; }
        public string UserId { get; set; }

    }
}