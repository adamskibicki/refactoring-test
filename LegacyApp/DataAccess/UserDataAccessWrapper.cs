namespace LegacyApp
{
    public class UserDataAccessWrapper : IUserDataAccess
    {
        public void AddUser(User user)
        {
            UserDataAccess.AddUser(user);
        }
    }
}