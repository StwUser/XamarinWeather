using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using XamarinWeather.Models;

namespace XamarinWeather.ViewModels
{
    public class WeatherViewModel : INotifyPropertyChanged
    {
        // INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        // ICommands
        public ICommand UpdateCommand { get; private set; } 
        
        // LocationModel
        public LocationModel LocationModel { get; private set; }
        // Properties Of Location
        public double Latitude
        {
            get { return LocationModel.Latitude; }
            set
            {
                if (LocationModel.Latitude != value)
                {
                    LocationModel.Latitude = value;
                    OnPropertyChanged("Latitude");
                }
            }
        }
        public double Longitude
        {
            get { return LocationModel.Longitude; }
            set
            {
                if (LocationModel.Longitude != value)
                {
                    LocationModel.Longitude = value;
                    OnPropertyChanged("Longitude");
                    
                    // Get weather when we know ours coordinates
                    this.LoadData();
                }
            }
        }

        // WeatherModel
        private string weatherMain;
        private string weatherDescription;
        private string weatherIcon;
        private double mainTemp;
        private double mainTempFeelsLike;
        private double mainTempMin;
        private double mainTempMax;
        private double mainPressure;
        private double mainHumidity;
        private double windSpeed;
        private int sysSunrise;
        private int sysSunset;
        private string name;
        // Peoperties of Weather
        public string WeatherMain 
        {
            get { return this.weatherMain; }
            set 
            {
                if (this.weatherMain != value)
                {
                    this.weatherMain = value;
                    OnPropertyChanged("WeatherMain");
                }
            }
        }
        public string WeatherDescription
        {
            get { return this.weatherDescription; }
            set
            {
                if (this.weatherDescription != value)
                {
                    this.weatherDescription = value;
                    OnPropertyChanged("WeatherDescription");
                }
            }
        }
        public string WeatherIcon
        {
            get { return this.weatherIcon; }
            set
            {
                if (this.weatherIcon != value)
                {
                    this.weatherIcon = value;
                    OnPropertyChanged("WeatherIcon");
                }
            }
        }
        public double MainTemp
        {
            get { return this.mainTemp; }
            set
            {
                if (this.mainTemp != value)
                {
                    this.mainTemp = value;
                    OnPropertyChanged("MainTemp");
                }
            }
        }
        public double MainTempFeelsLike
        {
            get { return this.mainTempFeelsLike; }
            set
            {
                if (this.mainTempFeelsLike != value)
                {
                    this.mainTempFeelsLike = value;
                    OnPropertyChanged("MainTempFeelsLike");
                }
            }
        }
        public double MainTempMin
        { 
            get { return mainTempMin; }
            set
            {
                if (mainTempMin != value)
                {
                    mainTempMin = value;
                    OnPropertyChanged("MainTempMin");
                }
            }
        }
        public double MainTempMax
        {
            get { return mainTempMax; }
            set
            {
                if (mainTempMax != value)
                {
                    mainTempMax = value;
                    OnPropertyChanged("MainTempMax");
                }
            }
        }
        public double MainPressure
        {
            get { return mainPressure; }
            set 
            {
                if (mainPressure != value)
                {
                    mainPressure = value;
                    OnPropertyChanged("MainPressure");
                }
            }
        }
        public double MainHumidity
        {
            get { return mainHumidity; }
            set
            {
                if (mainHumidity != value)
                {
                    mainHumidity = value;
                    OnPropertyChanged("MainHumidity");
                }
            }
        }
        public double WindSpeed
        {
            get { return windSpeed; }
            set
            {
                if (windSpeed!= value)
                {
                    windSpeed = value;
                    OnPropertyChanged("WindSpeed");
                }
            }
        }
        public int SysSunrise
        {
            get { return sysSunrise; }
            set
            {
                if (sysSunrise != value)
                {
                    sysSunrise = value;
                    OnPropertyChanged("SysSunrise");
                }
            }
        }
        public int SysSunset
        {
            get { return sysSunset; }
            set
            {
                if (sysSunset != value)
                {
                    sysSunset = value;
                    OnPropertyChanged("SysSunset");
                }
            }
        }
        public string Name
        {
            get { return this.name; }
            set
            {
                if (this.name != value)
                {
                    this.name = value;
                    OnPropertyChanged("Name");
                }
            }
        }


        // Constructor
        public WeatherViewModel()
        {
            LocationModel = new LocationModel();
            this.Update();


            UpdateCommand = new Command(Update);
        }

        //Commands
        private async void Update()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Low);
            var location = await Geolocation.GetLocationAsync(request);
            Latitude = Math.Round(location.Latitude, 2);
            Longitude = Math.Round(location.Longitude, 2);
        }

        // Work with weather
        private async void LoadData()
        {
            // Token From https://openweathermap.org
            string url = "https://api.openweathermap.org/data/2.5/weather?lat=" + Latitude + "&lon=" + Longitude + "&units=metric&appid= Input here your free token from https://openweathermap.org";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            var response = client.GetAsync(client.BaseAddress);
            var code = response.Status;
            
            // Get object in Json format
            var content = await response.Result.Content.ReadAsStringAsync();
            var cc = content.GetType();
            var cc2 = content.GetTypeCode();
            JObject jObject = JObject.Parse(content);
            
            // Deserialize to Our model
            var node = jObject.SelectToken(@"");
            var info = JsonConvert.DeserializeObject<WeatherModel>(node.ToString());

            // Fill the ViewModel fields
            var tempWeather = info.weather.FirstOrDefault<weather>();
            WeatherMain = tempWeather.main;
            WeatherDescription = tempWeather.description;
            WeatherIcon = "http://openweathermap.org/img/wn/" + tempWeather.icon + "@2x.png";
            MainTemp = info.main.temp;
            MainTempFeelsLike = info.main.feels_like;
            MainTempMin = info.main.temp_min;
            MainTempMax = info.main.temp_max;
            MainPressure = info.main.pressure;
            MainHumidity = info.main.humidity;
            WindSpeed = info.wind.speed;
            SysSunrise = info.sys.sunrise;
            SysSunset = info.sys.sunset;
            Name = info.name;
        }
    }
}
