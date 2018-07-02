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
using System.Net.NetworkInformation;

namespace AirportWeatherPro.ViewModel
{
    public class ApplicationStatus
    {


        public static void SetAppFirstTimeRun()
        {
            IsolatedStorageSettings FirstTime = IsolatedStorageSettings.ApplicationSettings;

            if (FirstTime["IsFirstTime"].ToString() == "true")
            {
                SetupEngine.SetupFirstTime();
                FirstTime["IsFirstTime"] = "false";
            }
        }
    }
}
