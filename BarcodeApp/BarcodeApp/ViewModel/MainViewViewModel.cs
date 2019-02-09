using BarcodeApp.Helpers;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BarcodeApp.ViewModel
{
    public class MainViewViewModel : INotifyPropertyChanged
    {
        private string _productName;
        private string _productImage;        

        public string ProductName
        {
            get => _productName;
            set
            {
                _productName = value;
                NotifyPropertyChanged();
            }
        }

        public string ProductImage
        {
            get => _productImage;
            set
            {
                _productImage = value;
                NotifyPropertyChanged();
            }
        }
        public MainViewViewModel()
        {
            _productName = StationManager.CurrentProduct.Name;
            _productImage = StationManager.CurrentProduct.Image;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
