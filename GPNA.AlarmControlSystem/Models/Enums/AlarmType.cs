using System.ComponentModel;

namespace GPNA.AlarmControlSystem.Models.Enums;

public enum AlarmType
{
    [Description("Активация")] Activation,
    [Description("Нормализация")] Normalization,
    [Description("Подавление")] Suppression,
    [Description("Снятие подавления")] SuppressionRelease,
    [Description("Имитация")] Imitation,
    [Description("Снятие имитации")] ImitationRelease,
    [Description("Входящая")] Incoming,
}