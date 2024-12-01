using System;
using ATM.Utils;

namespace ATM.Operations
{
    class CheckBalance : IATMOperations
    {
        public Guid TransactionId { get; private set; }
        public DateTime Timestamp { get; private set; }

        public CheckBalance()
        {
            TransactionId = Guid.NewGuid();
            Timestamp = DateTime.Now;
        }

        public void Execute(ATMContext context)
        {
            Console.WriteLine($"Your current balance is: {context.CurrentUser.Balance:C}");

            context.CurrentUser.FinancialOperations.Reports["CheckBalance"].
                Add($"ID({TransactionId}) : You have {context.CurrentUser.Balance:C} in your account, on {Timestamp}");
        }
    }

}
