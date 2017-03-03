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


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Epicture {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page {

        public LoginPage() {
            this.InitializeComponent();
        }

        private async void Login() {

            Auth0Client auth0 = new Auth0Client("slymp.eu.auth0.com", "hXdMFb2pqd1a0gz9bdtCJP43YB8IyARh");
            Auth0User user = await auth0.LoginAsync();

            if (user != null)
                this.Frame.Navigate(typeof(MainPage), new Params() { auth0 = auth0 });
            else
                textResponse.Text = "Connection failed.";
        }

        private  void LoginButton_Click(object sender, RoutedEventArgs e) {
            Login();
        }
    }
}
