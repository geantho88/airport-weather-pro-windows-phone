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
using System.Net.NetworkInformation;
using Microsoft.Phone.Info;
using System.Windows.Threading;
using Microsoft.Phone.Net.NetworkInformation;

namespace AirportWeatherPro.ViewModel
{
    public class ConditionDeviceEngine
    {
        double TotalRamMemory = (double)DeviceStatus.DeviceTotalMemory / 1048576;

        public static string CheckInternetStatus()
        {
            if ((DeviceNetworkInformation.IsCellularDataEnabled==false)&& (DeviceNetworkInformation.IsWiFiEnabled==false))
            {
                return "Off";
            }
            else if (DeviceNetworkInformation.IsNetworkAvailable == false)
            {
                return "Off";
            }
            return "OK";
        }
    }
}
