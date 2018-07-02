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
using System.IO.IsolatedStorage;

namespace AirportWeatherPro.ViewModel
{
    public class Settings
    {
      
        IsolatedStorageSettings LastUpdate = IsolatedStorageSettings.ApplicationSettings;
        IsolatedStorageSettings Favorites = IsolatedStorageSettings.ApplicationSettings;

        #region First Time Status

        public static void CheckFirstTime()
        {
            IsolatedStorageSettings FirstTime = IsolatedStorageSettings.ApplicationSettings;
            FirstTime["IsFirstTime"].ToString();
        }

        public static void SetFirstTime()
        {
            IsolatedStorageSettings FirstTime = IsolatedStorageSettings.ApplicationSettings;
            FirstTime["IsFirstTime"] = "true";
        }

        #endregion

        #region Update Status

        public void ShowLastUpdate()
        {
            LastUpdate["LastUpdate"].ToString();
        }

        public void SetLastUpdate()
        {
            LastUpdate["LastUpdate"] = "Last Update : Never";
        }

        #endregion
    }
}
