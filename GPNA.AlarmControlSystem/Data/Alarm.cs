namespace GPNA.AlarmControlSystem.Data
{
    public class Alarm
    {
        public DateTime DateTime {get;set;}
        public string Date => DateTime.Date.ToString("d");
        public string Time => DateTime.Date.ToString("T");        
        public string? ConditionName { get; set; }
        public string? Source { get; set; }

        public string? Name
        {
            get
            {
                return Source + "_" + ConditionName;
            }
        }
        public string? Description { get;set;}

        public string? Priority 
        {
            get
            {
                if (ConditionName == "PVLL")
                    return "Критичный";
                if (ConditionName == "PVLO" || ConditionName == "RSLO")
                    return "Высокий";
                if (ConditionName == "PVHI")
                    return "Низкий";
                if (ConditionName == "PVHH")
                    return "Высокий";
                if (ConditionName == "ALARM")
                    return "Низкий";
                else
                    return "0";
            }
        }
    }
}
