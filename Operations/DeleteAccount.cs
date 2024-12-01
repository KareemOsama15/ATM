using System;
using ATM.Utils;
using ATM.FileStorage;

namespace ATM.Operations
{
    class DeleteAccount : IATMOperations
    {
        public void Execute(ATMContext context)
        {
            Console.WriteLine("Are you sure you want to delete your account? (Y/N)");

            string choice = Validation.UserInput("Enter your choice: ").ToUpper();

            if (choice == "Y")
            {
                if (context.CurrentUser.DeleteAccountPossibility)
                {
                    context.Users.Remove(context.CurrentUser);
                    FileStorageService.SaveToFile(FileStorageService.ATMFilePath, context.Users);

                    Console.WriteLine("Your account has been deleted.");
                    Console.WriteLine("\n===================================\n");
                    context.AtmInstance.Start();
                }

                Console.WriteLine("Process Failed You cannot delete an account with balance Or have pending transfers.");
                context.AtmInstance.ShowMenu(context);
            }
            else
            {
                Console.WriteLine("Process Failed..");
                context.AtmInstance.ShowMenu(context);
            }
        }

    }

}
