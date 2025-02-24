using Microsoft.Extensions.Configuration;
using Serilog;
using System.Configuration;


namespace WorkStationAssistant
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.user.json", optional: true, reloadOnChange: true);

            var config = builder.Build();

            Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(config)
            //.WriteTo.RollingFile(AppDomain.CurrentDomain.BaseDirectory, "C:\\logs\\log-{Date}.txt"),
            //outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] [{SourceContext}] [{EventId}] {Message}{NewLine}{Exception}")
            //.ReadFrom.Configuration(Configuration)
            .CreateLogger();

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.\
            try
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new WorkStationAssistant(config));
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Error");
            }
        }
    }
}