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
using System.Collections.Generic;
using AirportWeatherPro.Model;
using System.Xml.Linq;
using System.Linq;

namespace AirportWeatherPro.ViewModel
{
    public class GetAirport
    {
        public static List<Airport> GetAllAirportsData()
        {
            List<Airport> AirportsList = new List<Airport>();

            try
            {

                XElement xdoc = XElement.Load("/AirportWeatherPro;component/Resources/Airports.xml");
                var Result = from e in xdoc.Descendants("CurrentWeather")
                             select new                            
                             {
                                 code = e.Element("Code"),
                                 weathercondition = e.Element("SkyConditions"),
                                 temperature = e.Element("Temperature"),
                                 humidity = e.Element("RelativeHumidity"),
                                 visibility = e.Element("Visibility"),
                                 wind = e.Element("Wind"),
                                 pressure = e.Element("Pressure"),
                                 airportimage = e.Element("AirportCode"),
                                 datetime = e.Element("Time"),
                             };

                foreach (var k in Result)
                {
                    Airport airport = new Airport();

                    airport.Code = k.code.Value;
                    airport.WeatherCondition = k.weathercondition.Value;
                    airport.Temperature = k.temperature.Value;
                    airport.Humidity = k.humidity.Value;
                    airport.Visibility = k.visibility.Value;
                    airport.Wind = k.wind.Value;
                    airport.Pressure = k.pressure.Value;
                    airport.DateTime = k.datetime.Value;
                    airport.IsFavorite = false;
                    airport.AirportImage = k.code.Value;

                    AirportsList.Add(airport);
                }

                return AirportsList;
            }
            catch { return null; }
        }      
    }
}
