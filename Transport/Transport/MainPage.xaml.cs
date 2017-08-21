using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Newtonsoft.Json;

namespace Transport
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        
        public MainPage()
        {
            InitializeComponent();
            // NavigationPage.SetHasNavigationBar(this, false);
            Task.Run(() => this.StationsRefresh()).Wait();
            map.MoveToRegion(MapSpan.FromCenterAndRadius(
                   new Position(56.13426, 47.2447563), Distance.FromMeters(1000)));

            addressSearch.TextChanged += async (s, e) =>
            {
                try
                {
                    if (!string.IsNullOrEmpty(addressSearch.Text))
                    {
                        var pos = (await new Geocoder().GetPositionsForAddressAsync("Чебоксары, " + addressSearch.Text)).ToList();
                        var addressList = new List<string>();
                        for (int i=0; i<6 && i<pos.Count; i++)
                        {
                            IEnumerable<string> _curraddress = await new Geocoder().GetAddressesForPositionAsync(pos[i]);
                            addressList.Add(_curraddress.ToList()[0]);
                        }
                        addressListView.ItemsSource = addressList;
                        addressListViewStackLayout.HeightRequest = 100 * addressList.Count();
                    }
                    else
                        addressListViewStackLayout.HeightRequest = 0;
                }
                catch { }
            };

        }

        async Task StationsRefresh()
        {
            try
            {
                List<Pin> pins = await GetStations();
                map.Pins.Clear();
                foreach (Pin p in pins)
                {
                    map.Pins.Add(p);
                }
            }
            catch { }
        }

        async Task<List<Pin>> GetStations()
        {
            try
            {
                List<Pin> pins = new List<Pin>();
                using (var httpClient = new HttpClient())
                {
                    var receivedStations = JsonConvert.DeserializeObject<List<Stations>>(await httpClient.GetStringAsync("http://buscheb.ru/php/getStations.php?city=cheboksari"));
                    foreach (var item in receivedStations)
                    {
                        pins.Add(new Pin()
                        {
                            Type = PinType.SavedPin,
                            Flat = true,
                            BindingContext = item,
                            Icon = BitmapDescriptorFactory.FromBundle("station.png"),
                            Label = item.name,
                            Address = item.type,
                            Position = new Position(item.lat / 1000000, item.lng / 1000000)
                        });
                    }
                    return pins;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}