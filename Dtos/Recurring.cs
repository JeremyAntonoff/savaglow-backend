using System;

namespace Savaglow.Dtos
{
    public class Recurring
    {
        public int RecurringFrequency { get; set; }
        public DateTime RecurringStartDate { get; set; }
        public DateTime? RecurringLastModified { get; set; }

    }
}