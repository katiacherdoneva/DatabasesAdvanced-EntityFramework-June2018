using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

public class StartUp
{
    static void Main()
    {
        int[] minionIds = Console.ReadLine()
            .Split(' ')
            .Select(int.Parse)
            .ToArray();

        var connection = new SqlConnection(Configuration.ConnectionString);
        using (connection)
        {
            connection.Open();

            for (int i = 0; i < minionIds.Length; i++)
            {
                UpdateMinionsAge(minionIds, connection, i);
                PrintNameAndAgeUpdateMinion(minionIds, connection, i);
            }

            connection.Close();
        }
    }

    private static void PrintNameAndAgeUpdateMinion(int[] minionIds, SqlConnection connection, int i)
    {
        string getNameAndAgeMinionText = $@"SELECT [Name], Age
                                        FROM Minions
                                       WHERE Id = {minionIds[i]}";

        using (SqlCommand getNameAndAgeMinion = new SqlCommand(getNameAndAgeMinionText, connection))
        {
            using (SqlDataReader reader = getNameAndAgeMinion.ExecuteReader()) 
            {
                while (reader.Read())
                {
                    string titleCase = CultureInfo.CurrentCulture.TextInfo.ToTitleCase($"{reader[0]} {reader[1]}");

                    Console.WriteLine(titleCase);
                }
            }
        }
    }

    private static void UpdateMinionsAge(int[] minionIds, SqlConnection connection, int i)
    {
        string updateMinionAgeText = $@"UPDATE Minions
                                               SET Age += 1
                                             WHERE Id = {minionIds[i]}";
        using (SqlCommand updateMinionAge = new SqlCommand(updateMinionAgeText, connection))
        {
            updateMinionAge.ExecuteNonQuery();
        }
    }
}
