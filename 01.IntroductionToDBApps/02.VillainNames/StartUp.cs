using System;
using System.Data.SqlClient;

public class StartUp
{
    static void Main()
    {
        var connectionString =
            @"Server=DESKTOP-03RNTR8\SQLEXPRESS;" +
            @"Database=MinionsDB;" +
            @"Integrated Security=True";

        var connection = new SqlConnection(connectionString);

        using (connection)
        {
            connection.Open();

            string commandText = @"SELECT v.Name, COUNT(MinionId) AS CountMinions
                              FROM MinionsVillains AS vm
                             INNER JOIN Villains AS v
                                ON v.Id = vm.VillainId
                             GROUP BY VillainId, v.Name
                            HAVING COUNT(MinionId) > 3
                             ORDER BY CountMinions DESC";

            SqlCommand command = new SqlCommand(commandText, connection);
            using (command)
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        Console.WriteLine($"{reader[0]} -> {reader[1]}");
                    }
                }
            }
                connection.Close();
        }
    }
}

