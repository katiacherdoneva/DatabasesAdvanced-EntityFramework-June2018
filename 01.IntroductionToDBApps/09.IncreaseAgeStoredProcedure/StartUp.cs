using System;
using System.Data.SqlClient;

public class StartUp
{
    static void Main()
    {
        int minionId = int.Parse(Console.ReadLine());

        var connection = new SqlConnection(Configuration.ConnectionString);
        using (connection)
        {
            connection.Open();

            CreateProcGetOlder(connection);
            ExecProcGetOlder(minionId, connection);

            PrintMinionGetOlder(minionId, connection);
            connection.Close();
        }
    }

    private static void PrintMinionGetOlder(int minionId, SqlConnection connection)
    {
        string getNameAndAgeMinionText = $@"SELECT [Name], Age
                                        FROM Minions
                                       WHERE Id = {minionId}";

        using (SqlCommand getNameAndAgeMinion = new SqlCommand(getNameAndAgeMinionText, connection))
        {
            using (SqlDataReader reader = getNameAndAgeMinion.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"{reader[0]} - {reader[1]} years old");
                }
            }
        }
    }

    private static void CreateProcGetOlder(SqlConnection connection)
    {
        string procGetOlderText = $@"CREATE PROC usp_GetOlder @MinionId INT
                                     AS
                                     BEGIN
                                       UPDATE Minions
                                          SET Age += 1
                                        WHERE Id = @MinionId
                                     END";

        using (SqlCommand procGetOlder = new SqlCommand(procGetOlderText, connection))
        {
            procGetOlder.ExecuteNonQuery();
        }
    }

    private static void ExecProcGetOlder(int minionId, SqlConnection connection)
    {
        string execProcGetOlderText = $@"EXEC usp_GetOlder {minionId}";

        using (SqlCommand execProcGetOlder = new SqlCommand(execProcGetOlderText, connection))
        {
            execProcGetOlder.ExecuteNonQuery();
        }
    }
}
