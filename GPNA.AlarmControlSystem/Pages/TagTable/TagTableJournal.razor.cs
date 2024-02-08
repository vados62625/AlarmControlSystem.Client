using Blazored.Modal.Services;
using GPNA.AlarmControlSystem.Application.Dto.Tag;
using GPNA.AlarmControlSystem.Application.Dto.TagChange;
using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto.Field;
using GPNA.AlarmControlSystem.Models.Dto.Queries;
using GPNA.AlarmControlSystem.Models.Dto.Tag;
using GPNA.AlarmControlSystem.Models.Dto.TagChange;
using GPNA.AlarmControlSystem.Models.Dto.Workstation;
using GPNA.AlarmControlSystem.Models.Enums;
using GPNA.AlarmControlSystem.Options;
using GPNA.AlarmControlSystem.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace GPNA.AlarmControlSystem.Pages.TagTable
{
    public partial class TagTableJournal : ComponentBase
    {
        [CascadingParameter] public IModalService Modal { get; set; } = default!;
        [Inject] protected IJSRuntime JS { get; set; } = default!;
        [Inject] protected ISpinnerService SpinnerService { get; set; } = default!;
        [Inject] protected ITagChangesService TagChangesService { get; set; } = default!;
        [Inject] private IOptions<AcsModuleOptions>? Options { get; set; }
        [Inject] private IFieldService? FieldService { get; set; }
        [Inject] private IExportService? ExportService { get; set; }
        [Inject] private IWorkStationService? WorkStationService { get; set; }
        [Parameter] [SupplyParameterFromQuery] public int? WorkstationId { get; set; }
        [Parameter] [SupplyParameterFromQuery] public int? FieldId { get; set; }
        
        private string? _workstationName, _fieldName;
        private WorkStationDto[]? _workstations;
        private FieldDto[]? _fields;
        private IDictionary<string, string>? _fieldLinksDictionary;
        private IDictionary<string, string>? _workstationLinksDictionary;

        private GetTagChangesListQuery _query = new();
        private TagChangesCollection _tagChangesCollection = new();

        protected override async Task OnParametersSetAsync()
        {
            await SetFieldWithWorkstation();
            await SpinnerService.Load(GetTags);
        }

        private async Task SetFieldWithWorkstation()
        {
            if (FieldService != null)
            {
                var fields = await FieldService.GetList();
                if (fields.Success)
                    _fields = fields.Payload.ToArray();
            }

            if (WorkStationService != null)
            {
                var workstations = await WorkStationService.GetList(new { FieldId = FieldId });
                if (workstations.Success)
                    _workstations = workstations.Payload.ToArray();
            }

            FieldId ??= _fields?.FirstOrDefault()?.Id;
            _fieldName = _fields?.FirstOrDefault(field => field.Id == FieldId)?.Name;
        
            WorkstationId ??= _workstations?.FirstOrDefault()?.Id;
            _workstationName = _workstations?.FirstOrDefault(ws => ws.Id == WorkstationId)?.Name;
        
            StateHasChanged();
        
            FillLinks();
        }
        
        private void FillLinks()
        {
            if (_fields != null)
            {
                _fieldLinksDictionary = _fields.ToDictionary(field => 
                        field.Name, 
                    field => $"/tag-table/?fieldId={field.Id}");
            }
        
            if (_workstations != null)
            {
                _workstationLinksDictionary = _workstations.ToDictionary(workStation => 
                        workStation.Name ?? Guid.NewGuid().ToString(), 
                    workStation => $"/tag-table/?fieldId={FieldId}&workstationId={workStation.Id}");
            }
        }

        private async Task GetTags()
        {
            _query.WorkStationId = WorkstationId ?? 1;
            _query.ItemsOnPage = 15;
            _query.Page ??= 1;
            var result = await TagChangesService.GetTagChangesCollection(_query);
            
            if (result.Success)
            {
                _tagChangesCollection = result.Payload;
                StateHasChanged();
            }
        }
        
        private async Task SetStateFilter(StateType? state)
        {
            _query.State = state;
            _query.Page = 1;
        
            await SpinnerService.Load(GetTags);
        }

        private async Task SetPriorityFilter(PriorityType? priority)
        {
            _query.Priority = priority;
            _query.Page = 1;
        
            await SpinnerService.Load(GetTags);
        }

        private async Task OnOrderingChanged(string orderBy)
        {
            if (orderBy == _query.OrderPropertyName) 
                _query.OrderByDescending = !_query.OrderByDescending;

            _query.OrderPropertyName = orderBy;
            _query.Page = 1;
        
            await SpinnerService.Load(GetTags);
        }

        private async Task OnPageChanged(int page)
        {
            _query.Page = page;
            await SpinnerService.Load(GetTags);
        }
    
        private async Task SearchTag(string tagName)
        {
            _query.Suggest = tagName;
            _query.Page = 1;
            await SpinnerService.Load(GetTags);
        }

        private async Task DropFilters()
        {
            _query = new GetTagChangesListQuery
            {
                Page = 1
            };

            await SpinnerService.Load(GetTags);
        }
        
        private async Task<Stream?> GetFileStream()
        {
            var result = await ExportService.ExportTags(new ExportTagsQuery
            {
                DocumentType = ExportDocumentType.Excel,
                WorkStationId = WorkstationId ?? 0,
                Suggest = _query.Suggest,
                State = _query.State,
                Priority = _query.Priority,
            });

            return new MemoryStream(result);
        }

        private async Task DownloadFileFromStream()
        {
            SpinnerService?.Show();

            var fileStream = await GetFileStream();

            if (fileStream == null) return;

            var fileName = "Теги.xlsx";

            using var streamRef = new DotNetStreamReference(stream: fileStream);

            await JS.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);

            SpinnerService?.Hide();
        }
    }
}
