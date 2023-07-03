using LegacyApp.Models;

namespace LegacyApp.DataAccess
{
    public class UserDataAccessWrapper : IUserDataAccess
    {
        public void AddUser(User user)
        {
            UserDataAccess.AddUser(user);
        }
    }
}