using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto.Field;
using GPNA.AlarmControlSystem.Models.Dto.Workstation;
using GPNA.AlarmControlSystem.Services;
using Microsoft.AspNetCore.Components;

namespace GPNA.AlarmControlSystem.Pages;

public class AcsPageBase : ComponentBase
{
    [Inject] protected ISpinnerService SpinnerService { get; set; } = default!;
    [Inject] protected NavigationManager? NavigationManager { get; set; }
    [Inject] protected IFieldService? FieldService { get; set; }
    [Inject] protected IWorkStationService? WorkStationService { get; set; }
    [Parameter][SupplyParameterFromQuery] public int? WorkstationId { get; set; }
    [Parameter][SupplyParameterFromQuery] public int? FieldId { get; set; }

    protected string? WorkstationName, FieldName;
    protected WorkStationDto[]? Workstations;
    protected FieldDto[]? Fields;
    protected IDictionary<string, string>? FieldLinksDictionary;
    protected IDictionary<string, string>? WorkstationLinksDictionary;
    
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
        if (FieldService != null)
        {
            var fields = await FieldService.GetList();
            if (fields.Success)
                Fields = fields.Payload.ToArray();
        }
        
        if (WorkStationService != null)
        {
            var workstations = await WorkStationService.GetList(new { FieldId });
            if (workstations.Success)
                Workstations = workstations.Payload.ToArray();
        }
        
        FieldId ??= Fields?.FirstOrDefault()?.Id;
        FieldName = Fields?.FirstOrDefault(field => field.Id == FieldId)?.Name;
        
        WorkstationId ??= Workstations?.FirstOrDefault()?.Id;
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