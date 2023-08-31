﻿using GPNA.AlarmControlSystem.Models.Enums;

namespace GPNA.AlarmControlSystem.Application.Dto
{
    public class CountAlarmsOnPriority
    {
        public PriorityType PriorityType { get; set; }
        public int Count { get; set; }
    }
}