using System;

namespace LegacyApp.CreditLimitCalculators
{
    public interface ICreditLimitCalculator
    {
        string ClientName { get; }
        (bool hasCreditLimit, int creditLimit) Calculate(string firstname, string surname, DateTime dateOfBirth);
    }
}