namespace Xpto.Core.Shared.Entities
{
    public class Phone
    {
        public Guid Id { get; set; }
        public string? Type { get; set; }
        public int Ddd { get; set; }
        public long Number { get; set; }
        public string? Note { get; set; }

        public Phone()
        {
            Id = Guid.NewGuid();
        }

        public void CreatePhone(PhoneParams phoneParams)
        {
            Type = phoneParams.Type;
            Ddd = phoneParams.Ddd;
            Number = phoneParams.Number;
            Note = phoneParams.Note;

            SeparateDDDFromNumber();

        }

        public void EditPhone(long number)
        {
            Number = number;
            SeparateDDDFromNumber();
        }

        public void SeparateDDDFromNumber()
        {
            Ddd = int.Parse(Number.ToString().Substring(0, 2));
            Number = long.Parse(Number.ToString().Substring(2));
        }

        public override string ToString()
        {
            return $"({Ddd}) {Number}";
        }


    }

    public class PhoneParams
    {
        public string? Type { get; set; }
        public int Ddd { get; set; }
        public long Number { get; set; }
        public string? Note { get; set; }
    }

}
