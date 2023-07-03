using System;

namespace LegacyApp
{
    public class ImportantClientCreditLimitCalculator : ICreditLimitCalculator
    {
        private readonly IUserCreditService _userCreditService;

        public ImportantClientCreditLimitCalculator(IUserCreditService userCreditService)
        {
            _userCreditService = userCreditService;
        }

        public string ClientName { get; } = "ImportantClient";
        public (bool hasCreditLimit, int creditLimit) Calculate(string firstname, string surname, DateTime dateOfBirth)
        {
            return (true, 2 * _userCreditService.GetCreditLimit(firstname, surname, dateOfBirth));
        }
    }
}