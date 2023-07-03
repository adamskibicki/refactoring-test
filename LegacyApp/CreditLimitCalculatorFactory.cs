using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LegacyApp
{
    public class CreditLimitCalculatorFactory : ICreditLimitCalculatorFactory
    {
        private readonly IDictionary<string, ICreditLimitCalculator> _calculatorsDictionary;
        private readonly IUserCreditService _userCreditService;

        public CreditLimitCalculatorFactory(IUserCreditService userCreditService)
        {
            _userCreditService = userCreditService;

            var calculatorInterfaceType = typeof(ICreditLimitCalculator);
            var calculatorTypes = calculatorInterfaceType.Assembly.GetTypes()
                .Where(x =>
                    x.IsAssignableFrom(calculatorInterfaceType) && x.IsAbstract == false && x.IsInterface == false)
                .ToArray();

            var instances = calculatorTypes.Select(x =>
                (ICreditLimitCalculator)Activator.CreateInstance(x, BindingFlags.Public | BindingFlags.Instance, null,
                    _userCreditService));

            _calculatorsDictionary = instances.ToDictionary(x => x.ClientName, x => x);
        }

        public ICreditLimitCalculator GetCalculator(string clientName)
        {
            return _calculatorsDictionary[clientName];
        }
    }
}