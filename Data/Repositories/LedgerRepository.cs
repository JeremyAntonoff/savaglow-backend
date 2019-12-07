using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Savaglow.Data.Interfaces;
using Savaglow.Models.Ledger;
using Microsoft.EntityFrameworkCore;

namespace Savaglow.Data.Repositories
{
    public class LedgerRepository : ILedgerRepository
    {
        private readonly DataContext _context;
        public LedgerRepository(DataContext context)
        {
            _context = context;

        }

        public async Task<T> AddLedgerItem<T>(T ledgerItem)
        {
            await _context.AddAsync(ledgerItem);
            return ledgerItem;
        }

        public void AddReoccuringLedgerItem()
        {
            throw new System.NotImplementedException();
        }

        public void GetLedgerForUser(int userId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<LedgerItem> GetLedgerItem(int id)
        {
            var ledger = await _context.LedgerItems.FirstOrDefaultAsync(i => i.Id == id);
            return ledger;
        }

        public async Task<RecurringLedgerItem> GetRecurringLedgerItem(int id)
        {
            var recurringLedgerItem = await _context.RecurringLedgerItems.FirstOrDefaultAsync(i => i.Id == id);
            return recurringLedgerItem;
        }

        public async Task<IEnumerable<LedgerItem>> GetLedgerForUser(string userId)
        {
            var ledger = await _context.LedgerItems.Where(i => i.UserId == userId).ToListAsync();
            return ledger;
        }

        public async Task<IEnumerable<RecurringLedgerItem>> GetRecurringLedgerForUser(string userId)
        {
            var recurringLedger = await _context.RecurringLedgerItems.Where(i => i.UserId == userId).ToListAsync();
            return recurringLedger;
        }
        public async Task<bool> Save()
        {
            var changes = await _context.SaveChangesAsync();
            return changes > 0;
        }
    }
}