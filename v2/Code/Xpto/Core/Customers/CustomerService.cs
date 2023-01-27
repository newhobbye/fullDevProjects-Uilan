using System.Globalization;
using Xpto.Core.Shared.Entities;
using Xpto.Core.Shared.Functions;

namespace Xpto.Core.Customers
{
    public class CustomerService
    {
        public void List()
        {
            App.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine((" Lista de Clientes").PadLeft(20,'-'));

            if (App.Customers.Count == 1)
            {
                Console.WriteLine();
                Console.WriteLine(" 1 registro encontrado!");
            }
            else if (App.Customers.Count > 1)
            {
                Console.WriteLine();
                Console.WriteLine(" {0} registros encontrados", App.Customers.Count);
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine(" Nenhum registro encontrado");
            }

            Console.WriteLine();
            Console.WriteLine(" Clientes:");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(("").PadRight(100, '-'));
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" CÓDIGO".PadRight(10, ' ') + "| NOME");

            foreach (var customer in App.Customers)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(("").PadRight(100, '-'));
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($" {customer.Code.ToString().PadRight(10, ' ')}| {customer.Name}");
                Console.ForegroundColor = ConsoleColor.Yellow;
            }

            Console.WriteLine(("").PadRight(100, '-'));

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" Pressione 0 - Voltar");
            Console.WriteLine();

            int.TryParse(Console.ReadLine(), out var action);

            while (action != 0)
            {
                Console.WriteLine("Comando inválido");
                int.TryParse(Console.ReadLine(), out action);
            }
        }

        public void Select()
        {
            App.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(("Consultar Cliente:").PadLeft(25, ' '));
            Console.WriteLine();
            Console.Write(" Informe o código do cliente ou 0 para sair: ");
            var repository = new CustomerRepository();

            while (true)
            {
                int.TryParse(Console.ReadLine(), out var code);

                if (code == 0)
                    return;

                var customer = App.Customers.FirstOrDefault(x => x.Code == code);

                if (customer == null)
                {
                    App.Clear();
                    Console.WriteLine(("Consultar Cliente:").PadLeft(25, ' '));
                    Console.WriteLine();
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" Cliente não encontrado ou código inválido.");
                    Console.ResetColor();
                }
                else
                {
                    App.Clear();
                    Console.WriteLine(("Consultar Cliente:").PadLeft(25, ' '));
                    Console.WriteLine();

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(("").PadRight(100, '-'));
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(" Cliente Selecionado");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(("").PadRight(100, '-'));

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(" Código: {0}", customer.Code);
                    Console.WriteLine(" Nome: {0}", customer.Name);
                    Console.WriteLine(" Tipo de Pessoa: {0}", customer.PersonType);

                    if (customer.PersonType?.ToUpper() == "PJ")
                    {
                        Console.WriteLine(" Nome Fantasia:: {0}", customer.Nickname);
                    }

                    Console.WriteLine(" CPF/CNPJ: {0}", customer.Identity);

                    if (customer.PersonType?.ToUpper() == "PF" && customer.BirthDate != null)
                    {
                        Console.WriteLine(" Data de Nascimento: {0}", ((DateTime)customer.BirthDate).ToString("dd/MM/yyyy"));
                    }


                    foreach (var item in customer.Addresses)
                    {
                        Console.WriteLine(" Endereço: {0}", item);
                    }

                    foreach (var item in customer.Phones)
                    {
                        Console.WriteLine(" Telefone: {0}", item);
                    }

                    foreach (var item in customer.Emails)
                    {
                        Console.WriteLine(" E-mail: {0}", item);
                    }



                    Console.WriteLine(" Observação: {0}", customer.Note);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(("").PadRight(100, '-'));

                    int actionMenu = -1;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("  Ações:");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(("").PadRight(100, '-'));

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(" Pressione: 1 - Adicionar dados | 2 - Editar dados | 3 - Remover dados");
                    Console.WriteLine(" 0 - Sair"); 
                    bool validActionMenu = int.TryParse(Console.ReadLine(), out actionMenu);

                    while (validActionMenu == false)
                    {
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(" Digito invalido!");
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Ações:");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(" Pressione: 1 - Adicionar dados | 2 - Editar dados | 3 - Remover dados");
                        Console.WriteLine(" 0 - Sair");

                        validActionMenu = int.TryParse(Console.ReadLine(), out actionMenu);
                    }

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(("").PadRight(100, '-'));



                    if (actionMenu == 1)
                    {
                        int actionSubMenu = -1;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(" Pressione: 1 - Adicionar endereço | 2 - Adicionar telefone | 3 - Adicionar E-mail");
                        Console.WriteLine(" 0 - Sair");
                        bool validActionSubMenu = int.TryParse(Console.ReadLine(), out actionSubMenu);

                        while (validActionSubMenu == false)
                        {
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(" Digito invalido!");
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Ações:");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine(" Pressione: 1 - Adicionar endereço | 2 - Adicionar telefone | 3 - Adicionar E-mail");
                            Console.WriteLine(" 0 - Sair");

                            validActionSubMenu = int.TryParse(Console.ReadLine(), out actionSubMenu);
                        }

                        switch (actionSubMenu)
                        {
                            case 1:
                                customer = ConsoleProcessOfCreateData(customer, Operation.ADDRESS, "Endereço");
                                break;

                            case 2:
                                customer = ConsoleProcessOfCreateData(customer, Operation.PHONE, "Telefone");
                                break;

                            case 3:
                                customer = ConsoleProcessOfCreateData(customer, Operation.EMAIL, "E-mail");
                                break;

                        }
                    }
                    else if (actionMenu == 2)
                    {
                        int actionSubMenu = -1;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(" Pressione: 1 - Editar endereço | 2 - Editar telefone | 3 - Editar E-mail");
                        Console.WriteLine(" 0 - Sair");
                        bool validActionSubMenu = int.TryParse(Console.ReadLine(), out actionSubMenu);

                        while (validActionSubMenu == false)
                        {
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(" Digito invalido!");
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Ações:");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine(" Pressione: 1 - Editar endereço | 2 - Editar telefone | 3 - Editar E-mail");
                            Console.WriteLine(" 0 - Sair");

                            validActionSubMenu = int.TryParse(Console.ReadLine(), out actionSubMenu);
                        }

                        switch (actionSubMenu)
                        {
                            case 1:
                                customer = ConsoleProcessOfEditData(customer.Addresses, "Endereço", Operation.ADDRESS, customer);
                                break;

                            case 2:
                                customer = ConsoleProcessOfEditData(customer.Phones, "Telefone", Operation.PHONE, customer);
                                break;

                            case 3:
                                customer = ConsoleProcessOfEditData(customer.Emails, "E-mail", Operation.EMAIL, customer);
                                break;

                        }
                    }
                    else if (actionMenu == 3)
                    {
                        int actionSubMenu = -1;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(" Pressione: 1 - Editar endereço | 2 - Editar telefone | 3 - Editar E-mail");
                        Console.WriteLine(" 0 - Sair");
                        bool validActionSubMenu = int.TryParse(Console.ReadLine(), out actionSubMenu);

                        while (validActionSubMenu == false)
                        {
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(" Digito invalido!");
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Ações:");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine(" Pressione: 1 - Editar endereço | 2 - Editar telefone | 3 - Editar E-mail");
                            Console.WriteLine(" 0 - Sair");

                            validActionSubMenu = int.TryParse(Console.ReadLine(), out actionSubMenu);
                        }

                        switch (actionSubMenu)
                        {
                            case 1:
                                customer = ConsoleProcessOfRemoveData(customer.Addresses, "Endereço", Operation.ADDRESS, customer);
                                break;

                            case 2:
                                customer = ConsoleProcessOfRemoveData(customer.Phones, "Telefone", Operation.PHONE, customer);
                                break;

                            case 3:
                                customer = ConsoleProcessOfRemoveData(customer.Emails, "E-mail", Operation.EMAIL, customer);
                                break;

                        }
                    }
                    else if (actionMenu == 0 || actionMenu < 0 || actionMenu > 3)
                    {
                        return;
                    }

                    repository.Save();


                }

                Console.WriteLine();
                Console.Write("Informe o código do cliente ou 0 para sair: ");
            }
        }

        public async void Create()
        {
            App.Clear();

            Console.WriteLine("Novo Cliente");
            Console.WriteLine();

            var customer = new Customer();


            Console.Write("Código (Número inteiro):");
            customer.Code = Convert.ToInt32(Console.ReadLine());

            Console.Write("Tipo de Pessoa (PF ou PJ):");
            customer.PersonType = Console.ReadLine();

            Console.Write("Nome:");
            customer.Name = Console.ReadLine();

            if (customer.PersonType?.ToUpper() == "PJ")
            {
                Console.Write("Nome Fantasia:");
                customer.Nickname = Console.ReadLine();
            }

            Console.Write("CPF/CNPJ:");
            customer.Identity = Console.ReadLine();

            if (customer.PersonType?.ToUpper() == "PF")
            {
                Console.Write("Data de Nascimento (dd/mm/aaaa):");

                while (true)
                {
                    if (DateTime.TryParseExact(
                            Console.ReadLine(),
                            "d/M/yyyy",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out var dt))
                    {
                        customer.BirthDate = dt;
                        break;
                    }
                    else
                    {
                        Console.Write("Data de Nascimento inválida:");
                    }
                }
            }

            customer = CreateAddress(customer);
            customer = CreatePhones(customer);
            customer = CreateEmails(customer);

            Console.Write("Observação:");
            customer.Note = Console.ReadLine();

            customer.CreationDate = new DateTime();
            App.Customers.Add(customer);


            var customerRepository = new CustomerRepository();
            customerRepository.Save();

            Console.WriteLine();
            Console.WriteLine("Cliente cadastrado com sucesso");

            Console.WriteLine();
            Console.WriteLine("0 - Voltar");
            Console.WriteLine();

            int.TryParse(Console.ReadLine(), out var action);

            while (action != 0)
            {
                Console.WriteLine("Comando inválido");
                int.TryParse(Console.ReadLine(), out action);
            }
        }

        public void Edit()
        {
            App.Clear();
            Console.WriteLine("Atualização de Cliente");
            Console.WriteLine();
            Console.Write("Informe o código do cliente ou 0 para sair: ");

            while (true)
            {
                int.TryParse(Console.ReadLine(), out var code);

                if (code == 0)
                    return;

                var customer = App.Customers.FirstOrDefault(x => x.Code == code);

                if (customer == null)
                {
                    App.Clear();
                    Console.WriteLine("Atualização de Cliente");
                    Console.WriteLine();
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Cliente não encontrato ou código inválido");
                    Console.ResetColor();
                }
                else
                {
                    App.Clear();
                    Console.WriteLine("Atualização de Cliente");
                    Console.WriteLine();


                    Console.WriteLine("Cliente Selecionado");
                    Console.WriteLine(("").PadRight(100, '-'));

                    Console.WriteLine("Código: {0}", customer.Code);
                    var text = Console.ReadLine();
                    if (text != "")
                        customer.Code = Convert.ToInt32(text);

                    Console.WriteLine("Nome: {0}", customer.Name);
                    text = Console.ReadLine();
                    if (text != "")
                        customer.Name = text;

                    Console.WriteLine("Tipo de Pessoa: {0}", customer.PersonType);
                    text = Console.ReadLine();
                    if (text != "")
                        customer.PersonType = text;

                    if (customer.PersonType?.ToUpper() == "PJ")
                    {
                        Console.WriteLine("Nome Fantasia:: {0}", customer.Nickname);
                        text = Console.ReadLine();
                        if (text != "")
                            customer.Nickname = text;
                    }

                    Console.WriteLine("CPF/CNPJ: {0}", customer.Identity);
                    text = Console.ReadLine();
                    if (text != "")
                        customer.Identity = text;

                    if (customer.PersonType?.ToUpper() == "PF" && customer.BirthDate != null)
                    {
                        Console.WriteLine("Data de Nascimento: {0}", ((DateTime)customer.BirthDate).ToString("dd/MM/yyyy"));
                        text = Console.ReadLine();
                        if (text != "")
                        {
                            while (true)
                            {
                                if (DateTime.TryParseExact(
                                        text,
                                        "d/M/yyyy",
                                        CultureInfo.InvariantCulture,
                                        DateTimeStyles.None,
                                        out var dt))
                                {
                                    customer.BirthDate = dt;
                                    break;
                                }
                                else
                                {
                                    Console.Write("Data de Nascimento inválida:");
                                }
                            }
                        }
                    }

                    //Console.WriteLine("Endereço: {0}", customer.Address);
                    //text = Console.ReadLine();
                    //if (text != "")
                    //    customer.Address = text;

                    //Console.WriteLine("E-mail: {0}", customer.Email);
                    //text = Console.ReadLine();
                    //if (text != "")
                    //    customer.Email = text;

                    Console.WriteLine("Observação: {0}", customer.Note);
                    text = Console.ReadLine();
                    if (text != "")
                        customer.Note = text;


                    var customerRepository = new CustomerRepository();
                    customerRepository.Save();

                    Console.WriteLine();
                    Console.WriteLine("Cliente atualizado com sucesso");
                }

                Console.WriteLine();
                Console.Write("Informe o código do cliente ou 0 para sair: ");
            }
        } //revisar

        public void Delete()
        {
            App.Clear();
            Console.WriteLine("Excluir de Cliente");
            Console.WriteLine();
            Console.Write("Informe o código do cliente ou 0 para sair: ");

            while (true)
            {
                int.TryParse(Console.ReadLine(), out var code);

                if (code == 0)
                    return;

                var customer = App.Customers.FirstOrDefault(x => x.Code == code);

                if (customer == null)
                {
                    App.Clear();
                    Console.WriteLine("Excluir de Cliente");
                    Console.WriteLine();
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Cliente não encontrato ou código inválido");
                    Console.ResetColor();
                }
                else
                {
                    App.Clear();
                    Console.WriteLine("Excluir de Cliente");
                    Console.WriteLine();

                    Console.WriteLine(("").PadRight(100, '-'));
                    Console.WriteLine("Código: {0}", customer.Code);
                    Console.WriteLine("Nome: {0}", customer.Name);
                    Console.WriteLine(("").PadRight(100, '-'));
                    Console.WriteLine();
                    Console.Write("Deseja excluir o cliente? (S - Sim, N - Não):");
                    var result = Console.ReadLine();
                    if (result?.ToUpper() == "S")
                    {
                        App.Customers.Remove(customer);

                        var customerRepository = new CustomerRepository();
                        customerRepository.Save();

                        App.Clear();
                        Console.WriteLine("Excluir de Cliente");
                        Console.WriteLine();
                        Console.WriteLine("Cliente exluído com sucesso");
                    }
                }

                Console.WriteLine();
                Console.Write("Informe o código do cliente ou 0 para sair: ");
            }
        }

        #region[Manipulação de dados do cliente]
        public Customer CreateAddress(Customer customer)
        {
            customer.Addresses.Add(ConsoleCreateAddressInteration());
            return customer;
        }

        public Customer CreatePhones(Customer customer)
        {
            Console.Write("Telefone com DDD:");
            var phoneParams = new PhoneParams();

            phoneParams.Number = Convert.ToInt64(Console.ReadLine());

            Phone phone = new Phone();
            phone.CreatePhone(phoneParams);
            customer.Phones.Add(phone);

            return customer;

        }

        public Customer CreateEmails(Customer customer)
        {
            Console.Write("E-mail:");

            var emailParams = new EmailParams();
            emailParams.Address = Console.ReadLine();

            var email = new Email();
            email.CreateEmail(emailParams);
            customer.Emails.Add(email);

            return customer;

        }

        public Customer EditAddress(Customer customer, Address address, int selectedIndex)
        {

            Console.WriteLine();
            Console.WriteLine(("").PadRight(100, '-'));
            Console.WriteLine();

            int editNumber = -1;
            Console.WriteLine();
            Console.WriteLine("Editar somente o numero do endereço escolhido?");
            Console.WriteLine(address);
            Console.WriteLine();
            Console.WriteLine("1 - Sim | 2 - Não");
            bool validation = int.TryParse(Console.ReadLine(), out editNumber);

            while (validation == false)
            {
                Console.WriteLine("Digito invalido!");
                Console.WriteLine("Editar somente o numero do endereço escolhido?");
                Console.WriteLine();
                Console.WriteLine("1 - Sim | 2 - Não");
                validation = int.TryParse(Console.ReadLine(), out editNumber);
            }

            if (editNumber == 1)
            {
                Console.WriteLine();
                Console.WriteLine("Informe o numero do endereço: ");
                int newNumber = int.Parse(Console.ReadLine());
                address.EditAddressNumber(newNumber.ToString());
                Console.WriteLine("Endereço atualizado!");
                Console.WriteLine(address);

                //customer.Addresses[selectedIndex - 1] = address;
            }
            else
            {
                address = ConsoleCreateAddressInteration();
                //customer.Addresses[selectedIndex - 1] = address;
            }
            return customer;
        }

        public Customer EditPhones(Customer customer, Phone phone, int selectedIndex)
        {
            Console.WriteLine();
            Console.WriteLine(("").PadRight(100, '-'));
            Console.WriteLine();

            Console.Write("Telefone com DDD:");
            long newPhone = Convert.ToInt64(Console.ReadLine());
            phone.EditPhone(newPhone);

            Console.WriteLine("Telefone editado com sucesso!");
            Console.WriteLine(phone);

            //customer.Phones[selectedIndex - 1] = phone; ALTERAÇÃO POR REFERENCIA DE MEMORIA DO OBJETO

            return customer;
        }

        public Customer EditEmails(Customer customer, Email email, int selectedIndex)
        {
            Console.WriteLine();
            Console.WriteLine(("").PadRight(100, '-'));
            Console.WriteLine();

            Console.Write("E-mail:");
            string newEmail = Console.ReadLine();
            email.EditEmail(newEmail);
            Console.WriteLine("E-mail editado com sucesso");
            Console.WriteLine(email);

            //customer.Emails[selectedIndex - 1] = email;

            return customer;
        }

        

        #endregion

        #region[Interation Console]

        public Address ConsoleCreateAddressInteration()
        {
            var zipFunction = new ZipCodeFunction();
            Console.WriteLine();
            Console.WriteLine("Endereço:");
            var addressParams = new AddressParams();
            string tempZipCode;

            Console.Write("CEP:");
            tempZipCode = Console.ReadLine();
            Console.WriteLine();
            addressParams = zipFunction.GetAddressByZipCode(tempZipCode);



            if (addressParams.Street != null)
            {
                Console.WriteLine("Endereço encontrado:");
                Console.WriteLine(addressParams);

                Console.WriteLine();
                Console.Write("Número:");
                addressParams.Number = Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Endereço não encontrado. Digite manualmente:");
                Console.WriteLine();
                addressParams.ZipCode = tempZipCode;
                Console.Write("Logradouro:");
                addressParams.Street = Console.ReadLine();
                Console.Write("Número:");
                addressParams.Number = Console.ReadLine();
                Console.Write("Complemento:");
                addressParams.Complement = Console.ReadLine();
                Console.Write("Bairro:");
                addressParams.District = Console.ReadLine();
                Console.Write("Cidade:");
                addressParams.City = Console.ReadLine();
                Console.Write("Estado:");
                addressParams.State = Console.ReadLine();
            }

            var address = new Address();
            address.CreateOrEditAddress(addressParams);

            return address;
        }

        public Customer ConsoleProcessOfCreateData(Customer customer, Operation operation, string operationName)
        {
            int numberInputValidation = 0;
            bool resultInputValidation = false;

            do
            {
                if (operation == Operation.ADDRESS)
                {
                    customer = CreateAddress(customer);

                }
                else if (operation == Operation.PHONE)
                {
                    customer = CreatePhones(customer);
                }
                else if (operation == Operation.EMAIL)
                {
                    customer = CreateEmails(customer);
                }

                Console.WriteLine($"Deseja cadastrar outro {operationName.ToLower()}? Digite: 1 - Sim, Qualquer numero - Não");
                resultInputValidation = int.TryParse(Console.ReadLine(), out numberInputValidation);

                if (resultInputValidation == false)
                {
                    do
                    {
                        Console.WriteLine("Digito invalido! Favor digitar novamente.");
                        Console.WriteLine($"Deseja cadastrar outro {operationName.ToLower()}? Digite: 1 - Sim, Qualquer numero - Não");
                        resultInputValidation = int.TryParse(Console.ReadLine(), out numberInputValidation);

                    } while (resultInputValidation == false);
                }

            } while (numberInputValidation == 1);

            return customer;
        }

        public Customer ConsoleProcessOfEditData<T>(IList<T> array, string operationName, Operation operation, Customer customer)
        {
            Console.WriteLine();
            Console.WriteLine($"Estes são os {operationName.ToLower()}s do cliente:");
            Console.WriteLine();

            for (int i = 0; i < array.Count; i++)
            {
                Console.WriteLine("{0} - {1}", i + 1, array[i]);
            }

            Console.WriteLine();
            Console.WriteLine($"Digite o numero que corresponde ao {operationName.ToLower()} que gostaria de editar:");
            int actionNumber = -1;
            bool actionValidation = int.TryParse(Console.ReadLine(), out actionNumber);

            while (actionValidation == false)
            {
                Console.WriteLine("Digito invalido!");
                Console.WriteLine($"Digite o numero que corresponde ao {operationName.ToLower()} que gostaria de editar:");
                actionValidation = int.TryParse(Console.ReadLine(), out actionNumber);

            }

            if (actionNumber < 0 || actionNumber > array.Count)
            {
                Console.WriteLine($"Este numero não corresponde a nenhum {operationName.ToLower()}!");
                return customer;
            }
            else
            {
                var dataEdit = array[actionNumber - 1];

                if (operation == Operation.ADDRESS)
                {
                    customer = EditAddress(customer, dataEdit as Address, actionNumber);

                }
                else if (operation == Operation.PHONE)
                {
                    customer = EditPhones(customer, dataEdit as Phone, actionNumber);


                }
                else if (operation == Operation.EMAIL)
                {
                    customer = EditEmails(customer, dataEdit as Email, actionNumber);
                }

            }

            return customer;
        }

        public Customer ConsoleProcessOfRemoveData<T>(IList<T> array, string operationName, Operation operation, Customer customer)
        {

            Console.WriteLine();
            Console.WriteLine(("").PadRight(100, '-'));
            Console.WriteLine();

            Console.WriteLine($"{operationName}s disponiveis para remoção:");
            Console.WriteLine();

            for (int i = 0; i < array.Count; i++)
            {
                Console.WriteLine("{0} - {1}", i + 1, array[i]);
            }

            Console.WriteLine();
            Console.WriteLine($"Digite o numero referente ao {operationName.ToLower()} que gostaria de remover: ");
            int numberDecision = -1;
            bool validationAction = int.TryParse(Console.ReadLine(), out numberDecision);

            while (validationAction == false)
            {
                Console.WriteLine("Digito invalido!");
                Console.WriteLine($"Digite o numero referente ao {operationName.ToLower()} que gostaria de remover: ");
                validationAction = int.TryParse(Console.ReadLine(), out numberDecision);
            }

            if (numberDecision < 0 || numberDecision > array.Count)
            {
                Console.WriteLine($"Este numero não corresponde a nenhum {operationName.ToLower()}!");
                return customer;
            }

            T dataRemove = array[numberDecision - 1];

            if (operation == Operation.ADDRESS)
            {
                customer.RemoveAddress(dataRemove as Address);
                Console.WriteLine($"{operationName} removido!");
                Console.WriteLine(dataRemove);
            }
            else if (operation == Operation.PHONE)
            {
                customer.RemovePhone(dataRemove as Phone);
                Console.WriteLine($"{operationName} removido!");
            }
            else if (operation == Operation.EMAIL)
            {
                customer.RemoveEmail(dataRemove as Email);
                Console.WriteLine($"{operationName} removido!");
            }

            return customer;

        }

        #endregion
    }
}
