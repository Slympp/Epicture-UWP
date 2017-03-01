using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using System.Diagnostics;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Epicture {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page {

        private string Output;

        public LoginPage() {
            this.InitializeComponent();
        }

        private void NotifyUser(String s) {
            Debug.WriteLine(s);
        }

        private async Task<string> SendDataAsync(String Url) {
            try {
                HttpClient httpClient = new HttpClient();
                return await httpClient.GetStringAsync(new Uri(Url));
            }
            catch (Exception Err) {
               NotifyUser("Error getting data from server." + Err.Message);
            }

            return null;
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e) {

            try {
                // Acquiring a request token
                TimeSpan SinceEpoch = DateTime.UtcNow - new DateTime(1970, 1, 1);
                Random Rand = new Random();

                String FlickrClientID = "1b6c20b3266a529176d406f1016534d3";
                String FlickrClientSecret = "7aa1c42fa1d0e066";
                String FlickrUrl = "https://secure.flickr.com/services/oauth/request_token";
                String FlickrCallbackUrl = "https://google.com/";

                Int32 Nonce = Rand.Next(1000000000);

                // Compute base signature string and sign it.
                // This is a common operation that is required for all requests even after the token is obtained.
                // Parameters need to be sorted in alphabetical order
                // Keys and values should be URL Encoded.
                String SigBaseStringParams = "oauth_callback=" + Uri.EscapeDataString(FlickrCallbackUrl);
                SigBaseStringParams += "&" + "oauth_consumer_key=" + FlickrClientID;
                SigBaseStringParams += "&" + "oauth_nonce=" + Nonce.ToString();
                SigBaseStringParams += "&" + "oauth_signature_method=HMAC-SHA1";
                SigBaseStringParams += "&" + "oauth_timestamp=" + Math.Round(SinceEpoch.TotalSeconds);
                SigBaseStringParams += "&" + "oauth_version=1.0";
                String SigBaseString = "GET&";
                SigBaseString += Uri.EscapeDataString(FlickrUrl) + "&" + Uri.EscapeDataString(SigBaseStringParams);

                IBuffer KeyMaterial = CryptographicBuffer.ConvertStringToBinary(FlickrClientSecret + "&", BinaryStringEncoding.Utf8);
                MacAlgorithmProvider HmacSha1Provider = MacAlgorithmProvider.OpenAlgorithm("HMAC_SHA1");
                CryptographicKey MacKey = HmacSha1Provider.CreateKey(KeyMaterial);
                IBuffer DataToBeSigned = CryptographicBuffer.ConvertStringToBinary(SigBaseString, BinaryStringEncoding.Utf8);
                IBuffer SignatureBuffer = CryptographicEngine.Sign(MacKey, DataToBeSigned);
                String Signature = CryptographicBuffer.EncodeToBase64String(SignatureBuffer);

                FlickrUrl += "?" + SigBaseStringParams + "&oauth_signature=" + Uri.EscapeDataString(Signature);
                string GetResponse = await SendDataAsync(FlickrUrl);
                NotifyUser("Received Data: " + GetResponse);


                if (GetResponse != null) {
                    String oauth_token = null;
                    String oauth_token_secret = null;
                    String[] keyValPairs = GetResponse.Split('&');

                    for (int i = 0; i < keyValPairs.Length; i++) {
                        String[] splits = keyValPairs[i].Split('=');
                        switch (splits[0]) {
                            case "oauth_token":
                                oauth_token = splits[1];
                                break;
                            case "oauth_token_secret":
                                oauth_token_secret = splits[1];
                                break;
                        }
                    }

                    if (oauth_token != null) {

                        NotifyUser("Oauth: " + oauth_token + "\nSecret: " + oauth_token_secret);

                        FlickrUrl = "https://secure.flickr.com/services/oauth/authorize?oauth_token=" + oauth_token + "&perms=read";
                        System.Uri StartUri = new Uri(FlickrUrl);
                        System.Uri EndUri = new Uri(FlickrCallbackUrl);

                        NotifyUser("Navigating to: " + FlickrUrl);
                        WebAuthenticationResult WebAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(
                                                                WebAuthenticationOptions.UseTitle,
                                                                StartUri,
                                                                EndUri);
                        if (WebAuthenticationResult.ResponseStatus == WebAuthenticationStatus.Success) {
                            Output = WebAuthenticationResult.ResponseData.ToString();
                        } else if (WebAuthenticationResult.ResponseStatus == WebAuthenticationStatus.ErrorHttp) {
                            Output = "HTTP Error returned by AuthenticateAsync() : " + WebAuthenticationResult.ResponseErrorDetail.ToString();
                        } else {
                            Output = "Error returned by AuthenticateAsync() : " + WebAuthenticationResult.ResponseStatus.ToString();
                        }

                        Debug.WriteLine(Output);
                    }
                }
            }
            catch (Exception Error) {
                NotifyUser(Error.Message);
            }
        }
    }
}
