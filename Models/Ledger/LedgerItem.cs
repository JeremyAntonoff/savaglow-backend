using System;
using savaglow_backend.Models.Interfaces;

namespace Savaglow.Models.Ledger
{
    public class LedgerItem : ILedgerItem
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }

    }
    public enum TransactionType { INCOME, EXPENSE }

}