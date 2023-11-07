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

namespace GPNA.AlarmControlSystem.Pages.Reports
{
    public partial class Reports : ComponentBase
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

        [Parameter] [SupplyParameterFromQuery] public int? ArmId { get; set; }

        [Parameter] public DateTimeOffset From { get; set; } = DateTimeOffset.Now.AddDays(-2);
        [Parameter] public DateTimeOffset To { get; set; } = DateTimeOffset.Now.AddDays(-1);

        [Parameter] public string? ArmName { get; set; }

        private string? FieldName => _fields?.FirstOrDefault(field => field.Id == FieldId)?.Name ?? "N/A";

        static int inputKpi = 12;
        private IDictionary<string, string>? FieldLinksDictionary { get; set; }

        private IDictionary<string, string>? ArmLinksDictionary { get; set; }

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
            await SetFieldWithArm();
            _generalCount = 0;
            _averageUrgent = await SetAverageUrgent();

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
            }


            SpinnerService.Hide();
            IsEnableRenderChart = true;
            StateHasChanged();
        }

        private async Task SetFieldWithArm()
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

            ArmId ??= _workstations?.FirstOrDefault()?.Id;

            ArmName = _workstations?.FirstOrDefault(ws => ws.Id == ArmId)?.Name;

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
                ArmLinksDictionary = _workstations.ToDictionary(workStation => workStation.Name,
                    workStation => $"/reports/?fieldId={FieldId}&armId={workStation.Id}");
            }
        }

        private async Task<int> SetAverageUrgent()
        {
            var incomingAlarmsResult = await IncomingAlarmService.GetCountInHour(new GetIncomingAlarmsByDatesQuery
            {
                WorkStationId = 1,
                ActivationFrom = From,
                ActivationTo = To,
            });

            if (incomingAlarmsResult.Success)
            {
                var incomingAlarms = incomingAlarmsResult.Payload;

                int countUrgent = 0;

                foreach (var alarmsOnHour in incomingAlarms)
                {
                    if (alarmsOnHour.Value is { Length: > 0 })
                        foreach (var alarm in alarmsOnHour.Value)
                        {
                            if (alarm.Priority == PriorityType.Urgent)
                            {
                                countUrgent += 1;
                            }
                        }
                }

                if (incomingAlarms.Count > 0)
                    return countUrgent / incomingAlarms.Count;
            }

            return -1;
        }

        private async Task<Stream?> GetFileStream()
        {
            var result = await ExportService.ExportActiveAlarmsReport(new ExportIncomingAlarmsByDatesQuery
            {
                DocumentType = ExportDocumentType.Excel,
                WorkStationId = ArmId ?? 0,
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