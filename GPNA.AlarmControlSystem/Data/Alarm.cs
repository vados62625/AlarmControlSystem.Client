namespace GPNA.AlarmControlSystem.Data
{
    public class Alarm
    {
        public DateTime DateTime {get;set;}
        public string Date => DateTime.Date.ToString("d");
        public string Time => DateTime.Date.ToString("T");        
        public string? ConditionName { get; set; }
        public string? Source { get; set; }
        public string? Description { get;set;}

        public int Priority 
        {
            get
            {
                if (ConditionName == "PVLL")
                    return 1;
                if (ConditionName == "PVLO" || ConditionName == "RSLO")
                    return 2;
                if (ConditionName == "PVHI")
                    return 3;
                if (ConditionName == "PVHH")
                    return 4;
                if (ConditionName == "ALARM")
                    return 5;
                else
                    return 0;
            }
        }
    }
}
