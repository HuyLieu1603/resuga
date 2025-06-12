using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PD_Store.Helper
{
    public class VnPayHelper
    {
        public bool ValidateSignature(IQueryCollection query, string hashSecret)
        {
            var sorted = query
                .Where(k => k.Key.StartsWith("vnp_") && k.Key != "vnp_SecureHash")
                .OrderBy(k => k.Key)
                .ToDictionary(k => k.Key, v => v.Value.ToString());

            string raw = string.Join("&", sorted.Select(kv => $"{kv.Key}={kv.Value}"));
            string hash = HmacSHA512(hashSecret, raw);

            return hash.Equals(query["vnp_SecureHash"], StringComparison.InvariantCultureIgnoreCase);
        }

        public static string HmacSHA512(string key, string inputData)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var inputBytes = Encoding.UTF8.GetBytes(inputData);

            using (var hmac = new HMACSHA512(keyBytes))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);
                StringBuilder hex = new StringBuilder(hashValue.Length * 2);
                foreach (byte b in hashValue)
                    hex.AppendFormat("{0:x2}", b);
                return hex.ToString();
            }
        }

    }
}