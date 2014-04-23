namespace EasySettings.SettingsStorage
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;

    /// <summary>
    /// Setting settingsStorage using SQL Server. Useful for long term setting settingsStorage.
    /// </summary>
    public class SqlServerSettingsStorage : ISettingsStorage
    {
        /// <summary>
        /// Table name that is used for storing the values
        /// </summary>
        private const string TableName = "EasySetting";

        /// <summary>
        /// Table creation script
        /// </summary>
        private const string TableScript = @"CREATE TABLE [" + TableName + "]([ID] [int] IDENTITY(1,1) NOT NULL,[Key] [varchar](255) NOT NULL,[Value] [varchar](255) NOT NULL,[CreatedOn] [datetime] NOT NULL,[ModifiedOn] [datetime] NOT NULL,CONSTRAINT [PK_EasySettings] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]) ON [PRIMARY]";

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerSettingsStorage"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string used to connect to the database</param>
        public SqlServerSettingsStorage(string connectionString)
        {
            ConnectionString = connectionString;
            Initialize();
        }

        /// <summary>
        /// Gets or sets the connection string
        /// </summary>
        string ConnectionString { get; set; }

        public void SaveSetting(string key, string value)
        {
            using (var connection = GetOpenConnection())
            using (var command = new SqlCommand("UPDATE [" + TableName + "] SET value = @Value, ModifiedOn = @DateNow WHERE [Key] = @Key IF @@ROWCOUNT=0 INSERT INTO [" + TableName + "] ([Key], [Value], [CreatedOn], [ModifiedOn]) VALUES (@Key, @Value, @DateNow, @DateNow);", connection))
            {
                command.Parameters.Add(new SqlParameter("@Key", key));
                command.Parameters.Add(new SqlParameter("@Value", value));
                command.Parameters.Add(new SqlParameter("@DateNow", DateTime.Now));

                command.ExecuteNonQuery();
            }
        }

        public string GetValue(string key)
        {
            using (var connection = GetOpenConnection())
            using (var command = new SqlCommand("SELECT [Value] FROM [" + TableName + "] WHERE Key = @Key", connection))
            {
                command.Parameters.Add(new SqlParameter("@Key", key));

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            return reader.GetString(0);
                        }
                    }
                    else return null;
                }
            }

            return null;
        }

        public Dictionary<string, string> GetAllValues()
        {
            var dictonary = new Dictionary<string, string>();
            using (var connection = GetOpenConnection())
            using (var command = new SqlCommand("SELECT [Key], [Value] FROM [" + TableName + "]", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dictonary.Add(reader.GetString(0), reader.GetString(1));
                    }
                }
            }

            return dictonary;
        }

        public void Initialize()
        {
            using (var connection = GetOpenConnection())
            {
                using(var command = new SqlCommand("SELECT COUNT(*) FROM sys.tables WHERE name = '" + TableName + "'", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.GetInt32(0) == 0)
                        {
                            CreateTable();
                        }
                    }
                }
            }
        }

        private void CreateTable()
        {
            using (var connection = GetOpenConnection())
            using (var command = new SqlCommand(TableScript, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        private SqlConnection GetOpenConnection()
        {
            var conn = GetConnection();
            if (conn.State != ConnectionState.Open) conn.Open();
            return conn;
        }
    }
}
