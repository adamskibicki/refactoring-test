using System;

namespace LegacyApp
{
    public interface IDateTimeProvider
    {
        public DateTime Now();
    }
}