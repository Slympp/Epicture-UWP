using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
        Params param;

        public ProfilePage() {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            param = (Params)e.Parameter;
            if (param != null) {
                FetchData();
            }
            base.OnNavigatedTo(e);
        }

        private async void FetchData() {
            string response = null;
            try {
                Imgur imgur = new Imgur();

                Uri albumsURI = new Uri("https://api.imgur.com/3/account/" + param.account_username);
                await imgur.GetRequest(albumsURI, param.access_token);
                response = imgur.response;

                var profile = Newtonsoft.Json.JsonConvert.DeserializeObject<Account>(response);

                if (!String.IsNullOrWhiteSpace(profile.data.avatar)) {
                    BitmapImage bitmap = new BitmapImage(new Uri(profile.data.avatar, UriKind.Absolute));
                    Avatar.Source = bitmap;
                    Debug.WriteLine("Url");
                } else {
                    Debug.WriteLine("Statique");
                    Avatar.Source = new BitmapImage(new Uri("ms-appx:///Assets/person.png"));
                }

                TextBlock Username = new TextBlock();
                Username.Text = profile.data.url + " (" + profile.data.reputation.ToString() + ")";
                Username.SetValue(TextBlock.MarginProperty, new Thickness(0, 0, 0, 5));
                Username.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                MainPanel.Children.Add(Username);

                TextBlock MemberSince = new TextBlock();
                MemberSince.SetValue(TextBlock.FontSizeProperty, 12);
                MemberSince.SetValue(TextBlock.MarginProperty, new Thickness(0, 0, 0, 10));
                System.DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                dt = dt.AddSeconds(profile.data.created).ToLocalTime();
                CultureInfo culture = new CultureInfo("en-US");
                MemberSince.Text = "Member since " + dt.ToString("D", culture);
                MemberSince.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                MainPanel.Children.Add(MemberSince);

                TextBlock Bio = new TextBlock();
                if (!String.IsNullOrWhiteSpace(profile.data.bio)) {
                    Bio.Text = profile.data.bio;
                } else {
                    Bio.Text = "This user has not filled his bio yet";
                }
                Bio.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                MainPanel.Children.Add(Bio);

            }
            catch (Exception ex) {
                Debug.WriteLine("Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message);
            }
        }
    }

    public class UserFollow {
        public bool status { get; set; }
    }

    public class AccountData {
        public int id { get; set; }
        public string url { get; set; }
        public string bio { get; set; }
        public string avatar { get; set; }
        public int reputation { get; set; }
        public string reputation_name { get; set; }
        public int created { get; set; }
        public bool pro_expiration { get; set; }
        public UserFollow user_follow { get; set; }
    }

    public class Account {
        public AccountData data { get; set; }
        public bool success { get; set; }
        public int status { get; set; }
    }
}
