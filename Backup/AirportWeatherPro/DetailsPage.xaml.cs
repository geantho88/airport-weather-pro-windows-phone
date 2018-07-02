using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using AirportWeatherPro.ViewModel;
using AirportWeatherPro.Model;
using AkazooProject.ResourcesAccessLogic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Threading;
using Microsoft.Phone.Shell;

namespace AirportWeatherPro
{
    public partial class DetailsPage : PhoneApplicationPage
    {
        ProgressIndicator progressIndicator;

        public static string CityCode { get; set; }
        public static string CityName { get; set; }
 
        public bool SyncFinished;

        DispatcherTimer Timer = new DispatcherTimer();

        public DetailsPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            // FIND THE CITY DETAILS FROM THE XML
            City city = GetCity.FindCity(CityCode);

            AirportImage.Source = MultimediaLoad.LoadPngImage("Airports/" + city.AirportImage);
            NameLabel.Text = city.AirportName;
            AdressLabel.Text = city.AirportAdress;
            PhoneLabel.Text = city.AirportPhone.ToString();

            if (ConditionDeviceEngine.CheckInternetStatus() == "Off")
            {
                ShowData();
                progressIndicator = new ProgressIndicator();
                progressIndicator.IsVisible = true;
                progressIndicator.Text = "Δεν βρέθηκε Σύνδεση στο Διαδίκτυο...";
                SystemTray.SetProgressIndicator(this, progressIndicator);
                //SHOW LAST UPDATED DATA BEFORE TRY TO SYNC AGAIN
            }

            else
            {   
                //SYNC
                ShowData();
                SyncEngine syncworker = new SyncEngine();
                syncworker.TestGetSaveWeatherForAirport(CityName, CityCode);

                Timer.Interval = TimeSpan.FromMilliseconds(2000);
                Timer.Tick += OnTimerTick;
                Timer.Start();
            }                        
        }

        void OnTimerTick(Object sender, EventArgs args)
        {

            if (SyncEngine.DownloadResult == SyncState.SyncResult.Succeed)
            {
                Timer.Stop();
                // GET AND SHOW THE DATA
                DbEngine engineworker = new DbEngine();
                Airport airport = engineworker.GetAirportData(CityCode);
                ShowData();
                progressIndicator = new ProgressIndicator();

                progressIndicator.IsVisible = false;
                progressIndicator.Text = "";
                progressIndicator.IsIndeterminate = false;
                SystemTray.SetProgressIndicator(this, progressIndicator);
            }

            else if (SyncEngine.DownloadResult == SyncState.SyncResult.Downloading)
            {
                progressIndicator = new ProgressIndicator();

                progressIndicator.IsVisible = true;
                progressIndicator.Text = "Συγχρονισμός Δεδομένων...";
                progressIndicator.IsIndeterminate = true;
                SystemTray.SetProgressIndicator(this, progressIndicator);
            }

            else if (SyncEngine.DownloadResult == SyncState.SyncResult.Failed)
            {
                Timer.Stop();
                MessageBox.Show("Αποτυχία ανανέωσης δεδομένων. Παρακαλώ ξαναπροσπαθήστε");
                progressIndicator = new ProgressIndicator();
                progressIndicator.IsVisible = false;
                progressIndicator.Text = "";
                progressIndicator.IsIndeterminate = false;
            }
        }

        public void ShowData()
        {
            DbEngine engineworker = new DbEngine();
            Airport airport = engineworker.GetAirportData(CityCode);

            HumidityBlovkLabel.Text = airport.Humidity.ToString();

            HumidityConditionGeneralLabel.Text = "Υγρασία " + airport.Humidity.ToString();

            string outputTemperature = airport.Temperature.ToString().Split(new char[] { '(', ')' })[1];
            outputTemperature = outputTemperature.Substring(0,2);
            TemperatureConditionGeneralLabel.Text = "Θερμοκρασία " + outputTemperature + "°C";
            TemperatureBlockLabel.Text = outputTemperature + "°C"; ;
          
            WindConditionGeneralLabel.Text = "Άνεμος " + airport.Wind;
            PressureConditionGeneralLabel.Text = "Ατμοσφαιρική Πίεση " + airport.Pressure.Substring(1,12);

            if (airport.Visibility == " greater than 7 mile(s):0")
            {
                VisibilityConditionGeneralLabel.Text = "Ορατότητα Μεγαλύτερη απο 7 Ν. Μίλια";
            }
            else {VisibilityConditionGeneralLabel.Text = "Ορατότητα " + airport.Visibility;  }

            int index = airport.DateTime.IndexOf('/');
            UpdateLabel.Text = "Ενημερώθηκε " + airport.DateTime.Substring(index + 1);
            
            switch (airport.WeatherCondition.ToString())
            {
                case " partly cloudy":
                    {
                        WeatherConditionLabel.Source = MultimediaLoad.LoadPngImage("WeatherConditions/Status-weather-clouds-icon");
                        WeatherConditionSub.Text = "Λίγα Σύννεφα";
                        WeatherConditionGeneralLabel.Text = "Καιρικές Συνθήκες " + WeatherConditionSub.Text;
                    } break;
                case " mostly cloudy":
                    {
                        WeatherConditionLabel.Source = MultimediaLoad.LoadPngImage("WeatherConditions/Status-weather-many-clouds-icon");
                        WeatherConditionSub.Text = "Συννεφιά";
                        WeatherConditionGeneralLabel.Text = "Καιρικές Συνθήκες " + WeatherConditionSub.Text;
                    } break;
                case " mostly clear":
                    {
                        WeatherConditionLabel.Source = MultimediaLoad.LoadPngImage("WeatherConditions/Status-weather-clear-icon");
                        WeatherConditionSub.Text = "Καθαρός";
                        WeatherConditionGeneralLabel.Text = "Καιρικές Συνθήκες " + WeatherConditionSub.Text;
                    } break;

                case "NaN":
                    {
                        WeatherConditionSub.Text = "Server Error";
                        WeatherConditionGeneralLabel.Text = "Καιρικές Συνθήκες " + WeatherConditionSub.Text;
                        MessageBox.Show("Ο Server Δεν επέστρεψε πλήρεις πληροφορίες για τις καιρικές συνθήκες στην περιοχή.");
                    } break;
            }     
        }

        private void TryUpdateManuallyButton_Click(object sender, RoutedEventArgs e)
        {
            if (ConditionDeviceEngine.CheckInternetStatus() == "Off")
            {
                progressIndicator.IsVisible = true;
                progressIndicator.Text = "Δεν βρέθηκε Σύνδεση στο Διαδίκτυο...";
                //SHOW LAST UPDATED DATA BEFORE TRY TO SYNC AGAIN
                return;
            }

            SyncEngine syncworker = new SyncEngine();
            syncworker.TestGetSaveWeatherForAirport(CityName, CityCode);

            Timer.Interval = TimeSpan.FromMilliseconds(2000);
            Timer.Tick += OnTimerTick;
            Timer.Start(); 
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Timer.Stop();

        }
    }
}