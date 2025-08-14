using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace empinquiry
{
    class SQLProvider
    {
        private string ConnectionString;

        public SQLProvider(String connectionString = "")
        {
            //constructor
            //allow overide of default connection string
            ConnectionString = connectionString;
        }

        public SqlConnection GetConnection()
        {
            try
            {
                if (!String.IsNullOrEmpty(ConnectionString))
                {
                    return new SqlConnection(ConnectionString);
                }
                else
                {
                    // sql connection needs passed parameter
                    Configuration ThisConfig;
                    ThisConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    return new SqlConnection(ThisConfig.ConnectionStrings.ConnectionStrings["SqlDB"].ToString());
                }
            }
            catch (Exception Ex)
            {
                Error error = new Error();
                error.handleError(Ex, "SQL GetConnection Error");

                return null;
            }
        }

        public SqlCommand GetCommand()
        {
            //
            //Default parameters for command object
            //
            SqlCommand SqlCmd = new SqlCommand
            {
                CommandTimeout = 500,
                CommandType = CommandType.Text
            };

            return SqlCmd;
        }

        public DataTable GetDataTable(String sqlStatement, out Boolean success, SqlParameter[] parameters = null)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection SqlConn = GetConnection())
                {
                    using (SqlCommand SqlCmd = GetCommand())
                    {
                        SqlCmd.Connection = SqlConn;
                        SqlCmd.CommandText = sqlStatement;

                        if (parameters != null)
                        {
                            //loop and add parameters
                            for (int i = 0; i < parameters.Length; i++)
                            {
                                SqlCmd.Parameters.Add(parameters[i]);
                            }
                        }

                        using (SqlDataAdapter SQLAdapter = new SqlDataAdapter(SqlCmd))
                        {
                            using (dt)
                            {
                                SQLAdapter.Fill(dt);
                                success = true;
                                return dt;
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                //capture the db parameters passed
                ICollection<KeyValuePair<string, string>> ParameterArray = new Dictionary<string, string>();

                if (parameters != null)
                {
                    foreach (SqlParameter parameter in parameters)
                    {
                        ParameterArray.Add(new KeyValuePair<string, string>(parameter.ParameterName, parameter.Value.ToString()));
                    }
                }

                Error error = new Error();
                error.handleError(Ex, "SQL GetDataTable Error");
                success = false;
                return dt;
            }
        }

        public int GetScalarInteger(String sqlStatement, out Boolean success, SqlParameter[] parameters = null)
        {
            int ReturnValue = 0;

            try
            {
                using (SqlConnection SqlConn = GetConnection())
                {
                    using (SqlCommand SqlCmd = GetCommand())
                    {
                        SqlCmd.Connection = SqlConn;
                        SqlCmd.CommandText = sqlStatement;

                        if (parameters != null)
                        {
                            //loop and add parameters
                            for (int i = 0; i < parameters.Length; i++)
                            {
                                SqlCmd.Parameters.Add(parameters[i]);
                            }
                        }

                        SqlConn.Open();
                        ReturnValue = Convert.ToInt32(SqlCmd.ExecuteScalar());
                        success = true;
                        return ReturnValue;
                    }
                }

            }
            catch (Exception Ex)
            {
                //capture the db parameters passed
                ICollection<KeyValuePair<string, string>> ParameterArray = new Dictionary<string, string>();

                if (parameters != null)
                {
                    foreach (SqlParameter parameter in parameters)
                    {
                        ParameterArray.Add(new KeyValuePair<string, string>(parameter.ParameterName, parameter.Value.ToString()));
                    }
                }

                Error error = new Error();
                error.handleError(Ex, "SQL GetScalarInteger Error");
                success = false;
                return 0;
            }
        }

        public string GetScalarString(String sqlStatement, out Boolean success, SqlParameter[] parameters = null)
        {
            string ReturnValue = "";

            try
            {
                using (SqlConnection SqlConn = GetConnection())
                {
                    using (SqlCommand SqlCmd = GetCommand())
                    {
                        SqlCmd.Connection = SqlConn;
                        SqlCmd.CommandText = sqlStatement;

                        if (parameters != null)
                        {
                            //loop and add parameters
                            for (int i = 0; i < parameters.Length; i++)
                            {
                                SqlCmd.Parameters.Add(parameters[i]);
                            }
                        }

                        SqlConn.Open();
                        ReturnValue = SqlCmd.ExecuteScalar().ToString();
                        success = true;
                        return ReturnValue;
                    }
                }

            }
            catch (Exception Ex)
            {
                //capture the db parameters passed
                ICollection<KeyValuePair<string, string>> ParameterArray = new Dictionary<string, string>();

                if (parameters != null)
                {
                    foreach (SqlParameter parameter in parameters)
                    {
                        ParameterArray.Add(new KeyValuePair<string, string>(parameter.ParameterName, parameter.Value.ToString()));
                    }
                }

                Error error = new Error();
                error.handleError(Ex, "SQL GetScalarString Error");
                success = false;
                return "";
            }
        }

        public int ExecuteNonQuery(String sqlStatement, out Boolean success, SqlParameter[] parameters = null)
        {
            int ReturnValue = 0;

            try
            {
                using (SqlConnection SqlConn = GetConnection())
                {
                    using (SqlCommand SqlCmd = GetCommand())
                    {
                        SqlCmd.Connection = SqlConn;
                        SqlCmd.CommandText = sqlStatement;

                        if (parameters != null)
                        {
                            //loop and add parameters
                            for (int i = 0; i < parameters.Length; i++)
                            {
                                SqlCmd.Parameters.Add(parameters[i]);
                            }
                        }

                        SqlConn.Open();

                        ReturnValue = SqlCmd.ExecuteNonQuery();
                        success = true;
                        return ReturnValue;
                    }
                }
            }
            catch (Exception Ex)
            {
                //capture the db parameters passed
                ICollection<KeyValuePair<string, string>> ParameterArray = new Dictionary<string, string>();

                if (parameters != null)
                {
                    foreach (SqlParameter parameter in parameters)
                    {
                        ParameterArray.Add(new KeyValuePair<string, string>(parameter.ParameterName, parameter.Value.ToString()));
                    }
                }

                Error error = new Error();
                error.handleError(Ex, "SQL ExecuteNonQuery Error");
                success = false;
                return ReturnValue;
            }
        }

        public void ExecuteStoredProcedure(String sqlStatement, out Boolean success, SqlParameter[] parameters = null)
        {
            try
            {
                using (SqlConnection SqlConn = GetConnection())
                {
                    using (SqlCommand SqlCmd = GetCommand())
                    {
                        SqlCmd.Connection = SqlConn;
                        SqlCmd.CommandText = sqlStatement;
                        SqlCmd.CommandType = CommandType.StoredProcedure;
                        SqlCmd.CommandTimeout = 1800;

                        if (parameters != null)
                        {
                            //loop and add parameters
                            for (int i = 0; i < parameters.Length; i++)
                            {
                                SqlCmd.Parameters.Add(parameters[i]);
                            }
                        }

                        SqlConn.Open();

                        SqlCmd.ExecuteNonQuery();

                        success = true;
                    }
                }
            }
            catch (Exception Ex)
            {
                //capture the db parameters passed
                ICollection<KeyValuePair<string, string>> ParameterArray = new Dictionary<string, string>();

                if (parameters != null)
                {
                    foreach (SqlParameter parameter in parameters)
                    {
                        ParameterArray.Add(new KeyValuePair<string, string>(parameter.ParameterName, parameter.Value.ToString()));
                    }
                }

                Error error = new Error();
                error.handleError(Ex, "SQL ExecuteStoredProcedure Error");
                success = false;
            }
        }
    }
}