using System;
using System.Data.SqlClient;

public class StartUp
{
    static void Main()
    {
        int villainId = int.Parse(Console.ReadLine());
        var connection = new SqlConnection(Configuration.ConnectionString);

        using (connection)
        {
            connection.Open();

            string findVillainText = $@"SELECT [Name] FROM  Villains WHERE Id = {villainId}";

            using (SqlCommand findVillain = new SqlCommand(findVillainText, connection))
            {
                if(findVillain.ExecuteScalar() == null)
                {
                    Console.WriteLine("No such villain was found.");
                }
                else
                {
                    Console.WriteLine($"{findVillain.ExecuteScalar()} was deleted.");

                    DeleteVillainsAndReleasedMinions(villainId, connection, findVillain);
                }
            }
                connection.Close();
        }
    }

    private static void DeleteVillainsAndReleasedMinions(int villainId, SqlConnection connection, SqlCommand findVillain)
    {
        string releasedMinionsText = $@"DELETE MinionsVillains
                                         WHERE VillainId = {villainId}";

        using (SqlCommand releasedMinions = new SqlCommand(releasedMinionsText, connection))
        {
            Console.WriteLine($"{releasedMinions.ExecuteNonQuery()} minions were released.");
        }

        string deleteVillainText = $@"DELETE FROM Villains 
                                       WHERE Id = {villainId}";

        using (SqlCommand deleteVillain = new SqlCommand(deleteVillainText, connection))
        {
            deleteVillain.ExecuteNonQuery();
        }
    }
}

