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
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace GPNA.AlarmControlSystem.Pages.TagTable.Archive
{
    public partial class TagTableArchive : AcsPageBase
    {
        [CascadingParameter] public IModalService Modal { get; set; } = default!;
        [Inject] protected IJSRuntime JS { get; set; } = default!;
        [Inject] protected ITagChangesService TagChangesService { get; set; } = default!;
        
        [Inject] private IOptions<AcsModuleOptions>? Options { get; set; }
        [Inject] private IExportService? ExportService { get; set; }
        
        private GetTagChangesListQuery _query = new();
        private TagChangesCollection _tagsChanges = new();

        protected override async Task LoadPageAsync()
        {
            await SpinnerService.Load(GetTagChanges);
        }

        private async Task GetTagChanges()
        {
            _query.WorkStationId = WorkstationId ?? 1;
            _query.ItemsOnPage = 15;
            _query.Page ??= 1;
            var result = await TagChangesService.GetTagChangesCollection(_query);
        
            if (result.Success)
            {
                _tagsChanges = result.Payload;
                StateHasChanged();
            }
        }
        
        private async Task SetStateFilter(StateType? state)
        {
            _query.State = state;
            _query.Page = 1;
        
            await LoadPageAsync();
        }
        
        private async Task SetPriorityFilter(PriorityType? priority)
        {
            _query.Priority = priority;
            _query.Page = 1;
        
            await LoadPageAsync();
        }
        
        private async Task OnOrderingChanged(string orderBy)
        {
            if (orderBy == _query.OrderPropertyName)
                _query.OrderByDescending = !_query.OrderByDescending;
        
            _query.OrderPropertyName = orderBy;
            _query.Page = 1;
        
            await LoadPageAsync();
        }
        
        private async Task OnPageChanged(int page)
        {
            _query.Page = page;
            await LoadPageAsync();
        }
        
        private async Task SearchTag(string tagName)
        {
            _query.Suggest = tagName;
            _query.Page = 1;
            await LoadPageAsync();
        }
        
        private async Task DropFilters()
        {
            _query = new GetTagChangesListQuery
            {
                Page = 1
            };
        
            await LoadPageAsync();
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
            SpinnerService.Show();
        
            var fileStream = await GetFileStream();
        
            if (fileStream == null) return;
        
            var fileName = "Архив изменений АСУ ТП.xlsx";
        
            using var streamRef = new DotNetStreamReference(stream: fileStream);
        
            await JS.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
        
            SpinnerService?.Hide();
        }
    }
}
