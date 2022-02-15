using System;
using System.Collections.Generic;

namespace TugaExchange
{
    class Program
    {
        static DateTime start;
        static void Main(string[] args)
        {
            Console.Title = "TugaExchange";

            start = DateTime.Now;
            Exchange.DefinePriceUpdateInSeconds(5);

            Exchange.AddCoin("CHOW");
            Exchange.AddCoin("DOCE");
            Exchange.AddCoin("GALO");
            Exchange.AddCoin("TUGA");
            Exchange.AddCoin("EUR");

            //Exchange.Read();
            //Investor.Read();

            Menu();

            //Utils.DeleteAllFiles();
        }

        static void Menu()
        {
            Console.WriteLine("Selecione uma das opções:\n");
            Console.WriteLine("-------------------------");
            Console.WriteLine("1. Investidor");
            Console.WriteLine("2. Administrador");

            string MenuOption = Console.ReadLine();

            Console.Clear();

            switch (MenuOption)
            {
                case "1":
                    InvestorMenu();
                    break;
                case "2":
                    AdministratorMenu();
                    break;
                default:
                    Menu();
                    break;
            }
        }

        static void InvestorMenu()
        {
            UpdatePrices();
            Console.Clear();
            string investor = "menu investidor";
            Console.WriteLine(investor.ToUpper());
            Console.WriteLine("\n");
            Console.WriteLine("Escolha uma das seguintes opções:\n");
            Console.WriteLine("---------------------------------");
            Console.WriteLine("1. Depositar");
            Console.WriteLine("2. Comprar moeda");
            Console.WriteLine("3. Vender moeda");
            Console.WriteLine("4. Mostrar portfólio");
            Console.WriteLine("5. Mostrar câmbio");
            Console.WriteLine("6. Sair");

            string MenuOption = Console.ReadLine();

            string error = null;

            Console.Clear();

            switch (MenuOption)
            {
                case "1":
                    int amount;
                    while (true)
                    {
                        //Console.Clear();
                        Console.WriteLine("Introduza o valor a depositar em euros:");
                        bool isInt = int.TryParse(Console.ReadLine(), out amount);
                        if (isInt == false || amount <= 0)
                        {
                            Console.WriteLine("Valor inválido. Por favor introduza um número inteiro positivo.");
                            MenuErrorCase("Menu Investidor");
                        }
                        else
                            break;
                    }
                    Investor.Deposit(amount);
                    Console.WriteLine("\n");
                    Console.WriteLine($"Foram depositados {amount} euros na sua wallet.");
                    Console.WriteLine("\n");
                    Investor.PrintPortfolio();
                    break;
                case "2":
                    string coinName;
                    int amount2;
                    while (true)
                    {
                        while (true)
                        {
                            //Console.Clear();
                            Exchange.PrintCoins();
                            Console.WriteLine("\n");
                            Console.WriteLine("Introduza a moeda que pretende comprar:");
                            coinName = Console.ReadLine();
                            coinName = coinName.ToUpper();
                            if (coinName == "EUR")
                            {
                                Console.WriteLine("Não é possível comprar euros!");
                                MenuErrorCase("Menu Investidor");
                            }
                            else
                            {
                                bool coinExist = Exchange.CheckIfCoinExists(coinName);
                                if (coinExist == false)
                                {
                                    Console.WriteLine("Essa moeda não existe.");
                                    MenuErrorCase("Menu Investidor");
                                }
                                else
                                    break;
                            }     
                        }
                        while (true)
                        {
                            Console.WriteLine("Introduza a quantidade que pretende comprar:");
                            bool isInt = int.TryParse(Console.ReadLine(), out amount2);
                            if (isInt == false || amount2 <= 0)
                            {
                                Console.WriteLine("Valor inválido. Por favor introduza um número inteiro positivo.");
                                MenuErrorCase("Menu Investidor");
                            }
                            else
                                break;
                        }
                        Investor.BuyCoin(coinName, amount2, ref error);
                        if (error != null)
                        {
                            Console.WriteLine(error);
                            error = null;
                            MenuErrorCase("Menu Investidor");
                        }
                        else
                            break;
                    }
                    Console.WriteLine("\n");
                    Investor.PrintPortfolio();
                    break;
                case "3":
                    string coinName2;
                    int amount3;
                    while (true)
                    {
                        while (true)
                        {
                            //Console.Clear();
                            Investor.PrintPortfolio();
                            Console.WriteLine("\n");
                            Console.WriteLine("Introduza a moeda que pretende vender:");
                            coinName2 = Console.ReadLine();
                            coinName2 = coinName2.ToUpper();
                            if (coinName2 == "EUR")
                            {
                                Console.WriteLine("Não é possível vender euros!");
                                MenuErrorCase("Menu Investidor");
                            }
                            else
                            {
                                bool coinExist = Exchange.CheckIfCoinExists(coinName2);
                                if (coinExist == false)
                                {
                                    Console.WriteLine("Essa moeda não existe.");
                                    MenuErrorCase("Menu Investidor");
                                }
                                else
                                    break;
                            }
                        }
                        while (true)
                        {
                            Console.WriteLine("Introduza a quantidade que pretende vender:");
                            bool isInt = int.TryParse(Console.ReadLine(), out amount3);
                            if (isInt == false && amount3 > 0)
                            {
                                Console.WriteLine("Valor inválido. Por favor introduza um número inteiro positivo.");
                                MenuErrorCase("Menu Investidor");
                            }
                            else
                                break;
                        }

                        Investor.SellCoin(coinName2, amount3, ref error);
                        if (error != null)
                        {
                            Console.WriteLine(error);
                            error = null;
                            MenuErrorCase("Menu Investidor");
                        }
                        else
                            break;
                    }
                    Console.WriteLine("\n");
                    Investor.PrintPortfolio();
                    break;
                case "4":
                    Investor.PrintPortfolio();
                    break;
                case "5":
                    Exchange.PrintCoins();
                    break;
                case "6":
                    Menu();
                    break;
                default:
                    InvestorMenu();
                    break;
            }

            Console.WriteLine("Prima qualquer tecla para voltar ao menu inicial.");
            Console.ReadLine();
            InvestorMenu();
        }

        static void AdministratorMenu()
        {
            UpdatePrices();
            //Console.Clear();
            string administrator = "menu administrador";
            Console.WriteLine(administrator.ToUpper());
            Console.WriteLine("\n");
            Console.WriteLine("Escolha uma das seguintes opções:\n");
            Console.WriteLine("--------------------------------");
            Console.WriteLine("1. Adicionar moeda");
            Console.WriteLine("2. Remover moeda");
            Console.WriteLine("3. Ver relatório de comissões");
            Console.WriteLine("4. Sair");

            string MenuOption = Console.ReadLine();

            Console.Clear();

            switch (MenuOption)
            {
                case "1":
                    string coinName;
                    while (true)
                    {
                        //Console.Clear();
                        Console.WriteLine("Introduza o nome da moeda que pretende adicionar:");
                        coinName = Console.ReadLine();
                        coinName = coinName.ToUpper();
                        bool coinExist = Exchange.CheckIfCoinExists(coinName);
                        if (coinExist == false)
                            break;
                        else
                        {
                            Console.WriteLine("Essa moeda já existe.");
                            MenuErrorCase("Menu Administrador");
                        }
                    }
                    Exchange.AddCoin(coinName);
                    Console.WriteLine($"A moeda {coinName} foi adicionada à base de dados.");
                    Console.WriteLine("\n");
                    Exchange.PrintCoins();
                    break;
                case "2":
                    string coinName2;
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("Introduza o nome da moeda que pretende remover:");
                        coinName2 = Console.ReadLine();
                        coinName2 = coinName2.ToUpper();
                        bool coinExist = Exchange.CheckIfCoinExists(coinName2);
                        if (coinExist == true)
                            break;
                        else
                        {
                            Console.WriteLine("Essa moeda não existe.");
                            MenuErrorCase("Menu Administrador");
                        }
                    }
                    Exchange.RemoveCoin(coinName2);
                    Console.WriteLine($"A moeda {coinName2} foi removida da base de dados.");
                    Console.WriteLine("\n");
                    Exchange.PrintCoins();
                    break;
                case "3":
                    Exchange.PrintCommissions();
                    break;
                case "4":
                    Menu();
                    break;
                default:
                    AdministratorMenu();
                    break;
            }

            Console.WriteLine("Prima qualquer tecla para voltar ao menu inicial.");
            Console.ReadLine();
            AdministratorMenu();
        }
        static void MenuErrorCase(string menuName)
        {
            Console.WriteLine("1) Voltar a tentar");
            Console.WriteLine($"2) Voltar ao {menuName}");

            string option = Console.ReadLine();

            if (option == "2")
            {
                if (menuName == "Menu Investidor")
                    InvestorMenu();
                else if (menuName == "Menu Administrador")
                    AdministratorMenu();
            }
        }

        // Criação de método que atualiza o valor das moedas de x em x segundos, entre -0,5% e 0,5%.
        static void UpdatePrices()
        {
            int secondsToUpdate = Exchange.GetPriceUpdateInSeconds();
            int secondsPassed = (int)Math.Round((DateTime.Now - start).TotalSeconds);

            if (secondsPassed > secondsToUpdate)
            {
                int nTimesToUpdate = secondsPassed / secondsToUpdate;

                Console.WriteLine(secondsPassed); //Para testar os segundos que passaram da última atualização. (comentar o console.clear dos menus para aparecerem estes cw)
                Console.WriteLine(nTimesToUpdate); //Para testar o número de vezes que tem de atualizar.

                for (int i = 0; i < nTimesToUpdate; i++)
                {
                    Exchange.UpdateCoinsPrices();
                    Console.WriteLine("fez update."); //Para testar número de updates.
                }
                start = DateTime.Now;
            }
        }
    }
}