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
    public class City
    {
       public string Name { get; set; }
       public string AirportName { get; set; }
       public string AirportCode { get; set; }
       public string AirportImage { get; set; }
       public string AirportPhone { get; set; }
       public string AirportUrl { get; set; }
       public string AirportAdress { get; set; }
       public string AirportIsFavoriteIcon { get; set; }

       public City(string name, string airportname, string airportcode, string airportimage, string airportphone, string airporturl, string airportadress, string airportisfavoriteicon)
       {
           this.Name = name;
           this.AirportName = airportname;
           this.AirportCode = airportcode;
           this.AirportImage = airportimage;
           this.AirportPhone = airportphone;
           this.AirportUrl = airporturl;
           this.AirportAdress = airportadress;
           this.AirportIsFavoriteIcon = airportisfavoriteicon;
       }

       public City()
       { 
       }
    }
}
