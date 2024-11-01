using System;
using System.Collections.Generic;

namespace ATM_App
{
   class ATM
   {
        public List<User> users = new List<User>()
        { new User("kareem", "123", 1000), new User("ali", "456", 2000), new User("amr", "789", 3000) };

        public void Start()
        {
            StartDetails();

            string userChoice = Validation.UserInput("Enter your choice: ");

            while (true)
            {
                switch (userChoice)
                {
                    case "1":
                        Login();
                        break;

                    case "2":
                        CreateNewUser();
                        break;

                    case "3":
                        Console.WriteLine("--- Thank you for using the ATM System  ---");
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        public void showMenu(ATMContext context)
        {
            IATMOperations[] atmOperations = new IATMOperations[5]
            {new CheckBalance(), new Deposit(), new Withdraw(), new DisplayReports(), new DeleteAccount()};

            bool logout = false;
            while (!logout)
            {
                AccountDetails(context);

                string choice = Validation.UserInput("Enter your choice: ");
                switch (choice)
                {
                    case "1":
                        atmOperations[0].Execute(context);
                        break;
                    case "2":
                        atmOperations[1].Execute(context);
                        break;
                    case "3":
                        atmOperations[2].Execute(context);
                        break;
                    case "4":
                        atmOperations[3].Execute(context);
                        break;
                    case "5":
                        atmOperations[4].Execute(context);
                        break;
                    case "6":
                        logout = true;
                        Console.WriteLine("Logout successful.");
                        Console.WriteLine("\n===================================\n");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
            Start();
        }

        private void Login()
        {
            string enteredUsername = Validation.UserInput("Enter your username: ");
            string enteredPassword = Validation.UserInput("Enter your password: ");
            User user = User.Authenticated(users, enteredUsername, enteredPassword);
            if (user != null)
            {
                ATMContext context = new ATMContext(this, users, user);
                showMenu(context);
            }
            else
            {
                Console.WriteLine("Invalid credentials. Exiting...");
                Console.WriteLine("\n===================================\n");
                Start();
            }
        }

        private void CreateNewUser()
        {
            string enteredUsername = Validation.UserInput("Enter your username: ");
            if (users.Any(user => user.Username == enteredUsername))
            {
                Console.WriteLine("Username already exists. Please try again.");
                return;
            }

            string enteredPassword = Validation.UserInput("Enter your password: ");

            decimal enteredBalance;
            while (!decimal.TryParse(Validation.UserInput("Enter your balance: "), out enteredBalance) || enteredBalance <= 0)
            {
                Console.WriteLine("Invalid balance. Please enter a Positive number.");
            }

            User newUser = new User(enteredUsername, enteredPassword, enteredBalance);
            users.Add(newUser);

            Console.WriteLine($"User {newUser.Username} created successfully.");
            Console.WriteLine("===================================\n");
            Console.WriteLine("You can Login to your account now.");
            Login();
        }

        public void StartDetails()
        {
            Console.WriteLine("---  Welcome to the ATM System  ---");
            Console.WriteLine("Please select a choice: ");
            Console.WriteLine("(1) Login to your account");
            Console.WriteLine("(2) Create a new User");
            Console.WriteLine("(3) Exit ATM system");
        }

        public void AccountDetails(ATMContext context)
        {
            Console.WriteLine("\n===================================\n");
            Console.WriteLine($"Welcome {context.CurrentUser.Username}\n");
            Console.WriteLine("Select an option:");
            Console.WriteLine("1. Check Balance");
            Console.WriteLine("2. Deposit");
            Console.WriteLine("3. Withdraw");
            Console.WriteLine("4. Display Reports");
            Console.WriteLine("5. Delete Account");
            Console.WriteLine("6. Logout");
        }
    }
}
