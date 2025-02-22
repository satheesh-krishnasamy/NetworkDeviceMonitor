using Microsoft.Data.Sqlite;
using WorkStationAssistant.Lib.Model.DataStore;

namespace WorkStationAssistant.Lib.DAL
{
    public class NetworkDataStore : IDeviceMonitorDataStore
    {
        public NetworkDataStore(string connectionString)
        {
            this.connectionString = connectionString;

            CreateUserTable();

        }

        private readonly string connectionString;

        public void CreateUserTable()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"CREATE TABLE IF NOT EXISTS BatteryData(RecordId INTEGER  PRIMARY KEY AUTOINCREMENT, EventDateTime DATETIME NOT NULL, BatteryPercentage INTEGER  NOT NULL, IsCharging BIT NOT NULL, SessionId VARCHAR(20) NULL)";

                    // Create table if not exist    
                    command.ExecuteNonQuery();
                }
            }
        }

        public async Task<IEnumerable<BatteryDataItem>> GetAsync(DateTime startDateTime, DateTime endDateTime)
        {
            List<BatteryDataItem> result = new List<BatteryDataItem>();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();

                //if (startDateTime == DateTime.MinValue)
                //    startDateTime = DateTime.Now.Date.AddYears(-1);
                //if (endDateTime == DateTime.MinValue)
                //    endDateTime = DateTime.Now;

                command.CommandText = @" SELECT EventDateTime, BatteryPercentage, IsCharging, SessionId FROM BatteryData WHERE EventDateTime >= $StartTime AND EventDateTime <= $EndTime";
                command.Parameters.AddWithValue("$StartTime", startDateTime);
                command.Parameters.AddWithValue("$EndTime", endDateTime);


                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var record = new BatteryDataItem();
                        record.EventDateTime = reader.GetDateTime(0);
                        record.BatteryPercentage = reader.GetInt32(1);
                        record.IsCharging = reader.GetBoolean(2);
                        record.ChargingSessionId = reader.GetString(3);
                        result.Add(record);
                    }
                }
            }

            return result;
        }

        public async Task<IEnumerable<BatteryDataItem>> GetLastBatteryChargingSessionsAsync(DateTime startDateTime, DateTime endDateTime)
        {
            List<BatteryDataItem> result = new List<BatteryDataItem>();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();

                //if (startDateTime == DateTime.MinValue)
                //    startDateTime = DateTime.Now.Date.AddYears(-1);
                //if (endDateTime == DateTime.MinValue)
                //    endDateTime = DateTime.Now;

                command.CommandText = @" SELECT top 1 EventDateTime, BatteryPercentage, IsCharging, SessionId FROM BatteryData WHERE EventDateTime >= $StartTime AND EventDateTime <= $EndTime and IsCharging = 1 order by EventDateTime desc ";
                command.Parameters.AddWithValue("$StartTime", startDateTime);
                command.Parameters.AddWithValue("$EndTime", endDateTime);


                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var record = new BatteryDataItem();
                        record.EventDateTime = reader.GetDateTime(0);
                        record.BatteryPercentage = reader.GetInt32(1);
                        record.IsCharging = reader.GetBoolean(2);
                        record.ChargingSessionId = reader.GetString(3);

                        result.Add(record);
                    }
                }
            }

            return result;
        }


        public async Task<bool> SaveAsync(BatteryDataItem batteryData)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.CommandText = @"INSERT INTO BatteryData(EventDateTime, BatteryPercentage, IsCharging, SessionId) VALUES($EventDateTime, $BatteryPercentage, $IsCharging, $SessionId)";
                command.Parameters.AddWithValue("$EventDateTime", batteryData.EventDateTime);
                command.Parameters.AddWithValue("$BatteryPercentage", batteryData.BatteryPercentage);
                command.Parameters.AddWithValue("$IsCharging", batteryData.IsCharging);
                command.Parameters.AddWithValue("$SessionId", batteryData.ChargingSessionId);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }
    }
}
