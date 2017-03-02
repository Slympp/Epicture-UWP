using Auth0.LoginClient;
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
        Params param = null;

        public MainPage()
        {
            this.InitializeComponent();            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {

            param = e.Parameter as Params;
            if (param != null) {
                contentFrame.Navigate(typeof(HomePage), param);
            }
            base.OnNavigatedTo(e);
        }

        private void OpenPaneButton_Click(object sender, RoutedEventArgs e) {
            MainSplitView.IsPaneOpen = !MainSplitView.IsPaneOpen;
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e) {
            contentFrame.Navigate(typeof(HomePage), param);
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e) {
            contentFrame.Navigate(typeof(UploadPage), param);
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e) {
            contentFrame.Navigate(typeof(SearchPage), param);
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e) {
            contentFrame.Navigate(typeof(ProfilePage), param);
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e) {

            //TODO: call to param.auth0 logout
            param = null;
            this.Frame.Navigate(typeof(LoginPage));
        }
    }

    public class Params {

        public Auth0Client auth0 { get; set; }
    }
}
