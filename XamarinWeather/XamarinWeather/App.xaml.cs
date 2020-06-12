using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinWeather.Views;

namespace XamarinWeather
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new WeatherPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
