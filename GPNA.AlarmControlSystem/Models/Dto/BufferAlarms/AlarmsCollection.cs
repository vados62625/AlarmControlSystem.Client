﻿using GPNA.AlarmControlSystem.Models.Enums;

namespace GPNA.AlarmControlSystem.Models.Dto.BufferAlarms;

public class AlarmsCollection<TItem>
{

    public TItem[]? Items { get; set; }
    
    public int PagesCount { get; set; }

    public int TotalCount { get; set; }
    
    public int IncomingAlarmsCount { get; set; }
    
    public int ActiveAlarmsCount { get; set; }
    
    public int ImitationParamsCount { get; set; }
    
    public int SuppressedAlarmsCount { get; set; }

    public bool Fire { get; set; }

    public bool Gas { get; set; }

    public Dictionary<PriorityType, int>? CountByPriority { get; set; }

    public Dictionary<StateType, int>? CountByState { get; set; }
}