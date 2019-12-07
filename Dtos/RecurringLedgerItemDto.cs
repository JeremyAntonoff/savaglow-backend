using System;

namespace Savaglow.Dtos
{
    public class RecurringLedgerItemDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; }
        public Recurring Recurring { get; set; }
        public string UserId { get; set; }
        
    }
}