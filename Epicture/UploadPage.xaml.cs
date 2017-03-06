using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class UploadPage : Page {
        public Windows.Storage.StorageFile file;
        public string endpoint;
        Params param;

        public UploadPage() {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            param = (Params)e.Parameter;
            base.OnNavigatedTo(e);
        }

        private async void SelectButton_Click(object sender, RoutedEventArgs e) {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".gif");
            picker.FileTypeFilter.Add(".mp4");

            file = await picker.PickSingleFileAsync();
            if (file != null) {
                pickerText.Text = "Selected file: " + file.Name;
            } else {
                pickerText.Text = "Operation cancelled.";
            }
        }

        private void GetImage() {
            endpoint += "&image=" + Uri.EscapeDataString(Convert.ToBase64String(File.ReadAllBytes(@file.Path)));
        }

        private async void UploadButton_Click(object sender, RoutedEventArgs e) {
            endpoint = "/3/upload?type=base64";

            //await Task.Run(() => GetImage());

            if (!String.IsNullOrWhiteSpace(titleBox.Text)) {
                endpoint += "&title=" + Uri.EscapeDataString(titleBox.Text);
            }

            if (!String.IsNullOrWhiteSpace(albumIdBox.Text)) {
                endpoint += "&album=" + Uri.EscapeDataString(albumIdBox.Text);
            }

            if (!String.IsNullOrWhiteSpace(descriptionBox.Text)) {
                endpoint += "&description=" + Uri.EscapeDataString(descriptionBox.Text);
            }


            // Sending post request
            Debug.WriteLine("Sending post request");

            using (HttpClient httpClient = new HttpClient()) {
                httpClient.BaseAddress = new Uri("https://api.imgur.com/");
                httpClient.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", param.access_token));
                httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("utf-8"));

                try {
                    //JsonConvert.SerializeObject(JsonObject)
                    HttpContent content = new StringContent("", Encoding.UTF8);
                    HttpResponseMessage response = await httpClient.PostAsync(endpoint, content);

                    Debug.WriteLine("Request: " + response.RequestMessage);
                    Debug.WriteLine("ReasonPhrase: " + response.ReasonPhrase);
                    if (response.IsSuccessStatusCode) {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        Debug.WriteLine("Json response: " + jsonResponse);
                    }
                    Debug.WriteLine("Request sended");

                }
                catch (Exception ex) {
                    uploadReturn.Text = "Could not connect to server.";
                    Debug.WriteLine("Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message);
                }
            }
        }
    }
}
