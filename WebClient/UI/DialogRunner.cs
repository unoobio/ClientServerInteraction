using System;
using System.Threading.Tasks;
using WebClient.Clients;
using WebClient.RandomCustomerGeneration;

namespace WebClient.UI
{
    internal class DialogRunner
    {
        private readonly CustomerClient _customerClient;
        private readonly IRandomCustomerGenerator _randomCustomerGenerator;

        public DialogRunner(CustomerClient customerClient, IRandomCustomerGenerator randomCustomerGenerator)
        {
            _customerClient = customerClient ?? throw new ArgumentNullException(nameof(customerClient));
            _randomCustomerGenerator = randomCustomerGenerator ?? throw new ArgumentNullException(nameof(randomCustomerGenerator));
        }

        public async Task Run()
        {
            while (true)
            {
                Console.WriteLine("Do you want to add or get customer (Y/N)?");
                ConsoleKeyInfo key = Console.ReadKey();
                Console.WriteLine();
                if (key.KeyChar == 'Y' || key.KeyChar == 'y')
                {
                    Console.WriteLine("ADD or GET?");
                    bool succeed = false;
                    do
                    {
                        string action = Console.ReadLine().Trim().ToUpper();
                        switch (action)
                        {
                            case "ADD":
                                succeed = true;
                                await this.AddCustomer();
                                break;
                            case "GET":
                                succeed = true;
                                Console.WriteLine("Enter customer id.");
                                string customerIdKey = Console.ReadLine();
                                Console.WriteLine();
                                await this.GetCustomer(long.Parse(customerIdKey.Trim()));
                                break;
                            default:
                                Console.WriteLine("Action type is not correct. Please write ADD or GET.");
                                break;
                        }
                    }
                    while (!succeed);  
                }
                else
                    break;
            }
        }

        private async Task AddCustomer()
        {
            Customer newCustomer = _randomCustomerGenerator.GenerateCustomer();
            AddCustomerResponse response = await _customerClient.AddCustomerAsync(newCustomer);
            if (!string.IsNullOrEmpty(response.ErrorMessage))
                Console.WriteLine($"Exception while adding new customer. {response.ErrorMessage}");
            else
                Console.WriteLine($"New customer was added. Id = {response.Id}");

            await this.GetCustomer(response.Id);
        }

        private async Task GetCustomer(long id)
        {
            GetCustomerResponse response = await _customerClient.GetCustomerAsync(id);
            if (!string.IsNullOrEmpty(response.ErrorMessage))
                Console.WriteLine($"Exception while getting a customer. {response.ErrorMessage}");
            else
                Console.WriteLine($"Id = {response.Customer.Id}, FirstName = {response.Customer.Firstname}, LastName = {response.Customer.Lastname}.");
        }
    }
}
