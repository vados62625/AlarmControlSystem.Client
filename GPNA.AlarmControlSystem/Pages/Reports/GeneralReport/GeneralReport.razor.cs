using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto.Field;
using GPNA.AlarmControlSystem.Models.Dto.IncomingAlarm;
using GPNA.AlarmControlSystem.Models.Dto.Workstation;
using GPNA.AlarmControlSystem.Models.Enums;
using GPNA.AlarmControlSystem.Options;
using GPNA.AlarmControlSystem.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace GPNA.AlarmControlSystem.Pages.Reports.GeneralReport
{
    public partial class GeneralReport : ComponentBase
    {
        [Inject] private IOptions<AcsModuleOptions>? Options { get; set; }

        [Inject] private IJSRuntime? JS { get; set; }

        [Inject] private IBufferAlarmService AlarmService { get; set; } = null!;
        [Inject] private IIncomingAlarmService IncomingAlarmService { get; set; } = null!;
        [Inject] private IExportService? ExportService { get; set; }
        [Inject] protected ISpinnerService SpinnerService { get; set; } = default!;

        [Inject] private IFieldService? FieldService { get; set; }

        [Inject] private IWorkStationService? WorkStationService { get; set; }

        [Parameter] [SupplyParameterFromQuery] public int? FieldId { get; set; }

        [Parameter] [SupplyParameterFromQuery] public int? WorkstationId { get; set; }

        [Parameter] public DateTimeOffset From { get; set; } = DateTimeOffset.Now.AddDays(-2);
        [Parameter] public DateTimeOffset To { get; set; } = DateTimeOffset.Now.AddDays(-1);

        [Parameter] public string? WorkstationName { get; set; }

        private string? FieldName => _fields?.FirstOrDefault(field => field.Id == FieldId)?.Name ?? "N/A";

        static int inputKpi = 12;
        private IDictionary<string, string>? FieldLinksDictionary { get; set; }

        private IDictionary<string, string>? WorkstationLinksDictionary { get; set; }

        private FieldDto[]? _fields;

        private WorkStationDto[]? _workstations;

        // Значения в плитках
        int _generalCount, _averageUrgent;

        private string _spinnerClass = string.Empty;

        [Parameter] public bool IsEnableRenderChart { get; set; } = false;
        public Dictionary<string, Dictionary<DateTimeOffset, IncomingAlarmDto[]>>? IncomingAlarms { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
        }

        protected override async Task OnParametersSetAsync()
        {
            await InitializePageAsync();
        }

        private async Task InitializePageAsync()
        {
            IsEnableRenderChart = false;
            SpinnerService.Show();
            StateHasChanged();
            await SetFieldWithWorkstation();
            _generalCount = 0;
            // _averageUrgent = await SetAverageUrgent();

            if (_workstations != null)
            {
                foreach (var workStation in _workstations)
                {
                    var incomingAlarmsResult = await IncomingAlarmService.GetCountInHour(
                        new GetIncomingAlarmsByDatesQuery
                        {
                            WorkStationId = workStation.Id,
                            ActivationFrom = From,
                            ActivationTo = To,
                        });

                    if (incomingAlarmsResult.Success)
                    {
                        IncomingAlarms[workStation.Name] = incomingAlarmsResult.Payload;
                    }
                }

                _generalCount = IncomingAlarms?.Sum(c => c.Value.Sum(x => x.Value.Length)) ?? 0;
                _averageUrgent = (IncomingAlarms?.Sum(c =>
                    c.Value.Sum(x => x.Value.Count(alarm => alarm.Priority == PriorityType.Urgent))) ?? 0)/(IncomingAlarms?.FirstOrDefault().Value.Keys.Count ?? 1);
            }


            SpinnerService.Hide();
            IsEnableRenderChart = true;
            StateHasChanged();
        }

        private async Task SetFieldWithWorkstation()
        {
            if (FieldService != null)
            {
                var fields = await FieldService.GetList();

                if (fields.Success)
                {
                    _fields = fields.Payload.ToArray();
                }
            }

            if (WorkStationService != null)
            {
                var workstations = await WorkStationService.GetList(new { FieldId });

                if (workstations.Success)
                {
                    _workstations = workstations.Payload.ToArray();
                }
            }

            FieldId ??= _fields?.FirstOrDefault()?.Id;

            WorkstationId ??= _workstations?.FirstOrDefault()?.Id;

            WorkstationName = _workstations?.FirstOrDefault(ws => ws.Id == WorkstationId)?.Name;

            FillLinks();
        }

        private void FillLinks()
        {
            if (_fields != null)
            {
                FieldLinksDictionary =
                    _fields.ToDictionary(field => field.Name, field => $"/reports/?fieldId={field.Id}");
            }

            if (_workstations != null)
            {
                WorkstationLinksDictionary = _workstations.ToDictionary(workStation => workStation.Name,
                    workStation => $"/reports/?fieldId={FieldId}&workstationId={workStation.Id}");
            }
        }

        private async Task<Stream?> GetFileStream()
        {
            var result = await ExportService.ExportActiveAlarmsReport(new ExportIncomingAlarmsByDatesQuery
            {
                DocumentType = ExportDocumentType.Excel,
                WorkStationId = WorkstationId ?? 0,
                ActivationFrom = From,
                ActivationTo = To,
            });

            return new MemoryStream(result);
        }

        private async Task DownloadFileFromStream()
        {
            SpinnerService?.Show();

            var fileStream = await GetFileStream();

            if (fileStream == null) return;

            var fileName = "report.xlsx";

            using var streamRef = new DotNetStreamReference(stream: fileStream);

            await JS.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);

            SpinnerService?.Hide();
        }

        private async Task RefreshBySpinner()
        {
            _spinnerClass = " active";

            await InitializePageAsync();

            _spinnerClass = string.Empty;
        }
    }
}