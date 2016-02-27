using UnityEngine;
using System;
using Mono.Data.SqliteClient;
using System.IO;
using System.Data;


namespace SQLConnect
{
    public class SQLiteConnector : MonoBehaviour
    {

        public static SQLiteConnector Conn = null;
        public bool DebugMode = false;

        /// <summary>
        /// Table name and file location
        /// TODO: Replace this with: https://msdn.microsoft.com/en-us/library/system.io.path.combine.aspx
        /// </summary>
        private const string SQL_DB_NAME = "WordAlchemy";
        private static readonly string SQL_DB_LOCATION = "URI=file:"
            + Application.dataPath + Path.DirectorySeparatorChar
            + "Plugins" + Path.DirectorySeparatorChar
            + "Databases" + Path.DirectorySeparatorChar
            + SQL_DB_NAME + ".db";

        // table name
        private const string SQL_TABLE_NAME = "Combinations";
        /// <summary>
        /// predefine columns here for the combinations table
        /// </summary>
        private const string COL_BASE_WORD = "Base";
        private const string COL_COMBINATION_WORD = "Combination";
        private const string COL_RESULT_WORD = "Result";

        /// <summary>
        /// DB Objects
        /// </summary>
        private IDbConnection mConnection = null;
        private IDbCommand mCommand = null;
        private IDataReader mReader = null;
        private string mSQLString;

        public bool mCreateNewTable = false;

        /// <summary>
        /// Initialize the DB connection on Awake
        /// </summary>

        void Awake()
        {
            Debug.Log(SQL_DB_LOCATION);
            Conn = this;
            SQLiteInit();
        }

        void OnDestory()
        {
            SQLiteClose();
        }

        private void SQLiteClose()
        {
            if (mReader != null && !mReader.IsClosed)
            {
                mReader.Close();
            }
            mReader = null;

            if (mCommand != null)
            {
                mCommand.Dispose();
            }
            mCommand = null;

            if (mConnection != null && mConnection.State != ConnectionState.Closed)
            {
                mConnection.Close();
            }
            mConnection = null;
        }

        private void SQLiteInit()
        {
            Debug.Log("Opening SQLite Connection at " + SQL_DB_LOCATION);
            mConnection = new SqliteConnection(SQL_DB_LOCATION);
            mCommand = mConnection.CreateCommand();
            mConnection.Open();

            //WAL = write ahead logging, very huge speed increase
            mCommand.CommandText = "PRAGMA journal_mode = WAL";
            mCommand.ExecuteNonQuery();

            //Not sure we need this but it was in the example
            mCommand.CommandText = "PRAGMA journal_mode";
            mReader = mCommand.ExecuteReader();
            if (DebugMode && mReader.Read())
            {
                Debug.Log("WAL value is: " + mReader.GetString(0));
            }
            mReader.Close();

            //more speed increases
            mCommand.CommandText = "PRAGMA synchronous = OFF";
            mCommand.ExecuteNonQuery();

            // again not sure if this is needed
            mCommand.CommandText = "PRAGMA synchronous";
            mReader = mCommand.ExecuteReader();
            if (DebugMode && mReader.Read())
                Debug.Log("synchronous value is: " + mReader.GetInt32(0));
            mReader.Close();

            // here we check if the table you want to use exists or not.  If it doesn't exist we create it.
            mCommand.CommandText = "SELECT name FROM sqlite_master WHERE name='" + SQL_TABLE_NAME + "'";
            mReader = mCommand.ExecuteReader();
            if (!mReader.Read())
            {
                Debug.Log("Could not find SQLite table " + SQL_TABLE_NAME);
                mCreateNewTable = true;
            }
            mReader.Close();

            // create new table if it wasn't found
            if (mCreateNewTable)
            {
                Debug.Log("Dropping old SQLite table if Exists: " + SQL_TABLE_NAME);

                // insurance policy, drop table
                mCommand.CommandText = "DROP TABLE IF EXISTS " + SQL_TABLE_NAME;
                mCommand.ExecuteNonQuery();

                Debug.Log("SQLiter - Creating new SQLite table: " + SQL_TABLE_NAME);

                // create new - SQLite recommendation is to drop table, not clear it
                mSQLString = "CREATE TABLE IF NOT EXISTS " + SQL_TABLE_NAME + " (" +
                    COL_BASE_WORD + " TEXT, " +
                    COL_COMBINATION_WORD + " TEXT, " +
                    COL_RESULT_WORD + " TEXT, " +
                    "PRIMARY KEY("
                    + COL_BASE_WORD + ", " + COL_COMBINATION_WORD + "))";
                mCommand.CommandText = mSQLString;
                mCommand.ExecuteNonQuery();
            }
            else
            {
                if (DebugMode)
                    Debug.Log("SQLite table " + SQL_TABLE_NAME + " was found");
            }

            // close connection
            mConnection.Close();
        }

        #region Insert

        public void InsertCombination(string baseWord, string combinationWord, string result)
        {
            mSQLString = "INSERT OR REPLACE INTO " + SQL_TABLE_NAME
                + " ("
                + COL_BASE_WORD + ","
                + COL_COMBINATION_WORD + ","
                + COL_RESULT_WORD + ") VALUES ("
                + "'" + baseWord + "',"
                + "'" + combinationWord + "',"
                + "'" + result + "');";
            if (DebugMode)
            {
                Debug.Log(mSQLString);
            }
            mCommand.CommandText = mSQLString;
            mCommand.ExecuteNonQuery();
        }
        #endregion

        #region MatchQuery
        public string MatchQuery(string baseWord, string combinationWord)
        {
            string result = "";
            mConnection.Open();
            mSQLString = "Select "
                + COL_RESULT_WORD
                + " FROM "
                + SQL_TABLE_NAME
                + " WHERE "
                + COL_BASE_WORD + "='" + baseWord + "'"
                + " AND "
                + COL_COMBINATION_WORD + "='" + combinationWord + "';";
            mCommand.CommandText = mSQLString;
            mReader = mCommand.ExecuteReader();
            if (mReader.Read())
            {
                result = mReader.GetString(0);
            }
            else
            {
                Debug.Log("QueryString - nothing to read...");
            }
            mReader.Close();
            mConnection.Close();
            return result;
        }
        #endregion

    }
}



