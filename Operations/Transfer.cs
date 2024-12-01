using System;
using ATM.Utils;

namespace ATM.Operations
{
    public enum TransferStatus
    {
        Pending,
        Completed,
        Rejected
    }

    public class Transfer : ITransaction
    {
        public Guid TransactionId { get; private set; }
        public DateTime Timestamp { get; private set; }
        public User? Sender { get; set; }
        public User? Receiver { get; set; }
        public decimal Amount { get; set; }
        public TransferStatus Status { get; set; }
        

        public Transfer()
        {
            TransactionId = Guid.NewGuid();
            Timestamp = DateTime.Now;
        }

        public Transfer(User sender, User receiver, decimal amount)
        {
            TransactionId = Guid.NewGuid();
            Timestamp = DateTime.Now;
            Sender = sender;
            Receiver = receiver;
            Amount = amount;
        }

        public void Execute(ATMContext context)
        {
            Console.WriteLine("1. Transfer Money\n2. Recieve Money");
            string choice = Validation.UserInput("Enter your choice: ");
            switch (choice)
            {
                case "1":
                    TransferMoney(context);
                    break;
                case "2":
                    ReceiveMoney(context);
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }

        public static void TransferMoney(ATMContext context)
        {
            if (context.CurrentUser.Balance > 0)
            {
                if (!Guid.TryParse(Validation.UserInput("Enter user's account Id: "), out Guid userAcountId))
                {
                    Console.WriteLine("Invalid account Id.");
                    return;
                }

                foreach (var user in context.Users)
                {
                    if (user.UserId == userAcountId && userAcountId != context.CurrentUser.UserId)
                    {
                        if (!decimal.TryParse(Validation.UserInput("Enter amount to Transfer: "), out decimal amount) || amount <= 0)
                            Console.WriteLine("Invalid amount. Please enter a positive number.");

                        else if (amount <= context.CurrentUser.Balance)
                        {
                            Transfer transfer = new Transfer(context.CurrentUser, user, amount);
                            context.CurrentUser.Balance -= amount;
                            transfer.Status = TransferStatus.Pending;
                            user.Transfers.Add(transfer);

                            Console.WriteLine($"You have transferred: {amount:C} to {user.Username} with Id {user.UserId} on {transfer.Timestamp}\nYour Current Balance is {context.CurrentUser.Balance:C}");
                            context.CurrentUser.FinancialOperations.Reports["Transfer"].Add($"ID({transfer.TransactionId}) : You Transferred {amount:C} to {user.Username} with Id {user.UserId} on {transfer.Timestamp}" +
                                $", Your Current Balance is {context.CurrentUser.Balance:C}");
                        }
                        else Console.WriteLine("Your balance is not enough.");
                    }
                }
                
            }
            else Console.WriteLine("Your balance is ZERO.");
        }


        public static void ReceiveMoney(ATMContext context)
        {
            if (context.CurrentUser.Transfers.Count == 0)
            {
                Console.WriteLine("You have no pending transfers.");
                return;
            }

            var transfersList = context.CurrentUser.Transfers.ToList();

            foreach (var receiveOperation in transfersList)
            {
                Console.WriteLine("\n---------------------\n");

                Console.WriteLine($"ID({receiveOperation.TransactionId}) : You Received {receiveOperation.Amount:C} from {receiveOperation.Sender.Username} with Id {receiveOperation.Sender.UserId} on {receiveOperation.Timestamp}");
                string choice = Validation.UserInput("Do you want to accept it or not?\n1)Accept   2)Reject\nEnter your choice: ");
                switch (choice)
                {
                    case "1":
                        context.CurrentUser.Balance += receiveOperation.Amount;
                        receiveOperation.Status = TransferStatus.Completed;

                        context.CurrentUser.FinancialOperations.Reports["Transfer"].Add($"ID({receiveOperation.TransactionId}) : You Accepted the transfer of {receiveOperation.Amount:C} from {receiveOperation.Sender.Username} with Id {receiveOperation.Sender.UserId} on {receiveOperation.Timestamp}" +
                                $", Your Current Balance is {context.CurrentUser.Balance:C}");
                        Console.WriteLine($"You have accepted the transfer, Your Current Balance is {context.CurrentUser.Balance:C}");
                        break;

                    case "2":
                        receiveOperation.Sender.Balance += receiveOperation.Amount;
                        receiveOperation.Status = TransferStatus.Rejected;

                        context.CurrentUser.FinancialOperations.Reports["Transfer"].Add($"ID({receiveOperation.TransactionId}) : You Rejected the transfer of {receiveOperation.Amount:C} from {receiveOperation.Sender.Username} with Id {receiveOperation.Sender.UserId} on {receiveOperation.Timestamp}" +
                                $", Your Current Balance is {context.CurrentUser.Balance:C}");
                        receiveOperation.Sender.FinancialOperations.Reports["Transfer"].Add($"ID({receiveOperation.TransactionId}) : Your request to transfer {receiveOperation.Amount:C} to {context.CurrentUser.Username} with Id {context.CurrentUser.UserId} on {DateTime.Now} is rejected" +
                                $", You got the money back and your Current Balance is {receiveOperation.Sender.Balance:C}");

                        Console.WriteLine($"You have rejected the transfer, Your Current Balance is still {context.CurrentUser.Balance:C}");
                        break;

                    default:
                        Console.WriteLine("Invalid choice.");
                        return;
                }

                receiveOperation.Sender.CheckDeleteAccountPossibility(receiveOperation);
                context.CurrentUser.CheckDeleteAccountPossibility(receiveOperation);

                context.CurrentUser.Transfers.Remove(receiveOperation);
            }
        }
    }

}

// sender > TransfersList
// reciever > TransfersList
// TransferObj created when sender transfer money to reciever
// TransferObj contains sender, reciever, amount, status
// TransferObj added to TransfersList of reciever
// when reciever accept transfer, its status be Completed
// when reciever reject transfer, its status be Rejected
// in case of delete account of reiever system will refuse (context.CurrentUser.CheckDeleteAccountPossibility(receiveOperation))
// in case of delete account of sender system will refuse (receiveOperation.Sender.CheckDeleteAccountPossibility(receiveOperation))
