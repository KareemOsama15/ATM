using System;

namespace ATM_App
{
    internal class ATMContext
    {
        public ATM AtmInstance { get; set; }
        public List<User> Users { get;  set; }
        public User CurrentUser { get; set; }
        
        public ATMContext(ATM atm, List<User> users, User user)
        {
            AtmInstance = atm;
            Users = users;
            CurrentUser = user;
        }
    }
}
