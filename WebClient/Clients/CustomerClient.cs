using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace WebClient.Clients
{
    internal class CustomerClient
    {
        private readonly HttpClient _httpClient;

        public CustomerClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GetCustomerResponse> GetCustomerAsync(long id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"/customers/{id}");
            GetCustomerResponse customerResponse = new();
            if (response.IsSuccessStatusCode)
                customerResponse.Customer = await response.Content.ReadFromJsonAsync<Customer>();
            else
                customerResponse.ErrorMessage = $"StatusCode: {(int)response.StatusCode}, ReasonPhrase: {response.ReasonPhrase}";
            return customerResponse;
        }

        public async Task<AddCustomerResponse> AddCustomerAsync(Customer customer)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"/customers", customer);
            AddCustomerResponse customerResponse = new();
            if (response.IsSuccessStatusCode)
                customerResponse.Id = long.Parse(await response.Content.ReadAsStringAsync());
            else
                customerResponse.ErrorMessage = $"StatusCode: {(int)response.StatusCode}, ReasonPhrase: {response.ReasonPhrase}";
            return customerResponse;
        }
    }

    public class AddCustomerResponse
    {
        public long Id { get; set; }

        public string ErrorMessage { get; set; }
    }

    public class GetCustomerResponse
    {
        public Customer Customer { get; set; }

        public string ErrorMessage { get; set; }
    }
}
