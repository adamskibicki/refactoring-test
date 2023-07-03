using System;
using LegacyApp.CreditLimitCalculators;
using LegacyApp.DataAccess;
using LegacyApp.Models;
using LegacyApp.Repositories;
using LegacyApp.Services;

namespace LegacyApp
{
    public class UserService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUserDataAccess _userDataAccess;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ICreditLimitCalculatorFactory _creditLimitCalculatorFactory;

        public UserService(IClientRepository clientRepository,
            IUserDataAccess userDataAccess, IDateTimeProvider dateTimeProvider,
            ICreditLimitCalculatorFactory creditLimitCalculatorFactory)
        {
            _clientRepository = clientRepository;
            _userDataAccess = userDataAccess;
            _dateTimeProvider = dateTimeProvider;
            _creditLimitCalculatorFactory = creditLimitCalculatorFactory;
        }

        public UserService() : this(new ClientRepository(), new UserDataAccessWrapper(),
            new DateTimeProvider(), new CreditLimitCalculatorFactory(new UserCreditServiceClient()))
        {
        }

        public bool AddUser(string firname, string surname, string email, DateTime dateOfBirth, int clientId)
        {
            if (!IsNameValid(firname, surname))
            {
                return false;
            }

            if (!IsEmailValid(email))
            {
                return false;
            }

            var age = CalculateAge(dateOfBirth);

            if (!IsAgeValid(age))
            {
                return false;
            }

            var clientRepository = _clientRepository;
            var client = clientRepository.GetById(clientId);

            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                Firstname = firname,
                Surname = surname
            };

            var (hasCreditLimit, creditLimit) = _creditLimitCalculatorFactory.GetCalculator(client.Name)
                .Calculate(user.Firstname, user.Surname, user.DateOfBirth);

            user.CreditLimit = creditLimit;
            user.HasCreditLimit = hasCreditLimit;

            if (!IsCreditLimitValid(user))
            {
                return false;
            }

            _userDataAccess.AddUser(user);

            return true;
        }

        private static bool IsCreditLimitValid(User user)
        {
            return !user.HasCreditLimit || user.CreditLimit >= 500;
        }

        private int CalculateAge(DateTime dateOfBirth)
        {
            var now = _dateTimeProvider.Now();
            int age = now.Year - dateOfBirth.Year;

            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day))
            {
                age--;
            }

            return age;
        }

        private static bool IsAgeValid(int age)
        {
            return age >= 21;
        }

        private static bool IsEmailValid(string email)
        {
            return !email.Contains("@") || email.Contains(".");
        }

        private static bool IsNameValid(string firname, string surname)
        {
            return !string.IsNullOrEmpty(firname) && !string.IsNullOrEmpty(surname);
        }
    }
}