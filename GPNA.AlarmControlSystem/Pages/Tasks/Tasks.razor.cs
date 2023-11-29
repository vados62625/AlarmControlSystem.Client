using Blazored.Modal;
using Blazored.Modal.Services;
using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto.BufferAlarms;
using GPNA.AlarmControlSystem.Models.Dto.Field;
using GPNA.AlarmControlSystem.Models.Dto.IncomingAlarm;
using GPNA.AlarmControlSystem.Models.Dto.Tag;
using GPNA.AlarmControlSystem.Models.Dto.Workstation;
using GPNA.AlarmControlSystem.Models.Enums;
using GPNA.AlarmControlSystem.Options;
using GPNA.AlarmControlSystem.Services;
using GPNA.AlarmControlSystem.Pages.TagTable.Modals;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace GPNA.AlarmControlSystem.Pages.Tasks
{
    public partial class Tasks : ComponentBase
    {
        [CascadingParameter] public IModalService Modal { get; set; } = default!;
        [Inject] protected ISpinnerService SpinnerService { get; set; } = default!;
        [Inject] protected IIncomingAlarmService IncomingAlarmService { get; set; } = default!;
        [Inject] private IOptions<AcsModuleOptions>? Options { get; set; }
        [Inject] private IFieldService? FieldService { get; set; }
        [Inject] private IWorkStationService? WorkStationService { get; set; }
        [Inject] private IExportService? ExportService { get; set; }
        [Parameter] [SupplyParameterFromQuery] public int? WorkstationId { get; set; }
        [Parameter] [SupplyParameterFromQuery] public int? FieldId { get; set; }
        
        private string? _workstationName, _fieldName;
        private WorkStationDto[]? _workstations;
        private FieldDto[]? _fields;
        private IDictionary<string, string>? _fieldLinksDictionary;
        private IDictionary<string, string>? _workstationLinksDictionary;

        private GetIncomingAlarmsByDatesQuery _query = new();
        
        // private List<IncomingAlarmDto>? _tasks;
        private AlarmsCollection<IncomingAlarmDto[]>? _tasks;

        string input = "";
        
        private int _pagesCount, _totalCount;
        private int _currentPage = 1;

        protected override async Task OnInitializedAsync()
        {
            await SetFieldWithWorkstation();
            await SpinnerService.Load(GetTasks);
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
                    field => $"/tasks/?fieldId={field.Id}");
            }
        
            if (_workstations != null)
            {
                _workstationLinksDictionary = _workstations.ToDictionary(workStation => 
                        workStation.Name ?? Guid.NewGuid().ToString(), 
                    workStation => $"/tasks/?fieldId={FieldId}&workstationId={workStation.Id}");
            }
        }

        private async Task GetTasks()
        {
            _query.WorkStationId = WorkstationId ?? 1;
            _query.Page ??= 1;
            _query.StatusAlarm = StatusAlarmType.InWork;
            _query.CountOnPage = 15;
            _query.DateTimeEnd = DateTimeOffset.Now;
            _query.DateTimeStart = DateTimeOffset.Now.AddYears(-10);
            var result = await IncomingAlarmService.GetAlarmsPerDate(_query); // TODO I make PageableCollectionDto
            
            if (result.Success)
            {
                _tasks = result.Payload;
                _pagesCount = 100;
                StateHasChanged();
            }
        }
        
        private async Task SetStateFilter(StateType? state)
        {
            _query.State = state;
            _query.Page = 1;
        
            await SpinnerService.Load(GetTasks);
        }

        private async Task SetPriorityFilter(PriorityType? priority)
        {
            _query.Priority = priority;
            _query.Page = 1;
        
            await SpinnerService.Load(GetTasks);
        }

        private async Task OnPageChanged(int page)
        {
            _query.Page = page;
            await SpinnerService.Load(GetTasks);
        }
    
        private async Task Search()
        {
            _query.Page = 1;
            await SpinnerService.Load(GetTasks);
        }
        
        private async Task DropFilters()
        {
            _query = new GetIncomingAlarmsByDatesQuery()
            {
                Page = 1,
                StatusAlarm = StatusAlarmType.InWork
            };

            await SpinnerService.Load(GetTasks);
        }
        
        private async Task<Stream?> GetFileStream()
        {
            var result = await ExportService.ExportIncomingAlarms(new ExportIncomingAlarmsByDatesQuery
            {
                DocumentType = ExportDocumentType.Excel,
                StatusAlarm = StatusAlarmType.InWork,
                WorkStationId = WorkstationId ?? 0,
                TagName = _query.TagName, 
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

            var fileName = "export.xlsx";

            using var streamRef = new DotNetStreamReference(stream: fileStream);

            await JS.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);

            SpinnerService?.Hide();
        }
    }
}
