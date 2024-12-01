using System;
using ATM.Utils;

namespace ATM.Operations
{
    public class FinancialReport : IATMOperations
    {
        public Dictionary<string, List<string>> Reports { get; set; }

        public FinancialReport()
        {
            Reports = new Dictionary<string, List<string>>()
            {
                { "Deposit", new List<string>() },
                { "Withdraw", new List<string>() },
                { "CheckBalance", new List<string>() },
                { "Transfer", new List<string>() }
            };
        }

        public void Execute(ATMContext context)
        {
            Console.WriteLine("\n-------");
            Console.WriteLine("Your Financial operations: ");

            foreach (var type in context.CurrentUser.FinancialOperations.Reports.Keys)
            {
                Console.WriteLine($"{type}: ");

                List<string> reportOperations = context.CurrentUser.FinancialOperations.Reports[type];
                foreach (var operation in reportOperations)
                {
                    Console.WriteLine($"    - {operation}");
                }
            }
            Console.WriteLine("-------\n");
        }
    }

}
