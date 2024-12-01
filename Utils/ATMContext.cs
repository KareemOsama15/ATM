using System;
using ATM;

namespace ATM.Utils
{
    public class ATMContext
    {
        public ATMApp AtmInstance { get; set; }
        public List<User> Users { get; set; }
        public User CurrentUser { get; set; }

        public ATMContext(ATMApp atm, List<User> users, User user)
        {
            AtmInstance = atm;
            Users = users;
            CurrentUser = user;
        }
    }
}
