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

            string commandText = @"CREATE PROC usp_MinionsOnVillain @VillainId INT
                                   AS
                                   BEGIN
                                    SELECT v.Name
                                      FROM Villains AS v
                                     WHERE ID = @VillainId

                                     SELECT m.[Name], Age
                                     FROM Minions AS m 
                                    INNER JOIN MinionsVillains AS mv
                                       ON mv.MinionId = m.Id
                                    WHERE mv.VillainId = @VillainId
                                   END";
            SqlCommand procMinionsOnVillain = new SqlCommand(commandText, connection);
            procMinionsOnVillain.ExecuteNonQuery();

            int villainId = int.Parse(Console.ReadLine());
            string execProcText = $"EXEC usp_MinionsOnVillain {villainId}";
            SqlCommand execProc = new SqlCommand(execProcText, connection);
            using (execProc)
            {
                using (SqlDataReader reader = execProc.ExecuteReader())
                {
                    if(!reader.HasRows)
                    {
                        Console.WriteLine($"No villain with ID {villainId} exists in the database.");
                        return;
                    }

                    reader.Read();
                    Console.WriteLine($"Villain: {reader[0]}");

                    if (reader.NextResult())
                    {
                        int index = 0;
                        while (reader.Read())
                        {
                            Console.WriteLine($"{++index}. {reader[0]} {reader[1]}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("(no minions)");
                    }
                }
            }
            connection.Close();
        }
    }
}
