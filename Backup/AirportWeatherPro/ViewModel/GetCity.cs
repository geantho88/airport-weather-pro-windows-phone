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
    public class GetCity
    {
        public static IEnumerable<City> GetAllCitiesData()
        {
            List<City> CitiesList = new List<City>();
            List<string> FavoriteIcons = DbEngine.GetFavoriteAirports();

            try
            {
                XElement xdoc = XElement.Load("/AirportWeatherPro;component/Resources/Cities.xml");
                var Result = from e in xdoc.Descendants("City")
                             select new
                             {
                                 cityname = e.Element("CityName"),
                                 airportname = e.Element("AirportName"),
                                 airportcode = e.Element("AirportCode"),
                             };

                foreach (var k in Result)
                {
                    City city = new City();

                    city.Name = k.cityname.Value;
                    city.AirportName = k.airportname.Value;
                    city.AirportCode = k.airportcode.Value;
                    city.AirportImage = "/AirportWeatherPro;component/Images/Airports/" + k.airportcode.Value + ".png";
                    city.AirportIsFavoriteIcon = "/AirportWeatherPro;component/Images/favs.addto.png";
                    
                    foreach (var item in FavoriteIcons)
                    {
                        if ((item.Replace("`", string.Empty)) == city.AirportCode)
                        {
                            city.AirportIsFavoriteIcon = "/AirportWeatherPro;component/Images/noimage.png";
                        }
                    }

                    CitiesList.Add(city);
                }

                return CitiesList;
            }
            catch { return null; }
        }

        public static City FindCity(string Code)
        {
            City city = new City();

            try
            {
                XElement xdoc = XElement.Load("/AirportWeatherPro;component/Resources/Cities.xml");
                var Result = from e in xdoc.Descendants("City")
                             where e.Element("AirportCode").Value.Contains(Code)
                             select new
                             {
                                 cityname = e.Element("CityName"),
                                 airportname = e.Element("AirportName"),
                                 airportcode = e.Element("AirportCode"),
                                 airportphone = e.Element("AirportPhone"),
                                 airportadress = e.Element("AirportAdress"),
                                 airportimage = e.Element("AirportCode"), //the image name is the same as the code. 
                             };

                foreach (var k in Result)
                {
                    city.Name = k.cityname.Value;
                    city.AirportName = k.airportname.Value;
                    city.AirportCode = k.airportcode.Value;
                    city.AirportPhone = k.airportphone.Value;
                    city.AirportAdress = k.airportadress.Value;
                    city.AirportImage = k.airportimage.Value;
                }

                return city;
            }
            catch (Exception) { return null; }
        }

        public static List<string> GetAllCitiesNames()
        {
            List<string> CitiesNamesList = new List<string>();

            try
            {
                XElement xdoc = XElement.Load("/AirportWeatherPro;component/Resources/Cities.xml");
                var Result = from e in xdoc.Descendants("City")
                             select new
                             {
                                 cityname = e.Element("CityName"),
                             };

                foreach (var k in Result)
                {
                    City city = new City();

                    city.Name = k.cityname.Value;
                }

                return CitiesNamesList;
            }
            catch { return null; }
        }

        public static IEnumerable<City> GetFavoriteCities(List<string> FavoritesIndexData)
        {

            List<City> AirportsList = new List<City>();

            try
            {
                foreach (string Fav in FavoritesIndexData)
                {
                    XElement xdoc = XElement.Load("/AirportWeatherPro;component/Resources/Cities.xml");
                    var Result = from e in xdoc.Descendants("City")
                                 where e.Element("AirportCode").Value.Equals(Fav.Replace("`", string.Empty))
                                 select new
                                 {
                                     cityname = e.Element("CityName"),
                                     airportname = e.Element("AirportName"),
                                     airportcode = e.Element("AirportCode"),
                                 };

                    foreach (var k in Result)
                    {
                        City city = new City();

                        city.Name = k.cityname.Value;
                        city.AirportName = k.airportname.Value;
                        city.AirportCode = k.airportcode.Value;
                        city.AirportImage = "/AirportWeatherPro;component/Images/Airports/" + k.airportcode.Value + ".png";
                        city.AirportIsFavoriteIcon = "/AirportWeatherPro;component/Images/minus.png";
                        AirportsList.Add(city);
                    }                  
                }
                return AirportsList;
            }
            catch (Exception) { return null; }
        }
    }
}
