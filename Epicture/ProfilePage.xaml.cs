using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Epicture {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProfilePage : Page {
        public ProfilePage() {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            Params param = (Params)e.Parameter;

            if (param != null) {
                PictureImage.Source = new BitmapImage(new Uri(param.auth0.CurrentUser.Profile["picture"].ToString(), UriKind.Absolute)); 
                NameText.Text = "Name: " + param.auth0.CurrentUser.Profile["name"].ToString();
                NicknameText.Text = "Nickname: " + param.auth0.CurrentUser.Profile["nickname"].ToString();
                EmailText.Text = "Email: " + param.auth0.CurrentUser.Profile["email"].ToString();

                if ((bool)param.auth0.CurrentUser.Profile["email_verified"] == true) {
                    EmailVerifiedText.Text = "Email address is verified";
                    EmailVerifiedText.Foreground = new SolidColorBrush(Colors.Green);
                } else {
                    EmailVerifiedText.Text = "Email address is not verified";
                    EmailVerifiedText.Foreground = new SolidColorBrush(Colors.Red);
                }
            }
            base.OnNavigatedTo(e);
        }
    }
}
