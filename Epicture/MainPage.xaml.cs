using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Epicture
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private bool connected = false;

        public MainPage()
        {
            this.InitializeComponent();

            if (connected) {
                MenuButtonHome.IsEnabled = true;
                MenuButtonUpload.IsEnabled = true;
                MenuButtonSearch.IsEnabled = true;
                MenuButtonProfile.IsEnabled = true;
                MenuButtonLogout.IsEnabled = true;
                contentFrame.Navigate(typeof(HomePage), new Params() { oauth_token = "token", api_key = "key" });
            } else {
                MenuButtonHome.IsEnabled = false;
                MenuButtonUpload.IsEnabled = false;
                MenuButtonSearch.IsEnabled = false;
                MenuButtonProfile.IsEnabled = false;
                MenuButtonLogout.IsEnabled = false;
                contentFrame.Navigate(typeof(LoginPage));
            }
        }

        internal void NotifyUser(string v, object statusMessage) {
            throw new NotImplementedException();
        }

        private void OpenPaneButton_Click(object sender, RoutedEventArgs e) {
            MainSplitView.IsPaneOpen = !MainSplitView.IsPaneOpen;
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e) {
            contentFrame.Navigate(typeof(HomePage), new Params() { oauth_token = "token", api_key="key" });
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e) {
            contentFrame.Navigate(typeof(UploadPage), new Params() { oauth_token = "token", api_key = "key" });
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e) {
            contentFrame.Navigate(typeof(SearchPage), new Params() { oauth_token = "token", api_key = "key" });
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e) {
            contentFrame.Navigate(typeof(ProfilePage), new Params() { oauth_token = "token", api_key = "key" });
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e) {
            Debug.WriteLine("Logout");
        }
    }

    public class Params {

        public string oauth_token { get; set; }
        public string api_key { get; set; }
    }
}
