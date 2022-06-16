namespace GPNA.AlarmControlSystem.Config
{
    public class Connection
    {
        public string Uri { get; set; } = "";
        public int TimeOut { get; set; } = 0;
    }
    public class ConnectionConfig
    {
        public Connection? LocalDb { get; set; }
    }
}
