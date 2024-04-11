using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto.Field;
using GPNA.AlarmControlSystem.Models.Dto.Workstation;
using GPNA.AlarmControlSystem.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace GPNA.AlarmControlSystem.Pages;

public class AcsPageBase : ComponentBase
{
    [Inject] private ProtectedLocalStorage ProtectedLocalStore { get; set; } = null!;
    [Inject] protected ISpinnerService SpinnerService { get; set; } = null!;
    [Inject] protected NavigationManager NavigationManager { get; set; } = null!;
    [Inject] protected IFieldService FieldService { get; set; } = null!;
    [Inject] protected IWorkStationService WorkStationService { get; set; } = null!;

    [Parameter]
    [SupplyParameterFromQuery]
    public int? WorkstationId
    {
        get => _workstationId;
        set
        {
            _workstationId = value;
            if (value.HasValue)
            {
                ProtectedLocalStore.SetAsync("workstationId", value.Value);
            }
        }
    }

    [Parameter]
    [SupplyParameterFromQuery]
    public int? FieldId
    {
        get => _fieldId;
        set
        {
            _fieldId = value;
            if (value.HasValue)
            {
                ProtectedLocalStore.SetAsync("fieldId", value.Value);
            }
        }
    }

    protected string? WorkstationName, FieldName;
    protected WorkStationDto[]? Workstations;
    protected FieldDto[]? Fields;
    protected IDictionary<string, string>? FieldLinksDictionary;
    protected IDictionary<string, string>? WorkstationLinksDictionary;
    private int? _workstationId;
    private int? _fieldId;

    protected override async Task OnParametersSetAsync()
    {
        await SetFieldWithWorkstation();
        await LoadPageAsync();
    }

    protected virtual Task LoadPageAsync()
    {
        return Task.CompletedTask;
    }

    private async Task SetFieldWithWorkstation()
    {
        var fields = await FieldService.GetList();
        if (fields.Success)
            Fields = fields.Payload.ToArray();

        var workstations = await WorkStationService.GetList(new { FieldId });
        if (workstations.Success)
            Workstations = workstations.Payload.ToArray();

        var fieldIdInLocalStorage = await ProtectedLocalStore.GetAsync<int?>("fieldId");
        var workstationIdInLocalStorage = await ProtectedLocalStore.GetAsync<int?>("workstationId");

        FieldId ??= fieldIdInLocalStorage is { Success: true, Value: not null }
            ? fieldIdInLocalStorage.Value
            : Fields?.FirstOrDefault()?.Id;
        FieldName = Fields?.FirstOrDefault(field => field.Id == FieldId)?.Name;

        WorkstationId ??= workstationIdInLocalStorage is { Success: true, Value: not null }
            ? workstationIdInLocalStorage.Value
            : Workstations?.FirstOrDefault()?.Id;
        WorkstationName = Workstations?.FirstOrDefault(ws => ws.Id == WorkstationId)?.Name;

        StateHasChanged();

        FillLinks();
    }

    protected virtual void FillLinks()
    {
        if (Fields != null)
        {
            FieldLinksDictionary = Fields.ToDictionary(field =>
                    field.Name,
                field => $"{GetPageFromUrl()}?fieldId={field.Id}");
        }

        if (Workstations != null)
        {
            WorkstationLinksDictionary = Workstations.ToDictionary(workStation =>
                    workStation.Name ?? Guid.NewGuid().ToString(),
                workStation => $"{GetPageFromUrl()}?fieldId={FieldId}&workstationId={workStation.Id}");
        }
    }

    protected string GetPageFromUrl()
    {
        return new Uri(NavigationManager?.Uri ?? "/").GetLeftPart(UriPartial.Path);
    }
}