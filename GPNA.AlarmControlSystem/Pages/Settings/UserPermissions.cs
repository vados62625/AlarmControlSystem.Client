using Blazored.Toast.Services;
using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto.Field;
using GPNA.AlarmControlSystem.Models.Dto.KpiSettings;
using GPNA.AlarmControlSystem.Models.Dto.Workstation;
using GPNA.AlarmControlSystem.Services;
using Microsoft.AspNetCore.Components;

namespace GPNA.AlarmControlSystem.Pages.Settings;

public partial class UserPermissions : ComponentBase
{
    [Parameter] public int Value { get; set; } = 256;

    [Parameter] [SupplyParameterFromQuery] public int? ArmId { get; set; }

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
    private string? ArmName { get; set; } = "N/A";

    private FieldDto[]? _fields;

    private WorkStationDto[]? _workstations;

    private AlarmJournalSettingsDto? _journalSettings;
    private MonitoringSettingsDto? _monitoringSettings;
    private ReportSettingsDto? _reportSettings;
    private TagTableSettingsDto? _tagTableSettings;
    private TaskSettingsDto? _taskSettings;

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

    private void ShowSuccess() => ToastService?.ShowSuccess("Данные сохранены");
}