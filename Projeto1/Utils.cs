using System;
using System.IO;

namespace TugaExchange
{
    class Utils
    {
        // Criação de um método com uma variável random que oscila entre -0,5 e 0,5 e que devolve essa variação.
        public static decimal DecimalRandom()
        {
            var rnd = new Random();
            int valorInteiro = rnd.Next(-5, 5);
            decimal variation = Convert.ToDecimal(valorInteiro) / 10;

            return variation;
        }

        public static void DeleteAllFiles()
        {
            string fileNameCoins = @"C:\coins.json";
            string fileNameCommissions = @"C:\commissions.json";
            string fileNameWallet = @"C:\wallet.json";

            if (File.Exists(fileNameCoins))
            {
                File.Delete(fileNameCoins);
            }

            if (File.Exists(fileNameCommissions))
            {
                File.Delete(fileNameCommissions);
            }

            if (File.Exists(fileNameWallet))
            {
                File.Delete(fileNameWallet);
            }
        }
    }
}
