using Blazored.Toast.Services;
using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto.Field;
using GPNA.AlarmControlSystem.Models.Dto.KpiSettings;
using GPNA.AlarmControlSystem.Models.Dto.Workstation;
using GPNA.AlarmControlSystem.Services;
using Microsoft.AspNetCore.Components;

namespace GPNA.AlarmControlSystem.Pages.Settings.KPI;

public partial class KPI : AcsPageBase
{
    [Parameter] public int Value { get; set; } = 256;
    [Inject] private IToastService? ToastService { get; set; }
    [Inject] private AlarmJournalSettingsService? AlarmJournalSettingsService { get; set; }
    [Inject] private MonitoringSettingsService? MonitoringSettingsService { get; set; }
    [Inject] private ReportSettingsService? ReportSettingsService { get; set; }
    [Inject] private TagTableSettingsService? TagTableSettingsService { get; set; }
    [Inject] private TaskSettingsService? TaskSettingsService { get; set; }

    private AlarmJournalSettingsDto _journalSettings = new();
    private MonitoringSettingsDto _monitoringSettings = new();
    private ReportSettingsDto _reportSettings = new();
    private TagTableSettingsDto _tagTableSettings = new();
    private TaskSettingsDto _taskSettings = new();
    
    protected override async Task LoadPageAsync()
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
    
    private bool _editJournalSettingsButtonShow = true;
    private bool _editMonitoringSettingsButtonShow = true;
    private bool _editTaskSettingsButtonShow = true;
    private bool _editReportSettingsButtonShow = true;
    private bool _editTagTableSettingsButtonShow = true;
    
    private bool _enabledEditJournalSettings;
    private bool _enabledEditMonitoringSettings;
    private bool _enabledEditTaskSettings;
    private bool _enabledEditReportSettings;
    private bool _enabledEditTagTableSettings;

    void EditJournalSettings()
    {
        _editJournalSettingsButtonShow = !_editJournalSettingsButtonShow;
        _enabledEditJournalSettings = true;
    }

    void CancelEditJournalSettings()
    {
        _editJournalSettingsButtonShow = !_editJournalSettingsButtonShow;
        _enabledEditJournalSettings = false;
    }
    void EditMonitoringSettings()
    {
        _editMonitoringSettingsButtonShow = !_editMonitoringSettingsButtonShow;
        _enabledEditMonitoringSettings = true;
    }

    void CancelEditMonitoringSettings()
    {
        _editMonitoringSettingsButtonShow = !_editMonitoringSettingsButtonShow;
        _enabledEditMonitoringSettings = false;
    }

    void EditTaskSettings()
    {
        _editTaskSettingsButtonShow = !_editTaskSettingsButtonShow;
        _enabledEditTaskSettings = true;
    }

    void CancelEditTaskSettings()
    {
        _editTaskSettingsButtonShow = !_editTaskSettingsButtonShow;
        _enabledEditTaskSettings = false;
    }
    void EditReportSettings()
    {
        _editReportSettingsButtonShow = !_editReportSettingsButtonShow;
        _enabledEditReportSettings = true;
    }

    void CancelEditReportSettings()
    {
        _editReportSettingsButtonShow = !_editReportSettingsButtonShow;
        _enabledEditReportSettings = false;
    }
    void EditTagTableSettings()
    {
        _editTagTableSettingsButtonShow = !_editTagTableSettingsButtonShow;
        _enabledEditTagTableSettings = true;
    }

    void CancelEditTagTableSettings()
    {
        _editTagTableSettingsButtonShow = !_editTagTableSettingsButtonShow;
        _enabledEditTagTableSettings = false;
    }

    private async Task UpdateJournalSettings()
    {
        if (AlarmJournalSettingsService != null && _journalSettings != null)
        {
            var result = await AlarmJournalSettingsService.Update(_journalSettings);

            if (result.Success)
            {
                _journalSettings = result.Payload;
                ShowSuccess();
                _editJournalSettingsButtonShow = !_editJournalSettingsButtonShow;
                _enabledEditJournalSettings = false;

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
                _monitoringSettings = result.Payload;
                ShowSuccess();
                _editMonitoringSettingsButtonShow = !_editMonitoringSettingsButtonShow;
                _enabledEditMonitoringSettings = false;
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
                _reportSettings = result.Payload;
                ShowSuccess();
                _editReportSettingsButtonShow = !_editReportSettingsButtonShow;
                _enabledEditReportSettings = false;
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
                _tagTableSettings = result.Payload;
                ShowSuccess();
                _editTagTableSettingsButtonShow = !_editTagTableSettingsButtonShow;
                _enabledEditTagTableSettings = false;
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
                _taskSettings = result.Payload;
                ShowSuccess();
                _editTaskSettingsButtonShow = !_editTaskSettingsButtonShow;
                _enabledEditTaskSettings = false;
            }
        }        
    }

    private void ShowSuccess() => ToastService?.ShowSuccess("Данные сохранены");
}