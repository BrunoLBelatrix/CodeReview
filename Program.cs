using System;
using System.Linq;
using System.Text;

public class JobLogger
{
    private bool _initialized;
    private static WhereToLog _whereToLog;
    private static LogType _logType;

    public enum WhereToLog 
    { 
        File,
        Console,
        Database
    };

    public enum LogType 
    { 
        Message=1,
        Error,
        Warning
    };

    public JobLogger(WhereToLog whereToLog, LogType logType)
    {
        _whereToLog = whereToLog;
        _logType = logType;
    }

    public static void LogMessage(string messageS, LogType type)
    {
        messageS.Trim();
        if (messageS == null || messageS.Length == 0)
        {
            return;
        }
        string l = DateTime.Now.ToShortDateString() + messageS;
        
        if (type == LogType.Message && _logType == LogType.Message)
        {
            Console.ForegroundColor = ConsoleColor.White;
        }
        if (type == LogType.Error && _logType == LogType.Error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
        if (type == LogType.Warning && _logType == LogType.Warning)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
        }

        try
        {
            System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"]);
            connection.Open();
            System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand("Insert into Log Values('" + messageS + "', " + (int)type + ")");
            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            // Do something
        }

        using (System.IO.StreamWriter wr = System.IO.File.AppendText(System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToShortDateString() + ".txt"))
        {
            wr.WriteLine(l);
        }
        Console.WriteLine(DateTime.Now.ToShortDateString() + messageS);
    }
}