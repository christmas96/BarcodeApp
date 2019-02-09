using BarcodeApp.Helpers;
using BarcodeApp.Models;
using BarcodeApp.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace BarcodeApp.ViewModel
{
    public class MainPageViewModel : ContentPage, INotifyPropertyChanged
    {
        private INavigation _navigation;
        private Command<object> _buttonCommand;
        private List<Barcode> _listOfProducts;
        public INavigation Navigation
        {
            get { return _navigation; }
            set { _navigation = value; }
        }

        public Command<object> ButtonCommand
        {
            get => _buttonCommand;
            set
            {
                _buttonCommand = value;
            }
        }

        public List<Barcode> ListOfProducts
        {
            get => _listOfProducts;
            set
            {
                _listOfProducts = value;
                NotifyPropertyChanged();
            }
        }

        public string ButtonText
        {
            get => "Push me to scan!";
        }

        public MainPageViewModel(INavigation navigation)
        {
            _navigation = navigation;
            _buttonCommand = new Command<object>(GoScan);
            GetProducts();
        }

        public void GoScan(object o)
        {
            Navigation.PushAsync(new ScanPage());
        }

        public async void GetProducts()
        {
            try
            {
                var content = "";
                HttpClient client = new HttpClient();
                var RestURL = string.Format(StationManager.Url + "/api/values");
                client.BaseAddress = new Uri(RestURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.Timeout = TimeSpan.FromSeconds(200);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(RestURL);
                if (response.IsSuccessStatusCode)
                {
                    content = await response.Content.ReadAsStringAsync();
                    ListOfProducts = JsonConvert.DeserializeObject<List<Barcode>>(content);
                }
                else
                {
                    await DisplayAlert("Error", response.StatusCode.ToString(), "Ok");
                }
            }
            catch (HttpRequestException e)
            {
                await DisplayAlert("Error", e.ToString(), "Ok");
            }
            catch (JsonSerializationException e)
            {
                await DisplayAlert("Error", e.ToString(), "Ok");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
