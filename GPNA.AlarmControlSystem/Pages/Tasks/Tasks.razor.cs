using Blazored.Modal.Services;
using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto.BufferAlarms;
using GPNA.AlarmControlSystem.Models.Dto.Field;
using GPNA.AlarmControlSystem.Models.Dto.IncomingAlarm;
using GPNA.AlarmControlSystem.Models.Dto.Queries;
using GPNA.AlarmControlSystem.Models.Dto.TagTask;
using GPNA.AlarmControlSystem.Models.Dto.Workstation;
using GPNA.AlarmControlSystem.Models.Enums;
using GPNA.AlarmControlSystem.Options;
using GPNA.AlarmControlSystem.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace GPNA.AlarmControlSystem.Pages.Tasks
{
    public partial class Tasks : ComponentBase
    {
        [CascadingParameter] public IModalService Modal { get; set; } = default!;
        [Inject] protected ISpinnerService SpinnerService { get; set; } = default!;
        [Inject] protected ITagTaskService TagTaskService { get; set; } = default!;
        [Inject] private IOptions<AcsModuleOptions>? Options { get; set; }
        [Inject] private IFieldService? FieldService { get; set; }
        [Inject] private IWorkStationService? WorkStationService { get; set; }
        [Inject] private IExportService? ExportService { get; set; }
        [Parameter] [SupplyParameterFromQuery] public int? WorkstationId { get; set; }
        [Parameter] [SupplyParameterFromQuery] public int? FieldId { get; set; }
        [Parameter] [SupplyParameterFromQuery] public int CurrentTab { get; set; } = 0;

        private string? _workstationName, _fieldName;
        private WorkStationDto[]? _workstations;
        private FieldDto[]? _fields;
        private IDictionary<string, string>? _fieldLinksDictionary;
        private IDictionary<string, string>? _workstationLinksDictionary;

        private GetTagTasksListQuery _query = new();

        // private List<IncomingAlarmDto>? _tasks;
        private TagTasksCollection? _tasks;
        private TagTasksCollection? _archive;
        
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
                var workstations = await WorkStationService.GetList(new { FieldId });
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
            _query.IsArchived = CurrentTab == 0;
            _query.ItemsOnPage = 15;
            _query.DateTimeEnd = DateTimeOffset.Now;
            _query.DateTimeStart = DateTimeOffset.Now.AddYears(-10);
            var result = await TagTaskService.GetTagTasksCollection(_query); // TODO I make PageableCollectionDto

            if (result.Success)
            {
                switch (CurrentTab)
                {
                    case 0:
                        _tasks = result.Payload;
                        _pagesCount = _tasks.PagesCount;
                        break;
                    case 1:
                        _archive = result.Payload;
                        _pagesCount = _archive.PagesCount;
                        break;
                }

                StateHasChanged();
            }
        }

        private async Task SetStateFilter(StateType? state)
        {
            if (state.HasValue && _query.State != default)
            {
                if (_query.State.Contains(state.Value))
                    _query.State.Add(state.Value);
                
                else 
                    _query.State.Remove(state.Value);

                _query.Page = 1;

                await SpinnerService.Load(GetTasks);
            }
        }

        private async Task SetPriorityFilter(PriorityType? priority)
        {
            if (priority.HasValue && _query.Priority != default)
            {
                if (_query.Priority.Contains(priority.Value))
                    _query.Priority.Add(priority.Value);

                else
                    _query.Priority.Remove(priority.Value);
                _query.Page = 1;

                await SpinnerService.Load(GetTasks);
            }
        }

        private async Task OnPageChanged(int page)
        {
            _query.Page = page;
            await SpinnerService.Load(GetTasks);
        }

        private async Task Search(string tagName)
        {
            _query.Suggest = tagName;
            _query.Page = 1;
            await SpinnerService.Load(GetTasks);
        }

        private async Task DropFilters()
        {
            _query = new GetTagTasksListQuery()
            {
                Page = 1,
                IsArchived = true
            };

            await SpinnerService.Load(GetTasks);
        }

        private async Task<Stream?> GetFileStream()
        {
            var result = await ExportService.ExportTagTasks(new ExportTagTasksQuery
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

            var fileName = "Задачи_по_тегам.xlsx";

            using var streamRef = new DotNetStreamReference(stream: fileStream);

            await JS.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);

            SpinnerService?.Hide();
        }
    }
}