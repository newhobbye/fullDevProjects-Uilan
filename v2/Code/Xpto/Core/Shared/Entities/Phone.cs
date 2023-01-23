using System.IO;
using System.Reflection.Emit;
using System.Text.RegularExpressions;

namespace Xpto.Core.Shared.Entities
{
    public class Phone
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public int Ddd { get; set; }
        public long Number { get; set; }
        public string Note { get; set; }

        public Phone()
        {
            Id = Guid.NewGuid();
        }

        public void SeparateDDDFromNumber()
        {
            string pattern = @"(\d{2})(\d{4,5})(\d{4})";

            var regex = new Regex(pattern);
            Ddd = int.Parse(regex.Replace(Number.ToString(), "$1"));
            Number = long.Parse(regex.Replace(Number.ToString(), "$2$3"));
        }

        public override string ToString()
        {
            return $"({Ddd}) {Number}"; //perguntar pro Uilan
        }


    }
}
