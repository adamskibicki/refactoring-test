using System;

namespace LegacyApp
{
    public class VeryImportantClientCreditLimitCalculator : ICreditLimitCalculator
    {
        private readonly IUserCreditService _userCreditService;

        public VeryImportantClientCreditLimitCalculator(IUserCreditService userCreditService)
        {
            _userCreditService = userCreditService;
        }
        
        public string ClientName { get; } = "VeryImportantClient";
        public (bool hasCreditLimit, int creditLimit) Calculate(string firstname, string surname, DateTime dateOfBirth)
        {
            return (false, 0);
        }
    }
}