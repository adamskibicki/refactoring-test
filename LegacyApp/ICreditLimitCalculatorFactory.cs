namespace LegacyApp
{
    public interface ICreditLimitCalculatorFactory
    {
        ICreditLimitCalculator GetCalculator(string clientName);
    }
}