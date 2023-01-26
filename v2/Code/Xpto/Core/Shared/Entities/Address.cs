using Newtonsoft.Json;

namespace Xpto.Core.Shared.Entities
{
    public class Address
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string? Complement { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Note { get; set; }

        public Address()
        {
            Id = Guid.NewGuid();
        }

        public void CreateOrEditAddress(AddressParams addressParams)
        {
            Street = addressParams.Street;  
            Number = addressParams.Number;
            Complement = addressParams.Complement;
            District = addressParams.District;
            City = addressParams.City;
            State = addressParams.State;
            ZipCode = addressParams.ZipCode;

        }

        public void EditAddressNumber(string number)
        {
            Number = number;
        }

        public override string ToString() //melhor usar o string builder porque ele não fica ocupando memoria criando variaveis
        {
            return $"{Street}, {Number} - {District}, {City} - {State}, CEP: {ZipCode}"; 
                
        }
    }

    public class AddressParams
    {
        [JsonProperty("logradouro")]
        public string Street { get; set; } = null!;
        public string Number { get; set; } = null!;

        [JsonProperty("complemento")]
        public string? Complement { get; set; }

        [JsonProperty("bairro")]
        public string District { get; set; } = null!;

        [JsonProperty("localidade")]
        public string City { get; set; } = null!;

        [JsonProperty("uf")]
        public string State { get; set; } = null!;

        [JsonProperty("cep")]
        public string ZipCode { get; set; } = null!;

        public override string ToString() 
        {
            return $"{Street}, {District}, {City} - {State}, CEP: {ZipCode}";

        }
    }
}
