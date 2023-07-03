namespace LegacyApp.CreditLimitCalculators
{
    public interface ICreditLimitCalculatorFactory
    {
        ICreditLimitCalculator GetCalculator(string clientName);
    }
}