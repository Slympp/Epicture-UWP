using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Epicture {
    public class Imgur {
        public string response { get; set; }

        public async Task GetRequest(Uri requestUri, string token) {
            HttpClient httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));
            var httpResponseMessage = await httpClient.GetAsync(requestUri);
            response = await httpResponseMessage.Content.ReadAsStringAsync();

            if (response != null) {
                Debug.WriteLine("____________Response_______________\n" + response + "\n_______________________\n");
            } else {
                // Error handling
            }
        }
    }
}
