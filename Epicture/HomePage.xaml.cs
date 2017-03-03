using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Epicture {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page {

        private string access_token = null;
        private string expires_in = null;

        public HomePage() {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            Params param = (Params)e.Parameter;
            if (param != null) {
                HelloText.Text = "Hello, " + param.auth0.CurrentUser.Profile["name"].ToString();
                GetAccessToken();
            }
            base.OnNavigatedTo(e);
        }

        private async void GetAccessToken() {

            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();

            Uri requestUri = new Uri("https://api.imgur.com/oauth2/authorize?response_type=token&client_id=236d67f77afbc1d");

            //Send the GET request asynchronously and retrieve the response as a string.
            //Windows.Web.Http.HttpResponseMessage httpResponse = new Windows.Web.Http.HttpResponseMessage();

            string httpResponseBody = "";
            try {
                //Send the GET request
                //httpResponse = await httpClient.GetAsync(requestUri);
                //httpResponse.EnsureSuccessStatusCode();
                string GetResponse = await httpClient.GetStringAsync(requestUri);

                Debug.WriteLine("____________Response_______________\n" + GetResponse + "\n_______________________\n");

                if (GetResponse != null) {
                    String[] keyValPairs = GetResponse.Split('&');

                    for (int i = 0; i < keyValPairs.Length; i++) {
                        String[] splits = keyValPairs[i].Split('=');
                        switch (splits[0]) {
                            case "access_token":
                                access_token = splits[1];
                                break;
                            case "expires_in":
                                expires_in = splits[1];
                                break;
                        }
                    }

                    if (access_token != null) {
                        Debug.WriteLine("Imgur AccessToken: " + access_token);
                    }
                }


                //Album d = Newtonsoft.Json.JsonConvert.DeserializeObject<Album>(httpResponseBody);
                //Debug.WriteLine("TEST CONVERTED: " + d.account_id);
            }
            catch (Exception ex) {
                httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
            }
        }
    }

    // JSON CONTAINERS

    public class Image {
        public string id { get; set; }
        public object title { get; set; }
        public object description { get; set; }
        public int datetime { get; set; }
        public string type { get; set; }
        public bool animated { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int size { get; set; }
        public int views { get; set; }
        public object bandwidth { get; set; }
        public string link { get; set; }
    }

    public class Album {
        public string id { get; set; }
        public string title { get; set; }
        public object description { get; set; }
        public int datetime { get; set; }
        public string cover { get; set; }
        public string account_url { get; set; }
        public int account_id { get; set; }
        public string privacy { get; set; }
        public string layout { get; set; }
        public int views { get; set; }
        public string link { get; set; }
        public int images_count { get; set; }
        public List<Image> images { get; set; }
    }
}
