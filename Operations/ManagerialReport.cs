using System;
using ATM.Utils;

namespace ATM.Operations
{

    public class ManagerialReport : IATMOperations
    {
        public Dictionary<string, List<string>> Reports { get; set; }

        public ManagerialReport()
        {
            Reports = new Dictionary<string, List<string>>()
            {
                { "Account Creation", new List<string>() },
                { "Login", new List<string>() },
                { "Logout", new List<string>() }
            };
        }

        public void Execute(ATMContext context)
        {
            Console.WriteLine("\n-------");
            Console.WriteLine("Your Managerial operations: ");

            foreach (var type in context.CurrentUser.ManagerialOperations.Reports.Keys)
            {
                Console.WriteLine($"{type}: ");

                List<string> reportOperations = context.CurrentUser.ManagerialOperations.Reports[type];
                foreach (var operation in reportOperations)
                {
                    Console.WriteLine($"    - {operation}");
                }
            }
            Console.WriteLine("-------\n");
        }
    }

}
