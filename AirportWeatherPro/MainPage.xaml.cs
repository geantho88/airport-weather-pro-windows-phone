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
using Microsoft.Phone.Info;
using System.Windows.Threading;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Shell;


namespace AirportWeatherPro
{
    public partial class MainPage : PhoneApplicationPage
    {
        IsolatedStorageSettings Favorites = IsolatedStorageSettings.ApplicationSettings;
        IsolatedStorageSettings LastUpdate = IsolatedStorageSettings.ApplicationSettings;

        DispatcherTimer MemoryCheckTimer = new DispatcherTimer();
        DispatcherTimer RefreshCheckTimer = new DispatcherTimer();

        ProgressIndicator progressIndicator;

        double TotalDeviceRamMemory = (double)DeviceStatus.DeviceTotalMemory / 1048576;
        bool FavoritesListEnabled = false;
        
        public MainPage()
        {         
            InitializeComponent();
            CanvasOption.Visibility = Visibility.Collapsed;
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (About.Aboutnavigation == true) { About.Aboutnavigation = false; return; }
           
            else
            {
                if (Favorites["LoadFavorites"].ToString() == "true") { TryLoadFavorites(); this.FavoritesSwitch.IsChecked = true; }
                else if (FavoritesListEnabled == true) { LoadFavorites(); }
                else { CityList.ItemsSource = GetCity.GetAllCitiesData(); this.FavoritesSwitch.IsChecked = false; }
            }
        }

        private void CityList_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                var selected = CityList.SelectedValue as City;
                DetailsPage.CityCode = selected.AirportCode.ToString();
                DetailsPage.CityName = selected.Name.ToString();

                NavigationService.Navigate(new Uri("/DetailsPage.xaml", UriKind.Relative));
            }
            catch { }

        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
               NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {

            if (CanvasOption.Visibility == Visibility.Visible) { CanvasOption.Visibility = Visibility.Collapsed; CityList.Visibility = Visibility.Visible; MainLabel.Text = "Airport Weather Pro"; }
            else { CanvasOption.Visibility = Visibility.Visible; CityList.Visibility = Visibility.Collapsed; MainLabel.Text = "Menu"; }

            this.InternetStatusLabel.Text = ConditionDeviceEngine.CheckInternetStatus();
            this.SyncDate.Visibility = Visibility.Collapsed;

            MemoryCheckTimer.Interval = TimeSpan.FromMilliseconds(60);
            MemoryCheckTimer.Tick += new EventHandler(MemoryCheckTimer_Tick);
            MemoryCheckTimer.Start();
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (FavoritesListEnabled == true) { e.Cancel = true; CityList.ItemsSource = GetCity.GetAllCitiesData(); FavoritesListEnabled = false; MainLabel.Text = "Airport Weather Pro"; return; }

            if (CanvasOption.Visibility == Visibility.Visible)
            {
                e.Cancel = true;
                CanvasOption.Visibility = Visibility.Collapsed; CityList.Visibility = Visibility.Visible;
                MainLabel.Text = "Airport Weather Pro";
            }

            else 
            {
                MessageBoxResult result = MessageBox.Show("Έξοδος ?", "Επιβεβαίωση", MessageBoxButton.OKCancel);
                if (result != MessageBoxResult.OK)
                {
                    e.Cancel = true;
                }
            }
        }

        public void MemoryCheckTimer_Tick(Object sender, EventArgs e)
        {
            double MemoryUsage = (double)DeviceStatus.ApplicationCurrentMemoryUsage / 1048576;
            MemoryStatusLabel.Text = Math.Round(MemoryUsage, 0).ToString();

            double PercentageMemory = (MemoryUsage / TotalDeviceRamMemory) * 100;
            int Result = (int)PercentageMemory;

            PercentageUsageMemory.Text = Result.ToString() + "%";

            MemoryStatusBar.Minimum = 0;
            MemoryStatusBar.Maximum = 100;
            MemoryStatusBar.Value = PercentageMemory;
        }

        private void AddRemoveFavoriteButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

            City itemViewModel = new City();
            DbEngine EngineWorker = new DbEngine();
            itemViewModel = CityList.SelectedItem as City;
            string Code = itemViewModel.AirportCode;
            CityList.SelectedItem = null;
            
            if (EngineWorker.AddAirportToFavorites(Code) == true) 
            {
                MessageBox.Show("Το Αεροδρόμιο " + itemViewModel.AirportName + " Προστέθηκε στα Αγαπημένα !"); this.CityList.ItemsSource = GetCity.GetAllCitiesData();
                return;
            }

            else 
            {
                MessageBox.Show("Το Aεροδρόμιο " + itemViewModel.AirportName + "Αφαιρέθηκε απο τα Αγαπημένα");
                DbEngine.GetFavoriteAirports();
                List<string> Favorites = new List<string>();
                Favorites = DbEngine.GetFavoriteAirports();
                this.CityList.ItemsSource = null;
                this.CityList.ItemsSource = GetCity.GetFavoriteCities(Favorites);
                if (this.CityList.Items.Count == 0) { MessageBox.Show("Δεν βρέθηκε καταχώρηση στα αγαπημένα. Επιστροφή..."); CityList.ItemsSource = GetCity.GetAllCitiesData(); }
                return;
            }
         
        }

        private void FavoritesButton_Click(object sender, RoutedEventArgs e)
        {
            LoadFavorites();
        }

        private void FavoritesSwitch_Checked(object sender, RoutedEventArgs e)
        {
            Favorites["LoadFavorites"] = "true";
        }

        private void FavoritesSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            Favorites["LoadFavorites"] = "false";
        }

        public void LoadFavorites()
        {
            List<string> Favorites = new List<string>();
            Favorites = DbEngine.GetFavoriteAirports();
            
            if (Favorites.Count == 0)
            {
                MessageBox.Show("Δεν βρέθηκε καταχώρηση στα αγαπημένα...");
                FavoritesListEnabled = false;
                return;
            }

            this.CityList.ItemsSource = GetCity.GetFavoriteCities(Favorites);
            this.CanvasOption.Visibility = Visibility.Collapsed;
            FavoritesListEnabled = true;
            this.MainLabel.Text = "Αγαπημένα";
            
            this.CityList.Visibility = Visibility.Visible;          
        }

        public void TryLoadFavorites()
        {
            List<string> Favorites = new List<string>();
            Favorites = DbEngine.GetFavoriteAirports();

            if (Favorites.Count == 0)
            {
                MessageBox.Show("Δεν βρέθηκε καταχώρηση στα αγαπημένα...");
                this.CityList.ItemsSource = GetCity.GetAllCitiesData();
                FavoritesListEnabled = false;
            }

            else
            {
                this.CityList.ItemsSource = GetCity.GetFavoriteCities(Favorites);
                this.CanvasOption.Visibility = Visibility.Collapsed;
                FavoritesListEnabled = true;
                this.MainLabel.Text = "Αγαπημένα";
            }

            this.CityList.Visibility = Visibility.Visible;
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            if (ConditionDeviceEngine.CheckInternetStatus() == "Off") 
            {
                MessageBox.Show("Δεν υπάρχει σύνδεση στο διαδίκτυο. Ο έλεγχος δεν μπορεί να πραγματοποιηθεί.");
                return;
            }

            SyncEngine syncworker = new SyncEngine();
            syncworker.GetLastUpdateStatus();

            RefreshCheckTimer.Interval = TimeSpan.FromMilliseconds(60);
            RefreshCheckTimer.Tick += new EventHandler(RefreshCheckTimer_Tick);
            RefreshCheckTimer.Start();
        }

        public void RefreshCheckTimer_Tick(Object sender, EventArgs e)
        {
            if (SyncEngine.DownloadServerResult == SyncState.ServerSyncResult.Succeed)
            {
                RefreshCheckTimer.Stop();
                // GET AND SHOW THE DATA
                progressIndicator = new ProgressIndicator();
                progressIndicator.IsVisible = false;
                progressIndicator.Text = "";
                progressIndicator.IsIndeterminate = false;
                SystemTray.SetProgressIndicator(this, progressIndicator);
                int index = LastUpdate["DateTime"].ToString().IndexOf('/');
                string DateTime = LastUpdate["DateTime"].ToString().Substring(index + 1);
                SyncDate.Text = (LastUpdate["DateTime"].ToString().Substring(index + 1));
                this.RefreshButton.Visibility = Visibility.Collapsed;
                this.SyncDate.Visibility = Visibility.Visible;
            }

            else if (SyncEngine.DownloadServerResult == SyncState.ServerSyncResult.Downloading)
            {
                progressIndicator = new ProgressIndicator();
                progressIndicator.IsVisible = true;
                progressIndicator.Text = "Έλεγχος του Server...";
                progressIndicator.IsIndeterminate = true;
                SystemTray.SetProgressIndicator(this, progressIndicator);
            }

            else if (SyncEngine.DownloadServerResult == SyncState.ServerSyncResult.Failed)
            {
                RefreshCheckTimer.Stop();
                MessageBox.Show("Αποτυχία Ελέγχου του Server");
                progressIndicator = new ProgressIndicator();
                progressIndicator.IsVisible = false;
                progressIndicator.Text = "";
                progressIndicator.IsIndeterminate = false;
            }
        }
    }
}