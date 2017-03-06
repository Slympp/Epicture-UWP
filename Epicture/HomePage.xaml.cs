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
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
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
            string response = null;
            try {
                Imgur imgur = new Imgur();

                Uri albumsURI = new Uri("https://api.imgur.com/3/account/" + param.account_username + "/albums/");
                await imgur.GetRequest(albumsURI, param.access_token);
                response = imgur.response;

                var albums = Newtonsoft.Json.JsonConvert.DeserializeObject<Albums>(response);
                if (albums != null) {
                    foreach (AlbumDatum a in albums.data) {
                        
                        TextBlock albumTitle = new TextBlock();
                        albumTitle.Text = a.title.ToUpper() + " (" + a.id + ")";
                        albumTitle.SetValue(TextBlock.FontWeightProperty, FontWeights.Bold);
                        albumTitle.SetValue(TextBlock.FontSizeProperty, 18);
                        albumTitle.SetValue(TextBlock.MarginProperty, new Thickness(10, 10, 10, 10));
                        albumTitle.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                        MainPanel.Children.Add(albumTitle);

                        if (!String.IsNullOrWhiteSpace(a.description)) {
                            TextBlock albumDescription = new TextBlock();
                            albumDescription.Text = a.description;
                            albumDescription.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                            MainPanel.Children.Add(albumDescription);
                        }

                        if (a.images_count > 0) {
                            Uri singleAlbumURI = new Uri("https://api.imgur.com/3/album/" + a.id + "/images");

                            await imgur.GetRequest(singleAlbumURI, param.access_token);
                            response = imgur.response;

                            var images = Newtonsoft.Json.JsonConvert.DeserializeObject<Images>(response);
                            if (images != null) {

                                foreach (ImageDatum i in images.data) {
                                    Image img = new Image();
                                    BitmapImage src = new BitmapImage(new Uri(i.link, UriKind.Absolute));

                                    if (i.animated == true)
                                        src.SetValue(BitmapImage.AutoPlayProperty, true);

                                    img.Source = src;
                                    img.SetValue(Image.MaxHeightProperty, 500);
                                    img.SetValue(Image.MaxWidthProperty, 500);
                                    img.SetValue(TextBlock.MarginProperty, new Thickness(10, 20, 10, 20));
                                    MainPanel.Children.Add(img);

                                    if (!String.IsNullOrWhiteSpace(i.title)) {
                                        TextBlock imageTitle = new TextBlock();
                                        imageTitle.Text = i.title.ToUpper();
                                        imageTitle.SetValue(TextBlock.FontWeightProperty, FontWeights.Bold);
                                        imageTitle.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                                        MainPanel.Children.Add(imageTitle);
                                    }

                                    if (!String.IsNullOrWhiteSpace(i.description)) {
                                        TextBlock imageDescription = new TextBlock();
                                        imageDescription.Text = i.description;
                                        imageDescription.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                                        imageDescription.SetValue(TextBlock.MarginProperty, new Thickness(0, 0, 0, 10));
                                        MainPanel.Children.Add(imageDescription);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                Debug.WriteLine("Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message);
            }
        }
    }

    public class AlbumDatum {
        public string id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int datetime { get; set; }
        public string cover { get; set; }
        public int cover_width { get; set; }
        public int cover_height { get; set; }
        public string account_url { get; set; }
        public int account_id { get; set; }
        public string privacy { get; set; }
        public string layout { get; set; }
        public int views { get; set; }
        public string link { get; set; }
        public bool favorite { get; set; }
        public object nsfw { get; set; }
        public object section { get; set; }
        public int images_count { get; set; }
        public bool in_gallery { get; set; }
        public bool is_ad { get; set; }
        public string deletehash { get; set; }
        public int order { get; set; }
    }

    public class Albums {
        public List<AlbumDatum> data { get; set; }
        public bool success { get; set; }
        public int status { get; set; }
    }

    public class ImageDatum {
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
        public object vote { get; set; }
        public bool favorite { get; set; }
        public object nsfw { get; set; }
        public object section { get; set; }
        public object account_url { get; set; }
        public object account_id { get; set; }
        public bool is_ad { get; set; }
        public List<object> tags { get; set; }
        public bool in_gallery { get; set; }
        public string deletehash { get; set; }
        public string name { get; set; }
        public string link { get; set; }
        public string gifv { get; set; }
        public string mp4 { get; set; }
        public int? mp4_size { get; set; }
        public bool? looping { get; set; }
    }

    public class Images {
        public List<ImageDatum> data { get; set; }
        public bool success { get; set; }
        public int status { get; set; }
    }
}
