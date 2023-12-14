﻿using GPNA.AlarmControlSystem.Models.Enums;

namespace GPNA.AlarmControlSystem.Models.Dto.IncomingAlarm
{
    public class GetIncomingAlarmsByDatesQuery
    {
        /// <summary>
        /// Id рабочей станции
        /// </summary>
        public int WorkStationId { get; set; }
        
        /// <summary>
        /// Начало периода
        /// </summary>
        public DateTimeOffset DateTimeStart { get; set; }
        
        /// <summary>
        /// Конец периода
        /// </summary>
        public DateTimeOffset DateTimeEnd { get; set; }
        
        /// <summary>
        /// Статус сигнализации
        /// </summary>
        public StatusAlarmType? StatusAlarm { get; set; }
        
        /// <summary>
        /// Тег
        /// </summary>
        public string? TagName { get; set; }
        
        /// <summary>
        /// Приоритет
        /// </summary>
        public List<PriorityType>? Priority { get; set; }
        
        /// <summary>
        /// Состояние
        /// </summary>
        public List<StateType>? State { get; set; }
        
        /// <summary>
        /// Страница
        /// </summary>
        public int? Page { get; set; }
    
        /// <summary>
        /// Количество элементов на странице
        /// </summary>
        public int? CountOnPage { get; set; }
    
        /// <summary>
        /// Имя поля для сортировки
        /// </summary>
        public string? OrderPropertyName { get; set; }
    
        /// <summary>
        /// Сортировать по убыванию
        /// </summary>
        public bool OrderByDescending { get; set; }
        
        /// <summary>
        /// Выводить передачи смены
        /// </summary>
        public bool DisplayShifts { get; set; }
        
        public string? Suggest { get; set; }
    }
}
