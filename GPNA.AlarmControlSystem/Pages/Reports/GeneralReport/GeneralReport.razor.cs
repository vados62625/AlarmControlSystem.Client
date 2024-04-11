using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto.BufferAlarms;
using GPNA.AlarmControlSystem.Models.Dto.Field;
using GPNA.AlarmControlSystem.Models.Dto.IncomingAlarm;
using GPNA.AlarmControlSystem.Models.Dto.KpiSettings;
using GPNA.AlarmControlSystem.Models.Dto.Workstation;
using GPNA.AlarmControlSystem.Models.Enums;
using GPNA.AlarmControlSystem.Options;
using GPNA.AlarmControlSystem.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace GPNA.AlarmControlSystem.Pages.Reports.GeneralReport
{
    public partial class GeneralReport : AcsPageBase
    {
        [Inject] private IJSRuntime? JS { get; set; }

        [Inject] private IBufferAlarmService AlarmService { get; set; } = null!;
        [Inject] private IIncomingAlarmService IncomingAlarmService { get; set; } = null!;
        [Inject] private IExportService? ExportService { get; set; }
        [Inject] private ReportSettingsService? ReportSettingsService { get; set; }

        private DateTimeOffset _from = DateTimeOffset.Now.AddDays(-2);
        private DateTimeOffset _to  = DateTimeOffset.Now.AddDays(-1);

        private ReportSettingsDto? _reportSettings;

        // Значения в плитках
        int _generalCount, _averageUrgent;

        private string _spinnerClass = string.Empty;

        [Parameter] public bool IsEnableRenderChart { get; set; } = false;
        public Dictionary<string, Dictionary<DateTimeOffset, IncomingAlarmDto[]>>? IncomingAlarms { get; set; } = new();

        protected override async Task LoadPageAsync()
        {
            IsEnableRenderChart = true;
            SpinnerService.Show();
            StateHasChanged();
            _reportSettings = await GetReportSettings();
            _generalCount = 0;

            if (Workstations != null)
            {
                foreach (var workStation in Workstations)
                {
                    var incomingAlarmsResult = await IncomingAlarmService.GetCountInHour(
                        new GetAlarmsCollectionQueryBase
                        {
                            WorkStationId = workStation.Id,
                            DateTimeStart = _from,
                            DateTimeEnd = _to,
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
            
            StateHasChanged();
            await Task.Delay(100);
            IsEnableRenderChart = false;
        }

        private async Task<Stream?> GetFileStream()
        {
            var result = await ExportService.ExportActiveAlarmsReport(new ExportAlarmsCollectionQueryBase
            {
                DocumentType = ExportDocumentType.Excel,
                WorkStationId = WorkstationId ?? 0,
                DateTimeStart = _from,
                DateTimeEnd = _to,
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

            await LoadPageAsync();

            _spinnerClass = string.Empty;
        }
        
        private async Task<ReportSettingsDto?> GetReportSettings()
        {
            if (ReportSettingsService != default && WorkstationId != default)
            {
                var result = await ReportSettingsService.GetSettings(WorkstationId.Value);

                if (result.Success) return result.Payload;
            }

            return null;
        }
    }
}