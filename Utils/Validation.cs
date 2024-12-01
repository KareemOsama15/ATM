using System;

namespace ATM.Utils
{
    public class Validation
    {
        public static string UserInput(string message)
        {
            string input;

            do
            {
                Console.Write(message);
                input = Console.ReadLine().Trim();

                if (string.IsNullOrEmpty(input))
                    Console.WriteLine("Input can't be empty. Try again: ");
            } while (string.IsNullOrEmpty(input));

            return input;
        }

    }
}
