using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Savaglow.Data.Interfaces;
using Savaglow.Models.Ledger;
using Microsoft.EntityFrameworkCore;
using System;
using savaglow_backend.Helpers;

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

        public async Task<IEnumerable<LedgerItem>> GetLedgerForUser(string userId, DateTime? date)
        {
            if (date != null)
            {
                DateTime dateToMatch = (DateTime)date;
                DateTime endOfMonth = dateToMatch.AddMonths(1);
                return await _context.LedgerItems.Where(i => i.UserId == userId).Where(x => x.TransactionDate >= dateToMatch && x.TransactionDate <= endOfMonth).ToListAsync();
            }
            else
            {
                var firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var lastDayOfMonth = firstDay.AddMonths(1).AddDays(-1);
                return await _context.LedgerItems.Where(i => i.UserId == userId && (i.TransactionDate >= firstDay && i.TransactionDate <= lastDayOfMonth)).ToListAsync();
            }
        }

        public async Task<IEnumerable<RecurringLedgerItem>> GetRecurringLedgerForUser(string userId, DateTime? date)
        {

            if (date != null)
            {
                DateTime dateToMatch = (DateTime)date;
                DateTime endOfMonth = dateToMatch.AddMonths(1);

                var recurringPosted = await _context.RecurringLedgerItems.Where(x => x.RecurringLastModified == null ?
                x.RecurringStartDate >= dateToMatch && x.RecurringStartDate <= endOfMonth
                : x.RecurringLastModified >= dateToMatch && x.RecurringLastModified <= endOfMonth)
                .ToListAsync();

                var recurringNotPosted = await _context.RecurringLedgerItems.Where(x => x.RecurringLastModified.Value.AddDays(x.RecurringFrequency) >= dateToMatch
                && x.RecurringLastModified.Value.AddDays(x.RecurringFrequency) <= endOfMonth).ToListAsync();

                var results = recurringPosted.Concat(recurringNotPosted);
                return results;

            }
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