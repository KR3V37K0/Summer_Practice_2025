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

    public static async Task<List<chanel>> GetChanels(int _id_user, string _role)
    {
        Debug.Log("1");
        int que = GenerateQue();
         while (queue[0] != que)
        { await Task.Delay(100); }
        Debug.Log("2");
        _db.OpenConnection();
        Debug.Log("3");

        string sqlQuery = @$"
                SELECT
                chanel.id,
                chanel.name,
                EXISTS (
                    SELECT 1
                    FROM Posts p
                    JOIN Reactions r ON r.id_post = p.id
                    WHERE p.id_chanel = chanel.id
                    AND r.id_user = {_id_user}
                    AND r.ver = 0
                ) AS has_unread
                FROM Chanels chanel
                JOIN Chanel_User cu ON cu.id_chanel = chanel.id
                WHERE cu.id_user = {_id_user} AND cu.role = '{_role}'";
                    
        _db.dbcmd.CommandText = sqlQuery;
        
        Debug.Log("4");
        List<chanel> chanels = new List<chanel>();
        List<int> ids = new List<int>();
        Debug.Log("5");
        _db.reader = _db.dbcmd.ExecuteReader();
        Debug.Log("6");
        while (_db.reader.Read())
        {
            Debug.Log("777");

            chanel cha = new chanel(
                _db.reader.GetInt32(0),
                _db.reader.GetString(1),
                _db.reader.GetInt32(2) == 1
            );

            if (!ids.Contains(cha.id))
            {
                chanels.Add(cha);
                ids.Add(cha.id);
            }
            

            
        }
        queue.RemoveAll(x => x == que);

        _db.CloseConnection();

        return chanels;
    }
   
   
   
   
    static int GenerateQue()
    {
        max_que++;
        queue.Add(max_que);
        return max_que;
    }
    

}