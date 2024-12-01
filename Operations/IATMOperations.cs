using System;
using ATM.Utils;

namespace ATM.Operations
{

    interface IATMOperations
    {
        void Execute(ATMContext context);
    }
}
