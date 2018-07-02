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
using Community.CsharpSqlite.SQLiteClient;
using AirportWeatherPro.Model;
using System.Collections.Generic;

namespace AirportWeatherPro.ViewModel
{
    public class DbEngine
    {
        static string StringConnection = "URI=file:AkazooDB.db";
        SqliteConnection AirWeatherProDatabase = new SqliteConnection(StringConnection);

        public bool CheckDBConnection()
        {
            try
            {
                if (AirWeatherProDatabase == null)
                {
                    using (this.AirWeatherProDatabase = new SqliteConnection(StringConnection))
                    {
                        AirWeatherProDatabase.Open();
                        return true;
                    }
                }

                else
                {
                    AirWeatherProDatabase.Open();
                    return true;
                }
            }

            catch
            {
                return false;
            }
        }

        public bool CreateTable(string name, string command)
        {
            try
            {
                AirWeatherProDatabase.Open();
                SqliteCommand cmd = new SqliteCommand("CREATE TABLE IF NOT EXISTS " + name + " " + command, AirWeatherProDatabase);
                cmd.ExecuteNonQuery();

                return true;
            }

            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
                return false;
            }
        }

        public bool FillTable(string TblName, object obj)
        {
            if (obj is Airport)
            {
                try
                {
                    Airport airport = obj as Airport;
                    AirWeatherProDatabase.Open();
                    SqliteCommand fillTable = new SqliteCommand("INSERT INTO " + TblName + "(Code,WeatherCondition,Temperature,Humidity,Visibility,Wind,Pressure,AirportImage,DateTime,IsFavorite) VALUES ('`" + airport.Code + "`' ,'`" + airport.WeatherCondition + "`' , '`" + airport.Temperature + "`' , '`" + airport.Humidity + "`' , '`" + airport.Visibility + "`', '`" + airport.Wind + "`','`" + airport.Pressure + "`', '`" + airport.AirportImage + "`', '`" + airport.DateTime + "`', '" + airport.IsFavorite + "')", AirWeatherProDatabase);
                    fillTable.ExecuteNonQuery();
                    return true;
                }

                catch (Exception exp)
                {
                    MessageBox.Show(exp.ToString());
                    return false;
                }

            }
            return false;
        }

        public bool UpdateTable(string TblName, object obj)
        {
            if (obj is Airport)
            {
                try
                {
                    Airport airport = obj as Airport;
                    AirWeatherProDatabase.Open();

                    SqliteCommand updateTable = new SqliteCommand("UPDATE " + TblName + " SET Code ='`" + airport.Code + "`', WeatherCondition = '`" + airport.WeatherCondition + "`', Temperature = '`" + airport.Temperature + "`', Humidity = '`" + airport.Humidity + "`', Visibility = '`" + airport.Visibility + "`', Wind = '`" + airport.Wind + "`', Pressure = '`" + airport.Pressure + "`', DateTime = '`" + airport.DateTime + "`', AirportImage = '`" + airport.Code + "`' WHERE Code = '`" + airport.Code + "`' ", AirWeatherProDatabase);

                    updateTable.ExecuteNonQuery();
                    return true;

                }
                catch { return false; }
            }

            return false;
        }

        public bool DropTable(string TblName)
        {
            try
            {
                AirWeatherProDatabase.Open();
                SqliteCommand cmd = new SqliteCommand("DROP TABLE" + TblName, AirWeatherProDatabase);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch { return false; }
        }

        public Airport GetAirportData(string Code)
        {
            Airport airport = new Airport();

            AirWeatherProDatabase.Open();
            string query = "SELECT * FROM Airport WHERE Code Like '%" + Code + "%'";

            using (SqliteCommand cmd = new SqliteCommand(query, AirWeatherProDatabase))
            {
                using (SqliteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        airport.Code = reader.GetString(0).Replace("`", string.Empty);
                        airport.WeatherCondition = reader.GetString(1).Replace("`", string.Empty);
                        airport.Temperature = reader.GetString(2).Replace("`", string.Empty);
                        airport.Humidity = reader.GetString(3).Replace("`", string.Empty);
                        airport.Visibility = reader.GetString(4).Replace("`", string.Empty);
                        airport.Wind = reader.GetString(5).Replace("`", string.Empty);
                        airport.Pressure = reader.GetString(6).Replace("`", string.Empty);
                        airport.AirportImage = reader.GetString(7).Replace("`", string.Empty);
                        airport.DateTime = (reader.GetString(8).Replace("`", string.Empty));
                    }
                }
            }

            return airport;
        }

        public static List<string> GetFavoriteAirports()
        {
            List<string> Favorites = new List<string>();
            SqliteConnection AirWeatherProDatabase = new SqliteConnection(StringConnection);
            AirWeatherProDatabase.Open();
            string query = "SELECT Code FROM Airport WHERE IsFavorite = 'true' ";
            string temp;

            using (SqliteCommand cmd = new SqliteCommand(query, AirWeatherProDatabase))
            {
                using (SqliteDataReader reader = cmd.ExecuteReader())
                {
                   
                    while (reader.Read())
                    {
                       temp = (reader.GetString(0));
                       Favorites.Add(temp);
                    }
                }
                return Favorites;
            }
        }

        //toggle switch method if true make it false is fals make it true
        public bool AddAirportToFavorites(string code)
        {
            // Open Connection
            AirWeatherProDatabase.Open();

            Airport airport = new Airport();
            string queryresult = null;
   
            // First Check if this airport is already to favorites list
            string query = "SELECT IsFavorite FROM Airport WHERE Code = '`" + code + "`'";

            using (SqliteCommand cmd = new SqliteCommand(query, AirWeatherProDatabase))
            {
                using (SqliteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        queryresult = (reader.GetString(0));
                    }
                }
            }

            // this airport code belongs already to favorites so return false and set airport IsFavorite to False
            if (queryresult == "true")
            {
                SqliteCommand updateTable = new SqliteCommand("UPDATE  Airport SET IsFavorite = 'false' WHERE Code = '`" + code + "`' ", AirWeatherProDatabase);
                updateTable.ExecuteNonQuery();
                return false;
            }

            // this airport code does not belong to favorites so return true and set airport IsFavorite to True
            SqliteCommand updateTable2 = new SqliteCommand("UPDATE  Airport SET IsFavorite = 'true' WHERE Code = '`" + code + "`' ", AirWeatherProDatabase);
            updateTable2.ExecuteNonQuery();
            return true;
        }

        public static bool RemoveAirportFromFavorites(string code)
        {
            SqliteConnection AirWeatherProDatabase = new SqliteConnection(StringConnection);

            SqliteCommand updateTable = new SqliteCommand("UPDATE  Airport SET IsFavorite = 'false' WHERE Code = '`" + code + "`' ", AirWeatherProDatabase);

            updateTable.ExecuteNonQuery();
            return true;
        }
    }
}
