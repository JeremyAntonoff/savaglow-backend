using System;
using System.Collections;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Savaglow.Data;

namespace savaglow_backend.Helpers
{
    public class JobsHelper
    {
        IServiceProvider _serviceProvider;
        public JobsHelper(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public Boolean UpdateRecurringLedgerItems()
        {
            using (IServiceScope scope = _serviceProvider.CreateScope())
            using (DataContext ctx = scope.ServiceProvider.GetRequiredService<DataContext>())
            {
                var todaysDate = DateTime.Now;
                var recurringItems = ctx.RecurringLedgerItems;
                foreach (var item in recurringItems)
                {
                    if (item.RecurringLastModified == null)
                    {
                        var nextPostDate = item.RecurringStartDate;
                        if (todaysDate >= nextPostDate)
                        {
                            var totalDaysBetweenDates = (todaysDate - item.RecurringStartDate).TotalDays;
                            var multiple = totalDaysBetweenDates / item.RecurringFrequency;
                            if (multiple >= 1)
                            {
                                item.RecurringLastModified = item.RecurringStartDate.AddDays(item.RecurringFrequency * Math.Floor(multiple));
                            }
                            else
                            {
                                item.RecurringLastModified = item.RecurringStartDate;
                            }
                        }


                    }
                    else if (item.RecurringLastModified.HasValue)
                    {
                        var lastModified = (DateTime)item.RecurringLastModified;
                        var nextPostDate = lastModified.AddDays(item.RecurringFrequency);
                        if (todaysDate >= nextPostDate)
                        {
                            var totalDaysBetweenDates = (todaysDate - lastModified).TotalDays;
                            var multiple = Math.Floor(totalDaysBetweenDates / item.RecurringFrequency);
                            item.RecurringLastModified = item.RecurringLastModified.Value.AddDays(item.RecurringFrequency * multiple);
                        }
                    }
                }
                var savedChanges = ctx.SaveChanges();
                if (savedChanges > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
