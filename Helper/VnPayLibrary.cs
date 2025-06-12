using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Web;


namespace PD_Store.Helper
{
    public class VnPayLibrary
    {
        private SortedList<string, string> _requestData = new SortedList<string, string>();

        public void AddRequestData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _requestData.Add(key, value);
            }
        }

        public string CreateRequestUrl(string baseUrl, string hashSecret)
        {
            var sortedData = _requestData.OrderBy(x => x.Key);
            StringBuilder data = new StringBuilder();
            StringBuilder query = new StringBuilder();

            foreach (KeyValuePair<string, string> kv in _requestData)
            {
                if (query.Length > 0)
                {
                    query.Append('&');
                }

                query.Append(HttpUtility.UrlEncode(kv.Key) + "=" + HttpUtility.UrlEncode(kv.Value));
                data.Append(kv.Key + "=" + kv.Value + "&");
            }

            string signData = data.ToString().TrimEnd('&');
            string secureHash = HmacSHA512(hashSecret, signData);

            return $"{baseUrl}?{query}&vnp_SecureHash={secureHash}";
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