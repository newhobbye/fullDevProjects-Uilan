using Newtonsoft.Json;
using RestSharp;
using Xpto.Core.Shared.Entities;

namespace Xpto.Core.Shared.Functions
{
    public class ZipCodeFunction
    {

        public Address GetAddressByZipCode(string zipCode)
        {
            var client = new RestClient("https://viacep.com.br/");
            var request = new RestRequest($"/ws/{zipCode}/json", Method.Get);
            var response =  client.Execute(request);

            var address = new Address();
            address = JsonConvert.DeserializeObject<Address>(response.Content!);
            return address!;
        }

    }
}
