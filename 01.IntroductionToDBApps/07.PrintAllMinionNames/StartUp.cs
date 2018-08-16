using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

public class StartUp
{
    static void Main()
    {
        var connection = new SqlConnection(Configuration.ConnectionString);
        using (connection)
        {
            connection.Open();

            List<string> minions = GetNameMinions(connection);

            PrintMinions(minions);

            connection.Close();
        }
}

    private static void PrintMinions(List<string> minions)
    {
        int indexMin = 0;
        int indexMax = minions.Count - 1;

        StringBuilder sb = new StringBuilder();
        while(indexMin < indexMax)
        {
            sb.AppendLine(minions[indexMin]);
            sb.AppendLine(minions[indexMax]);

            indexMax--;
            indexMin++;
        }
        string result = sb.ToString().TrimEnd();
        Console.WriteLine(result);
    }

    private static List<string> GetNameMinions(SqlConnection connection)
    {
        List<string> minions = new List<string>();

        string nameMinionsText = $@"SELECT [Name] FROM Minions";
        using (SqlCommand nameMinions = new SqlCommand(nameMinionsText, connection))
        {
            SqlDataReader reader = nameMinions.ExecuteReader();
            while (reader.Read())
            {
                minions.Add((string)reader[0]);
            }
        }
        return minions;
    }
}

