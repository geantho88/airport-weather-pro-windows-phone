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
using Microsoft.Phone.Tasks;

namespace AirportWeatherPro
{
    public partial class About : PhoneApplicationPage
    {
        public static bool Aboutnavigation = false;

        public About()
        {
            InitializeComponent();
        }

        private void ContactButton_Click(object sender, RoutedEventArgs e)
        {
            ContactUsLabel.Text = "avcit@outlook.com";
        }

        private void RateAppButton_Click(object sender, RoutedEventArgs e)
        {
            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();
            marketplaceReviewTask.Show();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Aboutnavigation = true;
        }
    }
}