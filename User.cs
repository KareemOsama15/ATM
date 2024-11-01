using System;

namespace ATM_App
{
    internal class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public decimal Balance { get; set; }
        public Dictionary<string, List<string>> Reports { get; set; }

        public User(string username, string password, decimal balance)
        { 
            Username = username;
            Password = password;
            Balance = balance;
            Reports = new Dictionary<string, List<string>>()
            {
                { "Deposit", new List<string>() },
                { "Withdraw", new List<string>() },
                { "CheckBalance", new List<string>() }
            };
        }
        public static User Authenticated(List<User> users, string enteredUsername, string enteredPassword)
        {
            foreach (User user in users)
            {
                if (user.Username == enteredUsername && user.Password == enteredPassword)
                    return user;

            }
            return null;
        }
    }
}
