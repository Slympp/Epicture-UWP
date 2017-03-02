using System;
using System.Collections.Generic;
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
using Windows.Web.Http;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Epicture {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page {
        public HomePage() {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            Params param = (Params)e.Parameter;
            if (param != null) {
                HelloText.Text = "Hello, " + param.auth0.CurrentUser.Profile["name"].ToString();



                //var client = new RestClient("https://api.imgur.com/3/account/me/images");
                //var request = new RestRequest(Method.GET);
                //request.AddHeader("content-type", "application/json");
                //IRestResponse response = client.Execute(request);
            }
            base.OnNavigatedTo(e);
        }
    }
}
