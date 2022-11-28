using System;
using System.Text;

namespace WebClient.RandomCustomerGeneration
{
    internal class RandomCustomerGenerator : IRandomCustomerGenerator
    {
        private readonly Char[] _letters = "abcdefghigjklmnopqrstuvwxyz".ToCharArray();

        public Customer GenerateCustomer()
        {
            Customer newCustomer = new Customer
            {
                Id = 1,
                //this.GenerateRandowId(),
                Firstname = this.GenerateRandomWord(),
                Lastname = this.GenerateRandomWord()
            };

            return newCustomer;
        }

        private long GenerateRandowId() => new Random().NextInt64();
        
        private string GenerateRandomWord()
        {
            Random random = new Random();   
           
            int lettersQuantityInWord = random.Next(5, 15);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < lettersQuantityInWord; i++)
            {
                int letterIndex = random.Next(0, _letters.Length - 1);
                char currentLetter = _letters[letterIndex];

                if (i == 0)
                    currentLetter = Char.ToUpper(currentLetter);

                sb.Append(currentLetter);
            }

            return sb.ToString();
        }
    }
}
