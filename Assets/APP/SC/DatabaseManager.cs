using System.Threading;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Mono.Data.Sqlite;
using UnityEngine;
using System.Linq;

public class DatabaseManager : MonoBehaviour
{
    public string conn = SetDataBaseClass.SetDataBase("DATABASE.db");
    public IDbConnection dbconn;
    public IDbCommand dbcmd;
    public IDataReader reader;
    public int writer;

    private void Start()
    {
        Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
        DatabaseCommands.Initialize(this);
    }
    public void OpenConnection()
    {
        dbconn = new SqliteConnection(conn);
        dbconn.Open();
        dbcmd = dbconn.CreateCommand();
    }
    public void CloseConnection()
    {
        dbcmd.Parameters.Clear();
        if (reader != null)
        {
            reader.Close();
            reader = null;
        }
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }
}

public static class DatabaseCommands
{
    private static DatabaseManager _db;
    static List<int> queue = new List<int>();
    static int max_que = 0;

    public static void Initialize(DatabaseManager manager)
    {
        _db = manager;
    }

    public static async Task<bool> Entering(string name, string password)
    {
        int que = GenerateQue();
        

        while (queue[0] != que)
        { await Task.Delay(100); }

        _db.OpenConnection();
        string sqlQuery = @$"SELECT id, tags, submited FROM Users WHERE name='{name}' AND password='{password}'";
        _db.dbcmd.CommandText = sqlQuery;
        _db.reader = _db.dbcmd.ExecuteReader();
        while (_db.reader.Read())
        {
            var user = new User
            (
                _db.reader.GetInt32(0),
                name,
                _db.reader.GetString(1),
                _db.reader.GetInt32(2)
            );
            Info.user = user;
            queue.RemoveAll(x => x == que);

            _db.CloseConnection();

            return true;
        }
        queue.RemoveAll(x => x == que);
        return false;
        


    }
    static int GenerateQue() {
        max_que++;
        queue.Add(max_que);
        return max_que;
    }
}