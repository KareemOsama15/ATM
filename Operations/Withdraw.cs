using System;
using ATM.Utils;
using ATM.FileStorage;

namespace ATM.Operations
{
    class Withdraw : IATMOperations
    {
        public Guid TransactionId { get; private set; }
        public DateTime Timestamp { get; private set; }

        public Withdraw() 
        {
            TransactionId = Guid.NewGuid();
            Timestamp = DateTime.Now;
        }

        public void Execute(ATMContext context)
        {
            if (!decimal.TryParse(Validation.UserInput("Enter amount to Withdraw: "), out decimal amount) || amount <= 0)
                Console.WriteLine("Invalid amount. Please enter a positive number.");

            else if (amount > 0 && amount <= context.CurrentUser.Balance)
            {
                context.CurrentUser.Balance -= amount;
                Console.WriteLine($"You have withdrawn: {amount:C}");
                context.CurrentUser.FinancialOperations.Reports["Withdraw"].Add($"ID({TransactionId}) : You withdrew {amount:C} on {Timestamp}");

                FileStorageService.SaveToFile(FileStorageService.ATMFilePath, context.Users);
            }
            else if (amount > context.CurrentUser.Balance)
                Console.WriteLine("Your balance is not enough.");
        }
    }

}
