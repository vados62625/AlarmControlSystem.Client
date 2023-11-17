using Blazored.Toast.Services;
using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto.Field;
using GPNA.AlarmControlSystem.Models.Dto.KpiSettings;
using GPNA.AlarmControlSystem.Models.Dto.Workstation;
using GPNA.AlarmControlSystem.Services;
using Microsoft.AspNetCore.Components;

namespace GPNA.AlarmControlSystem.Pages.Settings.KPI;

public partial class KPI : ComponentBase
{
    [Parameter] public int Value { get; set; } = 256;

    [Parameter] [SupplyParameterFromQuery] public int? WorkstationId { get; set; }

    [Parameter] [SupplyParameterFromQuery] public int? FieldId { get; set; }

    [Inject] private IToastService? ToastService { get; set; }

    [Inject] private IFieldService? FieldService { get; set; }

    [Inject] private IWorkStationService? WorkStationService { get; set; }

    [Inject] private AlarmJournalSettingsService? AlarmJournalSettingsService { get; set; }
    [Inject] private MonitoringSettingsService? MonitoringSettingsService { get; set; }
    [Inject] private ReportSettingsService? ReportSettingsService { get; set; }
    [Inject] private TagTableSettingsService? TagTableSettingsService { get; set; }
    [Inject] private TaskSettingsService? TaskSettingsService { get; set; }

    private string? FieldName { get; set; } = "N/A";
    private string? WorkstationName { get; set; } = "N/A";

    private FieldDto[]? _fields;

    private WorkStationDto[]? _workstations;

    private AlarmJournalSettingsDto? _journalSettings;
    private MonitoringSettingsDto? _monitoringSettings;
    private ReportSettingsDto? _reportSettings;
    private TagTableSettingsDto? _tagTableSettings;
    private TaskSettingsDto? _taskSettings;

    private IDictionary<string, string>? FieldLinksDictionary { get; set; }

    private IDictionary<string, string>? WorkstationLinksDictionary { get; set; }


    protected override async Task OnParametersSetAsync()
    {
        await SetFieldWithWorkstation();

        await InitializePageAsync();

        await base.OnParametersSetAsync();
    }

    private async Task InitializePageAsync()
    {
        await SetSettings();
    }

    private async Task SetSettings()
    {
        if (WorkstationId != null)
        {
            _journalSettings = await GetJournalSettings() ??
                               new AlarmJournalSettingsDto { WorkStationId = WorkstationId.Value };
            _monitoringSettings = await GetMonitoringSettings() ??
                                  new MonitoringSettingsDto { WorkStationId = WorkstationId.Value };
            _reportSettings = await GetReportSettings() ?? new ReportSettingsDto { WorkStationId = WorkstationId.Value };
            _tagTableSettings = await GetTagTableSettings() ?? new TagTableSettingsDto { WorkStationId = WorkstationId.Value };
            _taskSettings = await GetTaskSettings() ?? new TaskSettingsDto { WorkStationId = WorkstationId.Value };
        }
    }

    private async Task<AlarmJournalSettingsDto?> GetJournalSettings()
    {
        if (AlarmJournalSettingsService != default && WorkstationId != default)
        {
            var result = await AlarmJournalSettingsService.GetSettings(WorkstationId.Value);

            if (result.Success) return result.Payload;
        }

        return null;
    }

    private async Task<MonitoringSettingsDto?> GetMonitoringSettings()
    {
        if (MonitoringSettingsService != default && WorkstationId != default)
        {
            var result = await MonitoringSettingsService.GetSettings(WorkstationId.Value);

            if (result.Success) return result.Payload;
        }

        return null;
    }

    private async Task<ReportSettingsDto?> GetReportSettings()
    {
        if (ReportSettingsService != default && WorkstationId != default)
        {
            var result = await ReportSettingsService.GetSettings(WorkstationId.Value);

            if (result.Success) return result.Payload;
        }

        return null;
    }

    private async Task<TagTableSettingsDto?> GetTagTableSettings()
    {
        if (TagTableSettingsService != default && WorkstationId != default)
        {
            var result = await TagTableSettingsService.GetSettings(WorkstationId.Value);

            if (result.Success) return result.Payload;
        }

        return null;
    }

    private async Task<TaskSettingsDto?> GetTaskSettings()
    {
        if (TaskSettingsService != default && WorkstationId != default)
        {
            var result = await TaskSettingsService.GetSettings(WorkstationId.Value);

            if (result.Success) return result.Payload;
        }

        return null;
    }

    private async Task SetFieldWithWorkstation()
    {
        if (FieldService != null)
        {
            var fields = await FieldService.GetList();

            if (fields.Success)
            {
                _fields = fields.Payload.ToArray();
            }
        }

        if (WorkStationService != null)
        {
            var workstations = await WorkStationService.GetList(new { FieldId });

            if (workstations.Success)
            {
                _workstations = workstations.Payload.ToArray();
            }
        }

        FieldId ??= _fields?.FirstOrDefault()?.Id;

        FieldName = _fields?.FirstOrDefault(field => field.Id == FieldId)?.Name;

        WorkstationId ??= _workstations?.FirstOrDefault()?.Id;

        WorkstationName = _workstations?.FirstOrDefault(ws => ws.Id == WorkstationId)?.Name;

        FillLinks();
    }

    private void FillLinks()
    {
        if (_fields != null)
        {
            FieldLinksDictionary = _fields.ToDictionary(field =>
                    field.Name,
                field => $"/settings/KPI/?fieldId={field.Id}");
        }

        if (_workstations != null)
        {
            WorkstationLinksDictionary = _workstations.ToDictionary(workStation =>
                    workStation.Name ?? Guid.NewGuid().ToString(),
                workStation => $"/settings/KPI/?fieldId={FieldId}&workstationId={workStation.Id}");
        }
    }

    private async Task UpdateJournalSettings()
    {
        if (AlarmJournalSettingsService != null && _journalSettings != null)
        {
            var result = await AlarmJournalSettingsService.Update(_journalSettings);

            if (result.Success)
            {
                ShowSuccess();
            }
        }
    }

    private async Task UpdateMonitoringSettings()
    {
        if (MonitoringSettingsService != null && _monitoringSettings != null)
        {
            var result = await MonitoringSettingsService.Update(_monitoringSettings);

            if (result.Success)
            {
                ShowSuccess();
            }
        }
    }

    private async Task UpdateReportSettings()
    {
        if (ReportSettingsService != null && _reportSettings != null)
        {
            var result = await ReportSettingsService.Update(_reportSettings);

            if (result.Success)
            {
                ShowSuccess();
            }
        }
    }

    private async Task UpdateTagTableSettings()
    {
        if (TagTableSettingsService != null && _tagTableSettings != null)
        {
            var result = await TagTableSettingsService.Update(_tagTableSettings);

            if (result.Success)
            {
                ShowSuccess();
            }
        }
    }

    private async Task UpdateTaskSettings()
    {
        if (TaskSettingsService != null && _taskSettings != null)
        {
            var result = await TaskSettingsService.Update(_taskSettings);

            if (result.Success)
            {
                ShowSuccess();
            }
        }
    }

    private void ShowSuccess() => ToastService?.ShowSuccess("Данные сохранены");
}