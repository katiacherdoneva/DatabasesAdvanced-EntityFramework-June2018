using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

public class StartUp
{
    static void Main()
    {
        string nameCountry = Console.ReadLine();

        var connection = new SqlConnection(Configuration.ConnectionString);
        using (connection)
        {
            connection.Open();

            int countryId = GetCountryId(nameCountry, connection);

            UpdateTownNamesForAGivenCountry(countryId, connection);

            connection.Close();
        }
    
    }

    private static void UpdateTownNamesForAGivenCountry(int countryId, SqlConnection connection)
    {
        string countTownsText = $@"SELECT COUNT(*)
                                 FROM Towns
                                WHERE CountryCode = {countryId}";

        using (SqlCommand countTowns = new SqlCommand(countTownsText, connection))
        {
            if(countTowns.ExecuteScalar() == null)
            {
                Console.WriteLine("No town names were affected.");
            }
            else
            {
                Console.WriteLine($"{countTowns.ExecuteScalar()} town names were affected.");
                UppercaseTownNames(countryId, connection);
                PrintTownNames(countryId, connection);
            }
        }

    }

    private static void PrintTownNames(int countryId, SqlConnection connection)
    {
        string townNamesText = $@"SELECT Name
                                    FROM Towns
                                   WHERE CountryCode = {countryId}";
        using (SqlCommand townNamesCommand = new SqlCommand(townNamesText, connection))
        {
            using (SqlDataReader reader = townNamesCommand.ExecuteReader())
            {
                List<string> townNames = new List<string>();

                while (reader.Read())
                {
                    townNames.Add((string)reader[0]);
                }
                Console.WriteLine($"[{string.Join(", ", townNames)}]");
            }
        }
    }

    private static void UppercaseTownNames(int countryId, SqlConnection connection)
    {
        string uppdateTownNamesText = $@"UPDATE Towns
                                                SET Name = UPPER(Name)
                                              WHERE CountryCode = {countryId}";
        using (SqlCommand uppdateTownNames = new SqlCommand(uppdateTownNamesText, connection))
        {
            uppdateTownNames.ExecuteNonQuery();
        }
    }

    private static int GetCountryId(string nameCountry, SqlConnection connection)
    {
        int countryId;
        string getCountryIdText = $@"SELECT Id FROM Countries
                                          WHERE Name = '{nameCountry}'";
        using (SqlCommand getCountryId = new SqlCommand(getCountryIdText, connection))
        {
            countryId = (int)getCountryId.ExecuteScalar();
        }

        return countryId;
    }
}

