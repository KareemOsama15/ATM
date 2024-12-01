using System;
using ATM.Utils;

namespace ATM.Operations
{

    interface ITransaction : IATMOperations
    {
        Guid TransactionId { get; }
        DateTime Timestamp { get; }
    }
}
