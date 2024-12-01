using System;
using ATM.Utils;
using ATM.FileStorage;

namespace ATM.Operations
{
    class Deposit : ITransaction
    {
        public Guid TransactionId { get; private set; }
        public DateTime Timestamp { get; private set; }

        public Deposit() 
        {
            TransactionId = Guid.NewGuid();
            Timestamp = DateTime.Now;
        }

        public void Execute(ATMContext context)
        {
            if (!decimal.TryParse(Validation.UserInput("Enter amount to Deposit: "), out decimal amount) || amount <= 0)
                Console.WriteLine("Invalid amount. Please enter a positive number.");

            context.CurrentUser.Balance += amount;
            Console.WriteLine($"You have deposited: {amount:C}");
            context.CurrentUser.FinancialOperations.Reports["Deposit"].Add($"ID({TransactionId}) : You deposited {amount:C} on {Timestamp}");

            FileStorageService.SaveToFile(FileStorageService.ATMFilePath, context.Users);

        }
    }

}
