using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Xpto.Core.Shared.Entities
{
    public class Address
    {
        public Guid Id { get; set; }
        public string Type { get; set; }

        [JsonPropertyName("logradouro")]
        public string Street { get; set; }
        public string Number { get; set; }

        [JsonPropertyName("complemento")]
        public string Complement { get; set; }

        [JsonPropertyName("bairro")]
        public string District { get; set; }

        [JsonPropertyName("localidade")]
        public string City { get; set; }

        [JsonPropertyName("uf")]
        public string State { get; set; }

        [JsonPropertyName("cep")]
        public string ZipCode { get; set; }
        public string Note { get; set; }

        public Address()
        {
            Id = Guid.NewGuid();
        }

        public override string ToString() //melhor usar o string builder porque ele não fica ocupando memoria criando variaveis
        {
            return $"{Street}, {Number} - {District}, {City} - {State}, CEP: {ZipCode}"; //formatar zipcode
                //string.Join(" ", Street, Number, Complement, District, City, State, ZipCode);
        }
    }
}
