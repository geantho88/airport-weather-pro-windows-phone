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

namespace AirportWeatherPro.ViewModel
{
    public class SyncState
    {
        public enum SyncResult
        {
            InActive,
            Succeed,
            Failed,
            Downloading
        };

        public enum ServerSyncResult
        {
            InActive,
            Succeed,
            Failed,
            Downloading
        };

    }
}
