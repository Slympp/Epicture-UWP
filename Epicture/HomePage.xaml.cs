using Newtonsoft.Json.Linq;
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
        Params param;

        public HomePage() {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            param = (Params)e.Parameter;
            if (param != null) {
                HelloText.Text = "Hello, " + param.account_username;
                FetchData();
            }
            base.OnNavigatedTo(e);
        }

        private async void FetchData() {

            Uri requestUri = new Uri("	https://api.imgur.com/3/account/" + param.account_username + "/albums/");

            string response = null;
            try {
                Imgur imgur = new Imgur();

                await imgur.GetRequest(requestUri, param.access_token);
                response = imgur.response;
                Debug.WriteLine("response: " + response);

                // Doesn't work yet :/

                RootObject d = Newtonsoft.Json.JsonConvert.DeserializeObject<RootObject>(response);

                JObject obj_response = JObject.Parse(response);
                IList<JToken> results = obj_response["data"].Children().ToList();
                IList<Data> albums = new List<Data>();

                foreach (JToken result in results) {
                    Data tmp = Newtonsoft.Json.JsonConvert.DeserializeObject<Data>(result.ToString());
                    albums.Add(tmp);
                }

                if (albums != null) {
                    Debug.WriteLine("albums exist");
                }
            }
            catch (Exception ex) {
                response = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
            }
        }
    }

    public class Image {
        public string id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int datetime { get; set; }
        public string type { get; set; }
        public bool animated { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int size { get; set; }
        public int views { get; set; }
        public int bandwidth { get; set; }
        public string link { get; set; }
    }

    public class Data {
        public string id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
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

    public class RootObject {
        public Data data { get; set; }
        public bool success { get; set; }
        public int status { get; set; }
    }
}
