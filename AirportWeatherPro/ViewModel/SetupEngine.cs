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
using System.IO;
using System.Xml.Linq;
using System.Linq;
using System.IO.IsolatedStorage;


namespace AirportWeatherPro.ViewModel
{
    public class SetupEngine
    {
     
     
        public static void SetupFirstTime()
        {
            IsolatedStorageSettings FirstTime = IsolatedStorageSettings.ApplicationSettings;
            IsolatedStorageSettings Favorites = IsolatedStorageSettings.ApplicationSettings;
            IsolatedStorageSettings LastUpdate = IsolatedStorageSettings.ApplicationSettings;

            Favorites["LoadFavorites"] = "false";
            LastUpdate["DateTime"] = "15, 2014 - 02:20 PM EDT / 2014.06.15 1820 UTC";

            DbEngine DBWorker = new DbEngine();
            DBWorker.CreateTable("Airport", "(Code text primary key,WeatherCondition text,Temperature text,Humidity text,Visibility text, Wind text ,Pressure text,AirportImage text,DateTime text, IsFavorite bool)");
            
            var AirportData = GetAirport.GetAllAirportsData();

            foreach (var airport in AirportData)
            {
                DBWorker.FillTable("Airport", airport);
            }
        }
    }
}
