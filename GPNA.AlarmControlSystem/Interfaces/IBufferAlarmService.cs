using GPNA.AlarmControlSystem.Models.Dto;
using GPNA.AlarmControlSystem.Models.Dto.BufferAlarms;
using GPNA.AlarmControlSystem.Models.Enums;
using GPNA.RestClient.Models;

namespace GPNA.AlarmControlSystem.Interfaces;

public interface IBufferAlarmService
{
    Task<Result<PageableCollectionDto<ActiveAlarmDto>>> GetAllActiveAlarms(int workStationId);

    Task<Result<PageableCollectionDto<ActiveAlarmDto>>> GetActiveAlarmsByDates(int workStationId,
        DateTimeOffset dateTimeStart, DateTimeOffset dateTimeEnd);

    Task<Result<CountAlarmsOnDate[]>> GetCountActiveAlarmsByDates(int workStationId, DateTimeOffset dateTimeStart,
        DateTimeOffset dateTimeEnd);

    Task<Result<PageableCollectionDto<IncomingAlarmDto>>> GetAllIncomingAlarms(int workStationId);

    Task<Result<PageableCollectionDto<IncomingAlarmDto>>> GetIncomingAlarmsByDates(int workStationId,
        DateTimeOffset dateTimeStart, DateTimeOffset dateTimeEnd);

    Task<Result<PageableCollectionDto<SuppressedAlarmDto>>> GetAllSuppressedAlarms(int workStationId);

    Task<Result<PageableCollectionDto<SuppressedAlarmDto>>> GetSuppressedAlarmsByDates(int workStationId,
        DateTimeOffset dateTimeStart, DateTimeOffset dateTimeEnd);

    Task<Result<CountAlarmsOnDate[]>> GetCountSuppressedAlarmsByDates(int workStationId, DateTimeOffset dateTimeStart,
        DateTimeOffset dateTimeEnd);

    Task<Result<BufferAlarmDto>> AddComment(int id, string content);

    Task<Result<BufferAlarmDto>> ChangeStatus(int id, StatusAlarmType content);

    Task<List<List<IncomingAlarmDto>>> GetAlarmsPerDate(int idWorkStation, DateTime startDate, DateTime endDate);

    Task<Dictionary<DateTime, List<IncomingAlarmDto>>> GetCountInHour(int idWorkStation, DateTime startDate,
        DateTime endDate);
}