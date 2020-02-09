using Savaglow.Models.Ledger;

namespace savaglow_backend.Models.Interfaces
{
    public interface ILedgerItem
    {
        TransactionType TransactionType {get; set;}
        decimal Amount {get; set;}
         
    }
}