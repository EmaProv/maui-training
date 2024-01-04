using MauiWeather.MVVM.Models;
using PropertyChanged;
using System.Diagnostics;
using System.Text.Json;
using System.Windows.Input;

namespace MauiWeather.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class WeatherViewModel
    {
        public WeatherData WeatherData { get; set; }
        public string PlaceName { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        private HttpClient _httpClient;

        public bool IsVisible { get; set; }
        public bool IsLoading { get; set; }

        public WeatherViewModel()
        {
            _httpClient = new HttpClient();
        }



        public ICommand SearchCommand => new Command(async (searchText) =>
        {
            PlaceName = searchText.ToString();

            var location = await GetCoordinatesAsync(searchText.ToString());

            await GetWeather(location);
        });

        private async Task GetWeather(Location location)
        {
            var url = $"https://api.open-meteo.com/v1/forecast?latitude={location.Latitude}&longitude={location.Longitude}&current=temperature_2m,weathercode,windspeed_10m&daily=weathercode,temperature_2m_max,temperature_2m_min&timezone=Europe%2FLondon";

            IsLoading = true;

            var res = await _httpClient.GetAsync(url);

            if (res.IsSuccessStatusCode)
            {
                using (var responseStream = await res.Content.ReadAsStreamAsync())
                {
                    var data = await JsonSerializer.DeserializeAsync<WeatherData>(responseStream);
                    WeatherData = data;

                    for (int i = 0; i < WeatherData.daily.time.Length; i++)
                    {
                        var daily2 = new Daily2
                        {
                            time = WeatherData.daily.time[i],
                            temperature_2m_max = WeatherData.daily.temperature_2m_max[i],
                            temperature_2m_min = WeatherData.daily.temperature_2m_min[i],
                            weathercode = WeatherData.daily.weathercode[i]
                        };

                        WeatherData.daily2.Add(daily2);
                    }

                    IsVisible = true;
                }
                IsLoading = false;
            }
            else
            {
                Debug.WriteLine("[ERR] Impossible to get weather!");
            }
        }

        private async Task<Location> GetCoordinatesAsync(string address)
        {

            IEnumerable<Location> locations = await Geocoding.Default.GetLocationsAsync(address);

            Location location = locations?.FirstOrDefault();

            if (location != null)
            {
                Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}");
            }
            return location;
        }

    }
}
