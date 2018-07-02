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

namespace AirportWeatherPro.ViewModel
{
    public class TranslationEngine
    {
        public static List<string> TranslateCityNames(List<string> Cities)
        {
            List<string>TranslatedCityNames= new List<string>();

            foreach (var name in Cities)
            {
                switch (name)
                {
                    case "Αλεξανδρούπολη": { TranslatedCityNames.Add("Alexandroupoli"); } break;
                    case "Αθήνα": { TranslatedCityNames.Add("Elefsis"); } break;
                    case "Χίος": { TranslatedCityNames.Add("Chios"); } break;
                    case "Ηράκλειο": { TranslatedCityNames.Add("Heraklion"); } break;
                    case "Κεφαλονιά": { TranslatedCityNames.Add("Kefalhnia"); } break;
                    case "Κώς": { TranslatedCityNames.Add("Kos"); } break;
                    case "Κάρπαθος": { TranslatedCityNames.Add("Karpathos"); } break;
                    case "Κέρκυρα": { TranslatedCityNames.Add("Kerkyra"); } break;
                    case "Καβάλα": { TranslatedCityNames.Add("Chrysoupoli"); } break;
                    case "Κοζάνη": { TranslatedCityNames.Add("Kozani"); } break;
                    case "Λήμνος": { TranslatedCityNames.Add("Limnos"); } break;
                    case "Λάρισα": { TranslatedCityNames.Add("Larissa"); } break;
                    case "Μυτιλήνη": { TranslatedCityNames.Add("Mytilini"); } break;
                    case "Ρόδος": { TranslatedCityNames.Add("Rhodes"); } break;
                    case "Πάτρα": { TranslatedCityNames.Add("Araxos"); } break;
                    case "Σκιάθος": { TranslatedCityNames.Add("Skiathos"); } break;
                    case "Σάμος": { TranslatedCityNames.Add("Samos"); } break;
                    case "Σαντορίνη": { TranslatedCityNames.Add("Santorini"); } break;
                    case "Τρίπολη": { TranslatedCityNames.Add("Tripolis"); } break;
                    case "Θεσσαλονίκη": { TranslatedCityNames.Add("Thessaloniki"); } break;
                    case "Ζάκυνθος": { TranslatedCityNames.Add("Zakinthos"); } break;
                }

                return TranslatedCityNames;
            }

            return null;
        }

        public static string TranslatedCityName(string CityName)
        {
                switch (CityName)
                {
                    case "Αλεξ/πολη": { CityName = "Alexandroupoli"; } break;
                    case "Αθήνα": { CityName = "Elefsis"; } break;
                    case "Χίος": { CityName = "Chios"; } break;
                    case "Ηράκλειο": { CityName = "Heraklion"; } break;
                    case "Καστοριά": { CityName = "Kastoria"; } break;
                    case "Κεφαλονιά": { CityName = "Kefalhnia"; } break;
                    case "Κώς": { CityName = "Kos"; } break;
                    case "Κάρπαθος": { CityName = "Karpathos"; } break;
                    case "Κέρκυρα": { CityName = "Kerkyra"; } break;
                    case "Καβάλα": { CityName = "Chrysoupoli"; } break;
                    case "Κοζάνη": { CityName = "Kozani"; } break;
                    case "Λήμνος": { CityName = "Limnos"; } break;
                    case "Λάρισα": { CityName = "Larissa"; } break;
                    case "Μυτιλήνη": { CityName = "Mytilini"; } break;
                    case "Ρόδος": { CityName = "Rhodes"; } break;
                    case "Πάτρα": { CityName = "Araxos"; } break;
                    case "Σκιάθος": { CityName = "Skiathos"; } break;
                    case "Σάμος": { CityName = "Samos"; } break;
                    case "Σαντορίνη": { CityName = "Santorini"; } break;
                    case "Τρίπολη": { CityName = "Tripolis"; } break;
                    case "Θεσσαλονίκη": { CityName = "Thessaloniki"; } break;
                    case "Ζάκυνθος": { CityName = "Zakinthos"; } break;
                }

                return CityName;
        }
    }
}
