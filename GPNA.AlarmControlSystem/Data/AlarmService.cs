using System.Text.Json;

namespace GPNA.AlarmControlSystem.Data
{
    public class AlarmService
    {
        
        public Task<Alarm[]> GetAlarmsAsync()
        {
            var obj = Task.FromResult(JsonSerializer.Deserialize<Alarm[]>(File.ReadAllText("Alarms.json")));
            return obj!;
        }
    }
}
