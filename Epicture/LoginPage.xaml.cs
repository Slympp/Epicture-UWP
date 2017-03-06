using Auth0.LoginClient;
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
using System.Threading;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Epicture {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page {
        Params param;
        private string client_id = "236d67f77afbc1d";
        AutoResetEvent waitForNavComplete;

        public LoginPage() {
            this.InitializeComponent();
            waitForNavComplete = new AutoResetEvent(false);
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e) {
            Uri requestUri = new Uri("https://api.imgur.com/oauth2/authorize?response_type=token&client_id=" + client_id);

            // Show the pop up
            HomeContent.Visibility = Visibility.Collapsed;
            MyWebView.Visibility = Visibility.Visible;

            MyWebView.Navigate(requestUri);

            await Task.Run(() => { waitForNavComplete.WaitOne(); });

            waitForNavComplete.Reset();
            MyWebView.Visibility = Visibility.Collapsed;
            HomeContent.Visibility = Visibility.Visible;
        }

        private void MyWebView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args) {
            if (args.IsSuccess == true && args.Uri == new Uri("https://www.google.fr/")) {
                waitForNavComplete.Set();

                String[] Results = args.Uri.ToString().Substring(args.Uri.ToString().IndexOf('#') + 1).Split('&');
                string access_token, refresh_token, expires_in, token_type, account_username, account_id;
                access_token = refresh_token = expires_in = token_type = account_username = account_id = null;

                for (int i = 0; i < Results.Length; i++) {
                    String[] splits = Results[i].Split('=');
                    switch (splits[0]) {
                        case "access_token":
                            access_token = splits[1];
                            break;
                        case "refresh_token":
                            refresh_token = splits[1];
                            break;
                        case "expires_in":
                            expires_in = splits[1];
                            break;
                        case "token_type":
                            token_type = splits[1];
                            break;
                        case "account_username":
                            account_username = splits[1];
                            break;
                        case "account_id":
                            account_id = splits[1];
                            break;
                    }
                }
                if (access_token != null) {
                    param = new Params { access_token = access_token, refresh_token = refresh_token, expires_in = expires_in, token_type = token_type, account_username = account_username, account_id = account_id };
                    this.Frame.Navigate(typeof(MainPage), param);
                } else {
                    textResponse.Text = "Failed to connect.";
                }
            } else {
                textResponse.Text = "Failed to connect (" + args.WebErrorStatus.ToString() + ").";
            }
        }

        private void MyWebView_NavigationFailed(object sender, WebViewNavigationFailedEventArgs e) {
            textResponse.Text = "Failed to navigate.";
        }
    }
}
