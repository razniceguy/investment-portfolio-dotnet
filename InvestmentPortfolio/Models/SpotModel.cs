using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Globalization;
using System.Text;
using System;
using System.Linq;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Core;

namespace InvestmentPortfolio.Models
{
    public class SpotModel
    {
        private readonly HttpClient _httpClient;

        public DailyAccount Account { get; set; }

        public SpotModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DailyAccount> OnGet()
        {
            
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            string timeStamp = unixTimestamp.ToString() + "000";

            string queryParam = "type=SPOT&timestamp=" + timeStamp;

            string signature = CreateHashMACSHA256(queryParam, GetSecret("X-MBX-API-SECRET"));

            string url = "https://api.binance.com/sapi/v1/accountSnapshot?" + queryParam + "&signature=" + signature;

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            HttpClient httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json"); 
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-MBX-APIKEY", GetSecret("X-MBX-API-SECRET"));

            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                Account = await JsonSerializer.DeserializeAsync<DailyAccount>(responseStream);
            }
            else 
            { 
                Account = new DailyAccount();
            }

            return Account;
            
        }

        public string CreateHashMACSHA256(string message, string key)
        {

            Encoding ascii = Encoding.ASCII;
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            Byte[] key_bytes = ascii.GetBytes(key);

            Byte[] message_bytes = ascii.GetBytes(message);

            HMACSHA256 hmacSHA256 = new HMACSHA256(key_bytes);

            Byte[] hash = hmacSHA256.ComputeHash(message_bytes);
            String data = this.ByteToString(hash);

            return data;
        }

        private String ByteToString(Byte[] buff)
        {
            Char[] retval = new char[buff.Length * 2];
            for (int i = 0; i < buff.Length; i++)
            {
                String t = buff[i].ToString("X2");
                retval[i * 2] = t[0];
                retval[i * 2 + 1] = t[1];
            }
            return new String(retval);
        }

        public string GetSecret(string secretName)
        {
            SecretClientOptions options = new SecretClientOptions()
            {
                Retry =
                {
                    Delay= TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(16),
                    MaxRetries = 5,
                    Mode = RetryMode.Exponential
                }
            };
            var client = new SecretClient(new Uri("https://investmentportfoliokeys.vault.azure.net/"), new DefaultAzureCredential(), options);

            KeyVaultSecret secret = client.GetSecret(secretName);

            return secret.Value;
        }


    }
}
