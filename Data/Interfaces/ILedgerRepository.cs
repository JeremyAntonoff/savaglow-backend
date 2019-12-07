using System.Collections.Generic;
using System.Threading.Tasks;
using Savaglow.Models.Ledger;

namespace Savaglow.Data.Interfaces
{
    public interface ILedgerRepository
    {
        Task<LedgerItem> GetLedgerItem(int id);
        Task<RecurringLedgerItem> GetRecurringLedgerItem(int id);
        Task<IEnumerable<LedgerItem>> GetLedgerForUser(string userId);
        Task<IEnumerable<RecurringLedgerItem>> GetRecurringLedgerForUser(string userId);
        Task<T> AddLedgerItem<T>(T ledgerItem);
        void AddReoccuringLedgerItem();
        Task<bool> Save();

    }
}