using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace AirportWeatherPro.Model
{
    public class Airport
    {
        public string Code { get; set; }
        public string WeatherCondition { get; set; }
        public string Temperature { get; set; }
        public string Humidity { get; set; }
        public string Visibility { get; set; }
        public string Wind { get; set; }
        public string Pressure { get; set; }
        public string AirportImage { get; set; }
        public string DateTime { get; set; }
        public bool? IsFavorite { get; set; }

        public Airport(string code, string weathercondition, string temperature, string humidity, string visibility, string wind, string pressure, string airportimage, string datetime, bool? isfavorite)
        {
            this.Code = code;
            this.WeatherCondition = weathercondition;
            this.Temperature = temperature;
            this.Humidity = humidity;
            this.Visibility = visibility;
            this.Wind = wind;
            this.Pressure = pressure;
            this.AirportImage = airportimage;
            this.DateTime = datetime;
            this.IsFavorite = isfavorite.Value;
        }

        public Airport(string code, string weathercondition, string temperature, string humidity, string visibility, string wind, string pressure, string airportimage, string datetime)
        {
            this.Code = code;
            this.WeatherCondition = weathercondition;
            this.Temperature = temperature;
            this.Humidity = humidity;
            this.Visibility = visibility;
            this.Wind = wind;
            this.Pressure = pressure;
            this.AirportImage = airportimage;
            this.DateTime = datetime;
        }


        public Airport()
        { 
        }
    }
}
