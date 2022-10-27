﻿using LocalDbStorage.Repositories.Base;
using LocalDbStorage.Repositories.Models.Enum;

namespace LocalDbStorage.Repositories.Models
{
    public class BufferAlarm : EntityBase
    {

        /// <summary>
        /// Id рабочей станции
        /// </summary>
        public int IdWorkStation { get; set; }

        /// <summary>
        /// Время события сигнализации
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Событие сигнализации 
        /// </summary>
        public EventAlarm EventAlarm { get; set; }

        /// <summary>
        /// Имя тега
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Состояние
        /// </summary>
        public StateType State { get; set; }

        /// <summary>
        /// Приоритет
        /// </summary>
        public PriorityType Priority { get; set; }

        /// <summary>
        /// Предел
        /// </summary>
        public string? AlarmLimit { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// Комментарии
        /// </summary>
        public string? Comment { get; set; }

        /// <summary>
        /// Тест
        /// </summary>
        public bool Test { get; set; }

    }
}