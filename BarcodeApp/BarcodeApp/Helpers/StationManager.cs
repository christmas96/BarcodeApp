using BarcodeApp.Models;

namespace BarcodeApp.Helpers
{
    public static class StationManager
    {
        public const string Url = "https://webapibarcodes.azurewebsites.net";
        public static Barcode CurrentProduct { get; set; }
    }
}
