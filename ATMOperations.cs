using System;


namespace ATM_App
{
    
    internal interface IATMOperations
    {

        void Execute(ATMContext context);
    }

    class CheckBalance : IATMOperations
    {
        public void Execute(ATMContext context)
        {
            Console.WriteLine($"Your current balance is: {context.CurrentUser.Balance:C}");
            context.CurrentUser.Reports["CheckBalance"].Add($"You have {context.CurrentUser.Balance:C} in your account, on {DateTime.Now}");
        }
    }

    class Deposit : IATMOperations
    {
        public void Execute(ATMContext context)
        {
            decimal amount;

            if (!decimal.TryParse(Validation.UserInput("Enter amount to deposit: "), out amount) || amount <= 0)
            {
                Console.WriteLine("Invalid amount. Please enter a positive number.");
            }
            else
            {
                context.CurrentUser.Balance += amount;
                Console.WriteLine($"You have deposited: {amount:C}");
                context.CurrentUser.Reports["Deposit"].Add($"You deposited {amount:C} on {DateTime.Now}");
            }
        }
    }

    class Withdraw : IATMOperations
    {
        public void Execute(ATMContext context)
        {
            decimal amount;

            if (!decimal.TryParse(Validation.UserInput("Enter amount to withdraw: "), out amount) || amount <= 0)
                Console.WriteLine("Invalid amount. Please enter a positive number.");

            else if (amount > 0 && amount <= context.CurrentUser.Balance)
            {
                context.CurrentUser.Balance -= amount;
                Console.WriteLine($"You have withdrawn: {amount:C}");
                context.CurrentUser.Reports["Withdraw"].Add($"You withdrew {amount:C} on {DateTime.Now}");
            }

            else if (amount > context.CurrentUser.Balance)
                Console.WriteLine("Your balance is not enough.");
        }
    }

    class DisplayReports : IATMOperations
    {
        public void Execute(ATMContext context)
        {
            Console.WriteLine("\n-------");
            Console.WriteLine("Your reports: ");
            foreach (var reportType in context.CurrentUser.Reports.Keys)
            {
                Console.WriteLine($"{reportType}: ");
                foreach (var operation in context.CurrentUser.Reports[reportType])
                {
                    Console.WriteLine($"    - {operation}");
                }
            }
            Console.WriteLine("-------\n");
        }
    }

    class DeleteAccount : IATMOperations
    {
        public void Execute(ATMContext context)
        {
            Console.WriteLine("Are you sure you want to delete your account? (Y/N)");

            string choice = Validation.UserInput("Enter your choice: ").ToUpper();

            if (choice == "Y")
            {
                context.Users.Remove(context.CurrentUser);
                Console.WriteLine("Your account has been deleted.");
                Console.WriteLine("\n===================================\n");
                context.AtmInstance.Start();
            }
            else
            {
                Console.WriteLine("Process Failed..");
                context.AtmInstance.showMenu(context);
            }
        }
    }
}