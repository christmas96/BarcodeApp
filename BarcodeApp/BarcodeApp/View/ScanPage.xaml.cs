using BarcodeApp.Helpers;
using BarcodeApp.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;

namespace BarcodeApp.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ScanPage : ContentPage
	{
		public ScanPage ()
		{
			InitializeComponent ();
		}

        public async void Handle_OnScanResult(Result result)
        {
            Barcode bc = new Barcode();
            try
            {
                var content = "";
                HttpClient client = new HttpClient();
                var RestURL = string.Format(StationManager.Url + "/api/values/" + result.Text);
                client.BaseAddress = new Uri(RestURL);
                client.Timeout = TimeSpan.FromSeconds(20);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(RestURL);
                content = await response.Content.ReadAsStringAsync();
                bc = JsonConvert.DeserializeObject<Barcode>(content);
            }
            catch (HttpRequestException e)
            {
                await DisplayAlert("Error", e.ToString(), "Ok");
            }
            catch (JsonSerializationException e)
            {
                await DisplayAlert("Error", e.ToString(), "Ok");
            }
            if (bc != null)
            {
                StationManager.CurrentProduct = bc;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PushModalAsync(new MainView());
                });
            }
            else
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayAlert("Sorry, nothing was found(", result.Text, "OK");
                });
            }            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _scanView.IsScanning = true;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
    }
}