using System;
using System.Collections.Generic;
using Savaglow.Models.Ledger;
using savaglow_backend.Models.Interfaces;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
namespace savaglow_backend.Helpers
{
    public class ReportHelper
    {
        public static decimal AddLedgerItems(IEnumerable<ILedgerItem> ledgerItems, TransactionType type)
        {
            decimal amount = 0;
            foreach (var item in ledgerItems)
            {
                if (item.TransactionType == type)
                {
                    amount += item.Amount;
                }
            }
            return amount;
        }
        public static Run DisplayItems(IEnumerable<LedgerItem> ledgerItems, TransactionType type)
        {
            Run run = new Run();
            foreach (var item in ledgerItems)
            {
                if (item.TransactionType == type)
                {
                    run.AppendChild(new Text($"Date: {item.CreatedAt.ToString("MM/dd/yyyy")} Amount: {item.Amount.ToString("0.##")}"));
                    run.AppendChild(new Break());
                    run.AppendChild(new Text($"Description: {item.Description} Category: {item.Category}"));
                    run.AppendChild(new Break());
                    run.AppendChild(new Break());
                }
            }
            return run;
        }
             public static Run DisplayRecurringItems(IEnumerable<RecurringLedgerItem> ledgerItems, TransactionType type)
        {
            Run run = new Run();
            foreach (var item in ledgerItems)
            {
                if (item.TransactionType == type)
                {
                    run.AppendChild(new Text($"Date: {item.CreatedAt.ToString("MM/dd/yyyy")} Amount: {item.Amount.ToString("0.##")}"));
                    run.AppendChild(new Break());
                    run.AppendChild(new Text($"Description: {item.Description} Category: {item.Category}"));
                    run.AppendChild(new Break());
                    run.AppendChild(new Break());
                }
            }
            return run;
        }
        
        public static Boolean IsDateBetween(DateTime startDate, RecurringLedgerItem item)
        {
            DateTime endOfMonth = startDate.AddMonths(1);
            if (item.RecurringLastModified == null)
            {
                return item.RecurringStartDate >= startDate && item.RecurringStartDate <= endOfMonth;
            }
            else
            {
                DateTime recurringLastModified = (DateTime)item.RecurringLastModified;
                if (recurringLastModified >= startDate && recurringLastModified <= endOfMonth)
                {
                    return true;
                }
                else if ((recurringLastModified.AddDays(item.RecurringFrequency) >= startDate && recurringLastModified.AddDays(item.RecurringFrequency) <= endOfMonth))
                {
                    return true;
                }
            }
            return false;
        }
    }
}