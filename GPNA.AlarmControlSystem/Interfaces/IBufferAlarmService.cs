using GPNA.AlarmControlSystem.Models.Dto;
using GPNA.AlarmControlSystem.Models.Dto.ActiveAlarm;
using GPNA.AlarmControlSystem.Models.Dto.BufferAlarms;
using GPNA.AlarmControlSystem.Models.Dto.IncomingAlarm;
using GPNA.AlarmControlSystem.Models.Dto.SuppressedAlarm;
using GPNA.AlarmControlSystem.Models.Enums;
using GPNA.RestClient.Models;

namespace GPNA.AlarmControlSystem.Interfaces;

public interface IBufferAlarmService
{
    Task<Result<BufferAlarmDto>> AddComment(AddCommentInAlarmCommand content);
    Task<Result<BufferAlarmDto>> ChangeStatus(ChangeStatusAlarmCommand content);
}