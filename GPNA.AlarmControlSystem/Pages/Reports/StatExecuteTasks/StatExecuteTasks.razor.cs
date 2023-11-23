using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto.Field;
using GPNA.AlarmControlSystem.Models.Dto.Queries;
using GPNA.AlarmControlSystem.Models.Dto.Workstation;
using GPNA.AlarmControlSystem.Options;
using GPNA.AlarmControlSystem.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;

namespace GPNA.AlarmControlSystem.Pages.Reports.StatExecuteTasks;

public partial class StatExecuteTasks : ComponentBase
{
    [Inject]
    private IOptions<AcsModuleOptions>? Options { get; set; }

    [Inject]
    private IFieldService? FieldService { get; set; }

    [Inject]
    private IWorkStationService? WorkStationService { get; set; }

    [Inject]
    protected ISpinnerService SpinnerService { get; set; } = default!;

    [Parameter]
    public DateTimeOffset DateTime { get; set; } = DateTimeOffset.Now.AddDays(-1);

    [Parameter]
    [SupplyParameterFromQuery]
    public int? FieldId { get; set; }

    private bool IsEnableRenderChart { get; set; }

    private FieldDto[]? _fields;

    private WorkstationMainPageDto[]? _workstations;

    private IDictionary<string, string>? WorkstationLinksDictionary { get; set; }

    protected override async Task OnInitializedAsync()
    {
        IsEnableRenderChart = false;
        SpinnerService.Show();

        await InitializePageAsync();

        SpinnerService.Hide();
        IsEnableRenderChart = true;

        await base.OnInitializedAsync();
    }

    private async Task InitializePageAsync()
    {
        await GetFields();

        FieldId ??= _fields?.FirstOrDefault()?.Id ?? 1;

        await GetWorkstations();

        FillWorkstationLinks();
    }

    private async Task GetFields()
    {
        if (FieldService != null)
        {
            var result = await FieldService.GetList();

            if (result.Success)
            {
                _fields = result.Payload.ToArray();
            }
        }
    }

    private async Task GetWorkstations()
    {
        if (WorkStationService != null && FieldId != null)
        {
            var query = new GetAlarmsCountForFieldQuery { FieldId = FieldId.Value, DateTimeStart = DateTime.AddDays(-7), DateTimeEnd = DateTime };
            var result = await WorkStationService.GetWorkstationsWithStatistics(query);

            if (result.Success)
            {
                _workstations = result.Payload.ToArray();
            }
        }
    }

    private void FillWorkstationLinks()
    {
        if (_fields != null && _fields.Any())
        {
            WorkstationLinksDictionary = _fields.ToDictionary(field => field.Name, field => $"/?fieldId={field.Id}");
        }
    }
}