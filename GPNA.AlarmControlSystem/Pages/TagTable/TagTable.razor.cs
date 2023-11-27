using Blazored.Modal;
using Blazored.Modal.Services;
using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto.Field;
using GPNA.AlarmControlSystem.Models.Dto.Tag;
using GPNA.AlarmControlSystem.Models.Dto.Workstation;
using GPNA.AlarmControlSystem.Models.Enums;
using GPNA.AlarmControlSystem.Options;
using GPNA.AlarmControlSystem.Services;
using GPNA.AlarmControlSystem.Pages.TagTable.Modals;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Options;

namespace GPNA.AlarmControlSystem.Pages.TagTable
{
    public partial class TagTable : ComponentBase
    {
        [CascadingParameter] public IModalService Modal { get; set; } = default!;
        [Inject] protected ISpinnerService SpinnerService { get; set; } = default!;
        [Inject] protected ITagService TagService { get; set; } = default!;
        [Inject] private IOptions<AcsModuleOptions>? Options { get; set; }
        [Inject] private IFieldService? FieldService { get; set; }
        [Inject] private IWorkStationService? WorkStationService { get; set; }
        [Parameter] [SupplyParameterFromQuery] public int? WorkstationId { get; set; }
        [Parameter] [SupplyParameterFromQuery] public int? FieldId { get; set; }
        
        private string? _workstationName, _fieldName;
        private WorkStationDto[]? _workstations;
        private FieldDto[]? _fields;
        private IDictionary<string, string>? _fieldLinksDictionary;
        private IDictionary<string, string>? _workstationLinksDictionary;

        private GetTagsListQuery _query = new();
        
        private List<TagDto>? _tags;

        string input = "";
        
        private int _pagesCount, _totalCount;
        private int _currentPage = 1;

        protected override async Task OnInitializedAsync()
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
        
        public async Task AddTag()
        {
            var createModal = Modal.Show<AddTagModal>();
            var result = await createModal.Result;

            if (result.Confirmed)
            {
                await SpinnerService.Load(GetTags);
            }
        }


        private async Task Enter(KeyboardEventArgs e)
        {
            if (e.Code is "Enter" or "NumpadEnter")
            {
                await SpinnerService.Load(GetTags);
            }
        }

        private async Task GetTags()
        {
            _query.WorkStationId = WorkstationId ?? 1;
            _query.ItemsOnPage = 15;
            _query.Page ??= 1;
            var result = await TagService.GetCollection(_query); // TODO I make PageableCollectionDto
            
            if (result.Success)
            {
                _tags = result.Payload.Items;
                _pagesCount = 100;
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
    
        private async Task Search()
        {
            _query.Page = 1;
            await SpinnerService.Load(GetTags);
        }
    }
}
