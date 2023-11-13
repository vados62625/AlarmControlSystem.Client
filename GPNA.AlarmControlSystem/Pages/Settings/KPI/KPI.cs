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

    [Parameter] [SupplyParameterFromQuery] public int? ArmId { get; set; }

    [Parameter] [SupplyParameterFromQuery] public int? FieldId { get; set; }

    [Inject] private IFieldService? FieldService { get; set; }

    [Inject] private IWorkStationService? WorkStationService { get; set; }
    
    [Inject] private AlarmJournalSettingsService? AlarmJournalSettingsService { get; set; }
    [Inject] private MonitoringSettingsService? MonitoringSettingsService { get; set; }
    [Inject] private ReportSettingsService? ReportSettingsService { get; set; }
    [Inject] private TagTableSettingsService? TagTableSettingsService { get; set; }
    [Inject] private TaskSettingsService? TaskSettingsService { get; set; }
    
    private string? FieldName { get; set; } = "N/A";
    private string? ArmName { get; set; } = "N/A";

    private FieldDto[]? _fields;

    private WorkStationDto[]? _workstations;

    private AlarmJournalSettingsDto? _journalSettings;
    private MonitoringSettingsDto? _monitoringSettings;
    private ReportSettingsDto? _reportSettings;
    private TagTableSettingsDto? _tagTableSettings;
    private TaskSettingsDto? _taskSettings;

    private bool _editingJournalSettings;
    private bool _editingMonitoringSettings;
    private bool _editingReportSettings;
    private bool _editingTagTableSettings;
    private bool _editingTaskSettings;
    
    private IDictionary<string, string>? FieldLinksDictionary { get; set; }
    
    private IDictionary<string, string>? ArmLinksDictionary { get; set; }
    

    protected override async Task OnInitializedAsync()
    {
        await SetFieldWithArm();
        
        await InitializePageAsync();

        await base.OnParametersSetAsync();
    }

    private async Task InitializePageAsync()
    {
        await SetSettings();
    }

    private async Task SetSettings()
    {
        _journalSettings = await GetJournalSettings();
        _monitoringSettings = await GetMonitoringSettings();
        _reportSettings = await GetReportSettings();
        _tagTableSettings = await GetTagTableSettings();
        _taskSettings = await GetTaskSettings();
    }

    private async Task<AlarmJournalSettingsDto?> GetJournalSettings()
    {
        if (AlarmJournalSettingsService != default && ArmId != default)
        {
            var result = await AlarmJournalSettingsService.GetSettings(ArmId.Value);

            if (result.Success) return result.Payload;
        }

        return null;
    }
    
    private async Task<MonitoringSettingsDto?> GetMonitoringSettings()
    {
        if (MonitoringSettingsService != default && ArmId != default)
        {
            var result = await MonitoringSettingsService.Get(ArmId.Value);

            if (result.Success) return result.Payload;
        }

        return null;
    }
    
    private async Task<ReportSettingsDto?> GetReportSettings()
    {
        if (ReportSettingsService != default && ArmId != default)
        {
            var result = await ReportSettingsService.Get(ArmId.Value);

            if (result.Success) return result.Payload;
        }

        return null;
    }
    
    private async Task<TagTableSettingsDto?> GetTagTableSettings()
    {
        if (TagTableSettingsService != default && ArmId != default)
        {
            var result = await TagTableSettingsService.Get(ArmId.Value);

            if (result.Success) return result.Payload;
        }

        return null;
    }
    
    private async Task<TaskSettingsDto?> GetTaskSettings()
    {
        if (TaskSettingsService != default && ArmId != default)
        {
            var result = await TaskSettingsService.Get(ArmId.Value);

            if (result.Success) return result.Payload;
        }

        return null;
    }

    private async Task SetFieldWithArm()
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
        
        ArmId ??= _workstations?.FirstOrDefault()?.Id;

        ArmName = _workstations?.FirstOrDefault(ws => ws.Id == ArmId)?.Name;
        
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
            ArmLinksDictionary = _workstations.ToDictionary(workStation => 
                    workStation.Name ?? Guid.NewGuid().ToString(), 
                workStation => $"/settings/KPI/?fieldId={FieldId}&armId={workStation.Id}");
        }
    }

    private async Task UpdateJournalSettings()
    {
        if (AlarmJournalSettingsService != null && _journalSettings != null)
        {
            var result = await AlarmJournalSettingsService.Update(_journalSettings);

            if (result.Success) _editingJournalSettings = false;
        }
    }
    
    private async Task UpdateMonitoringSettings()
    {
        if (MonitoringSettingsService != null && _monitoringSettings != null)
        {
            var result = await MonitoringSettingsService.Update(_monitoringSettings);

            if (result.Success) _editingMonitoringSettings = false;
        }
    }
    private async Task UpdateReportSettings()
    {
        if (ReportSettingsService != null && _reportSettings != null)
        {
            var result = await ReportSettingsService.Update(_reportSettings);

            if (result.Success) _editingReportSettings = false;
        }
    }
    private async Task UpdateTagTableSettings()
    {
        if (TagTableSettingsService != null && _tagTableSettings != null)
        {
            var result = await TagTableSettingsService.Update(_tagTableSettings);

            if (result.Success) _editingTagTableSettings = false;
        }
    }
    private async Task UpdateTaskSettings()
    {
        if (TaskSettingsService != null && _taskSettings != null)
        {
            var result = await TaskSettingsService.Update(_taskSettings);

            if (result.Success) _editingTaskSettings = false;
        }
    }

    private void SetEditingJournalSettings(bool value)
    {
        _editingJournalSettings = value;
    }
    private void SetEditingMonitoringSettings(bool value)
    {
        _editingJournalSettings = value;
    }
    private void SetEditingReportSettings(bool value)
    {
        _editingJournalSettings = value;
    }
    private void SetEditingTagTableSettings(bool value)
    {
        _editingJournalSettings = value;
    }
    private void SetEditingTaskSettings(bool value)
    {
        _editingJournalSettings = value;
    }
}