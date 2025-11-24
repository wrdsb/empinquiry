using System;
using System.Diagnostics;
using System.Reflection;
using NLog;
using NLog.Config;
using NLog.AWS.Logger;
using System.Web.Configuration;

namespace empinquiry
{
    public class Error
    {
       
        public void handleError(Exception exception, string source)
        {
            Loggers.Log("SQL ERROR: " + exception.Message + " STACK: " + exception.StackTrace + " TEXT : " + source);

            if (exception != null)
            {

                System.Diagnostics.EventLog log = new System.Diagnostics.EventLog();
                string errorMessage = "Message: " + exception.Message + "\r\nSource: " + exception.Source + "\r\nStack: " + exception.StackTrace;
                if (exception.TargetSite != null)
                {
                    errorMessage += "\r\nTarget: " + exception.TargetSite.ToString();
                }

                if (exception.InnerException != null)
                {
                    errorMessage += "\r\nInner: " + exception.InnerException.ToString();
                }
                log.Source = WebConfigurationManager.AppSettings["loginTitle"].ToString();
                log.WriteEntry(errorMessage, System.Diagnostics.EventLogEntryType.Error);
            }                    
        }
    }
}