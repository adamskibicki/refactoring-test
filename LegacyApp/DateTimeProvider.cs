using System;

namespace LegacyApp
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now()
        {
            return DateTime.Now;
        }
    }
}