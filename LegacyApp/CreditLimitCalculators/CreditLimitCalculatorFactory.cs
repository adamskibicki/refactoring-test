using System;
using System.Collections.Generic;
using System.Linq;
using LegacyApp.Services;

namespace LegacyApp.CreditLimitCalculators
{
    public class CreditLimitCalculatorFactory : ICreditLimitCalculatorFactory
    {
        private readonly IDictionary<string, ICreditLimitCalculator> _calculatorsDictionary;

        public CreditLimitCalculatorFactory(IUserCreditService userCreditService)
        {
            var calculatorInterfaceType = typeof(ICreditLimitCalculator);
            var calculatorTypes = calculatorInterfaceType.Assembly.GetTypes()
                .Where(x =>
                    calculatorInterfaceType.IsAssignableFrom(x) && x.IsAbstract == false && x.IsInterface == false)
                .ToArray();
            
            var instances = calculatorTypes.Select(x =>
                (ICreditLimitCalculator)Activator.CreateInstance(x, args: userCreditService));

            _calculatorsDictionary = instances.ToDictionary(x => x.ClientName, x => x);
        }

        public ICreditLimitCalculator GetCalculator(string clientName)
        {
            return _calculatorsDictionary[clientName];
        }
    }
}