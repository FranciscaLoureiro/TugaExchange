using System;
using System.Collections.Generic;
using System.IO;

namespace TugaExchange
{
    class Investor
    {
        // Criação de uma lista com nome wallet do tipo Asset.
        static List<Asset> wallet = new List<Asset>();

        // Criação de um método que permite efetuar um depósito. Se não existir já uma quantia na wallet referente a um depósito em euros, a função cria um objeto e adiciona-o à lista.
        // Se já existir dinheiro na wallet, a função adiciona o montante depositado ao montante que já se encontra lá.
        public static void Deposit(int amount)
        {
            int index = wallet.FindIndex(el => el.CoinName == "EUR");

            if (index != -1) // Encontrou um index onde está o EUR, logo adiciona o montante recentemente depositado ao que já se encontra lá.
                wallet[index].Amount += amount;
            else
            {
                Asset euroAsset = new Asset { CoinName = "EUR", Amount = amount };
                wallet.Add(euroAsset);
            }

            Save();
        }

        // Criação de um método que permite a visualização do portfólio (quantidade, nome da moeda, preço unitário e total).
        public static void PrintPortfolio()
        {
            Console.WriteLine("Portfólio:");

            foreach (Asset asset in wallet)
            {
                decimal price = Exchange.GetCoinPrice(asset.CoinName);

                decimal amountEur = asset.Amount * price;
                Console.WriteLine($"{Math.Round(asset.Amount,2), 5}{asset.CoinName, 10}{price, 10}{Math.Round(amountEur, 2), 10} EUR");
            }
        }

        // Criação de um método que permite comprar moeda.
        public static void BuyCoin(string coinName, int amount, ref string error)
        {
            // Verificar se há dinheiro para a compra.
            int indexEur = wallet.FindIndex(el => el.CoinName == "EUR");

            // A compra só é válida se se verificarem 2 condições: se existe um depósito em EUR na wallet e, se existir, esse montante tem de ser superior ao valor da compra.
            if (indexEur == -1)
            {
                error = "Necessita de fazer um depósito antes de efetuar uma compra.";
                return;
            }

            if (wallet[indexEur].Amount < amount)
            {
                error = "Não tem saldo suficiente para realizar a compra.";
                return;
            }

            // Ir buscar o preço da moeda que se quer comprar.
            decimal coinPrice = Exchange.GetCoinPrice(coinName);

            // Verificar a quantidade de moedas a comprar.
            decimal coinAmount = (amount * 0.99M) / coinPrice;

            // Retirar da wallet o montante da transação.
            //int euroindex = wallet.FindIndex(el => el.CoinName == "EUR");
            wallet[indexEur].Amount -= amount;

            // Adicionar a quantidade da moeda que se comprou à wallet.
            int indexCoin = wallet.FindIndex(el => el.CoinName == coinName);

            if (indexCoin != -1)
                wallet[indexCoin].Amount += amount;
            else
            {
                Asset euroAsset = new Asset { CoinName = coinName, Amount = amount };
                wallet.Add(euroAsset);
            }

            // Acrescentar a comissão da compra efetuada, para posterior impressão no relatório de comissões.
            Exchange.AddCommission(coinName, Math.Round((amount * 0.01M), 2), "BUY");

            Exchange.Save();
            Save();
        }

        // Criação de um método que permite vender moeda.
        public static void SellCoin(string coinName, int amount, ref string error)
        {
            // Ir buscar o preço da moeda que se quer vender.
            decimal coinPrice = Exchange.GetCoinPrice(coinName);

            // Verificar a quantidade de moedas a vender.
            decimal eurAmount = (amount * coinPrice) * 0.99M;

            int indexCoin = wallet.FindIndex(el => el.CoinName == coinName);

            // A venda só é válida se se verificarem 2 condições: se existir a moeda que se pretende vender no portfólido do investidor;
            // e se a quantidade que se pretende vender é igual ou inferior à quantidade de moeda presente no portfólio.
            if (indexCoin == -1)
            {
                error = "A moeda que pretende vender não consta no seu portfólio.";
                return;
            }
            if (amount > wallet[indexCoin].Amount)
            {
                error = "A quantidade de venda excede o que possui em carteira.";
                return;
            }

            // Retirar a quantidade da moeda que se vendeu à wallet.
            wallet[indexCoin].Amount -= amount;

            // Acrescentar à wallet o montante da transação.
            int indexEur = wallet.FindIndex(el => el.CoinName == "EUR");
            wallet[indexEur].Amount += eurAmount;

            // Acrescentar a comissão da venda efetuada, para posterior impressão no relatório de comissões.
            Exchange.AddCommission(coinName, Math.Round((amount * coinPrice) * 0.01M, 2), "SELL");

            Exchange.Save();
            Save();
        }

        // Criação de um método que permite gravar os dados inseridos ou alterados no programa.
        // Neste caso a lista wallet.
        public static void Save()
        {
            string walletJson = System.Text.Json.JsonSerializer.Serialize(wallet);
            File.WriteAllText(@"c:\wallet.json", walletJson);
        }

        // Criação de um método que permite ler os dados previamente gravados, quando o programa se inicia.
        public static void Read()
        {
            string fileNameWallet = @"c:\wallet.json";

            if (File.Exists(fileNameWallet))
            {
                var walletJson = File.ReadAllText(fileNameWallet);
                var walletDeserialized = System.Text.Json.JsonSerializer.Deserialize<List<Asset>>(walletJson);
                wallet = walletDeserialized;
            }
        }
    }
}
