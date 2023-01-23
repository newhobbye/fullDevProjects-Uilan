using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xpto.Core.Shared.Entities;
using Xpto.Core.Shared.Functions;

namespace Xpto.Core.Customers
{
    public class CustomerService
    {
        public void List()
        {
            App.Clear();
            Console.WriteLine("Lista de Clientes");

            if (App.Customers.Count == 1)
                Console.WriteLine("1 registro encontrado");
            else if (App.Customers.Count > 1)
                Console.WriteLine("{0} registros encontrados", App.Customers.Count);
            else
                Console.WriteLine("nenhum registro encontrado");

            Console.WriteLine();
            Console.WriteLine("Lista de Clientes");
            Console.WriteLine(("").PadRight(100, '-'));
            Console.WriteLine("CÓDIGO".PadRight(10, ' ') + "| NOME");

            foreach (var customer in App.Customers)
            {
                Console.WriteLine(("").PadRight(100, '-'));
                Console.WriteLine($"{customer.Code.ToString().PadRight(10, ' ')}| {customer.Name}");
            }

            Console.WriteLine(("").PadRight(100, '-'));

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

        public void Select()
        {
            App.Clear();
            Console.WriteLine("Consulta de Cliente");
            Console.WriteLine();
            Console.Write("Informe o código do cliente ou 0 para sair: ");
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
                    Console.WriteLine("Consulta de Cliente");
                    Console.WriteLine();
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Cliente não encontrato ou código inválido");
                    Console.ResetColor();
                }
                else
                {
                    App.Clear();
                    Console.WriteLine("Consulta de Cliente");
                    Console.WriteLine();


                    Console.WriteLine(("").PadRight(100, '-'));
                    Console.WriteLine("Cliente Selecionado");
                    Console.WriteLine(("").PadRight(100, '-'));

                    Console.WriteLine("Código: {0}", customer.Code);
                    Console.WriteLine("Nome: {0}", customer.Name);
                    Console.WriteLine("Tipo de Pessoa: {0}", customer.PersonType);

                    if (customer.PersonType?.ToUpper() == "PJ")
                    {
                        Console.WriteLine("Nome Fantasia:: {0}", customer.Nickname);
                    }

                    Console.WriteLine("CPF/CNPJ: {0}", customer.Identity);

                    if (customer.PersonType?.ToUpper() == "PF" && customer.BirthDate != null)
                    {
                        Console.WriteLine("Data de Nascimento: {0}", ((DateTime)customer.BirthDate).ToString("dd/MM/yyyy"));
                    }


                    foreach (var item in customer.Addresses)
                    {
                        Console.WriteLine("Endereço: {0}", item);
                    }

                    foreach (var item in customer.Phones)
                    {
                        Console.WriteLine("Telefone: {0}", item);
                    }

                    foreach (var item in customer.Emails)
                    {
                        Console.WriteLine("E-mail: {0}", item);
                    }



                    Console.WriteLine("Observação: {0}", customer.Note);
                    Console.WriteLine(("").PadRight(100, '-'));

                    int actionInCustomer = -1;

                    Console.WriteLine("Ações:");
                    Console.WriteLine("Pressione: 1 - Adicionar um endereço | 2 - Adicionar um telefone | 3 - Adicionar um E-mail");
                    Console.WriteLine("Pressione: 4 - Editar endereços | 5 - Editar telefones | 6 - Adicionar E-mails");
                    Console.WriteLine("Pressione: 7 - Remover endereços | 8 - Remover telefones | 9 - Remover E-mails");
                    Console.WriteLine("0 - Sair"); //VALIDAR DEPOIS INTERVALO ENTRE NUMEROS VALIDOS E NÃO USADOS
                    bool validAction = int.TryParse(Console.ReadLine(), out actionInCustomer);

                    while(validAction == false)
                    {
                        Console.WriteLine("Digito invalido!");
                        Console.WriteLine("Ações:");
                        Console.WriteLine("Pressione: 1 - Adicionar um endereço | 2 - Adicionar um telefone | 3 - Adicionar um E-mail");
                        Console.WriteLine("Pressione: 4 - Editar endereços | 5 - Editar telefones | 6 - Editar E-mails");
                        Console.WriteLine("Pressione: 7 - Remover endereços | 8 - Remover telefones | 9 - Remover E-mails");
                        Console.WriteLine("0 - Sair");
                        
                        validAction = int.TryParse(Console.ReadLine(), out actionInCustomer);
                    }

                    if(actionInCustomer == 1) //melhorar
                    {
                        customer = CreateAddress(customer);
                    }
                    else if(actionInCustomer == 2)
                    {
                        customer = CreatePhones(customer);
                    }
                    else if(actionInCustomer == 3)
                    {
                        customer = CreateEmails(customer);
                    }
                    else if (actionInCustomer == 4)
                    {
                        customer = EditAddress(customer);
                    }
                    else if(actionInCustomer == 5)
                    {
                        customer = EditPhones(customer);
                    }
                    else if(actionInCustomer == 6)
                    {
                        customer = EditEmails(customer);
                    }
                    else if(actionInCustomer == 7)
                    {
                        customer = RemoveAdrress(customer);
                    }
                    else if (actionInCustomer == 8)
                    {
                        customer = RemovePhones(customer);
                    }
                    else if (actionInCustomer == 9)
                    {
                        customer = RemoveEmails(customer);
                    }
                    else if(actionInCustomer == 0)
                    {
                        break;
                    }

                    App.Customers.Add(customer);
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
            customer = ProcessOfCreateData(customer, Operation.ADDRESS, "Endereço");

            return customer;
        }

        public Customer CreatePhones(Customer customer)
        {
            customer = ProcessOfCreateData(customer, Operation.PHONE, "Telefone");

            return customer;

        }

        public Customer CreateEmails(Customer customer)
        {
            customer = ProcessOfCreateData(customer, Operation.EMAIL, "E-mail");

            return customer;

        }

        public Customer EditAddress(Customer customer)
        {

            Console.WriteLine();
            Console.WriteLine(("").PadRight(100, '-'));
            Console.WriteLine();
            var addresses = customer.Addresses;

            if(addresses.Count == 1)
            {
                var address = addresses.FirstOrDefault();
                Console.WriteLine("Endereço a ser editado:");
                Console.WriteLine(address);
                Console.WriteLine();
                address = AuxiliaryEditAddress(address!);
                customer.Addresses[0] = address;
            }
            else
            {
                Console.WriteLine("Estes são os endereços do cliente:");
                Console.WriteLine();

                for (int i = 0; i < addresses.Count; i++)
                {
                    Console.WriteLine("{0} - {1}", i + 1, addresses[i]);
                }
                Console.WriteLine();
                Console.WriteLine("Digite o numero que corresponde ao endereço que gostaria de editar:");
                int actionAddress = -1;
                bool actionValidation = int.TryParse(Console.ReadLine(), out actionAddress);

                while (actionValidation == false)
                {
                    Console.WriteLine("Digito invalido!");
                    Console.WriteLine("Digite o numero que corresponde ao endereço que gostaria de editar:");
                    actionValidation = int.TryParse(Console.ReadLine(), out actionAddress);

                }

                if(actionAddress < 0 || actionAddress > addresses.Count)
                {
                    Console.WriteLine("Este numero não corresponde a nenhum endereço!");
                    return customer;
                }
                else
                {
                    var addressEdit = addresses[actionAddress - 1];
                    addressEdit = AuxiliaryEditAddress(addressEdit!);
                    customer.Addresses[actionAddress - 1] = addressEdit;

                }

                
            }


            return customer;
        }

        public Customer EditPhones(Customer customer)
        {
            Console.WriteLine();
            Console.WriteLine(("").PadRight(100, '-'));
            Console.WriteLine();
            var phones = customer.Phones;

            if (phones.Count == 1)
            {
                var phone = phones.FirstOrDefault();
                Console.WriteLine("Telefone a ser editado:");
                Console.WriteLine(phone);
                Console.WriteLine();
                phone = AuxiliaryEditPhone(phone);
                customer.Phones[0] = phone; //perguntar pro uilan o que da pra fazer
            }
            else
            {
                Console.WriteLine("Estes são os telefones do cliente:");
                Console.WriteLine();

                for (int i = 0; i < phones.Count; i++)
                {
                    Console.WriteLine("{0} - {1}", i + 1, phones[i]);
                }
                Console.WriteLine();
                Console.WriteLine("Digite o numero que corresponde ao telefone que gostaria de editar:");
                int actionPhone = -1;
                bool actionValidation = int.TryParse(Console.ReadLine(), out actionPhone);

                while (actionValidation == false)
                {
                    Console.WriteLine("Digito invalido!");
                    Console.WriteLine("Digite o numero que corresponde ao telefone que gostaria de editar:");
                    actionValidation = int.TryParse(Console.ReadLine(), out actionPhone);

                }

                if (actionPhone < 0 || actionPhone > phones.Count)
                {
                    Console.WriteLine("Este numero não corresponde a nenhum telefone!");
                    return customer;
                }
                else
                {
                    var phoneEdit = phones[actionPhone - 1];
                    phoneEdit = AuxiliaryEditPhone(phoneEdit);
                    customer.Phones[actionPhone - 1] = phoneEdit;

                }


            }

            return customer;
        }

        public Customer EditEmails(Customer customer)
        {
            Console.WriteLine();
            Console.WriteLine(("").PadRight(100, '-'));
            Console.WriteLine();
            var emails = customer.Emails;

            if (emails.Count == 1)
            {
                var email = emails.FirstOrDefault();
                Console.WriteLine("Email a ser editado:");
                Console.WriteLine(email);
                Console.WriteLine();
                email = AuxiliaryEditEmail(email!);
                customer.Emails[0] = email; 
            }
            else
            {
                Console.WriteLine("Estes são os E-mails do cliente:");
                Console.WriteLine();

                for (int i = 0; i < emails.Count; i++)
                {
                    Console.WriteLine("{0} - {1}", i + 1, emails[i]);
                }
                Console.WriteLine();
                Console.WriteLine("Digite o numero que corresponde ao email que gostaria de editar:");
                int actionEmail = -1;
                bool actionValidation = int.TryParse(Console.ReadLine(), out actionEmail);

                while (actionValidation == false)
                {
                    Console.WriteLine("Digito invalido!");
                    Console.WriteLine("Digite o numero que corresponde ao email que gostaria de editar:");
                    actionValidation = int.TryParse(Console.ReadLine(), out actionEmail);

                }

                if (actionEmail < 0 || actionEmail > emails.Count)
                {
                    Console.WriteLine("Este numero não corresponde a nenhum email!");
                    return customer;
                }
                else
                {
                    var emailEdit = emails[actionEmail - 1];
                    emailEdit = AuxiliaryEditEmail(emailEdit);
                    customer.Emails[actionEmail - 1] = emailEdit;

                }

            }

            return customer;
        }

        public Customer RemoveAdrress(Customer customer)
        {
            Console.WriteLine();
            Console.WriteLine(("").PadRight(100, '-'));
            Console.WriteLine();

            customer = GenericProcessOfRemoveData(customer.Addresses, "Endereço", Operation.ADDRESS, customer);

            return customer;
        }

        public Customer RemovePhones(Customer customer)
        {
            Console.WriteLine();
            Console.WriteLine(("").PadRight(100, '-'));
            Console.WriteLine();

            customer = GenericProcessOfRemoveData(customer.Phones, "Telefone", Operation.PHONE, customer);

            return customer;
        }

        public Customer RemoveEmails(Customer customer)
        {
            Console.WriteLine();
            Console.WriteLine(("").PadRight(100, '-'));
            Console.WriteLine();

            customer = GenericProcessOfRemoveData(customer.Emails, "E-mail", Operation.EMAIL, customer);

            return customer;
        }

        public Address AuxiliaryEditAddress(Address address)
        {
            var zipFunction = new ZipCodeFunction();

            Console.Write("CEP:");
            string tempZipCode = Console.ReadLine();

            address = zipFunction.GetAddressByZipCode(tempZipCode);

            if (address.Street != null)
            {
                Console.Write("Número:");
                address.Number = Console.ReadLine();
                Console.WriteLine();
                Console.WriteLine("Endereço editado com sucesso!");
            }
            else
            {
                Console.Write("Logradouro:");
                address.Street = Console.ReadLine();
                Console.Write("Número:");
                address.Number = Console.ReadLine();
                Console.Write("Complemento:");
                address.Complement = Console.ReadLine();
                Console.Write("Bairro:");
                address.District = Console.ReadLine();
                Console.Write("Cidade:");
                address.City = Console.ReadLine();
                Console.Write("Estado:");
                address.State = Console.ReadLine();
                Console.WriteLine("Endereço editado com sucesso!");
            }

            return address;
        }

        public Phone AuxiliaryEditPhone(Phone phone)
        {
            Console.WriteLine();
            Console.Write("Telefone com DDD:");
            phone.Number = Convert.ToInt64(Console.ReadLine());
            phone.SeparateDDDFromNumber();
            Console.WriteLine("Telefone editado com sucesso!");
            return phone;
        }

        public Email AuxiliaryEditEmail(Email email)
        {
            Console.Write("E-mail:");
            email.Address = Console.ReadLine();
            Console.WriteLine("E-mail cadastrado com sucesso");
            return email;
        }

        public Customer ProcessOfCreateData(Customer customer, Operation operation, string operationName)
        {
            int numberInputValidation = 0;
            bool resultInputValidation = false;
            var zipFunction = new ZipCodeFunction();

            do
            {
                if (operation == Operation.ADDRESS)
                {
                    Console.WriteLine("Endereço:");

                    var address = new Address();
                    string tempZipCode;

                    Console.Write("CEP:");
                    tempZipCode = Console.ReadLine();

                    address = zipFunction.GetAddressByZipCode(tempZipCode);

                    if (address.Street != null)
                    {
                        Console.Write("Número:");
                        address.Number = Console.ReadLine();
                    }
                    else
                    {
                        Console.Write("Logradouro:");
                        address.Street = Console.ReadLine();
                        Console.Write("Número:");
                        address.Number = Console.ReadLine();
                        Console.Write("Complemento:");
                        address.Complement = Console.ReadLine();
                        Console.Write("Bairro:");
                        address.District = Console.ReadLine();
                        Console.Write("Cidade:");
                        address.City = Console.ReadLine();
                        Console.Write("Estado:");
                        address.State = Console.ReadLine();
                    }

                    customer.Addresses.Add(address);
                    
                }
                else if(operation == Operation.PHONE)
                {
                    Console.Write("Telefone com DDD:");

                    var phone = new Phone();
                    phone.Number = Convert.ToInt64(Console.ReadLine());
                    phone.SeparateDDDFromNumber();
                    customer.Phones.Add(phone);
                }
                else if (operation == Operation.EMAIL)
                {
                    Console.Write("E-mail:");

                    var email = new Email();
                    email.Address = Console.ReadLine();
                    customer.Emails.Add(email);
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

        public Customer GenericProcessOfEditData<T>(IList<T> array, string operationName, Operation operation, Customer customer)
        {

            if (array.Count == 1)
            {
                T data = array.FirstOrDefault();

                Console.WriteLine($"{operationName} a ser editado:");
                Console.WriteLine(data);
                Console.WriteLine();
                if(operation == Operation.ADDRESS)
                {
                    var address = data as Address;
                    address = AuxiliaryEditAddress(address!); 
                    customer.Addresses[0] = address;
                }
                else if(operation == Operation.PHONE)
                {
                    var phone = data as Phone;
                    phone = AuxiliaryEditPhone(phone!);
                    customer.Phones[0] = phone;
                }
                else if(operation == Operation.EMAIL)
                {
                    var email = data as Email;
                    email = AuxiliaryEditEmail(email!);
                    customer.Emails[0] = email;
                }

            }
            else
            {
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
                        var address = dataEdit as Address;
                        address = AuxiliaryEditAddress(address!);
                        customer.Addresses[actionNumber - 1] = address;
                    }
                    else if (operation == Operation.PHONE)
                    {
                        var phone = dataEdit as Phone;
                        phone = AuxiliaryEditPhone(phone!);
                        customer.Phones[actionNumber - 1] = phone;
                    }
                    else if (operation == Operation.EMAIL)
                    {
                        var email = dataEdit as Email;
                        email = AuxiliaryEditEmail(email!);
                        customer.Emails[actionNumber - 1] = email;
                    }

                }

            }

            return customer;
        }

        public Customer GenericProcessOfRemoveData<T>(IList<T> array, string operationName, Operation operation, Customer customer)
        {
            if (array.Count == 1)
            {
                Console.WriteLine($"Tem certeza que quer remover este {operationName.ToLower()}?");
                Console.WriteLine(array[0]);

                Console.WriteLine("1 - Sim | 2 - Não");
                int actionDecicion = -1;
                bool actionValidation = int.TryParse(Console.ReadLine(), out actionDecicion);

                while (actionValidation == false)
                {
                    Console.WriteLine("Digito invalido!");
                    Console.WriteLine($"Tem certeza que quer remover este {operationName.ToLower()}?");
                    Console.WriteLine("1 - Sim | 2 - Não");
                    actionValidation = int.TryParse(Console.ReadLine(), out actionDecicion);

                }

                if (actionDecicion == 1)
                {
                    if (operation == Operation.ADDRESS)
                    {
                        customer.Addresses.RemoveAt(0);
                        Console.WriteLine($"{operationName} removido!");
                    }
                    else if(operation == Operation.PHONE) 
                    {
                        customer.Phones.RemoveAt(0);
                        Console.WriteLine($"{operationName} removido!");
                    }
                    else if (operation == Operation.EMAIL)
                    {
                        customer.Phones.RemoveAt(0);
                        Console.WriteLine($"{operationName} removido!");
                    }
                    
                    return customer;

                }
                else if (actionDecicion < 0 || actionDecicion > array.Count)
                {
                    Console.WriteLine("Este numero não corresponde a nenhuma ação.");
                    return customer;
                }
                else if (actionDecicion == 2)
                {
                    Console.WriteLine("Operação cancelada!");
                    Console.WriteLine();
                    return customer;
                }
            }
            else
            {
                Console.WriteLine($"{operationName}s disponiveis para remoção:");

                for (int i = 0; i < array.Count; i++)
                {
                    Console.WriteLine("{0} - {1}", i + 1, array[i]);
                }

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

                if (operation == Operation.ADDRESS)
                {
                    customer.Addresses.RemoveAt(numberDecision - 1);
                    Console.WriteLine($"{operationName} removido!");
                }
                else if (operation == Operation.PHONE)
                {
                    customer.Phones.RemoveAt(numberDecision - 1);
                    Console.WriteLine($"{operationName} removido!");
                }
                else if (operation == Operation.EMAIL)
                {
                    customer.Emails.RemoveAt(numberDecision - 1);
                    Console.WriteLine($"{operationName} removido!");
                }

                return customer;

            }

            return customer; 
        }

        #endregion
    }
}
