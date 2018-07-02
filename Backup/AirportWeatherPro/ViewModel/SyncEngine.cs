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
using System.Xml.Linq;
using System.Linq;
using AirportWeatherPro.Model;
using System.IO;
using System.IO.IsolatedStorage;
using System.Collections.Generic;

namespace AirportWeatherPro.ViewModel
{
    public class SyncEngine
    {
        IsolatedStorageSettings LastUpdate = IsolatedStorageSettings.ApplicationSettings;

        string AirportCode;
        public static SyncState.SyncResult DownloadResult;
        public static SyncState.ServerSyncResult DownloadServerResult;

        public void GetSaveWeatherForAirport(string AirportName, string AirportCode)
        {
            this.AirportCode = AirportCode;
            DownloadResult = SyncState.SyncResult.InActive;
            try
            {
                WeatherService.GlobalWeatherSoapClient client = new WeatherService.GlobalWeatherSoapClient();
                client.GetWeatherCompleted += new EventHandler<WeatherService.GetWeatherCompletedEventArgs>(WeatherService_GetSaveWeather);
                client.GetWeatherAsync(TranslationEngine.TranslatedCityName(AirportName), "Greece");
                DownloadResult = SyncState.SyncResult.Downloading;
            }

            catch { MessageBox.Show("Σφάλμα ! Ο Server δεν Ανταποκρίθηκε έγκαιρα"); DownloadResult = SyncState.SyncResult.Failed; return; }
        }

        public void WeatherService_GetSaveWeather(object sender, WeatherService.GetWeatherCompletedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Result)) { MessageBox.Show("Αποτυχία ενημέρωσης δεδομώνων. Ξαναπροσπαθήστε"); DownloadResult = SyncState.SyncResult.Failed; return; }

            try
            {
                if (!string.IsNullOrEmpty(e.Result))
                {
                    XDocument xdoc = XDocument.Parse(e.Result);

                    var Result = from a in xdoc.Descendants("CurrentWeather")
                                 select new
                                 {
                                     weathercondition = a.Element("SkyConditions"),
                                     temperature = a.Element("Temperature"),
                                     humidity = a.Element("RelativeHumidity"),
                                     visibility = a.Element("Visibility"),
                                     wind = a.Element("Wind"),
                                     pressure = a.Element("Pressure"),
                                     datetime = a.Element("Time"),
                                 };

                    Airport airport = new Airport();

                    foreach (var k in Result)
                    {
                        try
                        {
                            airport.Code = this.AirportCode;
                            airport.WeatherCondition = (k.weathercondition.Value == null) ? k.weathercondition.Value = "NaN" : k.weathercondition.Value;
                            airport.Temperature = (k.temperature.Value == null) ? k.temperature.Value = "NaN" : k.temperature.Value;
                            airport.Humidity = (k.humidity.Value == null) ? k.humidity.Value = "NaN" : k.humidity.Value;
                            airport.Visibility = (k.visibility.Value == null) ? k.visibility.Value = "NaN" : k.visibility.Value;
                            airport.Wind = (k.wind.Value == null) ? k.wind.Value = "NaN" : k.wind.Value;
                            airport.Pressure = (k.pressure.Value == null) ? k.pressure.Value = "NaN" : k.pressure.Value;
                            airport.DateTime = (k.datetime.Value == null) ? k.datetime.Value = "NaN" : k.datetime.Value;
                        }

                        catch { MessageBox.Show("Αποτυχία ενημέρωσης δεδομώνων. Προέκυψε άγνωστο σφάλμα απο τον Server Ξαναπροσπαθήστε Αργότερα"); DownloadResult = SyncState.SyncResult.Failed; return; }
                    }

                    DbEngine DBWorker = new DbEngine();
                    DBWorker.UpdateTable("Airport", airport);
                    DownloadResult = SyncState.SyncResult.Succeed;
                }
            }
            catch
            {
                if (!string.IsNullOrEmpty(e.Result))
                {
                    XDocument xdoc = XDocument.Parse(e.Result);

                    var Result = from a in xdoc.Descendants("CurrentWeather")
                                 select new
                                 {
                                     temperature = a.Element("Temperature"),
                                     humidity = a.Element("RelativeHumidity"),
                                     visibility = a.Element("Visibility"),
                                     wind = a.Element("Wind"),
                                     pressure = a.Element("Pressure"),
                                     datetime = a.Element("Time"),
                                 };

                    Airport airport = new Airport();

                    foreach (var k in Result)
                    {
                        try
                        {
                            airport.Code = this.AirportCode;
                            airport.Temperature = (k.temperature.Value == null) ? k.temperature.Value = "NaN" : k.temperature.Value;
                            airport.Humidity = (k.humidity.Value == null) ? k.humidity.Value = "NaN" : k.humidity.Value;
                            airport.Visibility = (k.visibility.Value == null) ? k.visibility.Value = "NaN" : k.visibility.Value;
                            airport.Wind = (k.wind.Value == null) ? k.wind.Value = "NaN" : k.wind.Value;
                            airport.Pressure = (k.pressure.Value == null) ? k.pressure.Value = "NaN" : k.pressure.Value;
                            airport.DateTime = (k.datetime.Value == null) ? k.datetime.Value = "NaN" : k.datetime.Value;
                        }
                        catch { MessageBox.Show("Αποτυχία ενημέρωσης δεδομώνων. Προέκυψε άγνωστο σφάλμα απο τον Server Ξαναπροσπαθήστε Αργότερα"); return; }
                    }

                    DbEngine DBWorker = new DbEngine();
                    DBWorker.UpdateTable("Airport", airport);
                    DownloadResult = SyncState.SyncResult.Succeed;
                }
            }
        }


        public void GetLastUpdateStatus()
        {

            DownloadResult = SyncState.SyncResult.InActive;
            try
            {
                WeatherService.GlobalWeatherSoapClient client = new WeatherService.GlobalWeatherSoapClient();
                client.GetWeatherCompleted += new EventHandler<WeatherService.GetWeatherCompletedEventArgs>(WeatherService_GetSLastUpdate);
                client.GetWeatherAsync("Alexandroupoli", "Greece");
                DownloadServerResult = SyncState.ServerSyncResult.Downloading;
            }

            catch { MessageBox.Show("Σφάλμα ! Ο Server δεν Ανταποκρίθηκε έγκαιρα"); DownloadResult = SyncState.SyncResult.Failed; return; }
        }

        public void WeatherService_GetSLastUpdate(object sender, WeatherService.GetWeatherCompletedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(e.Result))
                {
                    XDocument xdoc = XDocument.Parse(e.Result);

                    var Result = from a in xdoc.Descendants("CurrentWeather")
                                 select new
                                 {
                                     datetime = a.Element("Time"),
                                 };

                    string dateTime = null;

                    foreach (var k in Result)
                    {
                        dateTime = k.datetime.Value;
                    }

                    LastUpdate["DateTime"] = dateTime.ToString();
                    DownloadServerResult = SyncState.ServerSyncResult.Succeed;
                }
            }
            catch { DownloadServerResult = SyncState.ServerSyncResult.Failed; }
        }


        public void TestGetSaveWeatherForAirport(string AirportName, string AirportCode)
        {
            this.AirportCode = AirportCode;
            DownloadResult = SyncState.SyncResult.InActive;
            try
            {
                WeatherService.GlobalWeatherSoapClient client = new WeatherService.GlobalWeatherSoapClient();
                client.GetWeatherCompleted += new EventHandler<WeatherService.GetWeatherCompletedEventArgs>(TestWeatherService_GetSaveWeather);
                client.GetWeatherAsync(TranslationEngine.TranslatedCityName(AirportName), "Greece");
                DownloadResult = SyncState.SyncResult.Downloading;
            }

            catch { MessageBox.Show("Σφάλμα ! Ο Server δεν Ανταποκρίθηκε έγκαιρα"); DownloadResult = SyncState.SyncResult.Failed; return; }
        }

        public void TestWeatherService_GetSaveWeather(object sender, WeatherService.GetWeatherCompletedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Result)) { MessageBox.Show("Αποτυχία ενημέρωσης δεδομώνων. Ξαναπροσπαθήστε"); DownloadResult = SyncState.SyncResult.Failed; return; }
           
            Airport airport = new Airport("NaN","NaN","NaN","NaN","NaN","NaN","NaN","NaN","NaN");
            airport.Code = this.AirportCode;
            airport.AirportImage = this.AirportCode;

            try
            {
                if (!string.IsNullOrEmpty(e.Result))
                {
                    XDocument xdoc = XDocument.Parse(e.Result);
                  
                    var query = xdoc.Descendants("CurrentWeather").Select(r => r.Elements()
                        .Select(b => new { Name = b.Name.LocalName, Value = b.Value }));

                    foreach (var result in query)
                    {
                        foreach (var item in result)
                        {
                            if (item.Name == "SkyConditions")
                            {
                                airport.WeatherCondition = (item.Value == null) ? airport.WeatherCondition = "NaN" : item.Value;
                            }

                            else if (item.Name == "Temperature")
                            {
                                airport.Temperature = (item.Value == null) ? airport.Temperature = "NaN" : item.Value;
                            }

                            else if (item.Name == "RelativeHumidity")
                            {
                                airport.Humidity = (item.Value == null) ? airport.Humidity = "NaN" : item.Value;
                            }

                            else if (item.Name == "Visibility")
                            {
                                airport.Visibility = (item.Value == null) ? airport.Visibility = "NaN" : item.Value;
                            }

                            else if (item.Name == "Wind")
                            {
                                airport.Wind = (item.Value == null) ? airport.Wind = "NaN" : item.Value;
                            }

                            else if (item.Name == "Pressure")
                            {
                                airport.Pressure = (item.Value == null) ? airport.Pressure = "NaN" : item.Value;
                            }

                            else if (item.Name == "Time")
                            {
                                airport.DateTime = (item.Value == null) ? airport.DateTime = "NaN" : item.Value;
                            }
                        }
                    }
                }

                DbEngine DBWorker = new DbEngine();
                DBWorker.UpdateTable("Airport", airport);
                DownloadResult = SyncState.SyncResult.Succeed;
            }
            catch { MessageBox.Show("Αποτυχία ενημέρωσης δεδομώνων. Προέκυψε άγνωστο σφάλμα απο τον Server Ξαναπροσπαθήστε Αργότερα"); DownloadResult = SyncState.SyncResult.Failed; return; }
        }
    }
}