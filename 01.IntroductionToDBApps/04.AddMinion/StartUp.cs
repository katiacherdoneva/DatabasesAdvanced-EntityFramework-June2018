using System;
using System.Data.SqlClient;
using System.Linq;

public class StartUp
{
    static void Main()
    {
        string[] minion = Console.ReadLine()
            .Split(' ')
            .ToArray();
        string[] villain = Console.ReadLine()
            .Split(' ')
            .ToArray();

        string nameMinion = minion[1];
        string ageMinion = minion[2];
        string townMinion = minion[3];
        string villainName = villain[1];

        var connection = new SqlConnection(Configuration.ConnectionString);

        using (connection)
        {
            connection.Open();
            int townId = CheckTownAndAddDB(connection, townMinion);
            int villainId = CheckVillainAndAddDB(connection, villainName);
            int minionId = CheckMinionAndAddDB(connection, nameMinion, ageMinion, townId);

            InsertMinionAndVillainInMinionsVillains(nameMinion, minionId, villainName, villainId, connection);
            connection.Close();
        }
    }

    private static int CheckMinionAndAddDB(SqlConnection connection, string nameMinion, string ageMinion, int townId)
    {
        int minionId;
        string getMinionIdText = $@"SELECT Id 
                                  FROM Minions
                                 WHERE Name = '{nameMinion}'";
        using (SqlCommand getMinionId = new SqlCommand(getMinionIdText, connection))
        {
            if (getMinionId.ExecuteScalar() == null)
            {
                string insertMinionText = $@"INSERT INTO Minions(Name, Age, TownId) VALUES ('{nameMinion}', {ageMinion}, {townId})";
                SqlCommand insertMinion = new SqlCommand(insertMinionText, connection);
                using (insertMinion)
                {
                    insertMinion.ExecuteNonQuery();
                }
            }

            minionId = (int)getMinionId.ExecuteScalar();
        }
        return minionId;
    }

    private static void InsertMinionAndVillainInMinionsVillains(string nameMinion, int minionId, string nameVallain, int villainId, SqlConnection connection)
    {
        string insertIntoMinoinsVillainsSql = $"INSERT INTO MinionsVillains (MinionId , VillainId) VALUES ({minionId}, {villainId})";

        using (SqlCommand insertMinionToHisVillain = new SqlCommand(insertIntoMinoinsVillainsSql, connection))
        {
            insertMinionToHisVillain.ExecuteNonQuery();
            Console.WriteLine($"Successfully added {nameMinion} to be minion of {nameVallain}.");
        }
    }

    private static int CheckVillainAndAddDB(SqlConnection connection, string villainName)
    {
        int villainId;
        string getVillainIdText = $@"SELECT Id 
                                  FROM Villains
                                 WHERE Name = '{villainName}'";
        using (SqlCommand getVillainId = new SqlCommand(getVillainIdText, connection))
        {
            if (getVillainId.ExecuteScalar() == null)
            {
                string insertVallainText = $@"INSERT INTO Villains(Name, EvilnessFactorId) VALUES ('{villainName}', 4)";
                SqlCommand insertVallain = new SqlCommand(insertVallainText, connection);
                using (insertVallain)
                {
                    insertVallain.ExecuteNonQuery();
                }
                Console.WriteLine($"Villain {villainName} was added to the database");
            }

            villainId = (int)getVillainId.ExecuteScalar();
        }
        return villainId;
    }

    private static int CheckTownAndAddDB(SqlConnection connection, string townMinion)
    {
        int townId;
        string getTownIdText = $@"SELECT Id 
                                  FROM Towns
                                 WHERE Name = '{townMinion}'";
        using (SqlCommand getTownId = new SqlCommand(getTownIdText, connection))
        {
            if (getTownId.ExecuteScalar() == null)
            {
                string insertTownText = $@"INSERT INTO Towns(Name) VALUES ('{townMinion}')";
                SqlCommand insertTown = new SqlCommand(insertTownText, connection);
                using (insertTown)
                {
                    insertTown.ExecuteNonQuery();
                }
                Console.WriteLine($"Town {townMinion} was added to the database.");
            }

            townId = (int)getTownId.ExecuteScalar();
        }
        return townId;
    }

}
