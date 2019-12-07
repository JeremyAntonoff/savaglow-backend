using System;
using Savaglow.Models.Ledger;

namespace Savaglow.Dtos
{
    public class LedgerItemDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; }
        public string UserId { get; set; }
    }
}