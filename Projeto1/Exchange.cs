using System;
using System.Collections.Generic;
using System.IO;

namespace TugaExchange
{
    public class Exchange
    {
        static int secondsToUpdatePrices;

        // Criação de uma lista com nome coins do tipo Coin.
        static List<Coin> coins = new List<Coin>();

        // Criação de uma lista com nome coins do tipo Commission.
        static List<Commission> commissions = new List<Commission>();

        //Criação de um método onde se verifica a criação de um objeto (newCoin) que é adicionado à lista coins.
        public static void AddCoin(string coinName)
        {
            Coin newCoin = new Coin { Name = coinName, Price = 1 };
            coins.Add(newCoin);
            Save();
        }

        // Criação de um método que permite a visualização do nome e do preço das moedas presentes na lista coin (câmbio).
        public static void PrintCoins()
        {
            foreach (Coin coin in coins)
                Console.WriteLine($"{coin.Name, 0}{coin.Price, 10} EUR");
        }

        // Criação de um método que permite a eliminação de uma determinada moeda à lista coins.
        public static void RemoveCoin(string coinName)
        {
            coins.RemoveAll(coin => coin.Name == coinName);
            Save();
        }

        // Criação de um método onde se verifica a atualização de todas as moedas com uma variação obtida aletaoriamente (entre -0,5% e 0,5%).
        public static void UpdateCoinsPrices()
        {
            foreach (Coin coin in coins)
            {
                decimal priceVariation = Utils.DecimalRandom();
                decimal newPrice = Math.Round(coin.Price * (1 + priceVariation / 100), 4);

                if (newPrice > 0 && coin.Name != "EUR")
                    coin.Price = newPrice;
            }
           Save();
        }

        // Criação de um método que retorna o preço duma moeda, obtida através do seu coinName. Vai ser usado na compra e venda da moeda.
        public static decimal GetCoinPrice(string coinName)
        {
            int index = coins.FindIndex(el => el.Name == coinName);
            return coins[index].Price;
        }

        // Criação de um método para adicionar a comissão na compra e na venda de moedas.
        public static void AddCommission(string coinName, decimal amount, string type)
        {
            Commission newCommission = new Commission
            {
                CoinName = coinName,
                Amount = amount,
                Type = type,
                Date = DateTime.Now.ToString("yyyy-MM-dd")
            };

            commissions.Add(newCommission);
            Save();
        }

        // Criação de um método que permite a visualização do relatório de comissões.
        public static void PrintCommissions()
        {
            foreach (Commission commission in commissions)
                Console.WriteLine($"{commission.Date, 0}{commission.CoinName, 10}{commission.Amount, 10}{commission.Type, 10}");
        }

        // Criação de um método para verificar se a moeda existe na lista de coins. É usado na validação de erros.
        public static bool CheckIfCoinExists(string name)
        {
            int index = coins.FindIndex(el => el.Name == name);

            if (index != -1)
                return true;
            else
                return false;
        }

        // Criação de um método que define o tempo em segundos em que há atualização das moedas.
        public static void DefinePriceUpdateInSeconds(int seconds)
        {
            secondsToUpdatePrices = seconds;
        }

        // Criação de um método que retorna o valor gerado anteriormente.
        public static int GetPriceUpdateInSeconds()
        {
            return secondsToUpdatePrices;
        }

        // Criação de um método que permite gravar os dados inseridos ou alterados no programa.
        // Neste caso a lista coins e a lista commissions.
        public static void Save()
        {
            string coinsJson = System.Text.Json.JsonSerializer.Serialize(coins);
            File.WriteAllText(@"c:\coins.json", coinsJson);

            string commissionsJson = System.Text.Json.JsonSerializer.Serialize(commissions);
            File.WriteAllText(@"c:\commissions.json", commissionsJson);
        }

        // Criação de um método que permite ler os dados previamente gravados, quando o programa se inicia.
        public static void Read()
        {
            string fileNameCoins = @"c:\coins.json";
            string fileNameComissions = @"c:\commissions.json";

            if (File.Exists(fileNameCoins))
            {
                var coinsJson = File.ReadAllText(fileNameCoins);
                var coinsDeserialized = System.Text.Json.JsonSerializer.Deserialize<List<Coin>>(coinsJson);
                coins = coinsDeserialized;
            }

            if (File.Exists(fileNameComissions))
            {
                var commissionsJson = File.ReadAllText(fileNameComissions);
                var commissionsDeserialized = System.Text.Json.JsonSerializer.Deserialize<List<Commission>>(commissionsJson);
                commissions = commissionsDeserialized;
            }
        }
    }
}
