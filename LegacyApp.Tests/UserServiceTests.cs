using FluentAssertions;
using LegacyApp.Repositories;
using Moq;

namespace LegacyApp.Tests;

public class UserServiceTests
{
    private readonly IUserService _userService;
    private readonly Mock<IClientRepository> _clientRepository;
    private readonly Mock<IUserCreditService> _userCreditService;
    private readonly Mock<IUserDataAccess> _userDataAccess;
    private readonly Mock<IDateTimeProvider> _dateTimeProvider;

    public UserServiceTests()
    {
        _clientRepository = new Mock<IClientRepository>();
        _userCreditService = new Mock<IUserCreditService>();
        _userDataAccess = new Mock<IUserDataAccess>();
        _dateTimeProvider = new Mock<IDateTimeProvider>();
        _userService = new UserService(_clientRepository.Object, _userDataAccess.Object,
            _dateTimeProvider.Object, new CreditLimitCalculatorFactory(_userCreditService.Object));
    }
    
    public static IEnumerable<object[]> ValidData => new List<object[]>()
    {
        new object[] { "VeryImportantClient", "John", "Doe", 0, "john@t.t", new DateTime(1997, 9, 12), new DateTime(2023, 1, 1) },
        new object[] { "VeryImportantClient", "John", "Doe", 0, "john@t.t", new DateTime(1997, 9, 12), new DateTime(1997 + 21, 9, 12) },
        new object[] { "ImportantClient", "John", "Doe", 250, "john@t.t", new DateTime(1997, 9, 12), new DateTime(2023, 1, 1) },
        new object[] { "", "John", "Doe", 500, "john@t.t", new DateTime(1997, 9, 12), new DateTime(2023, 1, 1) },
    };

    [Theory]
    [MemberData(nameof(ValidData))]
    public void AddUser_ShouldCreateUser_WhenValidDataIsProvided(string clientName, string firstName,
        string surname, int creditLimit, string emailAddress, DateTime dateOfBirth, DateTime now)
    {
        // Arrange
        var client = new Client()
        {
            ClientStatus = ClientStatus.none,
            Name = clientName,
            Id = 1
        };
        var user = new User()
        {
            Firstname = firstName,
            Surname = surname,
            EmailAddress = emailAddress,
            DateOfBirth = dateOfBirth
        };
        _userDataAccess.Setup(x => x.AddUser(user));
        _dateTimeProvider.Setup(x => x.Now()).Returns(now);
        _clientRepository.Setup(x => x.GetById(client.Id)).Returns(client);
        _userCreditService.Setup(x => x.GetCreditLimit(user.Firstname, user.Surname, user.DateOfBirth))
            .Returns(creditLimit);

        // Act
        var result = _userService.AddUser(user.Firstname, user.Surname, user.EmailAddress, user.DateOfBirth, client.Id);
        // Assert
        result.Should().BeTrue();
    }

    public static IEnumerable<object[]> InvalidData => new List<object[]>()
    {
        new object[] { "VeryImportantClient", "", "Doe", 0, "john@t.t", new DateTime(1997, 9, 12), new DateTime(2023, 1, 1) },
        new object[] { "VeryImportantClient", null, "Doe", 0, "john@t.t", new DateTime(1997, 9, 12), new DateTime(2023, 1, 1) },
        new object[] { "VeryImportantClient", "John", "", 0, "john@t.t", new DateTime(1997, 9, 12), new DateTime(2023, 1, 1) },
        new object[] { "VeryImportantClient", "John", null, 0, "john@t.t", new DateTime(1997, 9, 12), new DateTime(2023, 1, 1) },
        new object[] { "VeryImportantClient", "John", "Doe", 0, "john@t.t", new DateTime(1997, 9, 12), new DateTime(1997 + 21, 9 - 1, 12) },
        new object[] { "VeryImportantClient", "John", "Doe", 0, "john@t.t", new DateTime(1997, 9, 12), new DateTime(1997 + 21, 9, 12 - 1) },
        new object[] { "VeryImportantClient", "John", "Doe", 0, "john@t.t", new DateTime(1997, 9, 12), new DateTime(1997 + 21 - 1, 9, 12) },
        new object[] { "ImportantClient", "John", "Doe", 249, "john@t.t", new DateTime(1997, 9, 12), new DateTime(2023, 1, 1) },
        new object[] { "", "John", "Doe", 499, "john@t.t", new DateTime(1997, 9, 12), new DateTime(2023, 1, 1) },
    };
    
    [Theory]
    [MemberData(nameof(InvalidData))]
    public void AddUser_ShouldNotCreateUser_WhenDataProvidedIsInvalidTheory(string clientName, string firstName,
        string surname, int creditLimit, string emailAddress, DateTime dateOfBirth, DateTime now)
    {
        // Arrange
        var client = new Client()
        {
            ClientStatus = ClientStatus.none,
            Name = clientName,
            Id = 1
        };
        var user = new User()
        {
            Firstname = firstName,
            Surname = surname,
            EmailAddress = emailAddress,
            DateOfBirth = dateOfBirth
        };
        _userDataAccess.Setup(x => x.AddUser(user));
        _dateTimeProvider.Setup(x => x.Now()).Returns(now);
        _clientRepository.Setup(x => x.GetById(client.Id)).Returns(client);
        _userCreditService.Setup(x => x.GetCreditLimit(user.Firstname, user.Surname, user.DateOfBirth))
            .Returns(creditLimit);

        // Act
        var result = _userService.AddUser(user.Firstname, user.Surname, user.EmailAddress, user.DateOfBirth, client.Id);
        // Assert
        result.Should().BeFalse();
    }
}