using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto.Field;
using GPNA.AlarmControlSystem.Models.Dto.Queries;
using GPNA.AlarmControlSystem.Models.Dto.Workstation;
using GPNA.AlarmControlSystem.Options;
using GPNA.AlarmControlSystem.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;

namespace GPNA.AlarmControlSystem.Pages.Main;

public partial class Index : AcsPageBase
{
    [Inject]
    private IOptions<AcsModuleOptions>? Options { get; set; }

    private DateTimeOffset DateTimeStart { get; } = new(DateTimeOffset.Now.AddDays(-8).Year, DateTimeOffset.Now.AddDays(-8).Month, DateTimeOffset.Now.AddDays(-8).Day, 0, 0, 0, DateTimeOffset.Now.AddDays(-8).Offset);
    private DateTimeOffset DateTimeEnd { get; } = new(DateTimeOffset.Now.AddDays(-1).Year, DateTimeOffset.Now.AddDays(-1).Month, DateTimeOffset.Now.AddDays(-1).Day, 23, 59, 59, DateTimeOffset.Now.AddDays(-1).Offset);

    private bool IsEnableRenderChart { get; set; }

    private WorkstationMainPageDto[]? _workstations;

    private FieldDto? _activeField, _nextField, _previousField;
    
    protected override async Task LoadPageAsync()
    {
        IsEnableRenderChart = false;
        await SpinnerService.Load(InitializePageAsync);
        IsEnableRenderChart = true;
    }

    private async Task InitializePageAsync()
    {
        _activeField = FieldId != null
            ? Fields?.FirstOrDefault(f => f.Id == FieldId) 
            : Fields?.FirstOrDefault();
        FieldId = _activeField?.Id;
        if (_activeField != null && Fields != null)
        {
            var x = Array.IndexOf(Fields, _activeField);
            _nextField = Fields.Length > (x + 1) ? Fields[x + 1] : Fields.First();
            _previousField = x - 1 >= 0 ? Fields[x - 1] : Fields.Last();
        }

        await GetWorkstations();
    }

    private async Task GetWorkstations()
    {
        if (FieldId != null)
        {
            var query = new GetAlarmsCountForFieldQuery { FieldId = FieldId.Value, DateTimeStart = DateTimeStart, DateTimeEnd = DateTimeEnd };
            var result = await WorkStationService.GetWorkstationsWithStatistics(query);

            if (result.Success)
            {
                _workstations = result.Payload.ToArray();
            }
        }
    }
}