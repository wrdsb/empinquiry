using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;
using System.Data;

namespace empinquiry
{
    class ErrorProvider
    {
        private Exception Ex;
        private string SqlStatement;
        private string Parameters;

        public ErrorProvider(Exception ex, string sqlStatement = "", ICollection<KeyValuePair<string, string>> dbParameters = null)
        {
            this.Ex = ex;
            this.SqlStatement = sqlStatement;

            if (dbParameters != null)
            {
                foreach (KeyValuePair<string, string> element in dbParameters)
                {
                    this.Parameters += element.Key + " : " + element.Value + " ";
                }
            }
        }

        public Boolean LogError(string connString)
        {
            //attempt to log to the db first.  If not succesfull then write to file

            //if (!LogToDB(connString))
            {
                //cant log to db, try something else
                //LogToFile();
            }

            return true;
        }

        private Boolean LogToDB(string connString)
        {
            Boolean Success = true;

            //schema specific error log table
            string SQLStatement = @"INSERT dbo.hd_empinquiry_error_log
                                              ( error_source
                                              , error_message
                                              , stack_trace
                                              , transaction_date_time
                                              , sqlstatement
                                              , parameters)
                                    VALUES 
                                              ( @ErrorSource
                                              , @ErrorMessage
                                              , @StackTrace
                                              , GETDATE()
                                              , @Sqlstatement
                                              , @Parameters)";

            SqlParameter[] Parameters = new SqlParameter[5];

            Parameters[0] = new SqlParameter("ErrorMessage", SqlDbType.VarChar, 300);
            Parameters[0].Value = this.Ex.Message;
            Parameters[1] = new SqlParameter("ErrorSource", SqlDbType.VarChar, 300);
            Parameters[1].Value = this.Ex.Source;
            Parameters[2] = new SqlParameter("StackTrace", SqlDbType.VarChar, 3000);
            Parameters[2].Value = this.Ex.StackTrace;
            Parameters[3] = new SqlParameter("SqlStatement", SqlDbType.VarChar, 3000);
            Parameters[3].Value = this.SqlStatement;
            Parameters[4] = new SqlParameter("Parameters", SqlDbType.VarChar, 1000);
            if (this.Parameters != null)
            {
                Parameters[4].Value = this.Parameters;
            }
            else
            {
                Parameters[4].Value = DBNull.Value;
            }
            

            SQLProvider SqlDB = new SQLProvider(connString);

            SqlDB.ExecuteNonQuery(SQLStatement, out Success, Parameters);

            return Success;
        }

        private Boolean LogToFile()
        {
            //write error out to file

            //create file if none exist or append to todays file if there is one
            string LogFileName = "wrdsb-" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + "-error-" + DateTime.Now.ToShortDateString() + ".log";

            using (StreamWriter LogFile = File.AppendText(LogFileName))
            {

                LogFile.WriteLine("*******************************");
                LogFile.WriteLine("Date: " + DateTime.Now.ToShortDateString());
                LogFile.WriteLine("Time: " + DateTime.Now.ToShortTimeString());
                LogFile.WriteLine("Error Source: " + this.Ex.Source);
                LogFile.WriteLine("Error Stack Trace: " + this.Ex.StackTrace);
                LogFile.WriteLine("SQL Statement: " + this.SqlStatement);
                LogFile.WriteLine("Parameters: " + this.Parameters);
                LogFile.WriteLine();
            }

            return true;
        }

        private Boolean LogSystemEvent()
        {
            //
            //This method is not currently used.
            //

            System.Diagnostics.EventLog log = new System.Diagnostics.EventLog();
            string errorMessage = "";

            if (this.Ex != null)
            {
                errorMessage += "\r\nMessage: " + this.Ex.Message +
                "\r\nSource: " + this.Ex.Source +
                "\r\nStack: " + this.Ex.StackTrace;
            }

            if (this.Ex.TargetSite != null)
            {
                errorMessage += "\r\nTarget: " + this.Ex.TargetSite.ToString();
            }

            if (this.Ex.InnerException != null)
            {
                errorMessage += "\r\nInner: " + this.Ex.InnerException.ToString();
            }

            //info: need log source
            log.Source = "empinquiry";
            log.WriteEntry(errorMessage, System.Diagnostics.EventLogEntryType.Error);

            return true;
        }
    }
}
