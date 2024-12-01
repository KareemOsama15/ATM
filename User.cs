using System;
using ATM.Operations;

namespace ATM
{
    enum UserCategory
    {
        Ordinary,
        Vip
    }

    public class User
    {
        public Guid UserId { get; private set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public decimal Balance { get; set; }
        public bool DeleteAccountPossibility { get; set; }
        public ManagerialReport ManagerialOperations { get; set; }
        public FinancialReport FinancialOperations { get; set; }
        public List<Transfer> Transfers { get; private set; }


        public User(string username, string password, string email, decimal balance)
        {
            UserId = Guid.NewGuid();
            Username = username;
            Password = password;
            Email = email;
            Balance = balance;
            DeleteAccountPossibility = false;
            ManagerialOperations = new ManagerialReport();
            FinancialOperations = new FinancialReport();
            Transfers = new List<Transfer>();
        }


        public static User? Authenticated(List<User> users, string enteredUsername, string enteredPassword)
        {
            foreach (User user in users)
            {
                if (user.Username == enteredUsername && user.Password == enteredPassword)
                    return user;

            }
            return null;
        }

        public void DisplayUserInfo()
        {
            Console.WriteLine($"UserId: {this.UserId}");
            Console.WriteLine($"Username: {this.Username}");
            Console.WriteLine($"Email: {this.Email}");
            Console.WriteLine($"Password: {this.Password}");
        }

        public void CheckDeleteAccountPossibility(Transfer transfer)
        {
            if (this.Balance > 0 || this.Transfers.Count > 0 || transfer.Status == TransferStatus.Pending)
                this.DeleteAccountPossibility = false;
            else
                this.DeleteAccountPossibility = true;
        }
    }
}
