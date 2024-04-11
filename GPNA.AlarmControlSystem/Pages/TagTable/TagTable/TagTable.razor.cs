using Blazored.Modal;
using Blazored.Modal.Services;
using GPNA.AlarmControlSystem.Application.Dto.Tag;
using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto.Field;
using GPNA.AlarmControlSystem.Models.Dto.Queries;
using GPNA.AlarmControlSystem.Models.Dto.Tag;
using GPNA.AlarmControlSystem.Models.Dto.TagChange;
using GPNA.AlarmControlSystem.Models.Dto.Workstation;
using GPNA.AlarmControlSystem.Models.Enums;
using GPNA.AlarmControlSystem.Options;
using GPNA.AlarmControlSystem.Services;
using GPNA.AlarmControlSystem.Pages.TagTable.TagTable.Modals;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace GPNA.AlarmControlSystem.Pages.TagTable.TagTable
{
    public partial class TagTable : AcsPageBase
    {
        [CascadingParameter] public IModalService Modal { get; set; } = default!;
        [Inject] protected IJSRuntime JS { get; set; } = default!;
        [Inject] protected ITagService TagService { get; set; } = default!;
        [Inject] private IOptions<AcsModuleOptions> Options { get; set; } = null!;
        [Inject] private IExportService ExportService { get; set; } = null!;

        private GetTagsListQuery _query = new();
        private TagsCollection _tags = new();
        private List<int> _tagIdsToChange = new();

        protected override async Task LoadPageAsync()
        {
            await SpinnerService.Load(GetTags);
        }

        private async Task AddTag()
        {
            var parameters = new ModalParameters { { "Tag", new TagDto{ WorkStationId = WorkstationId ?? 1 } } };
            var createModal = Modal.Show<AddTagModal>("", parameters);
            var result = await createModal.Result;

            if (result.Confirmed)
            {
                await SpinnerService.Load(GetTags);
            }
        }

        private async Task GetTags()
        {
            _query.WorkStationId = WorkstationId ?? 1;
            _query.ItemsOnPage = 15;
            _query.Page ??= 1;
            var result = await TagService.GetTagsCollection(_query);
            
            if (result.Success)
            {
                _tags = result.Payload;
                StateHasChanged();
            }
        }

        private async Task SendTagListToJournal()
        {
            var parameters = new ModalParameters { { "TagIds", _tagIdsToChange } };
            var modal = Modal.Show<SendTagsToJournalModal>("", parameters);
            var result = await modal.Result;

            if (result.Confirmed)
            {
                await SpinnerService.Load(GetTags);
            }
        }
        
        private void AddTagToTagChangeList(TagDto tag)
        {
            _tagIdsToChange.Add(tag.Id);
            StateHasChanged();
        }
        
        private void DeleteTagInTagChangeList(TagDto tag)
        {
            _tagIdsToChange.Remove(tag.Id);
            StateHasChanged();
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
            _query = new GetTagsListQuery
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
                TagName = _query.TagName,
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

            const string fileName = "Теги.xlsx";

            using var streamRef = new DotNetStreamReference(stream: fileStream);

            await JS.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);

            SpinnerService?.Hide();
        }
    }
}
