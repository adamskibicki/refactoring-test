using System;

namespace LegacyApp
{
    public class NormalClientCreditLimitCalculator : ICreditLimitCalculator
    {
        private readonly IUserCreditService _userCreditService;

        public NormalClientCreditLimitCalculator(IUserCreditService userCreditService)
        {
            _userCreditService = userCreditService;
        }

        public string ClientName { get; } = "";
        public (bool hasCreditLimit, int creditLimit) Calculate(string firstname, string surname, DateTime dateOfBirth)
        {
            return (true, _userCreditService.GetCreditLimit(firstname, surname, dateOfBirth));
        }
    }
}