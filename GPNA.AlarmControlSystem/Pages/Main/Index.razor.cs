using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto.Field;
using GPNA.AlarmControlSystem.Models.Dto.Queries;
using GPNA.AlarmControlSystem.Models.Dto.Workstation;
using GPNA.AlarmControlSystem.Options;
using GPNA.AlarmControlSystem.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;

namespace GPNA.AlarmControlSystem.Pages.Main;

public partial class Index : ComponentBase
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
    public DateTimeOffset DateTime { get; set; } = new(DateTimeOffset.Now.AddDays(-1).Year, DateTimeOffset.Now.AddDays(-1).Month, DateTimeOffset.Now.AddDays(-1).Day, 23, 59, 59, DateTimeOffset.Now.AddDays(-1).Offset);

    [Parameter]
    [SupplyParameterFromQuery]
    public int? FieldId { get; set; }

    private bool IsEnableRenderChart { get; set; }

    private List<FieldDto>? _fields;

    private WorkstationMainPageDto[]? _workstations;

    private IDictionary<string, string>? WorkstationLinksDictionary { get; set; }

    private FieldDto? _activeField, _nextField, _previousField;
    
    protected override async Task OnParametersSetAsync()
    {
        IsEnableRenderChart = false;
        await SpinnerService.Load(InitializePageAsync);
        IsEnableRenderChart = true;
    }

    private async Task InitializePageAsync()
    {
        await GetFields();
        
        _activeField = FieldId != null
            ? _fields?.FirstOrDefault(f => f.Id == FieldId) 
            : _fields?.FirstOrDefault();
        FieldId = _activeField?.Id;
        if (_activeField != null && _fields != null)
        {
            var x = _fields.IndexOf(_activeField);
            _nextField = _fields.Count > (x + 1) ? _fields[x + 1] : _fields.First();
            _previousField = x - 1 >= 0 ? _fields[x - 1] : _fields.Last();
        }

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
                _fields = result.Payload.ToList();
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