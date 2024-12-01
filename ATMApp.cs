using ATM.Operations;
using ATM.Utils;
using ATM.FileStorage;
using System;

namespace ATM
{
    public class ATMApp
    {
        public List<User> users = FileStorageService.LoadFromFile<User>(FileStorageService.ATMFilePath);

        public void Start()
        {
            StartDetails();

            string userChoice = Validation.UserInput("-->Enter your choice: ");

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
                        FileStorageService.SaveToFile(FileStorageService.ATMFilePath, users);
                        Console.WriteLine("--- Thank you for using the ATM System  ---");
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        public void ShowMenu(ATMContext context)
        {

            IATMOperations[] atmOperations = new IATMOperations[4]
            {new CheckBalance(), new Deposit(), new Withdraw(), new DeleteAccount()};

            Transfer transferOperation;
            bool logout = false;

            while (!logout)
            {
                AccountDetails(context);

                string choice = Validation.UserInput("-->Enter your choice: ");
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
                        context.CurrentUser.FinancialOperations.Execute(context);
                        break;

                    case "5":
                        context.CurrentUser.ManagerialOperations.Execute(context);
                        break;

                    case "6":
                        transferOperation = new Transfer();
                        transferOperation.Execute(context);
                        break;

                    case "7":
                        context.CurrentUser.DisplayUserInfo();
                        break;

                    case "8":
                        atmOperations[3].Execute(context);
                        break;

                    case "9":
                        logout = true;
                        context.CurrentUser.ManagerialOperations.Reports["Logout"].Add($"You Logged out from your account on {DateTime.Now}");
                        FileStorageService.SaveToFile(FileStorageService.ATMFilePath, users);

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
                user.ManagerialOperations.Reports["Login"].Add($"You logged in to your account on {DateTime.Now}");

                FileStorageService.SaveToFile(FileStorageService.ATMFilePath, users);
                ShowMenu(context);
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
            string enteredUsername = Validation.UserInput("Enter your Username: ");
            if (users.Any(user => user.Username == enteredUsername))
            {
                Console.WriteLine("Username already exists. Please try again.");
                return;
            }

            string enteredPassword = Validation.UserInput("Enter your password: ");

            string enteredEmail = Validation.UserInput("Enter your Email: ");
            if (users.Any(user => user.Email == enteredEmail))
            {
                Console.WriteLine("Email already exists. Please try again.");
                return;
            }

            decimal enteredBalance;
            while (!decimal.TryParse(Validation.UserInput("Enter your balance: "), out enteredBalance) || enteredBalance <= 0)
            {
                Console.WriteLine("Invalid balance. Please enter a Positive number.");
            }

            User newUser = new User(enteredUsername, enteredPassword, enteredEmail, enteredBalance);
            users.Add(newUser);
            newUser.ManagerialOperations.Reports["Account Creation"].Add($"Your account created on {DateTime.Now}");

            FileStorageService.SaveToFile(FileStorageService.ATMFilePath, users);

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
            Console.WriteLine("4. Financial Reports");
            Console.WriteLine("5. Managerial Reports");
            Console.WriteLine("6. Transfer or Receive Money");
            Console.WriteLine("7. Display User Info");
            Console.WriteLine("8. Delete Account");
            Console.WriteLine("9. Logout");
        }
    }
}

